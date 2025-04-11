using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

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
        //ViewData["CurrentPageSize"] = pageSize;
        //ViewData["CurrentPage"] = page;
        //ViewData["CurrentFilter"] = searchString;

        //var allUsers = await _userService.GetAllUsers();
        var allUsers = await _userService.GetPaginatedUsers(page, pageSize, searchString, 1);

        return View(allUsers);
    }

    #endregion

    #region Add Edit User
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
                var userRole = await _userService.GetUserRolesAsync(userId);
                userDtoModel.RoleId = roles.FirstOrDefault(r => r.Name == userRole.ToList()[0]).Id;
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
                    return PartialView("AddEditUser", model);

                model.CreatedBy = User.Identity.Name;
                //get selected role to Add, set default password th user for first login
                var selectedRole = roles.FirstOrDefault(r => r.Id == model.RoleId);
                //Add new user
                await _userService.CreateUser(model, "Aa@123456", selectedRole.Name);

                return Ok(new { success = true, data = "تم الحفظ بنجاح" });
            }

            //Update Exist user
            if (model.ActionType == 1)
            {
                if (!ModelState.IsValid)
                    return PartialView("AddEditUser", model);

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

    #region Delete user
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return Json(new { success = true, data = "هذا المستخدم غير موجود حاليا، فضلا تواصل مع الدعم الفني" });

            //update user IsDeleted
            user.IsDeleted = true;
            user.IsActive = false;
            var result = await _userService.UpdateUserAsync(user);
            return Json(new { success = true, data = "تم الحفظ بنجاح" });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ اثناء حفظ البيانات !" + ex.ToString());
            return Json(new { success = false, data = "حدث خطأ اثناء حفظ البانات، فضلا تواصل مع الدعم الفني" + ex.ToString() });
        }
    }
    #endregion

    #region Get All Roles
    public async Task<IActionResult> GetRoles(string searchString = "", int page = 1, int pageSize = 10)
    {
        var allUsers = await _roleService.GetPaginatedRoles(page, pageSize, searchString);

        return View(allUsers);
    }

    #endregion

    #region Add Edit Role
    public async Task<IActionResult> AddEditRole(int? actionType, string roleId)
    {
        RoleDto roleDtoModel = null;
        try
        {
            var roles = await _roleService.GetAllRolesAsync();
            //ViewData["roles"] = new SelectList(roles, "Id", "RoleNameArabic");

            if (actionType == 0)
                roleDtoModel = new RoleDto();

            if (actionType == 1)
            {
                roleDtoModel = _mapper.Map<RoleDto>(await _roleService.FindByIdAsync(roleId));
                if (roleDtoModel == null)
                {
                    return PartialView("AddEditRole", roleDtoModel);
                }
                roleDtoModel.ActionType = 1;
                roleDtoModel.Id = roleId;
            }
            return PartialView("AddEditRole", roleDtoModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ!" + ex.ToString());
            return PartialView("AddEditRole", roleDtoModel);
        }

    }

    [HttpPost]
    public async Task<IActionResult> AddEditRole(RoleDto model)
    {
        try
        {
            //Add new role
            if (model.ActionType == 0)
            {
                if (!ModelState.IsValid)
                    return PartialView("AddEditRole", model);

                model.CreatedById = User.Identity.Name;

                //Add new role
                await _roleService.CreateRoleAsync(model);

                return Ok(new { success = true, data = "تم الحفظ بنجاح" });
            }

            //Update Exist role
            if (model.ActionType == 1)
            {
                if (!ModelState.IsValid)
                    return PartialView("AddEditRole", model);

                model.LastModifiedById = User.Identity.Name;
                //update user
                await _roleService.UpdateRoleAsync(model);

                return Ok(new { success = true, data = "تم الحفظ بنجاح" });
            }

            return PartialView("AddEditRole", model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ اثناء حفظ البيانات !" + ex.ToString());
            return PartialView("AddEditRole", model);
        }
    }
    #endregion

    #region Delete Role
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        try
        {
            var role = await _roleService.FindByIdAsync(roleId);
            if (role == null)
                return Json(new { success = true, data = "هذه الصلاحية غير موجودة حاليا، فضلا تواصل مع الدعم الفني" });

            //update role IsDeleted
            role.IsDeleted = true;
            var result = await _roleService.UpdateRoleAsync(_mapper.Map<RoleDto>(role));
            return Json(new { success = true, data = "تم الحفظ بنجاح" });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "حدث خطأ اثناء حفظ البيانات !" + ex.ToString());
            return Json(new { success = false, data = "حدث خطأ اثناء حفظ البانات، فضلا تواصل مع الدعم الفني" + ex.ToString() });
        }
    }
    #endregion

    #region Test Paging    
    public IActionResult TestPaging(string searchString, int page = 1, int pageSize = 10)
    {
        // Store the current page size and page in ViewData for persistence
        //ViewData["CurrentPageSize"] = pageSize;
        //ViewData["CurrentPage"] = page;

        List<CountriesDto> result = new();
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchString))
        {
            result = CreateCountriesList(searchString);
            //ViewData["CurrentFilter"] = searchString;
        }
        else
        {
            result = CreateCountriesList(searchString);
        }

        var returnResult = new PaginatedResult<CountriesDto>()
        {
            Items = (result.ToPagedList<CountriesDto>(page, pageSize)),
            TotalCount = result.Count,
            PageNumber = page,
            PageSize = pageSize,
            SearchString = searchString
        };
        return View(returnResult); //View(result.ToPagedList<CountriesDto>(page, pageSize));
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
