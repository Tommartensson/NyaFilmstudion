using filmstudion.api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Filmstudio
{
    public class RegisterFilmStudio : IRegisterFilmStudio
    {
        public string FilmStudioCity { get; set; }
        public string FilmStudioName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
