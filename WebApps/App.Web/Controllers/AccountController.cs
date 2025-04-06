using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Web.Controllers;
public class AccountController : BaseController
{
    public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
    { }

    #region Login

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        //Get user 
        var user = await _userService.FindByNameAsync(model.Username);
        //Check account exist
        if (user == null)
        {
            ModelState.AddModelError("", "هذا المستخدم غير مسجل في النظام !");
            return View(model);
        }

        //Check if password entered wrong more than 3 times account must be deActivated
        //Check account IsActive
        var resultPasswordChecking = await _userService.ValidateLoginAsync(model.Username, model.Password);
        if (!resultPasswordChecking.Success)
        {
            ModelState.AddModelError("error", $"{resultPasswordChecking.Message}");
            return View(model);
        }

        //Retrieve CAPTCHA from session
        var storedCaptcha = HttpContext.Session.GetString("CaptchaCode");
        //Check CAPTCHA Entered Correct Here
        if (string.IsNullOrEmpty(storedCaptcha) || model.CaptchaInput != storedCaptcha)
        {
            ModelState.AddModelError("captchaError", "رمز التحقق غير صحيح!");
            return View(model);
        }

        //Check user password expire - in case password not changed more than 3 months - 90 days
        var lastPasswordChange = await _userService.GetLastUserPasswordLogsAsync(user.Id);
        int daysRemaining = 0;
        DateTime lastPasswordChangeDate = lastPasswordChange != null
                                    ? lastPasswordChange.PasswordChange
                                    : DateTime.UtcNow.AddDays(-80);

        int daysSinceLastChange = (int)(DateTime.UtcNow - lastPasswordChangeDate).TotalDays;
        //Password expires after 90 days
        int passwordExpiryDays = 90;
        daysRemaining = passwordExpiryDays - daysSinceLastChange;

        //Check if password has expired
        if (daysRemaining <= 0)
        {
            //Password has expired
            return RedirectToAction("ResetPassword", new { username = model.Username, expired = true });
        }

        //Add claims values to use in views, when signing in:
        List<Claim> claims = addExtraClaimsValues(user);

        //Everything is ok, then sign in user
        var result = await _userService.SignInAsync(
                user, false, claims);

        if (result.Succeeded)
        {
            //Check first login - in case was yes: navigate into change password 
            if ((bool)user.FirstLogin)
            {
                return RedirectToAction("ResetPassword", new { username = model.Username, expired = true });
            }

            // Check if password will expire soon (within 10 days)
            //To Do: show message to user in Layout view
            if (daysRemaining <= 10)
                // Store the warning in session so it can be displayed on the next page
                HttpContext.Session.SetString("passwordExpire", $"ستنتهي صلاحية كلمة المرور بعد {daysRemaining} ايام، نرجو إعادة تعيين كلمة المرور والا سيتم تعطيل الحساب");

            //Log every login action in UserLoginLog table
            await _userService.CreateUserLoginLogAsync(new UserLoginLogDto()
            {
                UserId = user.Id,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                LogginDateTime = DateTime.UtcNow
            });

            //Login successful, Navigate into Home page 
            HttpContext.Session.SetString("firstName", user.FirstName);
            HttpContext.Session.SetString("lastName", user.LastName);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("error", "حدث خطأ نرجو المحاولة مرة أخرى!");
        return View(model);
    }

    private static List<Claim> addExtraClaimsValues(User user)
    {
        var claims = new List<Claim>
                    {
                        // Standard claims
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
    
                        // Custom claims                        
                        new Claim("firstName", user.FirstName ?? ""),
                        new Claim("lastName", user.LastName ?? "")
                        // Add any other custom claims you need
                    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return claims;
    }

    public IActionResult Captcha()
    {
        var captchaBytes = CaptchaService.GenerateCaptcha(out string captchaText);

        // Store CAPTCHA in session
        HttpContext.Session.SetString("CaptchaCode", captchaText);

        return File(captchaBytes, "image/png");
    }
    #endregion

    #region Reset Password
    [HttpGet]
    public IActionResult ResetPassword(string username = null, bool expired = false)
    {
        var model = new ResetPasswordDto
        {
            Username = username,
            IsExpired = expired
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.FindByNameAsync(model.Username);
        if (user == null)
        {
            ModelState.AddModelError("error", "حدث خطأ يرجى التواصل مع مدير النظام");
            return View(model);
        }

        // Validate current password first
        var isCurrentPasswordValid = await _userService.CheckPasswordAsync(user, model.CurrentPassword);
        if (!isCurrentPasswordValid)
        {
            ModelState.AddModelError("CurrentPassword", "كلمة المرور الحالية غير مطابقة");
            return View(model);
        }

        // Validate that the new password is different from current            
        if (model.CurrentPassword == model.NewPassword)
        {
            ModelState.AddModelError("NewPassword", "كلمة المرور الجديدة يجب ان تكون مختلفة عن القديمة");
            return View(model);
        }

        //To Do: check also last 3 password must not be same
        if (await _userService.IsPasswordInRecentHistoryAsync(user.Id, model.CurrentPassword, model.NewPassword, 3))
        {
            ModelState.AddModelError("NewPassword", "كلمة المرور الجديدة يجب ان تكون مختلفة عن اخر 3 كلمات مرور مستخدمة من قبل");
            return View(model);
        }
        // Change the password
        var result = await _userService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
        {
            // Log the password change
            await _unitOfWork.Repository<UserPasswordLog>().AddAsync(new UserPasswordLog
            {
                UserId = user.Id,
                PasswordChange = DateTime.UtcNow,
                OldPassword = _encryptionService.Encrypt(model.CurrentPassword),
                NewPassword = _encryptionService.Encrypt(model.NewPassword),
                CreatedById = user.Id,
                //ip = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            await _unitOfWork.Complete(user.Id, HttpContext.Connection.RemoteIpAddress?.ToString());

            //update FirstLogin in User table
            user.FirstLogin = false;
            await _userService.UpdateUserAsync(user);

            ViewBag.passwordChanged = "true";
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
    #endregion

    #region Logout
    public async Task<IActionResult> Logout()
    {
        var user = HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        //Log every logout action in UserLoginLog table
        await _userService.CreateUserLoginLogAsync(new UserLoginLogDto()
        {
            UserId = userId,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            ActivityType = "Logout",
            Description = "Logout",
            LoggedOutDateTime = DateTime.UtcNow
        });

        string cacheKey = $"UserPermissions_{userId}";
        await _cache.RemoveAsync(cacheKey);

        HttpContext.Session.Clear();
        await _userService.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }
    #endregion
}

