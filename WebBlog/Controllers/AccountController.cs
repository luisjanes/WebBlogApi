﻿using Microsoft.AspNetCore.Mvc;
using WebBlog.Services;

namespace WebBlog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/login")]
        public IActionResult Login([FromServices]TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);

            return Ok(token);
        }
    }
}
