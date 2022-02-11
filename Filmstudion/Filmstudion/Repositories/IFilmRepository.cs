using Filmstudion.Models.Film;
using Filmstudion.Models.Filmstudio;
using Filmstudion.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Repositories
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Films>> Get();

        Task<Films> GetById(int id);

        Task<bool> SaveChangesAsync();

        Task<Films> Create(CreateFilm movie);

        Task<FilmCopy> AddLoan(Films Film, FilmStudio Filmstudio);

        Task<FilmCopy> RemoveLoan(Films movie, FilmStudio movieCo);
    }
}
