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

        public DbSet<MovieApp.Models.Movie> Movie { get; set; }

        public DbSet<MovieApp.Models.Director> Director { get; set; }

        public DbSet<MovieApp.Models.Soundtrack> Soundtrack { get; set; }

        public DbSet<MovieApp.Models.MovieReview> MovieReview { get; set; }
    }
}
