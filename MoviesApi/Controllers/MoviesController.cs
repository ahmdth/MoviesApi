using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;
        private List<string> extensions = new List<string>{ ".jpg", ".png", ".jpeg" };
        private long maxFileSize = 1204 * 1204;

        public MoviesController(MoviesDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Movies
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var moviesDbContext = _context.Movies.Include(m => m.Genre);
            return Ok(await moviesDbContext.ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm, Bind("Title,Year,Rate,Description,GenreId")] MovieDto movie)
        {
            var genre = await _context.Genres.AnyAsync(g => g.Id == movie.GenreId);
            if(!genre)
            {
                return BadRequest("Invalide Genre Id.");
            }
            string uniqueFile = string.Empty;
            if (movie.Poster != null )
            {
                if (!extensions.Contains(Path.GetExtension(movie.Poster.FileName).ToLower()))
                {
                    return BadRequest("allowed fille .png .jpg and .jpeg");
                }
                if (movie.Poster.Length > maxFileSize)
                {
                    return BadRequest("Maximume file size 1MG");
                }
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "movies");
                uniqueFile = Guid.NewGuid().ToString() + "_" + movie.Poster.FileName;
                string fpath = Path.Combine(uploadsFolder, uniqueFile);
                movie.Poster.CopyTo(new FileStream(fpath, FileMode.Create));
            }
            var m = new Movie
            {
                Title = movie.Title,
                Description = movie.Title,
                Rate = movie.Rate,
                Year = movie.Year,
                GenreId = movie.GenreId,
                Poster = uniqueFile
            };
            _context.Add(m);
            await _context.SaveChangesAsync();
            return Ok(movie);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Year,Rate,Description,GenreId,Poster")] Movie movie)
        {
            var genre = _context.Genres.Any(g => g.Id == id);
            if (!genre)
            {
                return NotFound();
            }
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(movie);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            return NotFound($"can't find movie with id = {id}");
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
