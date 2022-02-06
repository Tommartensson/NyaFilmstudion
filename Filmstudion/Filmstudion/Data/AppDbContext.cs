using Filmstudion.Models.Authentication;
using Filmstudion.Models.Film;
using Filmstudion.Models.Filmstudio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmStudio> FilmStudios { get; set; }
        //public DbSet<MovieLoan> MovieLoan { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Film>().ToTable("Movies");
            builder.Entity<Film>().HasKey(p => p.FilmId);
            builder.Entity<Film>().Property(p => p.FilmId).IsRequired();
            
            builder.Entity<Film>().Property(p => p.Name);
            builder.Entity<Film>().Property(p => p.ReleaseDate);
            builder.Entity<Film>().Property(p => p.Country);
            builder.Entity<Film>().Property(p => p.Director);



            builder.Entity<Film>().HasData
                (
                new Film { FilmId = 1, Name = "LoR", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1998) },
                new Film { FilmId = 2, Name = "Transformers", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(2000) },
                new Film { FilmId = 3, Name = "Ted", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1997) },
                new Film { FilmId = 4, Name = "Backstage", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1990) },
                new Film { FilmId = 5, Name = "TMNT", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1991) },
                new Film { FilmId = 6, Name = "Star-wars", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1992) }


            ) ;
            builder.Entity<FilmStudio>().HasData
            (
                new FilmStudio
                {
                    FilmStudioId = 1,
                    Name= "Universal",
                    City = "Cali",



                },
            new FilmStudio
            {
                FilmStudioId = 2,
                Name = "Dreamworks",
                City = "Colorado",

            }); ;



        }
    }
}
