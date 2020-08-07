using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MoviesController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Movies/Create
        public IActionResult Create()
        {
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            var languages = new SelectList(CultureHelper.LanguageList(), "Key", "Value");
            ViewBag.Languages = languages.OrderBy(p => p.Text).ToList();

            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,Language,Year,Genre,Duration,TrailerUrl,Rating,Image,ImageUrl")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.Image != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(movie.Image.FileName);
                    string extension = Path.GetExtension(movie.Image.FileName);
                    movie.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/movies/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await movie.Image.CopyToAsync(fileStream);
                    }
                }
                
                movie.Country = CultureHelper.GetCountryByIdentifier(movie.Country);
                movie.Language = CultureHelper.GetLanguageByIdentifier(movie.Language);

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            var languages = new SelectList(CultureHelper.LanguageList(), "Key", "Value");
            ViewBag.Languages = languages.OrderBy(p => p.Text).ToList();
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,Language,Year,Genre,Duration,TrailerUrl,Rating,Image,ImageUrl")] Movie movie)
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
                    if (movie.Image != null)
                    {
                        if (original_data.ImageUrl != null)
                        {
                            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/movies", original_data.ImageUrl);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        string fileName = Path.GetFileNameWithoutExtension(movie.Image.FileName);
                        string extension = Path.GetExtension(movie.Image.FileName);
                        movie.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/movies/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await movie.Image.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        movie.ImageUrl = original_data.ImageUrl;
                    }

                    movie.Country = CultureHelper.GetCountryByIdentifier(movie.Country);
                    movie.Language = CultureHelper.GetLanguageByIdentifier(movie.Language);

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie.ImageUrl != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/movies", movie.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
