using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    public class Official : Controller
    {
        // GET: Official
        public ActionResult Index()
        {
            return View();
        }

        // GET: Official/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Official/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Official/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Official/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Official/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Official/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Official/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
