using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDeveloperRepository developerRepository;
        private readonly IGenreRefRepository genreRefRepository;

        public UnitOfWorks(IGameRepository gameRepository, IGenreRepository genreRepository, IDeveloperRepository developerRepository, IGenreRefRepository genreRefRepository)
        {
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
            this.developerRepository = developerRepository;
            this.genreRefRepository = genreRefRepository;
        }

        public IGameRepository Games => gameRepository;
        public IGenreRepository Genres => genreRepository;
        public IDeveloperRepository Developers => developerRepository;
        public IGenreRefRepository GenreRefs => genreRefRepository;

        public async Task<Game?> InsertNewGame(string NameArg, string GenresArg, string DeveloperArg)
        {
            if (String.IsNullOrEmpty(NameArg)) return null;

            string[] genres = new string[0];

            if (!String.IsNullOrEmpty(GenresArg)) genres = GenresArg.Split(',').ToArray();
            if (String.IsNullOrEmpty(DeveloperArg)) return null;  // т.к. DeveloperId required

            var gameObj = gameRepository.FindFirstWhereAsync(game => game.Name.ToUpper().Equals(NameArg.ToUpper())).Result;
            if (gameObj != null) return null;   // игра с таким именем уже существует
            gameObj = new Game() { Name = NameArg };

            var refs = new List<GenreRef>();    // создаем ссылки для таблицы соответствий GenreRef (GameId, GenreId)
            foreach (var genre in genres)
            {
                var genreObj = genreRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(genre.ToUpper())).Result;
                if (genreObj == null)
                {
                    genreObj = new Genre() { Name = genre };
                    genreObj = genreRepository.InsertAsync(genreObj).Result;   // если нет такого жанра
                }
                refs.Add(new GenreRef() { GameId = gameObj.Id, GenreId = genreObj.Id });
            }

            if (refs.Count > 0) await genreRefRepository.InsertRangeAsync(refs);   // вставляем ссылки
                       
            var developerObj = developerRepository.FindFirstWhereAsync(dev => dev.Name.ToUpper().Equals(DeveloperArg.ToUpper())).Result;  
            if (developerObj == null)
            {
                developerObj = new Developer() { Name = DeveloperArg };
                developerObj = developerRepository.InsertAsync(developerObj).Result;  // аналогично с разрабами, если нет такого, вставляем
            }

            gameObj.Developer = developerObj.Id;    // привязка разраба к игре
            gameObj = gameRepository.InsertAsync(gameObj).Result;   // вставка самой игры

            return gameObj;
        }
        
        public async Task<Game?> DeleteGameAsync(Game gameToDelete)
        {
            if (gameToDelete == null) return null;

            gameToDelete = await gameRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(gameToDelete.Name.ToUpper()));
            if (gameToDelete == null) return null;

            await genreRefRepository.DeleteDependenсies(gameToDelete);
            return await gameRepository.DeleteAsync(gameToDelete);
        }
    }
}
