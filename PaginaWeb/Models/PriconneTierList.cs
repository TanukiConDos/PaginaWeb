using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;


namespace PaginaWeb.Models
{
    public class PriconneTierList: TierList
    {
        public PriconneTierList() : base("Priconne")
        {
        }

        protected override void Update(Game game)
        {
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
                        var cAux = tierListsDb.Characters.Find(c.name);
                        if (cAux == null) tierListsDb.Characters.Add(c);
                        else cAux = c;
                        tierListsDb.SaveChanges();
                    }
                }
            }
            game.numTiers = tier;
            game.date = DateTime.Now;


            tierListsDb.Update(game);

            tierListsDb.SaveChanges();
        }
    }
}
