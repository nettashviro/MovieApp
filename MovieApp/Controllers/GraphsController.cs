﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class GraphsController : Controller
    {
        private readonly MovieAppContext _context;

        public GraphsController(MovieAppContext context)
        {
            _context = context;
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
                } 
                else if (context == "Movies")
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
                }
                else if (context == "Soundtracks")
                {
                    var list = _context.Movie
                        .Join(_context.SoundtrackOfMovie,
                            m => m.Id,
                            som => som.MovieId,
                            (m, som) => new
                            {
                                movieId = m.Id,
                                soundtrackId = som.SoundtrackId,
                                groupByAvg = som.Soundtrack.GetType().GetProperty(avgOf).GetValue(som.Soundtrack, null),
                                groupByKey = m.GetType().GetProperty(avgBy).GetValue(m, null)
                            })
                        .ToList()
                        .GroupBy(p => p.groupByKey)
                        .Select(gb => new
                        {
                            Key = gb.Key.ToString(),
                            Count = gb.Average(a => (double)a.groupByAvg)
                        })
                        .ToList();
                    return Json(list);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // GET: Graphs/Groupby/
        public async Task<IActionResult> Groupby()
        {
            var list = _context.Movie
                .Join(_context.OfficialOfMovie,
                    m => m.Id,
                    oom => oom.MovieId,
                    (m, oom) => new
                    {
                        movieId = m.Id,
                        rating = m.Rating,
                        duration = m.Duration,
                        genre = m.Genre,
                        name = m.Name,
                        officialId = oom.OfficialId
                    })
                .GroupBy(p => new { p.duration, p.rating, p.genre, p.movieId, p.name})
                .Select(gb => new
                {
                    Duration = gb.Key.duration,
                    Rating = gb.Key.rating,
                    Genre = gb.Key.genre,
                    Name = gb.Key.name,
                    Count = gb.Count()
                })
                .ToList();
            return Json(list);
        }
    }
}
