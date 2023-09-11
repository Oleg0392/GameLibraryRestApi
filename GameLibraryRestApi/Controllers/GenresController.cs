using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;

namespace GameLibraryRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGenres()
        {
            return Ok( await genreRepository.GetAllAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetGenreByName(string name)
        {
            if (ModelState.IsValid)
            {
                return Ok(await genreRepository.FindAllByWhereAsync(g => g.Name.StartsWith(name)));
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
                await genreRepository.InsertAsync(genre);
                return Ok(genre);
            }
            else return BadRequest();
        }
    }
}
