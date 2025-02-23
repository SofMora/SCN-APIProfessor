using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ScnAdminDbContext _context;

        public CommentController(ScnAdminDbContext context)
        {
            _context = context;
        }

        // GET: api/Comment/GetAllComments
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllComments()
        {
            var comments = await _context.Comments
                .Include(c => c.IdNewsNavigation)
                .Select(comment => new
                {
                    Id = comment.Id,
                    IdNews = comment.IdNews,
                    Description = comment.Description,
                    Author = comment.Author,
                    CommentDate = comment.CommentDate,
                    News = new
                    {
                        Id = comment.IdNewsNavigation.Id,
                        Title = comment.IdNewsNavigation.Title,
                        DateNews = comment.IdNewsNavigation.DateNews
                    }
                })
                .ToListAsync();

            return Ok(comments);
        }

        // GET: api/Comment/GetCommentById/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<object>> GetCommentById(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.IdNewsNavigation)
                .Where(c => c.Id == id)
                .Select(comment => new
                {
                    Id = comment.Id,
                    IdNews = comment.IdNews,
                    Description = comment.Description,
                    Author = comment.Author,
                    CommentDate = comment.CommentDate,
                    News = new
                    {
                        Id = comment.IdNewsNavigation.Id,
                        Title = comment.IdNewsNavigation.Title,
                        DateNews = comment.IdNewsNavigation.DateNews
                    }
                })
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return NotFound($"No se encontró ningún comentario con el ID {id}.");
            }

            return Ok(comment);
        }
        // GET: api/Comment/GetCommentsByNewsId/{newsId}
        [HttpGet]
        [Route("[action]/{newsId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCommentsByNewsId(int newsId)
        {
            var comments = await _context.Comments
                .Where(c => c.IdNews == newsId)
                .Select(comment => new
                {
                    Id = comment.Id,
                    IdNews = comment.IdNews,
                    Description = comment.Description,
                    Author = comment.Author,
                    CommentDate = comment.CommentDate,
                    News = new
                    {
                        Id = comment.IdNewsNavigation.Id,
                        Title = comment.IdNewsNavigation.Title,
                        DateNews = comment.IdNewsNavigation.DateNews
                    }
                })
                .ToListAsync();

            if (comments == null || !comments.Any())
            {
                return NotFound($"No se encontraron comentarios para la noticia con ID {newsId}.");
            }

            return Ok(comments);
        }



        // POST: api/Comment/AddComment
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Comment>> AddComment(Comment comment)
        {
            try
            { 
                DateTime  fecha = DateTime.Now;
                comment.CommentDate = fecha;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return StatusCode(500, "Error al insertar el comentario.");
            }
        }

        // PUT: api/Comment/UpdateComment/{id}
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound($"No se encontró ningún comentario con el ID {id}.");
            }

            existingComment.IdNews = comment.IdNews;
            existingComment.Description = comment.Description;
            existingComment.Author = comment.Author;
            existingComment.CommentDate = comment.CommentDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ocurrió un error de concurrencia al actualizar el comentario.");
            }

            return NoContent();
        }

        // DELETE: api/Comment/DeleteComment/{id}
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound($"No se encontró ningún comentario con el ID {id}.");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
