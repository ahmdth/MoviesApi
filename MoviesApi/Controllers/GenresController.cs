using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly MoviesDbContext _context;

        public GenresController(MoviesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
              return _context.Genres != null ? 
                          Ok(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'MoviesDbContext.Genres'  is null.");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return Ok(genre);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(genre);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
            return NotFound($"can't find genre with id = {id}");
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
