using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;
using MovieApp.Models.TMDB;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [Authorize]

    public class MoviesController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private TMDB TMDBService = new TMDB();
        private TwitterController twitter;

        public MoviesController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            twitter = new TwitterController(context);
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            Account account = await _context.Account.FirstOrDefaultAsync(m => m.Email == userId);
            ViewData["account"] = account;
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Seen
        public async Task<IActionResult> Seen()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            Account account = await _context.Account.Include(m => m.MovieWatched).FirstOrDefaultAsync(m => m.Email == userId);
            ViewData["account"] = account;
            return View();
        }

        // GET: Movies/Watchlist
        public async Task<IActionResult> Watchlist()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            Account account = await _context.Account.Include(m => m.MovieWatchlist).FirstOrDefaultAsync(m => m.Email == userId);
            ViewData["account"] = account;
            return View();
        }



        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.OfficialOfMovies).ThenInclude(oom => oom.Official)
                .Include(m => m.SoundtracksOfMovie).ThenInclude(som => som.Soundtrack).ThenInclude(s => s.Performer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }


            if (!User.Identity.IsAuthenticated) return BadRequest("User not logged in");

            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (userId == null) return BadRequest("User email claim is empty");


            var account = await _context.Account.Include(x => x.MovieClicked).FirstOrDefaultAsync(m => m.Email == userId);
            if (account == null) return BadRequest("User not found");

            if (account.MovieClicked == null)
            {
                List<Movie> movies = new List<Movie>();
                movies.Add(movie);
                account.MovieClicked = movies;
            }
            else
            {
                if (account.MovieClicked.Count == 5)
                {
                    var moviesList = account.MovieClicked.ToList();
                    moviesList.RemoveAt(0);
                    account.MovieClicked = moviesList;
                }

                account.MovieClicked.Add(movie);
            }

            await _context.SaveChangesAsync();


            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            var languages = new SelectList(CultureHelper.LanguageList(), "Key", "Value");
            ViewBag.Languages = languages.OrderBy(p => p.Text).ToList();

            ViewBag.OfficialId = new SelectList(_context.Official, "Id", "Id");
            IEnumerable<SelectListItem> officialNameSelectList = from o in _context.Official
                                                     select new SelectListItem
                                                     {
                                                         Value = o.Id.ToString(),
                                                         Text = o.FirstName +  " " + o.LastName
                                                     };
            ViewBag.OfficialName = officialNameSelectList;

            ViewBag.SoundtrackId = new SelectList(_context.Soundtrack, "Id", "Id");
            ViewBag.SoundtrackName = new SelectList(_context.Soundtrack, "Id", "Name");

            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,Language,Year,Genre,Duration,TrailerUrl,Rating,Image,ImageUrl,MovieIdInTMDB")] Movie movie, int[] OfficialsIds, int[] SoundtracksIds)
        {
            if (ModelState.IsValid)
            {
                movie.ImageUrl =  (movie.ImageUrl == null)? "/img/movies/defaultMoviePoster.png": ("http://image.tmdb.org/t/p/w188_and_h282_bestv2" + movie.ImageUrl);
                movie.Language = CultureHelper.GetLanguageByIdentifier(movie.Language);

                movie.OfficialOfMovies = new List<OfficialOfMovie>();
                foreach (var id in OfficialsIds)
                {
                    movie.OfficialOfMovies.Add(new OfficialOfMovie() { MovieId = movie.Id, OfficialId = id });
                }

                movie.SoundtracksOfMovie = new List<SoundtrackOfMovie>();
                foreach (var id in SoundtracksIds)
                {
                    movie.SoundtracksOfMovie.Add(new SoundtrackOfMovie() { MovieId = movie.Id, SoundtrackId = id });
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();

                var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (userId == null)
                {
                    return BadRequest("User email claim is empty");
                }

                try
                {
                    var message = "HOT ALRET: new movie was added: " + movie.Name + " , Don't missed it!!";
                    string imagePath;
                    if (!movie.ImageUrl.StartsWith("http://") && !movie.ImageUrl.StartsWith("https://"))
                    {
                        imagePath = Path.Combine(_hostEnvironment.WebRootPath, movie.ImageUrl);
                    }else
                    {
                        imagePath =  movie.ImageUrl;

                    }

                    await twitter.PublishTweetAsync(userId, movie.Id, message, imagePath, Tweet.TweetType.MovieAdded);
                }
                catch (WebException)
                { }


                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }


        // GET: Movies/FindMovieId
        public async Task<List<MovieSearchResult>> FindMovieId(string name)
        {
            List<MovieSearchResult> movieResult = TMDBService.GetMovieIdByName(name);
            return movieResult;
        }

        // GET: Movies/FindMovieReviews
        public async Task<List<MovieReviewsResult>> FindMovieReviews(string id)
        {
            List<MovieReviewsResult> movieResult = TMDBService.GetMovieReviewsById(id);
            return movieResult;
        }


        // GET: Movies/FindMovieVideos
        public async Task<string> FindMovieVideos(string id)
        {
            string movieResult =  TMDBService.GetMovieTrailerById(id);
            return movieResult;
        }


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.OfficialOfMovies)
                .Include(m => m.SoundtracksOfMovie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> countriesSelectList = from c in CultureHelper.CountryList()
                                                              select new SelectListItem
                                                                 {
                                                                     Value = c.Key,
                                                                     Text = c.Value,
                                                                     Selected = CultureHelper.GetCountryByIdentifier(c.Key) == movie.Country
                                                                 };
            ViewBag.Countries = countriesSelectList.OrderBy(p => p.Text).ToList();

            IEnumerable<SelectListItem> languagesSelectList = from c in CultureHelper.LanguageList()
                                                              select new SelectListItem
                                                              {
                                                                  Value = c.Key,
                                                                  Text = c.Value,
                                                                  Selected = CultureHelper.GetLanguageByIdentifier(c.Key) == movie.Language
                                                              };

            ViewBag.Languages = languagesSelectList.OrderBy(p => p.Text).ToList();

            List<SelectListItem> officialNameSelectList = new List<SelectListItem>();
            foreach(var o in _context.Official)
            {
                SelectListItem s = new SelectListItem();
                s.Value = o.Id.ToString();
                s.Text = o.FirstName + " " + o.LastName;
                s.Selected = movie.OfficialOfMovies.Any(oom => o.Id == oom.OfficialId);
                officialNameSelectList.Add(s);
            }

            ViewBag.OfficialName = officialNameSelectList;

            List<SelectListItem> soundtrackNameSelectList = new List<SelectListItem>();
            foreach (var st in _context.Soundtrack)
            {
                SelectListItem s = new SelectListItem();
                s.Value = st.Id.ToString();
                s.Text = st.Name;
                s.Selected = movie.SoundtracksOfMovie.Any(som => st.Id == som.SoundtrackId);
                soundtrackNameSelectList.Add(s);
            }
            ViewBag.SoundtrackName = soundtrackNameSelectList;
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,Language,Year,Genre,Duration,TrailerUrl,Rating,Image,ImageUrl,MovieIdInTMDB")] Movie movie, int[] OfficialsIds, int[] SoundtracksIds)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var original_data = _context.Movie.AsNoTracking().Where(m => m.Id == id).FirstOrDefault();

                    if (movie.ImageUrl == null)
                    {
                        movie.ImageUrl = "/img/movies/defaultMoviePoster.png";
                    }
                    else
                    {
                        movie.ImageUrl = "http://image.tmdb.org/t/p/w188_and_h282_bestv2" + movie.ImageUrl;
                    }

                    movie.Country = CultureHelper.GetCountryByIdentifier(movie.Country);
                    movie.Language = CultureHelper.GetLanguageByIdentifier(movie.Language);

                    movie.OfficialOfMovies = new List<OfficialOfMovie>();
                    foreach (var officialId in OfficialsIds)
                    {
                        movie.OfficialOfMovies.Add(new OfficialOfMovie() { MovieId = movie.Id, OfficialId = officialId });
                    }

                    movie.SoundtracksOfMovie = new List<SoundtrackOfMovie>();
                    foreach (var soundtrackId in SoundtracksIds)
                    {
                        movie.SoundtracksOfMovie.Add(new SoundtrackOfMovie() { MovieId = movie.Id, SoundtrackId = soundtrackId });
                    }

                    _context.OfficialOfMovie.RemoveRange(_context.OfficialOfMovie.Where(oom => oom.MovieId == movie.Id));
                    _context.OfficialOfMovie.AddRange(movie.OfficialOfMovies);

                    _context.SoundtrackOfMovie.RemoveRange(_context.SoundtrackOfMovie.Where(som => som.MovieId == movie.Id));
                    _context.SoundtrackOfMovie.AddRange(movie.SoundtracksOfMovie);

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie.ImageUrl == null)
            {
                movie.ImageUrl = "/img/movies/defaultMoviePoster.png";
            }
            else
            {
                movie.ImageUrl = "http://image.tmdb.org/t/p/w188_and_h282_bestv2" + movie.ImageUrl;
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            // TODO: REMOVE FROM USER WATCHED IF MOVIE DELETED
            try
            {
                await twitter.DeleteTweetAsync(movie.Id);
            }
            catch (WebException)
            { }

            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/CountriesMapped
        public async Task<IActionResult> CountriesMapped()
        {
            var allMovies = await _context.Movie.ToListAsync();
            var moviesWithLocation = allMovies.Where(m => !String.IsNullOrEmpty(m.Country));
            ViewBag.locations = moviesWithLocation.ToDictionary(movie => "Id" + movie.Id,
                movie => new { name = movie.Name, country = movie.Country, imageUrl = movie.ImageUrl });

            return View();
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovieWatched(int id, string path = "Index")
        {
            if (!User.Identity.IsAuthenticated) return BadRequest("User not logged in");
            

            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (userId == null) return BadRequest("User email claim is empty");
       

            Account account = await _context.Account.Include(a => a.MovieWatched).FirstOrDefaultAsync(m => m.Email == userId);
            if (account == null) return BadRequest("User not found");
         

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound("movie not found");
        

            if (account.MovieWatched == null)
            {
                List<Movie> movies = new List<Movie>();
                movies.Add(movie);
                account.MovieWatched = movies;
            }
            else
            {
                var isMovieAlreadyWatched = account.MovieWatched.FirstOrDefault(m => m.Id == id);
                if (isMovieAlreadyWatched != null)
                {
                    return BadRequest("Movie already watched");
                }

                account.MovieWatched.Add(movie);
            }

            await _context.SaveChangesAsync();


            try
            {
                var message = "User " + account.Username + " marked the movie " + movie.Name + " as watched! Go see it if you haven't seen it already! Rating: " + movie.Rating + " stars";
                string imgPath = (movie.ImageUrl != null) ? movie.ImageUrl : Path.Combine(_hostEnvironment.WebRootPath, "/img/movies/defaultMoviePoster.png");
                
                await twitter.PublishTweetAsync(userId, movie.Id, message, imgPath, Tweet.TweetType.MovieWatched);
            }
            catch (WebException)
            { }


            return RedirectToAction(path);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovieWatched(int id, string path = "Index")
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("User not logged in");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (userId == null) return BadRequest("User email claim is empty");

            var account = await _context.Account.Include(a => a.MovieWatched).FirstOrDefaultAsync(m => m.Email == userId);

            if (account == null) return BadRequest("User not found");


            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();

            var isMovieAlreadyWatched = account.MovieWatched.FirstOrDefault(m => m.Id == id);

            if (isMovieAlreadyWatched == null)
            {
                return NotFound();
            }
            else
            {
                account.MovieWatched.Remove(movie);
            }

            await _context.SaveChangesAsync();


            try
            {
                await twitter.DeleteTweetAsync(movie.Id, userId, Tweet.TweetType.MovieWatched);
            }
            catch (WebException)
            { }


            return RedirectToAction(path);

        }

        [HttpPost]
        public async Task<IActionResult> AddMovieWatchlist(int id, string path = "Index")
        {
            if (!User.Identity.IsAuthenticated) return BadRequest("User not logged in");


            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (userId == null) return BadRequest("User email claim is empty");


            var account = await _context.Account.Include(a => a.MovieWatchlist).FirstOrDefaultAsync(m => m.Email == userId);
            if (account == null) return BadRequest("User not found");


            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound("movie not found");


            if (account.MovieWatchlist == null)
            {
                List<Movie> movies = new List<Movie>();
                movies.Add(movie);
                account.MovieWatchlist = movies;
            }
            else
            {
                var isMovieInWatchlist = account.MovieWatchlist.FirstOrDefault(m => m.Id == id);
                if (isMovieInWatchlist != null)
                {
                    return BadRequest("Movie already in watchlist");
                }

                account.MovieWatchlist.Add(movie);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(path);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovieWatchlist(int id, string path = "Index")
        {
            if (!User.Identity.IsAuthenticated) return BadRequest("User not logged in");

            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (userId == null) return BadRequest("User email claim is empty");

            var account = await _context.Account.Include(a => a.MovieWatchlist).FirstOrDefaultAsync(m => m.Email == userId);

            if (account == null) return BadRequest("User not found");


            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();

            var isMovieAlreadyWatched = account.MovieWatchlist.FirstOrDefault(m => m.Id == id);

            if (isMovieAlreadyWatched == null)
            {
                return NotFound();
            }
            else
            {
                account.MovieWatchlist.Remove(movie);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(path);
        }

    }
}
