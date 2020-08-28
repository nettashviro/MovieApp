using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;

namespace MovieApp.Controllers
{
    public class GraphsController : Controller
    {
        private readonly MovieAppContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GraphsController(MovieAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Movies/Graphs/Screen/1
        public async Task<IActionResult> Screen(int? demantion)
        {
            if (demantion == 1)
            {
                return View("OneDemantionGraphs");
            } else if(demantion == 2)
            {
                return View("TwoDemantionGraphs");
            } else if(demantion == 3)
            {
                return View("ThreeDemantionGraphs");
            }

            return View();
        }

        // GET: Graphs/Count/:splice&:context
        public async Task<IActionResult> Count(string? splice, string? context)
        {
            if (splice != null)
            {
                if (context == "Officials")
                {
                    var list =
                        _context.Official
                        .ToList()
                       .GroupBy(m => m.GetType().GetProperty(splice).GetValue(m, null))
                       .Select(m => new
                       {
                           Key = m.Key.ToString(),
                           Count = m.Count()
                       })
                       .ToList();
                    return Json(list);
                }
                else if (context == "Movies")
                {
                    var list =
                        _context.Movie
                        .ToList()
                       .GroupBy(m => m.GetType().GetProperty(splice).GetValue(m, null))
                       .Select(m => new
                       {
                           Key = m.Key.ToString(),
                           Count = m.Count()
                       })
                       .ToList();
                    return Json(list);
                }
                else
                {
                    return null;
                }
            } else { 
                return null;
            }
        }

        // GET: Graphs/Average/:avgOf:avgBy:context
        public async Task<IActionResult> Average(string? avgOf, string? avgBy, string? context)
        {
            if (avgOf != null && avgBy != null)
            {
                if (context == "Officials")
                {
                    var list = _context.Official
                    .ToList()
                   .GroupBy(m => m.GetType().GetProperty(avgBy).GetValue(m, null))
                   .Select(m => new
                   {
                       Key = m.Key.ToString(),
                       Count = m.Average(r => Convert.ToDouble(DateTime.Now.Year - r.Birthdate.Year))
                   })
                   .ToList();
                    return Json(list);
                } else if (context == "Movies")
                {
                    var list = _context.Movie
                    .ToList()
                   .GroupBy(m => m.GetType().GetProperty(avgBy).GetValue(m, null))
                   .Select(m => new
                   {
                       Key = m.Key.ToString(),
                       Count = m.Average(r => Convert.ToDouble(r.GetType().GetProperty(avgOf).GetValue(r, null).ToString()))
                   })
                   .ToList();
                    return Json(list);
                } else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
