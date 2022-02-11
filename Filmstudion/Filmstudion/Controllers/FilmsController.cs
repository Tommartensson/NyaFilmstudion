using AutoMapper;
using filmstudion.api.Models;
using Filmstudion.Models.Authentication;
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
        private readonly IFilmStudioRepository _studio;
        public FilmsController(IFilmRepository repository, LinkGenerator link, IMapper mapper, IFilmStudioRepository studio)
        {
            _link = link;
            _repository = repository;
            _mapper = mapper;
            _studio = studio;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Films>>> Get()
        {
            try
            {
                var result = await _repository.Get();
                if (User.IsInRole("Admin") || User.IsInRole("FilmStudio"))
                {
                    return Ok(result);
                }
                else
                {
                    var allFilmsWithoutLoan = result.Select(n =>
                    {
                        var newFilm = _mapper.Map<Films, FilmAsUnauthorized>(n);
                        return newFilm;
                    });

                    return Ok(allFilmsWithoutLoan);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            try
            {
                var result = await _repository.GetById(id);
                if (User.IsInRole("Admin") || User.IsInRole("FilmStudio"))
                {
                    return Ok(result);
                }
                else
                {
                    var newFilm = _mapper.Map<Films, FilmAsUnauthorized>(result);
                    return Ok(newFilm);
                }

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
                return Ok(newMovie);

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }


        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Films>> Patch(int id, [FromBody] Films movie)
        {
            try
            {
                var oldMovie = await _repository.GetById(id);
                if (oldMovie == null) return NotFound("Couldnt not find");

               
                _mapper.Map(movie, oldMovie);
                oldMovie.FilmId = id;
                if (await _repository.SaveChangesAsync())
                {

                    return Ok(oldMovie);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        
        public async Task<ActionResult<Films>> Update(int id, [FromBody] numberOfCopies numberOfCopies)
        {
            try
            {
                var oldMovie = await _repository.GetById(id);
                if (oldMovie == null) return NotFound("Couldnt not find");


                oldMovie.NumberOfCopies = numberOfCopies.NumberOfCopies;
                if (await _repository.SaveChangesAsync())
                {

                    return Ok(oldMovie);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("rent")]
        public async Task<ActionResult<Films>> LoanFilm([FromQuery]int id, int StudioId)
        {
            try{
                var film = await _repository.GetById(id);
                var filmStudio = await _studio.GetById(StudioId);
               
                if(film == null)
                {
                    return Conflict("This film does not exist");
                }
                if (film.NumberOfCopies == 0)
                {
                    return Conflict("We have no copies atm");
                }
                if (filmStudio == null)
                {
                    return Conflict("This Filmstudio does not exist");
                }
                if (StudioId == int.Parse(User.Claims.Where(n => n.Type == "userId").FirstOrDefault()?.Value))
                {
                    film.NumberOfCopies = film.NumberOfCopies - 1;
                    await _repository.AddLoan(film, filmStudio);
                    return Ok();
                }
                else
                {
                    return Conflict("You are not authorized to loan as someone else");
                }
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("return")]
        public async Task<ActionResult<Films>> GiveBackFilm([FromQuery] int id, int StudioId)
        {
            try
            {
                var film = await _repository.GetById(id);
                var filmStudio = await _studio.GetById(StudioId);
                if (film == null)
                {
                    return Conflict("This film does not exist");
                }
                if (filmStudio == null)
                {
                    return Conflict("This Filmstudio does not exist");
                }
                if (StudioId == int.Parse(User.Claims.Where(n => n.Type == "userId").FirstOrDefault()?.Value))
                {
                    film.NumberOfCopies = film.NumberOfCopies + 1;
                    await _repository.RemoveLoan(film, filmStudio);
                    return Ok();
                }
                else
                {
                    return Conflict("You are not authorized to give back a film as someone else");
                }
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }

        }
    }
}
