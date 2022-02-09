using filmstudion.api.Models;
using Filmstudion.Models.Film;
using Filmstudion.Models.Filmstudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Loan
{
    public class FilmCopy : IFilmCopy
    {
        public int FilmCopyId { get; set; }
        public int FilmId { get; set; }
        public bool RentedOut { get; set; }
        public int FilmStudioId { get; set; }
    }
}
