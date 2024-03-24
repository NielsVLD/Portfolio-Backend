using System.ComponentModel.DataAnnotations;

namespace Portfolio_Backend.Models;

public class ProjectDTO
{
    public long Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public string[]? Skills { get; set; }
}