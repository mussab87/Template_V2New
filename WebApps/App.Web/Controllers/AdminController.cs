using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var allUsers = await _userService.GetPaginatedUsers(page, pageSize, searchString);
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
    public async Task<IActionResult> AddEditUser()
    {
        ViewData["roles"] = new SelectList(await _roleService.GetAllRolesAsync(), "Id", "RoleNameArabic");
        return PartialView("AddEditUser");
    }

    [HttpPost]
    public async Task<IActionResult> AddEditUser(UserDto model)
    {
        var roles = await _roleService.GetAllRolesAsync();
        ViewData["roles"] = new SelectList(roles, "Id", "RoleNameArabic");
        if (!ModelState.IsValid)
        {
            //return View(model);
            return PartialView("AddEditUser", model);
        }
        model.UserStatus = true;
        model.EmailConfirmed = true;
        model.CreatedBy = User.Identity.Name;

        var selectedRole = roles.FirstOrDefault(r => r.Id == model.RoleId);
        await _userService.CreateUser(model, "Aa@123456", selectedRole.Name);

        return Json("success");
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
