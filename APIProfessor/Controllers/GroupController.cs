using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ScnProfessorDbContext _context;

        public GroupController(ScnProfessorDbContext context)
        {
            _context = context;
        }

        // GET: api/Group/GetAllGroups
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllGroups()
        {
            return await _context.Groups
                .Include(g => g.IdCourseNavigation)
                .Include(g => g.IdProfessorNavigation)
                .Select(group => new
                {
                    Id = group.Id,
                    IdCourse = group.IdCourse,
                    CourseName = group.IdCourseNavigation.Name,
                    IdProfessor = group.IdProfessor,
                    ProfessorName = group.IdProfessorNavigation.Name,
                    NumberGroup = group.NumberGroup
                })
                .ToListAsync();
        }

        // GET: api/Group/GetGroupById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<object>> GetGroupById(int id)
        {
            var group = await _context.Groups
                .Where(g => g.Id == id)
                .Include(g => g.IdCourseNavigation)
                .Include(g => g.IdProfessorNavigation)
                .Select(group => new
                {
                    Id = group.Id,
                    IdCourse = group.IdCourse,
                    CourseName = group.IdCourseNavigation.Name,
                    IdProfessor = group.IdProfessor,
                    ProfessorName = group.IdProfessorNavigation.Name,
                    NumberGroup = group.NumberGroup
                })
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // POST: api/Group/AddGroup
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Group>> AddGroup(Group group)
        {
            try
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inserting group: {ex.Message}");
            }
        }

        // PUT: api/Group/UpdateGroup/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] Group group)
        {
            if (id != group.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingGroup = await _context.Groups.FindAsync(id);
            if (existingGroup == null)
            {
                return NotFound($"No se encontró ningún grupo con el ID {id}.");
            }

            existingGroup.IdCourse = group.IdCourse;
            existingGroup.IdProfessor = group.IdProfessor;
            existingGroup.NumberGroup = group.NumberGroup;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ocurrió un error de concurrencia al actualizar el grupo.");
            }

            return NoContent();
        }

        // DELETE: api/Group/DeleteGroup/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound($"No se encontró ningún grupo con el ID {id}.");
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
