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

        [AllowAnonymous]
        // GET: Soundtracks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Soundtrack.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Soundtracks/Details/5
        public async Task<IActionResult> Details(int? id)
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


        // GET: Soundtracks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Soundtracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Writer,Performer")] Soundtrack soundtrack)
        {
            if (ModelState.IsValid)
            {
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
