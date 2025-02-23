using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ScnAppointmentsDbContext _context;

        public AppointmentController(ScnAppointmentsDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointment/GetAllAppointments
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointment/GetAppointmentById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        // POST: api/Appointment/AddAppointment
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Appointment>> AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("La cita no puede ser nula.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }

        // PUT: api/Appointment/UpdateAppointment/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest("El ID de la cita no coincide.");
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound($"No se encontró la cita con el ID {id}.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound($"No se encontró la cita con el ID {id}.");
            }

            appointment.StatusAppointment = request.statusAppointment;
            appointment.CommentStatus = request.commentStatus; // Guarda la razón de la decisión

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la cita.");
            }

            return NoContent();
        }

        // Modelo para recibir el estado y la razón
        public class UpdateStatusRequest
        {
            public bool statusAppointment { get; set; }
            public string commentStatus { get; set; } = string.Empty;
        }



        // DELETE: api/Appointment/DeleteAppointment/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound($"No se encontró la cita con el ID {id}.");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        // GET: api/Appointment/GetAppointmentsByProfessor/{professorId}

        [HttpGet]
        [Route("[action]/{idProfessor}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByProfessor(int idProfessor)
        {
            var appointments = await _context.Appointments
                                            .Where(a => a.IdProfessor == idProfessor)
                                            .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound($"No se encontraron citas para el profesor con ID {idProfessor}.");
            }

            return appointments;
        }

    }
}
