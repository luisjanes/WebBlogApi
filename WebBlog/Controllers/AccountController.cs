using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Services;

namespace WebBlog.Controllers
{
    //[Authorize] coloca autenticação para todos os metodos
    [ApiController]
    public class AccountController : ControllerBase
    {
        //[AllowAnonymous] para permitir que utilizem sem autenticação
        [HttpPost("v1/login")]
        public IActionResult Login([FromServices]TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);

            return Ok(token);
        }

        [Authorize(Roles = "user")]
        [HttpGet("v1/user")]
        public IActionResult GetUser() => Ok(User.Identity.Name);

        [Authorize(Roles = "author")]
        [HttpGet("v1/author")]
        public IActionResult GetAuthor() => Ok(User.Identity.Name);

        [Authorize(Roles = "admin")]
        [HttpGet("v1/admin")]
        public IActionResult GetAdmin() => Ok(User.Identity.Name);
    }
}
