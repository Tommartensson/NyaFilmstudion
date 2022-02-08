using AutoMapper;
using Filmstudion.Models.Authentication;
using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;
        public FilmstudioController(IFilmStudioRepository repository, LinkGenerator link, UserManager<User> user, IMapper mapper)
        {
            _link = link;
            _repository = repository;
            _user = user;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<FilmStudio>>> Get()
        {
            try
            {
                var result = await _repository.Get();
                var allStudiosWithoutLoan = result.Select(n =>
                {
                    var StudioResults = _mapper.Map<FilmStudioAsUnauthorized>(n);
                    return StudioResults;
                });

                return Ok(allStudiosWithoutLoan);
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
            try
            {
                var location = _link.GetPathByAction("Get", "FilmStudio", new { Name = model.FilmStudioName });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Coldnt use this name");
                }



                var newModel = await _repository.Create(model);
                var newFilmStudio = new User
                {
                    UserName = model.Username,
                    Password = model.Password,
                    FilmStudioId = newModel.FilmStudioId,
                    Role = "FilmStudio"

                };
                var CreatedFilmStudio = await _user.CreateAsync(newFilmStudio, newFilmStudio.Password);
                if (!CreatedFilmStudio.Succeeded)
                {
                    return BadRequest(CreatedFilmStudio);
                }
                return Created("", newModel);
            }
            catch(Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}
