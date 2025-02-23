using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ScnProfessorDbContext _context;

        public CourseController(ScnProfessorDbContext context)
        {
            _context = context;
        }

        // GET: api/Course/GetAllCourses
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCourses()
        {
            return await _context.Courses
                .Include(c => c.IdProfessorNavigation)
                .Select(course => new
                {
                    Id = course.Id,
                    Name = course.Name,
                    Cycle = course.Cycle,
                    StatusCourse = course.StatusCourse,
                    Description = course.Description,
                    Professor = new
                    {
                        Id = course.IdProfessorNavigation.Id,
                        Name = course.IdProfessorNavigation.Name,
                        LastName = course.IdProfessorNavigation.LastName,
                        Email = course.IdProfessorNavigation.Email
                    }
                })
                .ToListAsync();
        }

        // GET: api/Course/GetCourseById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<object>> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.IdProfessorNavigation)
                .Where(c => c.Id == id)
                .Select(course => new
                {
                    Id = course.Id,
                    Name = course.Name,
                    Cycle = course.Cycle,
                    StatusCourse = course.StatusCourse,
                    Description = course.Description,
                    Professor = new
                    {
                        Id = course.IdProfessorNavigation.Id,
                        Name = course.IdProfessorNavigation.Name,
                        LastName = course.IdProfessorNavigation.LastName,
                        Email = course.IdProfessorNavigation.Email
                    }
                })
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // POST: api/Course/AddCourse
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Course>> AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting course: {ex.Message}");
                return StatusCode(500, "Error inserting course");
            }
        }

        // PUT: api/Course/UpdateCourse/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (id != course.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
            {
                return NotFound($"No se encontró ningún curso con el ID {id}.");
            }

            existingCourse.Name = course.Name;
            existingCourse.Cycle = course.Cycle;
            existingCourse.StatusCourse = course.StatusCourse;
            existingCourse.Description = course.Description;
            existingCourse.IdProfessor = course.IdProfessor;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ocurrió un error de concurrencia al actualizar el curso.");
            }

            return NoContent();
        }

        // DELETE: api/Course/DeleteCourse/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound($"No se encontró ningún curso con el ID {id}.");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
