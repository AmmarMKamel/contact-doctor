using System.Diagnostics;
using ContactDoctor.Data;
using Microsoft.AspNetCore.Mvc;
using ContactDoctor.Models;
using ContactDoctor.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContactDoctor.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ContactDoctorContext _context = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var specializations = await _context.Doctors
            .Select(d => d.Specialization)
            .Distinct()
            .ToListAsync();

        Home data = new Home()
        {
            Specializations = specializations
        };
        
        return View(data);
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