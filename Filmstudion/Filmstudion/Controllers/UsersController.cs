using AutoMapper;
using Filmstudion.Models.Authentication;
using Filmstudion.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> user, IMapper mapper)
        { 
            _user = user;
            _mapper = mapper;
        }    

        [HttpPost]

        public async Task<ActionResult<User>> Register([FromBody] UserRegister user)
        {
            var newUser = new User
            {
                UserName = user.Username,
                Password = user.Password,
                Role = "Admin"
            };
            var createdAdmin = await _user.CreateAsync(newUser, newUser.Password);

            if(!createdAdmin.Succeeded)
            {
                return BadRequest(createdAdmin);
            }
            var adminResults = _mapper.Map<User, AdminResults>(newUser);
            return Created("", adminResults);
        }

    }
}
