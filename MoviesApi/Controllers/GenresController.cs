using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Models;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController(MoviesDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await context.Genres.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genre = await context.Genres
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
        context.Add(genre);
        await context.SaveChangesAsync();
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
            context.Update(genre);
            await context.SaveChangesAsync();
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
        var genre = await context.Genres.FindAsync(id);
        if (genre != null)
        {
            context.Genres.Remove(genre);
            await context.SaveChangesAsync();
        }
        return NotFound($"can't find genre with id = {id}");
    }

    private bool GenreExists(int id)
    {
        return context.Genres.Any(e => e.Id == id);
    }
}