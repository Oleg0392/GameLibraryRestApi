using Microsoft.AspNetCore.Mvc;
using GameLibraryRestApi.Repositories.Interfaces;
using System.Linq.Expressions;

namespace GameLibraryRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperRepository devRepository;

        public DeveloperController(IDeveloperRepository devRepository)
        {
            this.devRepository = devRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllDevs()
        {
            return Ok(await devRepository.GetAllAsync());
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetDevByName(string name)
        {
            if (ModelState.IsValid)
            {
                return Ok(await devRepository.FindAllByWhereAsync(d => d.Name.StartsWith(name)));
            }
            else return BadRequest();
        }
    }
}
