using APIProfessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIProfessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewController : ControllerBase
    {
        private readonly ScnAdminDbContext _context;

        public NewController(ScnAdminDbContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAllNews()
        {
            var newsList = await _context.News
                .Include(n => n.TypeNewsNavigation)
                .Include(n => n.Comments) // Incluir los comentarios relacionados
                .ToListAsync();

            if (newsList == null || newsList.Count == 0)
            {
                return NotFound("No se encontraron noticias.");
            }

            return Ok(newsList);
        }

        // GET: api/News/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            var news = await _context.News
                .Include(n => n.TypeNewsNavigation)
                .Include(n => n.Comments) // Incluir los comentarios relacionados
                .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
            {
                return NotFound($"No se encontró la noticia con el ID {id}.");
            }

            return Ok(news);
        }

        // POST: api/News
        [HttpPost]
        public async Task<ActionResult<News>> AddNews(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNewsById), new { id = news.Id }, news);
        }

        // PUT: api/News/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] News news)
        {
            if (id != news.Id)
            {
                return BadRequest("El ID del cuerpo no coincide con el de la ruta.");
            }

            var existingNews = await _context.News.FindAsync(id);
            if (existingNews == null)
            {
                return NotFound($"No se encontró la noticia con el ID {id}.");
            }

            existingNews.Title = news.Title;
            existingNews.Author = news.Author;
            existingNews.TextNews = news.TextNews;
            existingNews.DateNews = news.DateNews;
            existingNews.Images = news.Images;
            existingNews.TypeNews = news.TypeNews;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar la noticia.");
            }

            return NoContent();
        }

        // DELETE: api/News/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound($"No se encontró la noticia con el ID {id}.");
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
