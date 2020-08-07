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
    public class DirectorsController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DirectorsController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Director.ToListAsync());
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Director
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Director director)
        {
            if (ModelState.IsValid)
            {
                if (director.Image != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(director.Image.FileName);
                    string extension = Path.GetExtension(director.Image.FileName);
                    director.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/directors/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await director.Image.CopyToAsync(fileStream);
                    }
                }

                director.OriginCountry = CultureHelper.GetCountryByIdentifier(director.OriginCountry);

                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Director.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var original_data = _context.Director.AsNoTracking().Where(d => d.Id == id).FirstOrDefault();
                    if (director.Image != null)
                    {
                        if (original_data.ImageUrl != null)
                        {
                            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/directors", original_data.ImageUrl);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }

                        string fileName = Path.GetFileNameWithoutExtension(director.Image.FileName);
                        string extension = Path.GetExtension(director.Image.FileName);
                        director.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/img/directors/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await director.Image.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        director.ImageUrl = original_data.ImageUrl;
                    }

                    director.OriginCountry = CultureHelper.GetCountryByIdentifier(director.OriginCountry);

                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
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
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Director
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Director.FindAsync(id);

            if (director.ImageUrl != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img/directors", director.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Director.Remove(director);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
            return _context.Director.Any(e => e.Id == id);
        }
    }
}
