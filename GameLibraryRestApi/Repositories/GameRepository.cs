using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.DbContexts;

namespace GameLibraryRestApi.Repositories
{
    public class GameRepository : EFCoreRepository<Game>, IGameRepository
    {
        private readonly GameLibraryContext _context;
        
        public GameRepository(GameLibraryContext context) :base(context)
        {
            _context = context;
        }
    }
}
