using Filmstudion.Models.Authentication;
using Filmstudion.Models.Film;
using Filmstudion.Models.Filmstudio;
using Filmstudion.Models.Loan;
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
        public DbSet<Films> Films { get; set; }
        public DbSet<FilmStudio> FilmStudios { get; set; }
        public DbSet<FilmCopy> FilmCopy { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Films>().ToTable("Movies");
            builder.Entity<Films>().HasKey(p => p.FilmId);
            builder.Entity<Films>().Property(p => p.FilmId).IsRequired();
            
            builder.Entity<Films>().Property(p => p.Name);
            builder.Entity<Films>().Property(p => p.ReleaseDate);
            builder.Entity<Films>().Property(p => p.Country);
            builder.Entity<Films>().Property(p => p.Director);



            builder.Entity<Films>().HasData
                (
                new Films { FilmId = 1, Name = "LoR", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1998), NumberOfCopies= 3 },
                new Films { FilmId = 2, Name = "Transformers", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(2000), NumberOfCopies = 1 },
                new Films { FilmId = 3, Name = "Ted", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1997), NumberOfCopies = 3 },
                new Films { FilmId = 4, Name = "Backstage", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1990), NumberOfCopies = 3 },
                new Films { FilmId = 5, Name = "TMNT", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1991), NumberOfCopies = 3 },
                new Films { FilmId = 6, Name = "Star-wars", Country = "Sweden", Director = "blank", ReleaseDate = new DateTime(1992), NumberOfCopies = 3 }


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
