using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBookAPI.Data;
using SimpleBookAPI.DTOs.CategoryDTOs;
using SimpleBookAPI.Models;

namespace SimpleBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet("GetCategories")]
        public ActionResult<IEnumerable<GetCategoriesDTO>> GetCategories()
        {
            var res = _context.Categories.ToList().Select(c => new GetCategoriesDTO() { Id = c.Id, Name = c.Name });
            if (res.Count() > 0)
                return Ok(res);
            return NotFound("No record found");
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CreateOrUpdateCategoryDTO>> CreateCategory(CreateOrUpdateCategoryDTO category)
        {
            try
            {
                await _context.AddAsync(new Category() { Name = category.Name });
                await _context.SaveChangesAsync();
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<CreateOrUpdateCategoryDTO>> UpdateCategory(int id, CreateOrUpdateCategoryDTO category)
        {
            var res = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (res == null)
                return NotFound("No record found");
            res.Name = category.Name;
            await _context.SaveChangesAsync();
            return Ok(res);
        }

        [HttpDelete("DeleteCategoryById/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound("No record found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
