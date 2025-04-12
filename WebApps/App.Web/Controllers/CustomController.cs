using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace App.Web.Controllers;

//[PermissionAuthorize]
public class CustomController : BaseController
{
    private readonly ILogger<AdminController> _logger;
    public CustomController(IServiceProvider serviceProvider, ILogger<AdminController> logger) : base(serviceProvider)
    {
        _logger = logger;
    }

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

    #region HijriDate  
    public IActionResult HijriDate()
    {

        return View();
    }
    #endregion
}
