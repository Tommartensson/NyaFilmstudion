using Filmstudion.Models.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Repositories
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> Get();

        Task<Film> GetById(int id);
    }
}
