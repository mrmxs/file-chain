using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.File
{
    [Route("api/file/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}