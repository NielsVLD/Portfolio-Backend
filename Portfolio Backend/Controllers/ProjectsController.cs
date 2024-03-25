using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Models;

namespace Portfolio_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(ProjectsContext context) : ControllerBase
    {
        // GET: api/Projects
        [EnableRateLimiting("token")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects()
        {
            return await context.Projects
                .Select(x => ProjectToDTO(x))
                .ToListAsync();
        }

        // GET: api/Projects/5
        [EnableRateLimiting("token")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjects(long id)
        {
            var projects = await context.Projects.FindAsync(id);

            if (projects == null)
            {
                return NotFound();
            }

            return ProjectToDTO(projects);
        }

        // PUT: api/Projects/5
        [EnableRateLimiting("token")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects(long id, ProjectDTO projectDTO)
        {
            if (id != projectDTO.Id)
            {
                return BadRequest();
            }

            context.Entry(projectDTO).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        [EnableRateLimiting("token")]

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> PostProjects(ProjectDTO projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                DescriptionLong = projectDto.DescriptionLong,
                Skills = projectDto.Skills,
                Icons = projectDto.Icons
            };
            
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, ProjectToDTO(project));
        }

        // DELETE: api/Projects/5
        [EnableRateLimiting("token")]

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjects(long id)
        {
            var projects = await context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }

            context.Projects.Remove(projects);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectsExists(long id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
        private static ProjectDTO ProjectToDTO(Project project) =>
            new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                DescriptionLong = project.DescriptionLong,
                Skills = project.Skills,
                Icons = project.Icons
            };
    }
}
