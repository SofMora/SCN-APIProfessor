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
    public class CommentConsultController : ControllerBase
    {
        private readonly ScnAppointmentsDbContext _context;

        public CommentConsultController(ScnAppointmentsDbContext context)
        {
            _context = context;
        }

        // GET: api/CommentConsult/GetAllComments
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<CommentConsult>>> GetAllComments()
        {
            return await _context.CommentConsults.Include(c => c.IdConsultNavigation).ToListAsync();
        }

        // GET: api/CommentConsult/GetCommentById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<CommentConsult>> GetCommentById(int id)
        {
            var comment = await _context.CommentConsults
                .Include(c => c.IdConsultNavigation)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
        [HttpGet("GetReplies/{consultId}")]
        public IActionResult GetReplies(int consultId)
        {
            try
            {
                var comments = _context.CommentConsults
                    .Where(c => c.IdConsult == consultId)
                    .OrderBy(c => c.DateComment)
                    .ToList();

                if (comments == null || comments.Count == 0)
                {
                    return NotFound("No replies found for this consultation.");
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST: api/CommentConsult/AddComment
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<CommentConsult>> AddComment(CommentConsult comment)
        {
            _context.CommentConsults.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        // PUT: api/CommentConsult/UpdateComment/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentConsult comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingComment = await _context.CommentConsults.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound($"No se encontró el comentario con el ID {id}.");
            }

            existingComment.IdConsult = comment.IdConsult;
            existingComment.DescriptionComment = comment.DescriptionComment;
            existingComment.Author = comment.Author;
            existingComment.DateComment = comment.DateComment;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar el comentario.");
            }

            return NoContent();
        }

        // DELETE: api/CommentConsult/DeleteComment/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.CommentConsults.FindAsync(id);
            if (comment == null)
            {
                return NotFound($"No se encontró el comentario con el ID {id}.");
            }

            _context.CommentConsults.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
