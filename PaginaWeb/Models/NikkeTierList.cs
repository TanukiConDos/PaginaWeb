using HtmlAgilityPack;
using System.Xml.Linq;

namespace PaginaWeb.Models
{
    public class NikkeTierList : TierList
    {
        public NikkeTierList(): base("Nikke") { }

        protected override void Update(Game game)
        {
            HtmlWeb request = new();
            var html = request.Load("https://www.prydwen.gg/nikke/tier-list/");
            var div = html.DocumentNode.SelectSingleNode("//div[@class=\"custom-tier-list-nikke\"]");
            int numTier = 0;
            foreach ( var tier in div.ChildNodes )
            {
                if (!tier.GetAttributeValue("class", "").Contains("custom-tier tier-")) continue;
                foreach( var burstGroup in tier.ChildNodes[1].ChildNodes)
                {
                    if (!burstGroup.GetAttributeValue("class", "").Contains("custom-tier-burst burst-")) continue;
                    foreach (var a in burstGroup.SelectNodes("./span/div/span/a"))
                    {
                        var url = "https://www.prydwen.gg" + a.GetAttributeValue("href", "");
                        var img = "https://www.prydwen.gg" + a.SelectSingleNode("./div/div/picture/img").GetAttributeValue("data-src", "");
                        var name = a.GetAttributeValue("href", "").Split("/").Last();
                        Character c = new()
                        {
                            gameName = gameName,
                            name = name,
                            url = url,
                            img = img,
                            game = game,
                            tier = numTier
                        };
                        tierListsDb.Characters.Add(c);
                        tierListsDb.SaveChanges();
                    }
                }
                numTier++;
            }
            game.numTiers = numTier;
            game.date = DateTime.Now;


            tierListsDb.Update(game);

            tierListsDb.SaveChanges();
        }
    }
}
