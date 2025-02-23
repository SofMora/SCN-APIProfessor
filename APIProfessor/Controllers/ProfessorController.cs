using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIProfessor.Utils;
using Microsoft.IdentityModel.Tokens;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly ScnProfessorDbContext _context;
        private Encryption encryption;

        public ProfessorController(ScnProfessorDbContext context)
        {
            _context = context;
            encryption = new Encryption();
        }

        // GET: api/Professor/GetAllProfessors
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllProfessors()
        {
            return await _context.Professors
                .Include(p => p.Courses) // Incluir cursos relacionados
                .Select(professor => new
                {
                    professor.Id,
                    professor.Name,
                    professor.LastName,
                    professor.Email,
                    professor.UserName,
                    professor.Description,
                    professor.SocialLink,
                    professor.StatusProfessor,
                    Courses = professor.Courses.Select(c => new { c.Id, c.Name, c.Description })
                })
                .ToListAsync();
        }

        // GET: api/Professor/GetProfessorById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<object>> GetProfessorById(int id)
        {
            var professor = await _context.Professors
                .Include(p => p.Courses) // Incluir cursos relacionados
                .Where(p => p.Id == id)
                .Select(professor => new
                {
                    professor.Id,
                    professor.Name,
                    professor.LastName,
                    professor.Email,
                    professor.UserName,
                    professor.Description,
                    professor.SocialLink,
                    professor.StatusProfessor,
                    Courses = professor.Courses.Select(c => new { c.Id, c.Name, c.Description })
                })
                .FirstOrDefaultAsync();

            if (professor == null)
            {
                return NotFound();
            }

            return Ok(professor);
        }

        // POST: api/Professor/AddProfessor
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Professor>> AddProfessor(Professor professor)
        {
            try
            {
                professor.Password = encryption.Encrypt(professor.Password);// pmc
                _context.Professors.Add(professor);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProfessorById), new { id = professor.Id }, professor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting professor: {ex.Message}");
                return StatusCode(500, "Error inserting professor");
            }
        }

        // PUT: api/Professor/UpdateProfessor/{id}

        [HttpPut("UpdateProfessor/{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorDto professorDto)
        {
            if (professorDto == null)
            {
                return BadRequest("Profesor no encontrado.");
            }

            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            // Actualizar propiedades del profesor
            professor.Name = professorDto.Name;
            professor.LastName = professorDto.LastName;
            professor.Email = professorDto.Email;
            professor.UserName = professorDto.UserName;
            professor.Description = professorDto.Description;
            professor.SocialLink = professorDto.SocialLink;
            professor.StatusProfessor = professorDto.StatusProfessor;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(professorDto.Password))
            {
                professor.Password = encryption.Encrypt(professorDto.Password);
            }

            // Si la foto no es null o vacía, convertirla de base64 a byte[] y asignarla
            if (!string.IsNullOrEmpty(professorDto.Photo))
            {
                professor.Photo = Convert.FromBase64String(professorDto.Photo); // Convertir base64 a byte[]
            }

            _context.Professors.Update(professor);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Profesor actualizado correctamente." });
        }






            //[HttpPut]
            //[Route("[action]/{id}")]
            //public async Task<IActionResult> UpdateProfessor(int id, [FromBody] Professor professor)
            //{
            //    if (id != professor.Id)
            //    {
            //        return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            //    }

            //    var existingProfessor = await _context.Professors.FindAsync(id);
            //    if (existingProfessor == null)
            //    {
            //        return NotFound($"No se encontró ningún profesor con el ID {id}.");
            //    }

            //    existingProfessor.Name = professor.Name;
            //    existingProfessor.LastName = professor.LastName;
            //    existingProfessor.Email = professor.Email;
            //    existingProfessor.UserName = professor.UserName;
            //    existingProfessor.Password = encryption.Encrypt(professor.Password);//pmc
            //    existingProfessor.Description = professor.Description;
            //    existingProfessor.Photo = professor.Photo;
            //    existingProfessor.SocialLink = professor.SocialLink;
            //    existingProfessor.StatusProfessor = professor.StatusProfessor;

            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        return StatusCode(500, "Ocurrió un error de concurrencia al actualizar el profesor.");
            //    }

            //    return NoContent();
            //}

            // DELETE: api/Professor/DeleteProfessor/{id}
            [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound($"No se encontró ningún profesor con el ID {id}.");
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Professor/AssignCourseToProfessor/{professorId}/{courseId}
        [HttpPost]
        [Route("[action]/{professorId}/{courseId}")]
        public async Task<IActionResult> AssignCourseToProfessor(int professorId, int courseId)
        {
            var professor = await _context.Professors.Include(p => p.Courses).FirstOrDefaultAsync(p => p.Id == professorId);
            var course = await _context.Courses.FindAsync(courseId);

            if (professor == null || course == null)
            {
                return NotFound("Profesor o curso no encontrado.");
            }

            if (!professor.Courses.Contains(course))
            {
                professor.Courses.Add(course);
                await _context.SaveChangesAsync();
            }

            return Ok("Curso asignado correctamente al profesor.");
        }

        // GET: api/Professor/GetValidateProfessor/{username}/{pwd}
        [HttpGet]
        [Route("[action]/{username}/{pwd}")]
        public async Task<ActionResult<object>> GetValidateProfessor(string username, string pwd)
        {
            // 1️⃣ Buscar el usuario sin desencriptar la contraseña
            var professor = await _context.Professors
                .Include(p => p.Courses)
                .Where(p => p.UserName == username)
                .FirstOrDefaultAsync();

            // 2️⃣ Si no se encontró el usuario, retornar error 404
            if (professor == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // 3️⃣ Desencriptar y validar la contraseña
            if (encryption.Decrypt(professor.Password) != pwd)
            {
                return Unauthorized(new { message = "Contraseña incorrecta" });
            }

            // 4️⃣ Devolver los datos del usuario si la contraseña es correcta
            return Ok(new
            {
                professor.Id,
                professor.Name,
                professor.LastName,
                professor.Email,
                professor.UserName,
                professor.Description,
                professor.SocialLink,
                professor.StatusProfessor,
                Courses = professor.Courses.Select(c => new { c.Id, c.Name, c.Description })
            });
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professors.Any(e => e.Id == id);
        }
    }
}
