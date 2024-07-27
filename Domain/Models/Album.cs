namespace Domain.Models;

public class Album
{
    public string? Id { get; set; }

    public required string Name { get; set; }

    public required string Link { get; set; }
}