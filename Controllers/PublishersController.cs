using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBookAPI.Data;
using SimpleBookAPI.DTOs.PublisherDTOs;
using SimpleBookAPI.Models;

namespace SimpleBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly DataContext _context;

        public PublishersController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet("GetPublishers")]
        public ActionResult<IEnumerable<GetPublishersDTO>> GetPublishers()
        {
            var res = _context.Publishers.ToList().Select(c => new GetPublishersDTO() { Id = c.Id, Name = c.Name });
            if (res.Count() > 0)
                return Ok(res);
            return NotFound("No record found");
        }

        [HttpPost("CreatePublisher")]
        public async Task<ActionResult<CreateOrUpdatePublisherDTO>> CreatePublisher(CreateOrUpdatePublisherDTO publisher)
        {
            try
            {
                await _context.AddAsync(new Publisher() { Name = publisher.Name });
                await _context.SaveChangesAsync();
                return Ok(publisher);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdatePublisher/{id}")]
        public async Task<ActionResult<Category>> UpdatePublisher(int id, CreateOrUpdatePublisherDTO publisher)
        {
            var res = _context.Publishers.SingleOrDefault(x => x.Id == id);
            if (res == null)
                return NotFound("No record found");
            res.Name = publisher.Name;
            await _context.SaveChangesAsync();
            return Ok(res);
        }

        [HttpDelete("DeletePublisherById/{id}")]
        public async Task<ActionResult> DeletePublisher(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
                return NotFound("No record found");
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
