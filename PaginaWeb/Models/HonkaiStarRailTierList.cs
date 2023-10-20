using HtmlAgilityPack;
using System.Text.Json;

namespace PaginaWeb.Models
{
    public class HonkaiStarRailTierList: TierList
    {
        public override void Update()
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
                                var div = a.ChildNodes[0].ChildNodes[0];
                                string name = "";
                                foreach (var child in div.ChildNodes)
                                {
                                    if (child.Name == "noscript")
                                    {
                                        HtmlDocument innerHtml = new();
                                        innerHtml.LoadHtml(child.InnerHtml);
                                        name = innerHtml.DocumentNode.SelectSingleNode("//picture/img").GetAttributeValue("alt", "");
                                    }
                                }
                                var url = "https://www.prydwen.gg" + a.GetAttributeValue("href", "");
                                var img = "https://www.prydwen.gg" + a.ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[1].GetAttributeValue("data-src", "");

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
            Tiers = tiers;
            CharacterImg = characterImg;
            CharactersUrl = characterUrl;
            Date = DateTime.Today;

            string jsonString = JsonSerializer.Serialize(this);
            System.IO.File.WriteAllText("honkaiStarRail.json", jsonString);
        }
    }
}
