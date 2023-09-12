using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.UnitOfWorks;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWorks unitOfWorks;

        public GamesController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await unitOfWorks.GetAllGames());
        }

        [HttpGet]
        [Route("genre&{names}")]
        public async Task<IActionResult> GetGamesByGenre(string names)
        {
            if (ModelState.IsValid && names != null)
            {
                return Ok(await unitOfWorks.GetGamesByGenres(names));
            }
            else return BadRequest();
        }

        [HttpGet]
        [Route("developers&{names}")]
        public async Task<IActionResult> GetGamesByDev(string names)
        {
            if (ModelState.IsValid && names != null)
            {
                return Ok(await unitOfWorks.GetGamesByDevelopers(names));
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

        [HttpPost]
        [Route("edit{id}&{name}&{desc}&{developer}")]
        public async Task<IActionResult> EditGame(int id, string name = "null", string desc = "null", string developer = "null")
        {
            if (ModelState.IsValid)
            {
                return Ok(await unitOfWorks.EditGameById(id, name, desc, developer));  // сюда метод ещё разрабатывается
            }
            else return BadRequest();
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

        [HttpDelete]
        [Route("gameref{id}")]
        public async Task<IActionResult> DelGameReference(int id)
        {
            if (ModelState.IsValid)
            {
                return Ok(await unitOfWorks.GenreRefs.DeleteDependenсies(id));
            }
            else return BadRequest();
        }

    }
}
