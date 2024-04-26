using System.ComponentModel.DataAnnotations;

namespace Portfolio_Backend.Models;

public class Project
{
    public long Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    [MaxLength(2000)]

    public string? DescriptionLong { get; set; }
    public string[]? Skills { get; set; }
    public string[]? Icons { get; set; }

    public string? Secret { get; set; }
}