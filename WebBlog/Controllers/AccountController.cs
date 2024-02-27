using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using WebBlog.Data;
using WebBlog.Extensions;
using WebBlog.Models;
using WebBlog.Services;
using WebBlog.ViewModels;

namespace WebBlog.Controllers
{
    //[Authorize] coloca autenticação para todos os metodos
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] BlogDataContext _context)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            };

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("Email já cadastrado, tente fazer login"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno de servidor"));
            }
        }
        //[AllowAnonymous] para permitir que utilizem sem autenticação
        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model,
            [FromServices] BlogDataContext _context,
            [FromServices] TokenService tokenService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = await _context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida"));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor"));
            }
        }

        //[Authorize(Roles = "user")]
        //[HttpGet("v1/user")]
        //public IActionResult GetUser() => Ok(User.Identity.Name);

        //[Authorize(Roles = "author")]
        //[HttpGet("v1/author")]
        //public IActionResult GetAuthor() => Ok(User.Identity.Name);

        //[Authorize(Roles = "admin")]
        //[HttpGet("v1/admin")]
        //public IActionResult GetAdmin() => Ok(User.Identity.Name);
    }
}
