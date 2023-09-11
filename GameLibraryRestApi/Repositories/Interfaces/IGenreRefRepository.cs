using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Repositories.Interfaces
{
    public interface IGenreRefRepository : IEFCoreRepository<GenreRef>
    {
        public async Task<int> DeleteDependenсies(Game gameToDelete);
    }
}
