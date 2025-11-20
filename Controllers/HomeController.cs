using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Example: Get all users from database
        var users = await _context.LegacyUsers.ToListAsync();
        return View(users);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
