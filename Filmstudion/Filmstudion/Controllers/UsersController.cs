using AutoMapper;
using Filmstudion.Models.Authentication;
using Filmstudion.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _sign;
        private readonly IConfiguration _config;

        public UsersController(UserManager<User> user, IMapper mapper, IConfiguration config, SignInManager<User> sign)
        { 
            _user = user;
            _mapper = mapper;
            _config = config;
            _sign = sign;
        }
        [Route("register")]
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
            return Ok(adminResults);
        }
        [Route("Authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticate UserAuth)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.FindByNameAsync(UserAuth.UserName);
                if (user != null)
                {
                    var result = await _sign.CheckPasswordSignInAsync(user, UserAuth.Password, true);
                    
                    if (result.Succeeded)
                    {
                        
                            var claims = new[]
                            {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                            new Claim("userId", user.UserId.ToString()),
                            new Claim("role", user.Role),



                        };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                                _config["Tokens:Audience"],
                                claims,
                                signingCredentials: creds,
                                expires: DateTime.UtcNow.AddMinutes(20));

                            user.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        var mappedUser = _mapper.Map<UserResults>(user);
                            return Created("", mappedUser);
                        }
                   


                }
            }
            return BadRequest("Couldnt sign in");
        }
    }
    }

