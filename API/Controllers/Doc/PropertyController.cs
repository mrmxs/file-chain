using System;
using System.Collections.Generic;
using API.Models.Doc;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Doc
{
    [Route("api/doc/[controller]")]
    [ApiController]
    public class FileInfoController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<PropertyDto>> Get()
        {
            return Ok(new[] {PropertyDto.Stub(), PropertyDto.Stub()});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<PropertyDto> Get(int id)
        {
            return Ok(PropertyDto.Stub());
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] PropertyDto value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PropertyDto value)
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