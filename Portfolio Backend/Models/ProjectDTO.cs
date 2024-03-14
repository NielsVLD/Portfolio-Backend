﻿namespace Portfolio_Backend.Models;

public class ProjectDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string[]? Skills { get; set; }
}