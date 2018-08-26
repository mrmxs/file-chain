using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Doc;
using API.Models.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Wallet
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<WalletDto>> Get()
        {
            return new[] {WalletDto.Stub(), WalletDto.Stub()};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<WalletDto> Get(int id)
        {
            return WalletDto.Stub();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] WalletDto value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WalletDto value)
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