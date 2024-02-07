using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext _context)
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id, [FromServices] BlogDataContext _context)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (category == null)
            {
                return NotFound($"With Id: {id}, couldn't find any Category");
            }
            return Ok(category);
        }
        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] Category model,[FromServices] BlogDataContext _context)
        {
            try
            {
                await _context.Categories.AddAsync(model);
                await _context.SaveChangesAsync();
                return Created($"v1/categories/{model.Id}", model);
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, "01X01 - Couldn't create this Category");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal faulure");
            }
        }
        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromServices] BlogDataContext _context, [FromBody]Category model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound($"With Id: {id}, couldn't find any Category");
            }
            category.Name = model.Name;
            category.Slug = model.Slug;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return Ok($"The updates are made {category}");
        }
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext _context)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound($"With Id: {id}, couldn't find any Category");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok($"{category.Name} was deleted");
        }
    }
}
