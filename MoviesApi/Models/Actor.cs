namespace MoviesApi.Models;

public class Actor
{
    public int Id { get; set; }
    public required string  FirstName { get; set; }
    public required string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime? BirthDate { get; set; }
    public List<Movie> Movies { get; set; } = new();
}