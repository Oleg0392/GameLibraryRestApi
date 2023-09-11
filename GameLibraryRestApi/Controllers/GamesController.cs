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
        public IActionResult GetAllGames()
        {
            return Ok("All");
        }

        [HttpGet]
        [Route("own")]
        public IActionResult GetOwnGames()
        {
            return Ok("Own");
        }

    }
}
