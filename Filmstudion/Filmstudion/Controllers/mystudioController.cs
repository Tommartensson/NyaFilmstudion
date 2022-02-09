using Filmstudion.Models.Loan;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class mystudioController : ControllerBase
    {


        private readonly AppDbContext _context;

        public mystudioController(AppDbContext context)
        {
            _context = context;
        }
    [AllowAnonymous]
    [Route("rentals")]
    [HttpGet]
    public async Task<ActionResult<FilmCopy>> FilmCopy()
    {
        try
        {
            if (User.IsInRole("FilmStudio"))
            {
                    var allLoans = _context.FilmCopy.ToList();
                    var theUser = User.Claims.Where(n => n.Type == "userId").FirstOrDefault()?.Value;
                    var userId = int.Parse(theUser);
                    var yourLoans = allLoans.FindAll(n => n.FilmStudioId == userId);
                    return Ok(yourLoans);
            }
            else
            {
                return Unauthorized("You are not a studio");
            }
        }
        catch (Exception err)
        {
            return BadRequest(err);
        }
    }
}
}
