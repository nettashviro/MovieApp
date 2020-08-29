using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    [Authorize]
    public class SoundtracksController : Controller
    {
        private readonly MovieAppContext _context;

        public SoundtracksController(MovieAppContext context)
        {
            _context = context;
        }

        // GET: Soundtracks
        public async Task<IActionResult> Index()
        {
            var soundtracks = await _context.Soundtrack
                .Include(s => s.Writer)
                .Include(s => s.Performer)
                .ToListAsync();
            return View(soundtracks);
        }

        // GET: Soundtracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack
                .Include(s => s.SoundtrackOfMovies).ThenInclude(sof => sof.Movie)
                .Include(s => s.Writer)
                .Include(s => s.Performer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soundtrack == null)
            {
                return NotFound();
            }

            return View(soundtrack);
        }


        // GET: Soundtracks/Create
        public IActionResult Create()
        {
            ViewBag.WriterId = new SelectList(_context.Official, "Id", "Id");
            ViewBag.PerformerId = new SelectList(_context.Official, "Id", "Id");
            IEnumerable<SelectListItem> officialNameSelectList = from o in _context.Official
                                                                 select new SelectListItem
                                                                 {
                                                                     Value = o.Id.ToString(),
                                                                     Text = o.FirstName + " " + o.LastName
                                                                 };
            ViewBag.WriterName = officialNameSelectList;
            ViewBag.PerformerName = officialNameSelectList;
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id");
            ViewBag.MovieName = new SelectList(_context.Movie, "Id", "Name");
            return View();
        }

        // POST: Soundtracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,TrailerUrl")] Soundtrack soundtrack, int WriterId, int PerformerId, int[] MovieId)
        {
            if (ModelState.IsValid)
            {
                soundtrack.Writer = _context.Official.First(o => o.Id == WriterId);
                soundtrack.Performer = _context.Official.First(o => o.Id == PerformerId);
                soundtrack.SoundtrackOfMovies = new List<SoundtrackOfMovie>();
                foreach (var id in MovieId)
                {
                    soundtrack.SoundtrackOfMovies.Add(new SoundtrackOfMovie() { MovieId = id, SoundtrackId = soundtrack.Id });
                }

                _context.Add(soundtrack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(soundtrack);
        }

        // GET: Soundtracks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack.FindAsync(id);
            if (soundtrack == null)
            {
                return NotFound();
            }
            return View(soundtrack);
        }

        // POST: Soundtracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Writer,Performer")] Soundtrack soundtrack)
        {
            if (id != soundtrack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soundtrack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoundtrackExists(soundtrack.Id))
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
            return View(soundtrack);
        }

        // GET: Soundtracks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soundtrack == null)
            {
                return NotFound();
            }

            return View(soundtrack);
        }

        // POST: Soundtracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soundtrack = await _context.Soundtrack.FindAsync(id);
            _context.Soundtrack.Remove(soundtrack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoundtrackExists(int id)
        {
            return _context.Soundtrack.Any(e => e.Id == id);
        }
    }
}
