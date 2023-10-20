using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using System.Diagnostics;
using System.Text.Json;

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
            TierList? model = null;
            bool update = false;
            try
            {
                string jsonString = System.IO.File.ReadAllText("Pricone.json");
                model = JsonSerializer.Deserialize<PriconneTierList>(jsonString);
            }
            catch(FileNotFoundException) {
                update = true;
            }

            if (model == null || DateTime.Today - model.Date > TimeSpan.FromDays(15))
            {
                update = true;
            }

            if (update)
            {
                model = new PriconneTierList();
                model.Update();
            }

            ViewData["Title"] = "Priconne";
            return View("TierList", model);
        }

        public IActionResult HonkaiStarRail()
        {
            TierList? model = null;
            bool update = false;
            try
            {
                string jsonString = System.IO.File.ReadAllText("honkaiStarRail.json");
                model = JsonSerializer.Deserialize<HonkaiStarRailTierList>(jsonString);
            }
            catch (FileNotFoundException)
            {
                update = true;
            }

            if (model == null || DateTime.Today - model.Date > TimeSpan.FromDays(15))
            {
                update = true;
            }

            if (update)
            {
                model = new HonkaiStarRailTierList();
                model.Update();
            }


            ViewData["Title"] = "Honkai Star Rail";
            return View("TierList",model);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
