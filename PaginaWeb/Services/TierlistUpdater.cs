using HtmlAgilityPack;
using PaginaWeb.Models;

namespace PaginaWeb.Services
{
    public class TierlistUpdater
    {
        public void updateHonkaiStarRail(TierListsDbContext dbContext)
        {
            var gameName = "Honkai Star Rail";
            var game = dbContext.Games.First(g => g.name == gameName);
            HtmlWeb request = new();
            var html = request.Load("https://www.prydwen.gg/star-rail/tier-list/");
            var table = html.DocumentNode.SelectSingleNode("//div[@class=\"custom-tier-list-hsr\"]");
            int numTier = -1;

            foreach (var node in table.ChildNodes)
            {
                if (node.GetAttributeValue("class", "").Contains("custom-tier"))
                {
                    var tier = node.ChildNodes[1];
                    foreach (var group in tier.ChildNodes)
                    {
                        if (group.GetClasses().Contains("custom-tier-burst"))
                        {
                            foreach (var character in group.ChildNodes)
                            {
                                var a = character.SelectSingleNode("./div/span/a");

                                var url = "https://www.prydwen.gg" + a.GetAttributeValue("href", "");
                                var img = "https://www.prydwen.gg" + a.ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[1].GetAttributeValue("data-src", "");
                                var name = a.GetAttributeValue("href", "").Split('/').Last();
                                Character c = new()
                                {
                                    gameName = gameName,
                                    name = name,
                                    url = url,
                                    img = img,
                                    game = game,
                                    tier = numTier
                                };
                                var cAux = dbContext.Characters.Find(c.name);
                                if (cAux == null) dbContext.Characters.Add(c);
                                else cAux = c;
                                dbContext.SaveChanges();

                            }
                        }
                    }
                    numTier++;
                }

            }

            game.numTiers = numTier;
            game.date = DateTime.Now;


            dbContext.Update(game);

            dbContext.SaveChanges();

        }

        public void updatePriconne(TierListsDbContext dbContext)
        {
            var gameName = "Priconne";
            var game = dbContext.Games.First(g => g.name == gameName);

            HtmlWeb request = new HtmlWeb();
            var html = request.Load("https://gamewith.jp/pricone-re/article/show/93068");
            var table = html.DocumentNode.SelectSingleNode("//div[@class=\"puri_5col-table\"]");
            int tier = -1;

            foreach (var node in table.SelectNodes("./table/tr"))
            {
                if (node.FirstChild.Name == "th")
                {
                    tier++;
                }
                else
                {

                    foreach (var td in node.ChildNodes)
                    {
                        if (td.FirstChild == null) continue;
                        var url = td.FirstChild.GetAttributeValue("href", "");
                        var name = td.FirstChild.GetDirectInnerText();
                        var img = td.FirstChild.ChildNodes[0].GetAttributeValue("data-original", "");
                        Character c = new()
                        {
                            gameName = gameName,
                            name = name,
                            url = url,
                            img = img,
                            game = game,
                            tier = tier
                        };
                        var cAux = dbContext.Characters.Find(c.name);
                        if (cAux == null) dbContext.Characters.Add(c);
                        else cAux = c;
                        dbContext.SaveChanges();
                    }
                }
            }
            game.numTiers = tier;
            game.date = DateTime.Now;


            dbContext.Update(game);

            dbContext.SaveChanges();
        }

        public void UpdateNikke(TierListsDbContext dbContext)
        {
            var gameName = "NIKKE";
            var game = dbContext.Games.First(g => g.name == gameName);

            HtmlWeb request = new();
            var html = request.Load("https://www.prydwen.gg/nikke/tier-list/");
            var div = html.DocumentNode.SelectSingleNode("//div[@class=\"custom-tier-list-nikke\"]");
            int numTier = 0;
            foreach (var tier in div.ChildNodes)
            {
                if (!tier.GetAttributeValue("class", "").Contains("custom-tier tier-")) continue;
                foreach (var burstGroup in tier.ChildNodes[1].ChildNodes)
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
                        var cAux = dbContext.Characters.Find(c.name);
                        if (cAux == null) dbContext.Characters.Add(c);
                        else cAux = c;
                        dbContext.SaveChanges();
                    }
                }
                numTier++;
            }
            game.numTiers = numTier;
            game.date = DateTime.Now;


            dbContext.Update(game);

            dbContext.SaveChanges();
        }
    }
}
