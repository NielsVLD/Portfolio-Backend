using Microsoft.EntityFrameworkCore;

namespace Portfolio_Backend.Models;

public class ProjectsContext(DbContextOptions<ProjectsContext> options) : DbContext(options)
{
    public DbSet<Project>? Projects { get; set; } = null;
}