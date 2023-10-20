namespace PaginaWeb.Models
{
    public abstract class TierList
    {
        public List<List<string>> Tiers { get; set; } = new();
        public Dictionary<string, string> CharactersUrl { get; set; } = new();
        public Dictionary<string, string> CharacterImg { get; set; } = new();
        public DateTime? Date { get; set; }

        public abstract void Update();
    }

    
}
