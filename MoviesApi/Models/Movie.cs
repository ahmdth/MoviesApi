using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; } = string.Empty;
        [MinYear(1900)]
        public int Year { get; set; }
        [Range(1.0, 10.0)]
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; } = null!;
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Poster { get; set; }
    }
}
