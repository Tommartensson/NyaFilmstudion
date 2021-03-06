using filmstudion.api.Models;
using Filmstudion.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Film
{
    public class Films : IFilm
    {
        public int FilmId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public List<FilmCopy> FilmCopies { get; set; }
        public int NumberOfCopies { get; set; }
    }
}
