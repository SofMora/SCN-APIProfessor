using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProfessor.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ScnStudentDbContext _context;

        public StudentController(ScnStudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Student/GetAllStudents
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllStudents()
        {
            return await _context.Students
                .Select(student => new
                {
                    Id = student.Id,
                    Name = student.Name,
                    LastName = student.LastName,
                    Email = student.Email,
                    UserName = student.UserName,
                    SocialLinks = student.SocialLinks,
                    StatusStudent = student.StatusStudent
                })
                .ToListAsync();
        }

        // GET: api/Student/GetStudentById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<object>> GetStudentById(int id)
        {
            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(student => new
                {
                    Id = student.Id,
                    Name = student.Name,
                    LastName = student.LastName,
                    Email = student.Email,
                    UserName = student.UserName,
                    SocialLinks = student.SocialLinks,
                    StatusStudent = student.StatusStudent
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: api/Student/AddStudent
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting student: {ex.Message}");
                return StatusCode(500, "Error inserting student");
            }
        }

        // PUT: api/Student/UpdateStudent/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            // 1️⃣ Buscar el estudiante en la base de datos
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound($"No se encontró ningún estudiante con el ID {id}.");
            }

            // 2️⃣ Actualizar solo los campos necesarios
            existingStudent.Name = student.Name;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.UserName = student.UserName;
            existingStudent.Password = student.Password;
            existingStudent.Photo = student.Photo;
            existingStudent.SocialLinks = student.SocialLinks;
            existingStudent.StatusStudent = student.StatusStudent;

            try
            {
                await _context.SaveChangesAsync(); // Guardar cambios
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ocurrió un error de concurrencia al actualizar el estudiante.");
            }

            return NoContent();
        }


        // DELETE: api/Student/DeleteStudent/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound($"No se encontró ningún estudiante con el ID {id}.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
