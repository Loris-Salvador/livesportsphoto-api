namespace Domain.Models;

public class Section
{
    public string? Id { get; set; }

    public required string Name { get; set; }

    public List<Album> Albums { get; set; } = [];
}