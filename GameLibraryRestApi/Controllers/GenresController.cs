using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.UnitOfWorks;

namespace GameLibraryRestApi.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWorks unitOfWorks;


        public GenresController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGenres()
        {
            return Ok( await unitOfWorks.Genres.GetAllAsync());
        }

        [HttpGet]
        [Route("allrefs")]
        public async Task<IActionResult> GetAllGenreRefs()
        {
            return Ok(await unitOfWorks.GenreRefs.GetAllAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetGenreByName(string name)
        {
            if (ModelState.IsValid)
            {
                return Ok(await unitOfWorks.Genres.FindAllByWhereAsync(g => g.Name.StartsWith(name)));
            }
            else return BadRequest();
        }

        [HttpPut]
        [Route("add&{name}")]
        public async Task<IActionResult> AddNewGenre(string name)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre();
                genre.Name = name;
                await unitOfWorks.Genres.InsertAsync(genre);
                return Ok(genre);
            }
            else return BadRequest();
        }

        [HttpPut]
        [Route("addref&{game}&{genre}")]
        public async Task<IActionResult> AddNewGenreReference(string game, string genre)
        {
            if (ModelState.IsValid)
            {
                return Ok(await unitOfWorks.InsertNewGenreReference(game, genre));
            }
            else return BadRequest();
        }
    }
}
