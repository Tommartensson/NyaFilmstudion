using filmstudion.api.Models;
using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Authentication
{
    public class User : IdentityUser, IUser
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public int FilmStudioId { get; set; }
        public FilmStudio FilmStudio { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
