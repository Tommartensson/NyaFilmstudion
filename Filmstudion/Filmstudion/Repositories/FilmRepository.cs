using AutoMapper;
using Filmstudion.Models.Film;
using Filmstudion.Models.Filmstudio;
using Filmstudion.Models.Loan;
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
        public async Task<IEnumerable<Films>> Get()
        {
            var allFilms = await _context.Films.ToListAsync();
            var allLoans = await _context.FilmCopy.ToListAsync();


            var Films = allFilms.Select(n =>
            {
                n.FilmCopies = allLoans.FindAll(l => l.FilmId == n.FilmId);
                return n;
            });

            return Films;
        }
        public async Task<Films> GetById(int id)
        {
            //Klar
            var allLoans = await _context.FilmCopy.ToListAsync();

            var allMovies = await _context.Films.ToListAsync();
            IQueryable<Films> query = _context.Films;

            query = query.Where(c => c.FilmId == id);
            var Film = await query.FirstOrDefaultAsync();

            Film.FilmCopies = allLoans.FindAll(l => l.FilmId == Film.FilmId);

            return Film;
        }
        public async Task<Films> Create(CreateFilm movie)
        {
            var newFilm = _mapper.Map<CreateFilm, Films>(movie);
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
        public async Task<FilmCopy> AddLoan(Films Film, FilmStudio Filmstudio)
        {
            try
            {
                var newLoan = new FilmCopy { FilmId = Film.FilmId, FilmStudioId = Filmstudio.FilmStudioId, RentedOut = true };


                _context.FilmCopy.Add(newLoan);
                await _context.SaveChangesAsync();
                return newLoan;
            }
            catch
            {
                throw new Exception("Movie was not loanble");
            }
        }
        public async Task<FilmCopy> RemoveLoan(Films Film, FilmStudio filmStudio)
        {
            var allLoans = await _context.FilmCopy.ToListAsync();
            var newLoan = new FilmCopy { FilmId = Film.FilmId, FilmStudioId = filmStudio.FilmStudioId };

            var removeLoan = allLoans.Find(n => n.FilmId == newLoan.FilmId && n.FilmStudioId == n.FilmStudioId);
            _context.FilmCopy.Remove(removeLoan);
            await _context.SaveChangesAsync();
            return removeLoan;
        }
    }
}
