using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesCrud.Models;
using MoviesCrud.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrud.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            //var viewModel = new MovieFormViewModel
            //{
            //    Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            //};
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View(model);
            }

            var files = Request.Form.Files;

            if(!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View(model);
            }

            var poster = files.FirstOrDefault();
            var allowedExtensions = new List<string> { ".jpg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Only .png, .jpg images are allowed!");
                return View(model);
            }

            return View(model);
        }
    }
}
