using Microsoft.AspNetCore.Mvc;

namespace MyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from .NET 8 API!" }); 
        }
    }
}
