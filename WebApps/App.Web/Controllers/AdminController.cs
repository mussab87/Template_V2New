using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App.Web.Controllers;

//[PermissionAuthorize]
public class AdminController : BaseController
{
    private readonly ILogger<AdminController> _logger;
    public AdminController(IServiceProvider serviceProvider, ILogger<AdminController> logger) : base(serviceProvider)
    {
        _logger = logger;
    }

    #region Get All Users
    public async Task<IActionResult> GetUsers(string searchString = "", int page = 1, int pageSize = 10)
    {
        // Store the current page size and page in ViewData for persistence
        ViewData["CurrentPageSize"] = pageSize;
        ViewData["CurrentPage"] = page;
        ViewData["CurrentFilter"] = searchString;

        //var allUsers = await _userService.GetAllUsers();
        var allUsers = await _userService.GetPaginatedUsers(page, pageSize, searchString, 1, "dsc");
        //if (!string.IsNullOrEmpty(searchString))
        //{
        //    allUsers = allUsers.Where(x => x.Username.Contains(searchString)
        //                || x.FirstName.Contains(searchString)
        //        || x.LastName.Contains(searchString)).ToList();
        //    ViewData["CurrentFilter"] = searchString;
        //}

        return View(allUsers);
    }

    #endregion

    #region Add New User
    public async Task<IActionResult> AddEditUser(int? actionType, string userId)
    {
        UserDto userDtoModel = null;
        try
        {
            var roles = await _roleService.GetAllRolesAsync();
            ViewData["roles"] = new SelectList(roles, "Id", "RoleNameArabic");

            if (actionType == 0)
                userDtoModel = new UserDto();

            if (actionType == 1)
            {
                userDtoModel = _mapper.Map<UserDto>(await _userService.GetUserByIdAsync(userId));
                if (userDtoModel == null)
                {
                    return PartialView("AddEditUser", userDtoModel);
                }
                userDtoModel.ActionType = 1;
                userDtoModel.Id = userId;
                //get user role and add to dto
                var userRole =  await _userService.GetUserRolesAsync(userId);
                userDtoModel.RoleId = roles.FirstOrDefault(r=>r.Name == userRole.ToList()[0]).Id;
            }
            return PartialView("AddEditUser", userDtoModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ!" + ex.ToString());
            return PartialView("AddEditUser", userDtoModel);
        }

    }

    [HttpPost]
    public async Task<IActionResult> AddEditUser(UserDto model)
    {
        try
        {
            var roles = await _roleService.GetAllRolesAsync();
            ViewData["roles"] = new SelectList(roles, "Id", "RoleNameArabic");

            //Add new user
            if (model.ActionType == 0)
            {
                if (!ModelState.IsValid)
                {
                    //return View(model);
                    return PartialView("AddEditUser", model);
                }

                model.CreatedBy = User.Identity.Name;
                //get selected role to Add, assign default password th user for first login
                var selectedRole = roles.FirstOrDefault(r => r.Id == model.RoleId);
                //Add new user
                await _userService.CreateUser(model, "Aa@123456", selectedRole.Name);

                return Ok(new { success = true, data = "تم الحفظ بنجاح" });
            }

            //Update Exist user
            if (model.ActionType == 1)
            {
                if (!ModelState.IsValid)
                {
                    //return View(model);
                    return PartialView("AddEditUser", model);
                }

                model.LastModifiedBy = User.Identity.Name;
                //update user
                await _userService.UpdateUserAsync(model);

                return Ok(new { success = true, data = "تم الحفظ بنجاح" });
            }

            return PartialView("AddEditUser", model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ اثناء حفظ البيانات !" + ex.ToString());
            return PartialView("AddEditUser", model);
        }
    }

    #endregion

    #region Test Paging    
    public IActionResult TestPaging(string searchString, int page = 1, int pageSize = 10)
    {
        // Store the current page size and page in ViewData for persistence
        ViewData["CurrentPageSize"] = pageSize;
        ViewData["CurrentPage"] = page;

        List<CountriesDto> result = new();
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchString))
        {
            result = CreateCountriesList(searchString);
            ViewData["CurrentFilter"] = searchString;
        }
        else
        {
            result = CreateCountriesList(searchString);
        }

        return View(result.ToPagedList<CountriesDto>(page, pageSize));
    }

    protected List<CountriesDto> CreateCountriesList(string searchString = null)
    {
        List<CountriesDto> countryList = new();
        //List<CountriesDto> countryListSearch = new();
        for (int i = 1; i < 100 + 1; i++)
        {
            countryList.Add(
                new CountriesDto()
                {
                    Id = i,
                    NameEnglish = "Firstname" + i,
                    NameArabic = "Lastname" + i,
                    CreatedDate = DateTime.Now
                }
            );
        }

        if (searchString is not null && countryList is not null)
        {
            countryList = countryList.Where(x => x.NameEnglish.Contains(searchString) ||
                                    x.NameArabic.Contains(searchString)).ToList();
        }
        return (countryList);
    }

    #endregion
}
