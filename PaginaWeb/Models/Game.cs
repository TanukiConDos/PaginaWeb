using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PaginaWeb.Models
{
    public class Game
    {
        [Key]
        public string name { get; set; }
        public int numTiers { get; set; }

        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        public ICollection<Character> characters { get; set; }

    }

}
