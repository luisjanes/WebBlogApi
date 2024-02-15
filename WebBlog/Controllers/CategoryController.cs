using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext _context)
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "01X01 - Couldn't get this Categories");
            }

        }
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id, [FromServices] BlogDataContext _context)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound($"With Id: {id}, couldn't find any Category");
                }
                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "01X02 - Couldn't get this Category");
            }

        }
        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model,[FromServices] BlogDataContext _context)
        {
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Posts = [],
                    Name = model.Name,
                    Slug = model.Slug.ToLower(),
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", category);
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, "01X03 - Couldn't create this Category");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal faulure");
            }
        }
        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromServices] BlogDataContext _context, [FromBody]EditorCategoryViewModel model)
        {
            try
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "01X04 - Couldn't update this Category");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal faulure");
            }

        }
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext _context)
        {
            try
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "01X05 - Couldn't delete this Category");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal faulure");
            }

        }
    }
}
