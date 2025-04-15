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

    #region HijriDate  
    public IActionResult HijriDate()
    {

        return View();
    }
    #endregion

    #region Typography  
    public IActionResult Typography()
    {
        return View();
    }
    #endregion

    #region Buttons  
    public IActionResult Buttons()
    {
        return View();
    }
    #endregion

    #region ButtonGgroup  
    public IActionResult ButtonGgroup()
    {
        return View();
    }
    #endregion

    #region Dropdown  
    public IActionResult Dropdown()
    {
        return View();
    }
    #endregion

    #region Tables  
    public IActionResult Tables()
    {
        return View();
    }
    #endregion

    #region Modal  
    public IActionResult Modal()
    {
        return View();
    }
    #endregion

    #region Alerts  
    public IActionResult Alerts()
    {
        return View();
    }
    #endregion

    #region Popover  
    public IActionResult Popover()
    {
        return View();
    }
    #endregion

    #region Tooltip  
    public IActionResult Tooltip()
    {
        return View();
    }
    #endregion

}
