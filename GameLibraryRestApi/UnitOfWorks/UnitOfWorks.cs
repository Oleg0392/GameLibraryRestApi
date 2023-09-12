using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.Models;

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
                refs.Add(new GenreRef() { GameId = 0, GenreId = genreObj.Id });
            }
                                  
            var developerObj = developerRepository.FindFirstWhereAsync(dev => dev.Name.ToUpper().Equals(DeveloperArg.ToUpper())).Result;  
            if (developerObj == null)
            {
                developerObj = new Developer() { Name = DeveloperArg };
                developerObj = developerRepository.InsertAsync(developerObj).Result;  // аналогично с разрабами, если нет такого, вставляем
            }

            gameObj.Developer = developerObj.Id;    // привязка разраба к игре
            gameObj = gameRepository.InsertAsync(gameObj).Result;   // вставка самой игры

            if (refs.Count > 0)
            {
                for (int i = 0; i < refs.Count; i++) { refs[i].GameId = gameObj.Id; }
                await genreRefRepository.InsertRangeAsync(refs);   // вставляем ссылки
            }

            return gameObj;
        }
        
        public async Task<Game?> DeleteGameAsync(Game gameToDelete)
        {
            if (gameToDelete == null) return null;

            gameToDelete = await gameRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(gameToDelete.Name.ToUpper()));
            if (gameToDelete == null) return null;

            await genreRefRepository.DeleteDependenсies(gameToDelete.Id);
            return await gameRepository.DeleteAsync(gameToDelete);
        }

        public async Task<List<GameModel>?> GetGamesByGenres(string genresArg)
        {
            if (String.IsNullOrEmpty(genresArg)) return null;

            genresArg = genresArg.Trim();
            string[] genresStrArray = genresArg.Split(',');
            List<Genre> Genres = new List<Genre>();

            foreach (string genresStr in genresStrArray)
            {
                var genre = genreRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(genresStr.ToUpper())).Result;
                if (genre == null) continue;
                Genres.Add(genre);
            }

            if (Genres.Count == 0) return null;

            List<int> GameIds = new List<int>();
            foreach (var genre in Genres)
            {
                var genreRefs = genreRefRepository.FindAllByIdAsync(genre.Id).Result;
                if (genreRefs == null) continue;
                foreach (var genreRef in genreRefs)
                {
                    var sameIds = GameIds.Where(item => item == genreRef.GameId);
                    if (sameIds.Any()) continue;
                    GameIds.Add(genreRef.GameId);
                }
            }

            if (!GameIds.Any()) return null;

            List<Game> Games = new List<Game>();
            foreach (var gameId in GameIds)
            {
                var game = gameRepository.FindFirstWhereAsync(g => g.Id == gameId).Result;
                if (game == null) continue;
                Games.Add(game);
            }

            if (!Games.Any()) return null;

            List<GameModel> gameModels = ConvertToGameModels(Games);
                     
            return gameModels;
        }

        public async Task<List<GameModel>?> GetGamesByDevelopers(string devsArg)
        {
            if (String.IsNullOrEmpty(devsArg)) return null;

            devsArg = devsArg.Trim();
            string[] devsStrArray = devsArg.Split(',');
            List<Developer> Developers = new List<Developer>();

            foreach (string devStr in devsStrArray)
            {
                var dev = developerRepository.FindFirstWhereAsync(d => d.Name.ToUpper().Equals(devStr.ToUpper())).Result;
                if (dev == null) continue;
                Developers.Add(dev);
            }

            if (Developers.Count == 0) return null;

            List<Game> Games = new List<Game>();
            foreach (var develop in Developers)
            {
                var games = gameRepository.FindAllByWhereAsync(g => g.Developer == develop.Id).Result;
                if (games == null) continue;
                foreach (var game in games)
                {
                    var sameIds = Games.Where(g => g.Id == game.Id);
                    if (sameIds.Any()) continue;
                    Games.Add(game);
                }
            }

            List<GameModel> gameModels = ConvertToGameModels(Games);

            return gameModels;
        }

        public async Task<GameModel?> EditGameById(int id, string name, string desc, string developer)
        {
            var oldGame = gameRepository.FindFirstWhereAsync(g => g.Id == id).Result;
            if (oldGame == null) return null;

            var newGame = new Game();
            newGame.Id = id;
            newGame.Name = name == "null" ? oldGame.Name : name;
            newGame.Description = desc == "null" ? oldGame.Description : desc;

            if (developer != "null")
            {
                var newDeveloper = developerRepository.FindFirstWhereAsync(d => d.Name.ToUpper().Equals(developer.ToUpper())).Result;
                if (newDeveloper == null)
                {
                    newDeveloper = new Developer() { Name = developer };
                    newDeveloper = developerRepository.InsertAsync(newDeveloper).Result;
                }
                newGame.Developer = newDeveloper.Id;
            }
            else newGame.Developer = oldGame.Developer;

            newGame = gameRepository.UpdateAsync(newGame).Result;
            List<Game> games = new List<Game>();
            games.Add(newGame);
            List<GameModel> gameModels = ConvertToGameModels(games);

            return gameModels[0];
        }

        private List<GameModel> ConvertToGameModels(List<Game> Games)
        {
            List<GameModel> gameModels = new List<GameModel>();
            int modelIndex = 0;
            foreach (var game in Games)
            {
                gameModels.Add(new GameModel()
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description
                });

                var genreList = genreRefRepository.FindAllByGameIdAsync(game.Id).Result;
                gameModels[modelIndex].Genres = new string[genreList.Count];
                int genreIndex = 0;
                foreach (var genr in genreList)
                {
                    var genre = genreRepository.FindFirstWhereAsync(g => g.Id == genr.GenreId).Result;
                    if (genre != null) gameModels[modelIndex].Genres[genreIndex] = genre.Name;
                    genreIndex++;
                }

                var developer = developerRepository.FindFirstWhereAsync(d => d.Id == game.Developer).Result;
                if (developer != null) gameModels[modelIndex].Developer = developer.Name;

                modelIndex++;
            }

            return gameModels;
        }

        public async Task<GenreRef?> InsertNewGenreReference(string gameName, string genreName)
        {
            var game = gameRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(gameName.ToUpper())).Result;
            if (game == null) return null;
            var genre = genreRepository.FindFirstWhereAsync(g => g.Name.ToUpper().Equals(genreName.ToUpper())).Result;
            if (genre == null)
            {
                genre = new Genre() { Name = genreName };
                genre = genreRepository.InsertAsync(genre).Result;
            }
            var genreRefs = new List<GenreRef> (1) { new GenreRef() { GameId = game.Id, GenreId = genre.Id } };
            var result = genreRefRepository.InsertRangeAsync(genreRefs).Result;

            return result[0];
        }

        public async Task<List<GameModel>> GetAllGames()
        {
            var games = gameRepository.GetAllAsync().Result;
            return ConvertToGameModels(games);
        }

    }
}
