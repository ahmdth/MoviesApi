using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(MoviesDbContext context, IWebHostEnvironment hostingEnvironment)
    : Controller
{
    private readonly List<string> _extensions = [".jpg", ".png", ".jpeg"];
    private const long MaxFileSize = 1024 * 1024;

    // GET: Movies
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var moviesDbContext = context.Movies.Include(m => m.Genre);
        return Ok(await moviesDbContext.ToListAsync());
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await context.Movies
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
        var genre = await context.Genres.AnyAsync(g => g.Id == movie.GenreId);
        if (!genre)
        {
            return BadRequest("Invalid Genre Id.");
        }

        string uniqueFile = string.Empty;
        if (movie.Poster != null)
        {
            if (!_extensions.Contains(Path.GetExtension(movie.Poster.FileName).ToLower()))
            {
                return BadRequest("allowed file extensions(png, jpg, jpeg)");
            }

            if (movie.Poster.Length > MaxFileSize)
            {
                return BadRequest("Maximum file size 1MG");
            }

            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "movies");
            uniqueFile = Guid.NewGuid().ToString() + "_" + movie.Poster.FileName;
            string fpath = Path.Combine(uploadsFolder, uniqueFile);
            await movie.Poster.CopyToAsync(new FileStream(fpath, FileMode.Create));
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
        context.Add(m);
        await context.SaveChangesAsync();
        return Ok(movie);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id, [Bind("Title,Year,Rate,Description,GenreId,Poster")] Movie movie)
    {
        var genre = context.Genres.Any(g => g.Id == id);
        if (!genre)
        {
            return NotFound();
        }

        try
        {
            context.Update(movie);
            await context.SaveChangesAsync();
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
        var movie = await context.Movies.FindAsync(id);
        if (movie != null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }

        return NotFound($"can't find movie with id = {id}");
    }

    [NonAction]
    private bool MovieExists(int id)
    {
        return context.Movies.Any(e => e.Id == id);
    }
}