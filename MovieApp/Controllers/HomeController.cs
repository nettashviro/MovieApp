using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieAppContext _context;

        public HomeController(MovieAppContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // TODO: If we want a session control (after 10 min the user required to login)
            // we need to add this section in every page
            // OR ELSE: if we don't want this feature, delete this section and only return the view
            string user = HttpContext.Session.GetString("Type");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.MovieCount = _context.Movie.Count();
            ViewBag.OfficialCount = _context.Official.Count();
            ViewBag.SoundtrackCount = _context.Soundtrack.Count();
            ViewBag.UsersCount = _context.Account.Count();

            var allMovies = await _context.Movie.ToListAsync();
            var moviesWithLocation = allMovies.Where(m => !String.IsNullOrEmpty(m.Country));
            ViewBag.locations = moviesWithLocation.ToDictionary(movie => "Id" + movie.Id,
                movie => new { name = movie.Name, country = movie.Country, imageUrl = movie.ImageUrl });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
