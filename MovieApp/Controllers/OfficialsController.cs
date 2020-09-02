using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;
using MovieApp.Models.TMDB;
using X.PagedList;

namespace MovieApp.Controllers
{
    [Authorize]
    public class OfficialsController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OfficialsController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Officials
        public async Task<IActionResult> Index(string nameFilter, string currentNameFilter, int? page)
        {
            var officials = await _context.Official.ToListAsync();

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
                officials = officials.Where(o => o.FirstName.ToLower().Contains(nameFilter.ToLower()) || o.LastName.ToLower().Contains(nameFilter.ToLower())).ToList();
            }

            int pageSize = 8;
            int pageNumber = page ?? 1;

            return View(officials.ToPagedList(pageNumber, pageSize));
        }

        // GET: Officials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var official = await _context.Official.Include(o => o.OfficialOfMovies).ThenInclude(oof => oof.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (official == null)
            {
                return NotFound();
            }

            return View(official);
        }

        // GET: Officials/Create
        public IActionResult Create()
        {
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.MovieId = new SelectList(_context.Movie, "Id", "Id");
            ViewBag.MovieName = new SelectList(_context.Movie, "Id", "Name");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            return View();
        }

        // POST: Officials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Role,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Official official, int[] MovieId)
        {
            if (ModelState.IsValid)
            {
                if (official.Image != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(official.Image.FileName);
                    string extension = Path.GetExtension(official.Image.FileName);
                    official.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/officials/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await official.Image.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    official.ImageUrl = "default-preson.jpg";
                }

                official.OriginCountry = CultureHelper.GetCountryByIdentifier(official.OriginCountry);
                official.OfficialOfMovies = new List<OfficialOfMovie>();
                foreach (var id in MovieId)
                {
                    official.OfficialOfMovies.Add(new OfficialOfMovie() { MovieId = id, OfficialId = official.Id });
                }

                _context.Add(official);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(official);
        }

        // GET: Officials/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var official = await _context.Official
                .Include(o => o.OfficialOfMovies)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (official == null)
            {
                return NotFound();
            }



            IEnumerable<SelectListItem> countriesSelectList = from c in CultureHelper.CountryList()
                                                              select new SelectListItem
                                                              {
                                                                  Value = c.Key,
                                                                  Text = c.Value,
                                                                  Selected = CultureHelper.GetCountryByIdentifier(c.Key) == official.OriginCountry
                                                              };
            ViewBag.Countries = countriesSelectList.OrderBy(p => p.Text).ToList();

            List<SelectListItem> movieNameSelectList = new List<SelectListItem>();
            foreach (var m in _context.Movie)
            {
                SelectListItem s = new SelectListItem();
                s.Value = m.Id.ToString();
                s.Text = m.Name;
                s.Selected = official.OfficialOfMovies.Any(oom => m.Id == oom.MovieId);
                movieNameSelectList.Add(s);
            }
            ViewBag.MovieName = movieNameSelectList;

            return View(official);
        }

        // POST: Officials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Role,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Official official, int[] MovieId)
        {
            if (id != official.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var original_data = _context.Official.AsNoTracking().Where(d => d.Id == id).FirstOrDefault();
                    if (official.Image != null)
                    {
                        if (original_data.ImageUrl != null)
                        {
                            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/officials", original_data.ImageUrl);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        string fileName = Path.GetFileNameWithoutExtension(official.Image.FileName);
                        string extension = Path.GetExtension(official.Image.FileName);
                        official.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/officials/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await official.Image.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        official.ImageUrl = original_data.ImageUrl;
                    }

                    official.OriginCountry = CultureHelper.GetCountryByIdentifier(official.OriginCountry);
                    if (official.OfficialOfMovies == null)
                    {
                        official.OfficialOfMovies = new List<OfficialOfMovie>();
                    }

                    foreach (var movieId in MovieId)
                    {
                        official.OfficialOfMovies.Add(new OfficialOfMovie() { MovieId = movieId, OfficialId = official.Id });
                    }

                    _context.OfficialOfMovie.RemoveRange(_context.OfficialOfMovie.Where(oom => oom.OfficialId == official.Id));
                    _context.OfficialOfMovie.AddRange(official.OfficialOfMovies);

                    _context.Update(official);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficialExists(official.Id))
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
            return View(official);
        }

        // GET: Officials/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var official = await _context.Official
                .FirstOrDefaultAsync(m => m.Id == id);
            if (official == null)
            {
                return NotFound();
            }

            return View(official);
        }

        // POST: Officials/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var official = await _context.Official.FindAsync(id);

            if (official.ImageUrl != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/officials", official.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Soundtrack.RemoveRange(_context.Soundtrack.Where(s => s.Writer.Id == official.Id || s.Performer.Id == official.Id));

            _context.Official.Remove(official);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficialExists(int id)
        {
            return _context.Official.Any(e => e.Id == id);
        }


    }
}
