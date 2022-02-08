using AutoMapper;
using Filmstudion.Models.Film;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FilmRepository> _logger;

        public FilmRepository(AppDbContext context, IMapper mapper, LinkGenerator link, ILogger<FilmRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _link = link;
            _logger = logger;
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
        public async Task<Film> Create(CreateFilm movie)
        {
            var newFilm = _mapper.Map<CreateFilm, Film>(movie);
            _context.Films.Add(newFilm);
            await _context.SaveChangesAsync();
            return newFilm;
        }
        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
