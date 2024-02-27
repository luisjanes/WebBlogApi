using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebBlog.Attributes;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        //[ApiKey]
        public IActionResult Get()
        {
            return Ok(new
            {
                Name = "TiltanesAPI",
                Version = "1.0.1",
            });
        }
    }
}
