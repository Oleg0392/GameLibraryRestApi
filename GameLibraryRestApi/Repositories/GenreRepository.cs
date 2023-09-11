using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.DbContexts;

namespace GameLibraryRestApi.Repositories
{
    public class GenreRepository : EFCoreRepository<Genre>, IGenreRepository
    {
        private readonly GameLibraryContext _context;

        public GenreRepository(GameLibraryContext context) :base(context)
        {
            _context = context;
        }
    }
}
