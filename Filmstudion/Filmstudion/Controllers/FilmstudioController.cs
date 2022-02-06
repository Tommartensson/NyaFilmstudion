using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmstudioController : Controller
    {
        private readonly IFilmStudioRepository _repository;
        private readonly LinkGenerator _link;
        public FilmstudioController(IFilmStudioRepository repository, LinkGenerator link)
        {
            _link = link;
            _repository = repository;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<FilmStudio>>> Get()
        {
            try
            {
                var result = await _repository.Get();

                return Ok(result);
            }
            catch(Exception err)
            {
                return BadRequest(err);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            try
            {
                var result = await _repository.GetById(id);

                return Ok(result);
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }


        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<FilmStudio>> Post([FromBody] RegisterFilmStudio model)
        {
            var location = _link.GetPathByAction("Get", "FilmStudio", new { Name = model.FilmStudioName });
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest("Kunde inte använda detta namnet");
            }
            var newModel = await _repository.Create(model);
            return Created("", newModel);
        }
    }
}
