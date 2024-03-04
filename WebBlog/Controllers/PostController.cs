using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;
using WebBlog.ViewModels.Posts;

namespace WebBlog.Controllers
{
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext _context, 
            [FromQuery]int page = 0, 
            [FromQuery]int pageSize = 20)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    //.Select(x=> new ListPostsViewModel
                    //{
                    //    Id = x.Id,
                    //    Title = x.Title,
                    //    Slug = x.Slug,
                    //    LastUpdateDate = x.LastUpdateDate,
                    //    Category = x.Category.Name,
                    //    Author = $"{x.Author.Name} ({x.Author.Email})"
                    //})
                    .OrderByDescending(x => x.LastUpdateDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor"));
            }

        }
        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
        [FromServices] BlogDataContext _context,
        [FromRoute]int id)
        {
            try
            {
                var post = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x=>x.Id == id);
                if (post == null)
                    return NotFound(new ResultViewModel<string>("Post não encontrado"));

                return Ok(new ResultViewModel<Post>(post));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor"));
            }

        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromServices] BlogDataContext _context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(x=>x.Category.Slug == category)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor"));
            }

        }
    }
}
