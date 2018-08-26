using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Doc
{
    [Route("api/doc/[controller]")]
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