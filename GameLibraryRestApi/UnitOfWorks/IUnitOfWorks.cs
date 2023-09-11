using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.UnitOfWorks
{
    public interface IUnitOfWorks
    {
        IGameRepository Games { get; }
        IGenreRepository Genres { get; }
        IDeveloperRepository Developers { get; }
        IGenreRefRepository GenreRefs { get; }

        Task<Game?> InsertNewGame(string name, string genres, string developer);
        Task<Game?> DeleteGameAsync(Game gameToDelete);
    }
}
