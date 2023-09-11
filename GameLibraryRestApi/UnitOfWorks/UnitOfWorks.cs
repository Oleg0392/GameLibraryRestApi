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

        public async Task<Game?> InsertNewGame(string Name, string Genres, string Developer)
        {
            if (String.IsNullOrEmpty(Name)) return null;

            string[] genres = new string[0];

            if (!String.IsNullOrEmpty(Genres)) genres = Genres.Split(',');

            if (String.IsNullOrEmpty(Developer)) Developer = String.Empty;

            var gameObj = gameRepository.FindFirstWhereAsync(game => game.Name.Equals(Name)).Result;
            if (gameObj != null) return null;   // игра с таким именем уже существует

            gameObj = new Game();
            gameObj.Name = Name;

            var refs = new List<GenreRef>();
            foreach (var genre in genres)
            {
                var genreObj = genreRepository.FindFirstWhereAsync(genre => genre.Name.Equals(genre)).Result;
                if (genreObj == null)
                {
                    genreObj = genreRepository.InsertAsync(genreObj).Result;
                }
                refs.Add(new GenreRef() { GameId = gameObj.Id, GenreId = genreObj.Id });
            }

            if (refs.Count > 0) await genreRefRepository.InsertRangeAsync(refs);
                       
            var developer = developerRepository.FindFirstWhereAsync(dev => dev.Name.Equals(Developer)).Result;
            if (developer == null)
            {
                developer = developerRepository.InsertAsync(developer).Result;
            }

            gameObj.Developer = developer.Id;

            gameObj = gameRepository.InsertAsync(gameObj).Result;

            return gameObj;
        }    
    }
}
