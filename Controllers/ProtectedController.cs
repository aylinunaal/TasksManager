using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace taskManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProtectedController : ControllerBase
    {
        
        [HttpGet("{id:int}")]
        public IActionResult GetProtectedData()
        {
            return Ok("This is protected data");
        }
    }
}
