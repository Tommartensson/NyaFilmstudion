using Filmstudion.Models.Filmstudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{

  interface IUser
  {
    public int UserId { get; set; }
    public string Role { get; set; }
    public int FilmStudioId { get; set; }
    public FilmStudio FilmStudio { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
  }
}