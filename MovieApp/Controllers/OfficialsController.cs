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

        [AllowAnonymous]
        // GET: Officials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Official.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Officials/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Officials/Create
        public IActionResult Create()
        {
            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            return View();
        }

        // POST: Officials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Role,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Official official)
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

                official.OriginCountry = CultureHelper.GetCountryByIdentifier(official.OriginCountry);

                _context.Add(official);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(official);
        }

        // GET: Officials/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var official = await _context.Official.FindAsync(id);
            if (official == null)
            {
                return NotFound();
            }

            var countries = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            ViewBag.Countries = countries.OrderBy(p => p.Text).ToList();

            return View(official);
        }

        // POST: Officials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Role,Gender,Birthdate,OriginCountry,Image,ImageUrl")] Official official)
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
