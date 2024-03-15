using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_Backend.Models;

public class MyUser : IdentityUser { }
public class ProjectsContext(DbContextOptions<ProjectsContext> options) : IdentityDbContext<MyUser>(options)
{
    public DbSet<Project>? Projects { get; set; } = null;
}