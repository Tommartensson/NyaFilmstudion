using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Film
{
    public class FilmAsUnauthorized
    {
        public int FilmId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }

        public int NumberOfCopies { get; set; }
    }
}
