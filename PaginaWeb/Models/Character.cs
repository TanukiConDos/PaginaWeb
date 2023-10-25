using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaginaWeb.Models
{
    public class Character
    {
        public string gameName { get; set; }

        public Game game { get; set; }
        public int tier { get; set; }

        [Key]
        public string name { get; set; }
        public string url { get; set; }
        public string img { get; set; }
    }
}
