using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.UnitOfWorks;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        //private readonly IGameRepository gameRepository;
        private readonly IUnitOfWorks unitOfWorks;

        public GamesController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await unitOfWorks.Games.GetAllAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetGamesByName(string name)
        {
            if (ModelState.IsValid && name != null)
            {
                return Ok(await unitOfWorks.Games.FindAllByWhereAsync(game => game.Name.StartsWith(name)));
            }
            else return BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            if (ModelState.IsValid)
            {
                return Ok(await unitOfWorks.Games.FindFirstWhereAsync(game => game.Id == id));
            }
            else return BadRequest();
        }

        [HttpPut]
        [Route("{name}&{genres}&{developer}")]
        public async Task<IActionResult> AddNewGame(string name, string genres, string developer)
        {
            
            return Ok(await unitOfWorks.InsertNewGame(name, genres, developer));
        }

        [HttpDelete]
        [Route("{name}")]
        public async Task<IActionResult> DelGameByName(string name)
        {
            if (ModelState.IsValid)
            {
                var game = new Game() { Name = name };
                return Ok(await unitOfWorks.DeleteGameAsync(game));
            }
            else return BadRequest();
        }

    }
}
