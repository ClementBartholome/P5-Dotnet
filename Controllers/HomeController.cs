using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Express_Voitures.Models;
using Express_Voitures.Services;

namespace Express_Voitures.Controllers;

public class HomeController(AnnonceService annonceService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var annonces = await annonceService.GetLastAnnonces(3);
        return View(annonces);
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