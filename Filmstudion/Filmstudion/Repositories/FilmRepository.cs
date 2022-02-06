using AutoMapper;
using Filmstudion.Models.Film;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _link;

        public FilmRepository(AppDbContext context, IMapper mapper, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _link = link;
        }
        public async Task<IEnumerable<Film>> Get()
        {
            var allFilms = await _context.Films.ToListAsync();

            return allFilms;
        }
        public async Task<Film> GetById(int id)
        {
            //Klar

            var allMovies = await _context.Films.ToListAsync();
            IQueryable<Film> query = _context.Films;

            query = query.Where(c => c.FilmId == id);
            var Film = await query.FirstOrDefaultAsync();

            return Film;
        }
    }
}
