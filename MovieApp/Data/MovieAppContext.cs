using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.Models;

namespace MovieApp.Data
{
    public class MovieAppContext : DbContext
    {
        public MovieAppContext (DbContextOptions<MovieAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfficialOfMovie>()
                .HasKey(oom => new { oom.MovieId, oom.OfficialId});

            modelBuilder.Entity<OfficialOfMovie>()
                .HasOne(oom => oom.Movie)
                .WithMany(o => o.OfficialOfMovies)
                .HasForeignKey(oom => oom.MovieId);

            modelBuilder.Entity<OfficialOfMovie>()
                .HasOne(oom => oom.Official)
                .WithMany(m => m.OfficialOfMovies)
                .HasForeignKey(oom => oom.OfficialId);
        }

        public DbSet<MovieApp.Models.Movie> Movie { get; set; }

        public DbSet<MovieApp.Models.Official> Official { get; set; }

        public DbSet<MovieApp.Models.Soundtrack> Soundtrack { get; set; }

        public DbSet<MovieApp.Models.MovieReview> MovieReview { get; set; }
        public DbSet<MovieApp.Models.Account> Account { get; set; }
        public DbSet<MovieApp.Models.OfficialOfMovie> OfficialOfMovie { get; set; }
    }
}
