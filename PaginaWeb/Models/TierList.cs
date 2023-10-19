namespace PaginaWeb.Models
{
    public class TierList
    {
        public List<List<string>> Tiers { get; set; } = new();
        public Dictionary<string, string>? CharactersUrl { get; set; }
        public Dictionary<string, string>? CharacterImg { get; set; }
        public DateTime? Date { get; set; }

    }
}
