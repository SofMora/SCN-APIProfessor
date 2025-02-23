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
    public class ScheduleProfessorController : ControllerBase
    {
        private readonly ScnAppointmentsDbContext _context;

        public ScheduleProfessorController(ScnAppointmentsDbContext context)
        {
            _context = context;
        }

        // GET: api/ScheduleProfessor/GetAllSchedules
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<ScheduleProfessor>>> GetAllSchedules()
        {
            return await _context.ScheduleProfessors.Include(s => s.Appointments).ToListAsync();
        }

        // GET: api/ScheduleProfessor/GetScheduleById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<ScheduleProfessor>> GetScheduleById(int id)
        {
            var schedule = await _context.ScheduleProfessors
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // POST: api/ScheduleProfessor/AddSchedule
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ScheduleProfessor>> AddSchedule(ScheduleProfessor schedule)
        {
            _context.ScheduleProfessors.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetScheduleById), new { id = schedule.Id }, schedule);
        }

        // PUT: api/ScheduleProfessor/UpdateSchedule/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] ScheduleProfessor schedule)
        {
            if (id != schedule.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingSchedule = await _context.ScheduleProfessors.FindAsync(id);
            if (existingSchedule == null)
            {
                return NotFound($"No se encontró el horario con el ID {id}.");
            }

            existingSchedule.IdProfessor = schedule.IdProfessor;
            existingSchedule.Day = schedule.Day;
            existingSchedule.Time = schedule.Time;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar el horario.");
            }

            return NoContent();
        }

        // DELETE: api/ScheduleProfessor/DeleteSchedule/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.ScheduleProfessors.FindAsync(id);
            if (schedule == null)
            {
                return NotFound($"No se encontró el horario con el ID {id}.");
            }

            _context.ScheduleProfessors.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ScheduleProfessor/GetSchedulesByProfessorId/{professorId}
        [HttpGet]
        [Route("[action]/{professorId}")]
        public async Task<ActionResult<IEnumerable<ScheduleProfessor>>> GetSchedulesByProfessorId(int professorId)
        {
            var schedules = await _context.ScheduleProfessors
                .Include(s => s.Appointments)
                .Where(s => s.IdProfessor == professorId)
                .ToListAsync();

            if (schedules == null || !schedules.Any())
            {
                return NotFound($"No se encontraron horarios para el profesor con el ID {professorId}.");
            }

            return Ok(schedules);
        }

    }
}
