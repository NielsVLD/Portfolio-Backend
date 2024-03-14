namespace Portfolio_Backend.Models;

public class Project
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string[]? Skills { get; set; }
    public string? Secret { get; set; }
}