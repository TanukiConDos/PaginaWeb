using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using System.Diagnostics;
using System.Text.Json;

namespace PaginaWeb.Controllers
{
    public class Juegos : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public Juegos(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Priconne()
        {
            TierList? modelo;
            try
            {
                string jsonString = System.IO.File.ReadAllText("Pricone.json");
                modelo = JsonSerializer.Deserialize<TierList>(jsonString);
                if (modelo.Date - DateTime.Today > TimeSpan.FromDays(15))
                {
                    modelo = ActualizarPriconeTierList();
                }
            }
            catch (FileNotFoundException)
            {
                modelo = ActualizarPriconeTierList();
            }
            ViewData["Title"] = "Priconne";
            return View("TierList", modelo);
        }

        private static TierList ActualizarPriconeTierList()
        {
            HtmlWeb request = new HtmlWeb();
            var html = request.Load("https://gamewith.jp/pricone-re/article/show/93068");
            var tabla = html.DocumentNode.SelectSingleNode("//div[@class=\"puri_5col-table\"]");
            int tier = -1;
            List<List<string>> tiers = new();
            Dictionary<string, string> characterUrl = new();
            Dictionary<string, string> characterImg = new();
            List<string> names = new();
            foreach (var node in tabla.SelectNodes(tabla.XPath + "/table/tr"))
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

            var modelo = new TierList
            {
                Tiers = tiers,
                CharacterImg = characterImg,
                CharactersUrl = characterUrl,
                Date = DateTime.Today,
            };
            
            string jsonString = JsonSerializer.Serialize(modelo);
            System.IO.File.WriteAllText("pricone.json", jsonString);

            return modelo;
        }

        public IActionResult HonkaiStarRail()
        {
            TierList? modelo;
            try
            {
                string jsonString = System.IO.File.ReadAllText("HonkaiStarRail.json");
                modelo = JsonSerializer.Deserialize<TierList>(jsonString);
                if (modelo.Date - DateTime.Today > TimeSpan.FromDays(15))
                {
                    modelo = ActualizarHonkaiStarRailTierList();
                }
            }
            catch (FileNotFoundException)
            {
                modelo = ActualizarHonkaiStarRailTierList();
            }

            ViewData["Title"] = "Honkai Star Rail";
            return View("TierList",modelo);
        }

        private static TierList ActualizarHonkaiStarRailTierList()
        {
            HtmlWeb request = new();
            var html = request.Load("https://www.prydwen.gg/star-rail/tier-list/");
            var tabla = html.DocumentNode.SelectSingleNode("//div[@class=\"custom-tier-list-hsr\"]");
            List<List<string>> tiers = new();
            Dictionary<string, string> characterUrl = new();
            Dictionary<string, string> characterImg = new();
            List<string> names = new();
            foreach (var node in tabla.ChildNodes)
            {
                if (node.GetAttributeValue("class", "").Contains(" tier-"))
                {
                    var tier = node.ChildNodes[1];
                    foreach (var grupo in tier.ChildNodes)
                    {
                        if (grupo.GetClasses().Contains("custom-tier-burst"))
                        {
                            foreach (var personaje in grupo.ChildNodes)
                            {
                                var a = personaje.ChildNodes[0].ChildNodes[0].ChildNodes[0];
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

            var modelo = new TierList
            {
                Tiers = tiers,
                CharacterImg = characterImg,
                CharactersUrl = characterUrl,
                Date = DateTime.Today,
            };

            string jsonString = JsonSerializer.Serialize(modelo);
            System.IO.File.WriteAllText("honkaiStarRail.json", jsonString);

            return modelo;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
