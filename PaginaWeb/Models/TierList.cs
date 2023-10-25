using Microsoft.EntityFrameworkCore;

namespace PaginaWeb.Models
{
    public abstract class TierList
    {
        protected string gameName;

        public TierList(string gameName)
        {
            //tierListsDb.Database.EnsureDeleted();
            tierListsDb.Database.EnsureCreated();
            this.gameName = gameName;
            var game = tierListsDb.Games.Include(g => g.characters).Where(g => g.name == gameName).FirstOrDefault();

            if (game == null)
            {
                game = new Game
                {
                    name = gameName
                };
                Update(game);
            }

            if (DateTime.Today - game.date > TimeSpan.FromDays(15))
            {
                Update(game);
            }

            for (int i = 0; i < game.numTiers; i++)
            {
                var tier = game.characters.Where(c => c.gameName == game.name).Where(c => c.tier == i).ToList();
                Tiers.Add(tier);
            }

        }

        protected abstract void Update(Game game);

        protected TierListsDbContext tierListsDb = new();
        public List<List<Character>> Tiers { get; set; } = new();
    }

    public class TierListsDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=.\\Data\\SQlLiteDatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasMany<Character>(g => g.characters).WithOne(c => c.game).HasForeignKey(c => c.gameName).IsRequired();
            //modelBuilder.Entity<Character>().HasOne<Game>(c => c.game).WithMany(g => g.characters).IsRequired();
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Character>().ToTable("character");
        }
    }
}
