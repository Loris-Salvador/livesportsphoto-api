namespace Domain.Models;

public class Section
{
    public required string Name { get; set; }

    public List<Album> Albums { get; set; } = [];
}