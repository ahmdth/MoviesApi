using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Data
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action", Description = "Action description" },
                new Genre { Id = 2, Name = "Drama", Description = "Drama description" },
                new Genre { Id = 3, Name = "Crime", Description = "Crime description" },
                new Genre { Id = 4, Name = "Biography", Description = "Biography description" },
                new Genre { Id = 5, Name = "Adventure", Description = "Adventure description" },
                new Genre { Id = 6, Name = "Hoirror", Description = "Hoirror description" },
                new Genre { Id = 7, Name = "Romance", Description = "Romance description" },
                new Genre { Id = 8, Name = "Commedy", Description = "Commedy description" },
                new Genre { Id = 9, Name = "War", Description = "War description" },
                new Genre { Id = 10, Name = "Fantasy", Description = "Fantasy description" }
            );
        }
    }
}
