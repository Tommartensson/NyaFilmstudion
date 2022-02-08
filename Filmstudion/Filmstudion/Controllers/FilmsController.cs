using AutoMapper;
using filmstudion.api.Models;
using Filmstudion.Models.Film;
using Filmstudion.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FilmsController : Controller
    {

        private readonly IFilmRepository _repository;
        private readonly LinkGenerator _link;
        private readonly IMapper _mapper;
        public FilmsController(IFilmRepository repository, LinkGenerator link, IMapper mapper)
        {
            _link = link;
            _repository = repository;
            _mapper = mapper;
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

        //Add a film
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<CreateFilm>> AddAFilm([FromBody] CreateFilm Film)
        {
            try
            {
               
                    var location = _link.GetPathByAction("Get", "Films", new { name = Film.Name });
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Couldnt use current name");
                    }

                    var newMovie = await _repository.Create(Film);
                    return Created("", newMovie);
               
            }
            catch(Exception err)
            {
                return BadRequest(err);
            }
        }



        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Film>> Put(int id, [FromBody] Film movie)
        {
            try
            {
                var oldMovie = await _repository.GetById(id);
                if (oldMovie == null) NotFound("Couldnt not find");


                if (await _repository.SaveChangesAsync())
                {
                    var newMovie = _mapper.Map<Film>(oldMovie);
                    return Ok(newMovie);
                }
            }
            catch
            {
                return BadRequest("DataBase Failure");
            }
            return BadRequest();
        }
    }
}
