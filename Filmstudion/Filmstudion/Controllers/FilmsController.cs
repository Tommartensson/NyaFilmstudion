using Filmstudion.Models.Film;
using Filmstudion.Repositories;
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
    public class FilmsController : Controller
    {

        private readonly IFilmRepository _repository;
        private readonly LinkGenerator _link;
        public FilmsController(IFilmRepository repository, LinkGenerator link)
        {
            _link = link;
            _repository = repository;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Film>>> Get()
        {
            try
            {
                var result = await _repository.Get();

                return Ok(result);
            }
            catch (Exception err)
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
    }
}
