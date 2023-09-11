using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.Repositories.Interfaces;

namespace GameLibraryRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository gameRepository;

        public GamesController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await gameRepository.GetAllAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetGamesByName(string name)
        {
            if (ModelState.IsValid && name != null)
            {
                return Ok(await gameRepository.FindAllByWhereAsync(game => game.Name.StartsWith(name)));
            }
            else return BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            if (ModelState.IsValid)
            {
                return Ok(await gameRepository.FindFirstWhereAsync(game => game.Id == id));
            }
            else return BadRequest();
        }

        [HttpPut]
        [Route("{genres[]}")]
        public IActionResult AddNewGame(string[] genres)
        {
            return Ok(genres);
        }

    }
}
