using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
