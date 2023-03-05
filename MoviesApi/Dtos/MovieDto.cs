using MoviesApi.Models;
using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class MovieDto
    {
        [Required, MaxLength(250)]
        public string Title { get; set; } = string.Empty;
        [MinYear(1900)]
        public int Year { get; set; }
        [Range(1.0, 10.0)]
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; } = null!;
        public int GenreId { get; set; }
        public IFormFile? Poster { get; set; }
    }
}
