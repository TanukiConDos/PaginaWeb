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
            TierList? model;
            string jsonString = System.IO.File.ReadAllText("Pricone.json");
            model = JsonSerializer.Deserialize<TierList>(jsonString);
            if (model == null) model = UpdatePriconeTierList();
            if (model.Date - DateTime.Today > TimeSpan.FromDays(15))
            {
                model = UpdatePriconeTierList();
            }
            ViewData["Title"] = "Priconne";
            return View("TierList", model);
        }

        private static TierList UpdatePriconeTierList()
        {
            HtmlWeb request = new HtmlWeb();
            var html = request.Load("https://gamewith.jp/pricone-re/article/show/93068");
            var table = html.DocumentNode.SelectSingleNode("//div[@class=\"puri_5col-table\"]");
            int tier = -1;
            List<List<string>> tiers = new();
            Dictionary<string, string> characterUrl = new();
            Dictionary<string, string> characterImg = new();
            List<string> names = new();
            foreach (var node in table.SelectNodes(table.XPath + "/table/tr"))
            {
                if (node.FirstChild.Name != "th")
                {

                    foreach (var td in node.ChildNodes)
                    {
                        if (td.FirstChild == null) continue;
                        var url = td.FirstChild.GetAttributeValue("href", "");
                        var name = td.FirstChild.GetDirectInnerText();
                        names.Add(name);
                        characterUrl.Add(name, url);
                        var img = td.FirstChild.ChildNodes[0].GetAttributeValue("data-original", "");
                        characterImg.Add(name, img);
                    }
                }
                else
                {
                    if (tier != -1)
                    {
                        tiers.Add(names);
                        names = new();
                    }
                    tier++;

                }
            }

            var model = new TierList
            {
                Tiers = tiers,
                CharacterImg = characterImg,
                CharactersUrl = characterUrl,
                Date = DateTime.Today,
            };
            
            string jsonString = JsonSerializer.Serialize(model);
            System.IO.File.WriteAllText("pricone.json", jsonString);

            return model;
        }

        public IActionResult HonkaiStarRail()
        {
            TierList? model;
            string jsonString = System.IO.File.ReadAllText("HonkaiStarRail.json");
            model = JsonSerializer.Deserialize<TierList>(jsonString);
            if (model == null) model = UpdateHonkaiStarRailTierList();
            if (model.Date - DateTime.Today > TimeSpan.FromDays(15))
            {
                model = UpdateHonkaiStarRailTierList();
            }


            ViewData["Title"] = "Honkai Star Rail";
            return View("TierList",model);
        }

        private static TierList UpdateHonkaiStarRailTierList()
        {
            HtmlWeb request = new();
            var html = request.Load("https://www.prydwen.gg/star-rail/tier-list/");
            var table = html.DocumentNode.SelectSingleNode("//div[@class=\"custom-tier-list-hsr\"]");
            List<List<string>> tiers = new();
            Dictionary<string, string> characterUrl = new();
            Dictionary<string, string> characterImg = new();
            List<string> names = new();
            foreach (var node in table.ChildNodes)
            {
                if (node.GetAttributeValue("class", "").Contains(" tier-"))
                {
                    var tier = node.ChildNodes[1];
                    foreach (var group in tier.ChildNodes)
                    {
                        if (group.GetClasses().Contains("custom-tier-burst"))
                        {
                            foreach (var character in group.ChildNodes)
                            {
                                var a = character.ChildNodes[0].ChildNodes[0].ChildNodes[0];
                                var name = "";
                                foreach(var span in a.ChildNodes)
                                {
                                    if (span.GetAttributeValue("class","") == "emp-name")
                                    {
                                        name = span.InnerText;
                                        continue;
                                    }
                                }
                                var url = "https://www.prydwen.gg" + a.GetAttributeValue("href","");
                                var img = "https://www.prydwen.gg" + a.ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[1].GetAttributeValue("data-src","");
                                
                                names.Add(name);
                                characterUrl.Add(name, url);
                                characterImg.Add(name, img);
                            }
                        }
                    }
                    tiers.Add(names);
                    names = new();
                }
            }

            var model = new TierList
            {
                Tiers = tiers,
                CharacterImg = characterImg,
                CharactersUrl = characterUrl,
                Date = DateTime.Today,
            };

            string jsonString = JsonSerializer.Serialize(model);
            System.IO.File.WriteAllText("honkaiStarRail.json", jsonString);

            return model;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
