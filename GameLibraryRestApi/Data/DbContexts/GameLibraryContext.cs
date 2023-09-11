using Microsoft.EntityFrameworkCore;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Data.DbContexts
{
    public class GameLibraryContext : DbContext
    {
        public GameLibraryContext(DbContextOptions<GameLibraryContext> options) : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreRef> GenreRefs { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }

    }
}
