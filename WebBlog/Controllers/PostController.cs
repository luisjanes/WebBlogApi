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
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext _context)
        {
            var posts = await _context
                .Posts
                .AsNoTracking()
                .Include(x=>x.Category)
                .Include(x=>x.Author)
                //.Select(x=> new ListPostsViewModel
                //{
                //    Id = x.Id,
                //    Title = x.Title,
                //    Slug = x.Slug,
                //    LastUpdateDate = x.LastUpdateDate,
                //    Category = x.Category.Name,
                //    Author = $"{x.Author.Name} ({x.Author.Email})"
                //})
                .ToListAsync();

            return Ok(new ResultViewModel<List<Post>>(posts, null));
        }
    }
}
