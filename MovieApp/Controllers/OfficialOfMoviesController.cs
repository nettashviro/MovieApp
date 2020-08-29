using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class OfficialOfMoviesController : Controller
    {
        private readonly MovieAppContext _context;

        public OfficialOfMoviesController(MovieAppContext context)
        {
            _context = context;
        }

        // GET: OfficialOfMovies
        public async Task<IActionResult> Index()
        {
            var movieAppContext = _context.OfficialOfMovie.Include(o => o.Movie).Include(o => o.Official);
            return View(await movieAppContext.ToListAsync());
        }

        // GET: OfficialOfMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialOfMovie = await _context.OfficialOfMovie
                .Include(o => o.Movie)
                .Include(o => o.Official)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (officialOfMovie == null)
            {
                return NotFound();
            }

            return View(officialOfMovie);
        }

        // GET: OfficialOfMovies/Create
        public IActionResult Create()
        {
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id");
            ViewBag.OfficialId = new SelectList(_context.Official, "Id", "Id");
            return View();
        }

        // POST: OfficialOfMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,OfficialId")] OfficialOfMovie officialOfMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(officialOfMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id", officialOfMovie.MovieId);
            ViewBag.OfficialId = new SelectList(_context.Official, "Id", "Id", officialOfMovie.OfficialId);
            return View(officialOfMovie);
        }

        // GET: OfficialOfMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialOfMovie = await _context.OfficialOfMovie.FindAsync(id);
            if (officialOfMovie == null)
            {
                return NotFound();
            }
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id", officialOfMovie.MovieId);
            ViewBag.OfficialId = new SelectList(_context.Official, "Id", "Id", officialOfMovie.OfficialId);
            return View(officialOfMovie);
        }

        // POST: OfficialOfMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,OfficialId")] OfficialOfMovie officialOfMovie)
        {
            if (id != officialOfMovie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(officialOfMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficialOfMovieExists(officialOfMovie.MovieId))
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
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id", officialOfMovie.MovieId);
            ViewBag.OfficialId = new SelectList(_context.Official, "Id", "Id", officialOfMovie.OfficialId);
            return View(officialOfMovie);
        }

        // GET: OfficialOfMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officialOfMovie = await _context.OfficialOfMovie
                .Include(o => o.Movie)
                .Include(o => o.Official)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (officialOfMovie == null)
            {
                return NotFound();
            }

            return View(officialOfMovie);
        }

        // POST: OfficialOfMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officialOfMovie = await _context.OfficialOfMovie.FindAsync(id);
            _context.OfficialOfMovie.Remove(officialOfMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficialOfMovieExists(int id)
        {
            return _context.OfficialOfMovie.Any(e => e.MovieId == id);
        }
    }
}
