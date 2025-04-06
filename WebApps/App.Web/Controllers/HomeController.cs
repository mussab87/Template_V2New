using App.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.Web.Controllers;

[PermissionAuthorize]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(IServiceProvider serviceProvider, ILogger<HomeController> logger) : base(serviceProvider)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.test = "";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
