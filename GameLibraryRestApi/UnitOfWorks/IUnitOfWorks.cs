using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.Models;

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
        Task<List<GameModel>?> GetGamesByGenres(string genresArg);
        Task<List<GameModel>?> GetGamesByDevelopers(string devsArg);
        Task<GameModel?> EditGameById(int id, string name, string desc, string developer);
        Task<GenreRef?> InsertNewGenreReference(string gameName, string genreName);
        Task<List<GameModel>> GetAllGames();
    }
}
