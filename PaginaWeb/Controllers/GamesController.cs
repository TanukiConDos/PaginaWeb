using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PaginaWeb.Models;
using PaginaWeb.Services;
using System.Diagnostics;

namespace PaginaWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;
        private readonly TierListsDbContext _context;
        private readonly TierlistUpdater _updater;

        public GamesController(ILogger<GamesController> logger, TierListsDbContext context, TierlistUpdater updater)
        {
            _logger = logger;
            _context = context;
            _updater = updater;

        }

        public IActionResult Priconne()
        {
            //tierListsDb.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var game = _context.Games.Include(g => g.characters).FirstOrDefault(g => g.name == "Priconne");
            if (game == null)
            {
                game = new Game
                {
                    name = "Priconne"
                };
                _updater.updatePriconne(_context);
            }

            if (DateTime.Today - game.date > TimeSpan.FromDays(15))
            {
                _updater.updatePriconne(_context);
            }

            List<List<Character>> tiers = new();

            for (int i = 0; i < game.numTiers; i++)
            {
                var tier = game.characters.Where(c => c.gameName == game.name).Where(c => c.tier == i).ToList();
                tiers.Add(tier);
            }

            ViewData["Title"] = "Priconne";
            return View("TierList", tiers);
        }

        public IActionResult HonkaiStarRail()
        {

            //tierListsDb.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var game = _context.Games.Include(g => g.characters).FirstOrDefault(g => g.name == "Honkai Star Rail");
            if (game == null)
            {
                game = new Game
                {
                    name = "Honkai Star Rail"
                };
                _updater.updateHonkaiStarRail(_context);
            }

            if (DateTime.Today - game.date > TimeSpan.FromDays(15))
            {
                _updater.updateHonkaiStarRail(_context);
            }

            List<List<Character>> tiers = new();

            for (int i = 0; i < game.numTiers; i++)
            {
                var tier = game.characters.Where(c => c.gameName == game.name).Where(c => c.tier == i).ToList();
                tiers.Add(tier);
            }

            ViewData["Title"] = "Honkai Star Rail";
            return View("TierList",tiers);
        }

        public IActionResult Nikke()
        {
            //tierListsDb.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var game = _context.Games.Include(g => g.characters).FirstOrDefault(g => g.name == "Nikke");
            if (game == null)
            {
                game = new Game
                {
                    name = "Nikke"
                };
                _updater.UpdateNikke(_context);
            }

            if (DateTime.Today - game.date > TimeSpan.FromDays(15))
            {
                _updater.UpdateNikke(_context);
            }

            List<List<Character>> tiers = new();

            for (int i = 0; i < game.numTiers; i++)
            {
                var tier = game.characters.Where(c => c.gameName == game.name).Where(c => c.tier == i).ToList();
                tiers.Add(tier);
            }

            ViewData["Title"] = "Nikke";
            return View("TierList", tiers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
