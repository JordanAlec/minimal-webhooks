using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        private readonly ILogger<Home> _logger;

        public Home(ILogger<Home> logger) => _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get() => 
            Ok(new { status = true, timeStamp = DateTime.Now });
    }
}