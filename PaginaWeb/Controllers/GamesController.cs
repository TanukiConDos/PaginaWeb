using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using System.Diagnostics;

namespace PaginaWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Priconne()
        {
            PriconneTierList model = new();
            
            ViewData["Title"] = "Priconne";
            return View("TierList", model);
        }

        public IActionResult HonkaiStarRail()
        {
            
            HonkaiStarRailTierList model = new();

            ViewData["Title"] = "Honkai Star Rail";
            return View("TierList",model);
        }

        public IActionResult Nikke()
        {
            NikkeTierList model = new();

            ViewData["Title"] = "Nikke";
            return View("TierList", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
