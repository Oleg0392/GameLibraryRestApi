using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.DbContexts;

namespace GameLibraryRestApi.Repositories
{
    public class DeveloperRepository : EFCoreRepository<Developer>, IDeveloperRepository
    {
        private readonly GameLibraryContext _context;

        public DeveloperRepository(GameLibraryContext context) :base(context)
        {
            _context = context;
        }
    }
}
