using Microsoft.AspNetCore.Mvc;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(new
            {
                Name = "TiltanesAPI",
                Version = "1.0.0",
            });
        }
    }
}
