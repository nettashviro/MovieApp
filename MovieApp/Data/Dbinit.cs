using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Data
{
    public class Dbinit
    {
        public static void init(MovieAppContext _context)
        {
            // Look for any accounts.
            if (!_context.Account.Any())
            {
                _context.Account.AddRange(
                  new List<Account>() {
                        new Account
                        {
                            // User Admin
                             Username ="Admin",
                             Password = "Admin",
                             Email = "admin@gmail.com",
                             Type = Account.UserType.Admin                        },
                         new Account
                         {
                            // User Customer
                             Username ="Customer",
                             Password = "Customer",
                             Email = "customer@gmail.com",
                             Type = Account.UserType.Customer,
                         }
                   }
              );
            }

            if (!_context.Movie.Any())
            {
                _context.Movie.AddRange(
                  new List<Movie>() {
                        new Movie
                        {
                            Name="Amélie",
                            Country="France",
                            Year=2001,
                            Genre=Movie.MovieGenre.Romance,
                            Duration=130,
                            TrailerUrl="https://www.youtube.com/watch?v=HUECWi5pX7o",
                            Rating=5,
                            Language="French",
                            ImageUrl="MV5BNDg4NjM1YjMtYmNhZC00MjM0LWFiZmYtNGY1YjA3MzZmODc5XkEyXkFqcGdeQXVyNDk3NzU2MTQ@._V1_UY1200_CR85,0,630,1200_AL_203544119.jpg",
                            MovieIdInTMDB=194
                       },
                     
                   }
              );
            }
            // TODO: ADD MOVIES / Officials / SOUNDTRACKS STARTER DATA

            _context.SaveChanges();
        }
    }
}
