using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.File;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<FileDto>> Get()
        {
            return new[] {FileDto.Stub(), FileDto.Stub()};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<FileDto> Get(int id)
        {
            return FileDto.Stub();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] FileDto value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FileDto value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}