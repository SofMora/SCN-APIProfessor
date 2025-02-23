using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultController : ControllerBase
    {
        private readonly ScnAppointmentsDbContext _context;

        public ConsultController(ScnAppointmentsDbContext context)
        {
            _context = context;
        }

        // GET: api/Consult/GetAllConsults
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Consult>>> GetAllConsults()
        {
            return await _context.Consults.Include(c => c.CommentConsults).ToListAsync();
        }

        // GET: api/Consult/GetConsultById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Consult>> GetConsultById(int id)
        {
            var consult = await _context.Consults
                .Include(c => c.CommentConsults)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consult == null)
            {
                return NotFound();
            }

            return Ok(consult);
        }

        // GET: api/Consult/GetConsultsByProfessor/{professorId}
        [HttpGet]
        [Route("GetConsultsByProfessor/{professorId}")]
        public async Task<ActionResult<IEnumerable<Consult>>> GetConsultsByProfessor(int professorId)
        {
            var consults = await _context.Consults
                .Where(c => c.Author == professorId)  // Filtra por el autor (profesor)
                .Include(c => c.CommentConsults)      // Incluye los comentarios de la consulta
                .ToListAsync();

            if (!consults.Any())
            {
                return NotFound($"No se encontraron consultas para el profesor con ID {professorId}.");
            }

            return Ok(consults);
        }

        // POST: api/Consult/AddConsult
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Consult>> AddConsult(Consult consult)
        {
            _context.Consults.Add(consult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultById), new { id = consult.Id }, consult);
        }

        // PUT: api/Consult/UpdateConsult/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateConsult(int id, [FromBody] Consult consult)
        {
            if (id != consult.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingConsult = await _context.Consults.FindAsync(id);
            if (existingConsult == null)
            {
                return NotFound($"No se encontró la consulta con el ID {id}.");
            }

            existingConsult.IdCourse = consult.IdCourse;
            existingConsult.DescriptionConsult = consult.DescriptionConsult;
            existingConsult.TypeConsult = consult.TypeConsult;
            existingConsult.Author = consult.Author;
            existingConsult.DateConsult = consult.DateConsult;
            existingConsult.StatusConsult = consult.StatusConsult;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar la consulta.");
            }

            return NoContent();
        }

        // DELETE: api/Consult/DeleteConsult/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteConsult(int id)
        {
            var consult = await _context.Consults.FindAsync(id);
            if (consult == null)
            {
                return NotFound($"No se encontró la consulta con el ID {id}.");
            }

            _context.Consults.Remove(consult);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
