﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;
using X.PagedList;

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
        public async Task<IActionResult> Index(string nameFilter, string currentNameFilter, int? page)
        {
            var soundtracks = await _context.Soundtrack
                .Include(s => s.Writer)
                .Include(s => s.Performer)
                .ToListAsync();

            if (nameFilter != null)
            {
                page = 1;
            }
            else
            {
                nameFilter = currentNameFilter;
            }

            ViewBag.CurrentNameFilter = nameFilter;

            if (!String.IsNullOrEmpty(nameFilter))
            {
                soundtracks = soundtracks.Where(s => s.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
            }

            int pageSize = 25;
            int pageNumber = page ?? 1;

            return View(soundtracks.ToPagedList(pageNumber, pageSize));
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
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,SoundtrackUrl")] Soundtrack soundtrack, int WriterId, int PerformerId, int[] MovieId)
        {
            if (ModelState.IsValid)
            {
                soundtrack.Writer = _context.Official.FirstOrDefault(o => o.Id == WriterId);
                soundtrack.Performer = _context.Official.FirstOrDefault(o => o.Id == PerformerId);
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack
                .Include(s => s.Writer)
                .Include(s => s.Performer)
                .Include(s => s.SoundtrackOfMovies).ThenInclude(som => som.Movie)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (soundtrack == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> writerNameSelectList = from o in _context.Official
                                                                 select new SelectListItem
                                                                 {
                                                                     Value = o.Id.ToString(),
                                                                     Text = o.FirstName + " " + o.LastName,
                                                                     Selected = o.Id == soundtrack.Writer.Id
                                                                 };
            IEnumerable<SelectListItem> performerNameSelectList = from o in _context.Official
                                                               select new SelectListItem
                                                               {
                                                                   Value = o.Id.ToString(),
                                                                   Text = o.FirstName + " " + o.LastName,
                                                                   Selected = o.Id == soundtrack.Performer.Id
                                                               };
            ViewBag.WriterName = writerNameSelectList;
            ViewBag.PerformerName = performerNameSelectList;

            List<SelectListItem> movieNameSelectList = new List<SelectListItem>();
            foreach (var m in _context.Movie)
            {
                SelectListItem s = new SelectListItem();
                s.Value = m.Id.ToString();
                s.Text = m.Name;
                s.Selected = soundtrack.SoundtrackOfMovies.Any(som => m.Id == som.MovieId);
                movieNameSelectList.Add(s);
            }

            ViewBag.MovieName = movieNameSelectList;
            
            return View(soundtrack);
        }

        // POST: Soundtracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,SoundtrackUrl")] Soundtrack soundtrack, int WriterId, int PerformerId, int[] MovieId)
        {
            if (id != soundtrack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    soundtrack.Writer = _context.Official.First(o => o.Id == WriterId);
                    soundtrack.Performer = _context.Official.First(o => o.Id == PerformerId);
                    soundtrack.SoundtrackOfMovies = new List<SoundtrackOfMovie>();
                    foreach (var movieId in MovieId)
                    {
                        soundtrack.SoundtrackOfMovies.Add(new SoundtrackOfMovie() { MovieId = movieId, SoundtrackId = soundtrack.Id });
                    }

                    _context.SoundtrackOfMovie.RemoveRange(_context.SoundtrackOfMovie.Where(som => som.SoundtrackId == soundtrack.Id));
                    _context.SoundtrackOfMovie.AddRange(soundtrack.SoundtrackOfMovies);

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
