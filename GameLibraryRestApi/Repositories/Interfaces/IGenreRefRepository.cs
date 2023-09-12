using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Repositories.Interfaces
{
    public interface IGenreRefRepository : IEFCoreRepository<GenreRef>
    {
        Task<int> DeleteDependenсies(int gameToDeleteId);
        Task<List<GenreRef>> FindAllByIdAsync(int genreId);
        Task<List<GenreRef>> FindAllByGameIdAsync(int genreId);
    }
}
