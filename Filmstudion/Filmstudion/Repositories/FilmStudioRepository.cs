using AutoMapper;
using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Repositories
{
    public class FilmStudioRepository : IFilmStudioRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _link;

        public FilmStudioRepository(AppDbContext context, IMapper mapper, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _link = link;
        }

        public async Task<IEnumerable<FilmStudio>> Get()
        {
            var allStudios = await _context.FilmStudios.ToListAsync();

            return allStudios;
        }
        public async Task<IEnumerable<FilmStudio>> GetAsAdmin()
        {
            var allStudios = await _context.FilmStudios.ToListAsync();
            var allLoans = await _context.FilmCopy.ToListAsync();
            var allFilms = await _context.Films.ToListAsync();

            var allStudiosWithLoan = allStudios.Select(n =>
            {

                n.RentedFilmCopies = allLoans.FindAll(l => l.FilmStudioId == n.FilmStudioId);
                allFilms.Select(n =>
                {
                    var movie = allLoans.FindAll(l => l.FilmId == n.FilmId);
                    return movie;
                });
                return n;
            });

            return allStudiosWithLoan;
        }
        public async Task<FilmStudio> Create(RegisterFilmStudio Filmstudio)
        {
           
            var newMovieCo = new FilmStudio
            {
                City = Filmstudio.FilmStudioCity,
                Name = Filmstudio.FilmStudioName
            };

                _context.FilmStudios.Add(newMovieCo);
                await _context.SaveChangesAsync();
                return newMovieCo;
            
        }
        public async Task<FilmStudio> GetById(int id)
        {
            //Klar
            
            var allMovies = await _context.FilmStudios.ToListAsync();
            var allLoans = await _context.FilmCopy.ToListAsync();
            IQueryable<FilmStudio> query = _context.FilmStudios;

            query = query.Where(c => c.FilmStudioId == id);
            var FilmStudio = await query.FirstOrDefaultAsync();

            FilmStudio.RentedFilmCopies = allLoans.FindAll(l => l.FilmStudioId == FilmStudio.FilmStudioId);

            return FilmStudio;
        }
    }
}
