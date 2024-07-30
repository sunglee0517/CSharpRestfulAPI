using Microsoft.AspNetCore.Mvc;

namespace company.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET: /Home
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to the Company API!");
        }
    }
}