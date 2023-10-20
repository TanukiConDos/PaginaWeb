using HtmlAgilityPack;
using System.Text.Json;


namespace PaginaWeb.Models
{
    public class PriconneTierList: TierList
    {

        public override void Update()
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
                if (node.FirstChild.Name == "th")
                {
                    if (tier != -1)
                    {
                        tiers.Add(names);
                        names = new();
                    }
                    tier++;

                }
                else
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
            }
            tiers.Add(names);

            Tiers = tiers;
            CharacterImg = characterImg;
            CharactersUrl = characterUrl;
            Date = DateTime.Today;

            string jsonString = JsonSerializer.Serialize(this);
            System.IO.File.WriteAllText("pricone.json", jsonString);
        }
    }
}
