using filmstudion.api.Models;
using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion
{
    public interface IFilmStudioRepository
    {
        Task<FilmStudio> Create(RegisterFilmStudio filmStudio);

        Task<IEnumerable<FilmStudio>> Get();

        Task<FilmStudio> GetById(int id);
    }
}
