using HtmlAgilityPack;

namespace PaginaWeb.Models
{
    public class HonkaiStarRailTierList: TierList
    {
        public HonkaiStarRailTierList() : base("Honkai Star Rail")
        {
        }

        protected override void Update(Game game)
        {
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
                                var cAux = tierListsDb.Characters.Find(c.name);
                                if (cAux == null) tierListsDb.Characters.Add(c);
                                else cAux = c;
                                tierListsDb.SaveChanges();

                            }
                        }
                    }
                    numTier++;
                }
                
            }

            game.numTiers = numTier;
            game.date = DateTime.Now;
            

            tierListsDb.Update(game);
            
            tierListsDb.SaveChanges();
            
        }
    }
}
