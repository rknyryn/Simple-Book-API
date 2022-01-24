using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBookAPI.Data;
using SimpleBookAPI.DTOs.AuthorDTOs;
using SimpleBookAPI.Models;

namespace SimpleBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthorsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAuthors")]
        public async Task<ActionResult<IEnumerable<GetAuthorsDTO>>> GetAuthors()
        {
            var res = _context.Authors.ToList().Select(c => new GetAuthorsDTO()
            {
                Id = c.Id,
                LastName = c.LastName,
                FirstName = c.FirstName
            });
            if (res.Count() > 0)
                return Ok(res);
            return NotFound("No record found");
        }

        [HttpGet("GetAuthor/{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound("No record found");
            }

            return author;
        }

        [HttpPut("UpdateAuthor/{id}")]
        public async Task<ActionResult<CreateOrUpdateAuthorDTO>> UpdateAuthor(int id, CreateOrUpdateAuthorDTO author)
        {
            var auth = AuthorExists(id);
            if (auth == null)
                return NotFound("No record found");

            try
            {
                auth.LastName = author.LastName;
                auth.FirstName = author.FirstName;

                await _context.SaveChangesAsync();
                return Ok(auth);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CreateAuthor")]
        public async Task<ActionResult<CreateOrUpdateAuthorDTO>> CreateAuthor(CreateOrUpdateAuthorDTO author)
        {
            Author newAuthor = new Author { LastName = author.LastName, FirstName = author.FirstName };
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = newAuthor.Id }, author);
        }

        [HttpDelete("DeleteAuthorById/{id}")]
        public async Task<IActionResult> DeleteAuthorById(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private Author AuthorExists(int id)
        {
            return _context.Authors.SingleOrDefault(e => e.Id == id);
        }
    }
}
