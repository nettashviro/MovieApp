using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Models
{
    public class OfficialController : Controller
    {
        // GET: OfficialController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OfficialController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OfficialController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OfficialController/Create
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

        // GET: OfficialController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OfficialController/Edit/5
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

        // GET: OfficialController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OfficialController/Delete/5
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
