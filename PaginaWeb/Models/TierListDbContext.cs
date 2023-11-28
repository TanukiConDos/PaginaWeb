using Microsoft.EntityFrameworkCore;

namespace PaginaWeb.Models
{
    public class TierListsDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Character> Characters { get; set; }

        public TierListsDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasMany<Character>(g => g.characters).WithOne(c => c.game).HasForeignKey(c => c.gameName).IsRequired();
            //modelBuilder.Entity<Character>().HasOne<Game>(c => c.game).WithMany(g => g.characters).IsRequired();
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Character>().ToTable("character");
        }
    }
}
