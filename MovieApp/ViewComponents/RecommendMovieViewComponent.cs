using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Data;
using MovieApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.ViewComponents
{
    public class RecommendMovieViewComponent : ViewComponent
    {
        private MovieAppContext _context;
        List<Movie> _moviesRecommend;

        public RecommendMovieViewComponent(MovieAppContext context)
        {
            _context = context;
            _moviesRecommend = new List<Movie>();
        }

        private void GetRecommendList(IEnumerable<Movie.MovieGenre> listGenre, IEnumerable<Movie> alreadyWatched)
        {
            if (_moviesRecommend.Count < 3)
            {
                foreach (var genre in listGenre)
                {
                    var moviesByGenre = _context.Movie.Where(x => x.Genre == genre).OrderByDescending(x => x.Rating).ToList();
                    var filter = alreadyWatched.AsQueryable().Concat(_moviesRecommend);
                    var moviesFilter = moviesByGenre.Except(filter);
                    _moviesRecommend.AddRange(moviesFilter);
                }
            }
        }

        public async Task<IViewComponentResult> InvokeAsync(string path = "Index")
        {
            if (!User.Identity.IsAuthenticated) ViewData["message"] = "The user not logged in";

            string userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email)?.Value;
            if (userId == null) ViewData["message"] = "User email claim is empty";

            Account account = await _context.Account.Include(a => a.MovieWatched).Include(a => a.MovieClicked).Include(a => a.MovieWatchlist).FirstOrDefaultAsync(a => a.Email == userId);
            if (account == null) ViewData["message"] = "The user not found";

            var both = account.MovieWatched.ToList();
            both.AddRange(account.MovieClicked.ToList());
            var bothGenre = both.Select(x => x.Genre)
                .GroupBy(x => x)
                .Select(x => new { Genre = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .Select(x => x.Genre);

            List<Movie> moviesReccomend; 
            if (account.MovieWatched.Count == 0 && account.MovieClicked.Count == 0) {
                moviesReccomend = _context.Movie.OrderByDescending(x => x.Rating).ToList();

            }
            else
            {

                GetRecommendList(bothGenre, account.MovieWatched);
                moviesReccomend = _moviesRecommend.Distinct().ToList();
            }

           
            ViewBag.moviesReccomend = (moviesReccomend.Count > 3) ? moviesReccomend.GetRange(0, 3) : moviesReccomend;
            ViewData["account"] = account;
            ViewBag.path = path;
            return View("_FindMoviesRecommend");
        }
    }
}
