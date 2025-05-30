using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LifeBridge.Models;

namespace LifeBridge.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Route("")]
    [Route("home")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("about")]
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [Route("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
