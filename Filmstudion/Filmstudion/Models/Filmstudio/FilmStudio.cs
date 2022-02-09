using filmstudion.api.Models;
using Filmstudion.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Filmstudio
{
    public class FilmStudio : IFIilmStudio
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<FilmCopy> RentedFilmCopies { get; set; }

     
    }
}
