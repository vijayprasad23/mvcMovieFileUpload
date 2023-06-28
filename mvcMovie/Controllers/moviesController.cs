using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcMovie.Areas.Identity.Data;
using mvcMovie.Models;

namespace mvcMovie.Controllers
{
    [Authorize]
    public class moviesController : Controller
    {
        private readonly mvcMovieDb _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public moviesController(mvcMovieDb context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: movies
        public async Task<IActionResult> Index()
        {
              return _context.movie != null ? 
                          View(await _context.movie.ToListAsync()) :
                          Problem("Entity set 'mvcMovieDb.movie'  is null.");
        }

        // GET: movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.movie == null)
            {
                return NotFound();
            }

            var movie = await _context.movie
                .FirstOrDefaultAsync(m => m.movieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("movieId,title,genre,rating,dateReleased,fileName,imageFile")] movie movie)
        {
            if (!ModelState.IsValid)
            {
                //save image to the images folder inside the wwwroot folder...

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(movie.imageFile.FileName);
                string extension = Path.GetExtension(movie.imageFile.FileName);
                movie.fileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);

                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await movie.imageFile.CopyToAsync(fileStream);
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.movie == null)
            {
                return NotFound();
            }

            var movie = await _context.movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("movieId,title,genre,rating,dateReleased,fileName, imageFile")] movie movie)
        {
            if (id != movie.movieId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {                   
                        if (movie.fileName != null)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(movie.imageFile.FileName);
                            string extension = Path.GetExtension(movie.imageFile.FileName);
                        
                            string path = Path.Combine(wwwRootPath + "/images/", fileName);
                            
                            if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                        }
                        else
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(movie.imageFile.FileName);
                            string extension = Path.GetExtension(movie.imageFile.FileName);
                            movie.fileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            string path = Path.Combine(wwwRootPath + "/images/", fileName);

                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await movie.imageFile.CopyToAsync(fileStream);
                            }
                        }
                    
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!movieExists(movie.movieId))
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
            return View(movie);
        }

        // GET: movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.movie == null)
            {
                return NotFound();
            }

            var movie = await _context.movie
                .FirstOrDefaultAsync(m => m.movieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.movie == null)
            {
                return Problem("Entity set 'mvcMovieDb.movie'  is null.");
            }
            var movie = await _context.movie.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", movie.fileName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            //Delete the record from the database

            if (movie != null)
            {
                _context.movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool movieExists(int id)
        {
          return (_context.movie?.Any(e => e.movieId == id)).GetValueOrDefault();
        }
    }
}
