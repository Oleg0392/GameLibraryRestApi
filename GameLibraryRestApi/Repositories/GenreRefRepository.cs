using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.DbContexts;

namespace GameLibraryRestApi.Repositories
{
    public class GenreRefRepository : EFCoreRepository<GenreRef>, IGenreRefRepository
    {
        private readonly GameLibraryContext _context;
        
        public GenreRefRepository(GameLibraryContext context) : base(context)
        {
            _context = context;
        }
    }
}
