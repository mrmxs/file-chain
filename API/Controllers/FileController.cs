using System;
using System.Collections.Generic;
using System.IO;
using API.Models;
using IPFSLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IConfiguration _configuration;
        private IIpfsService _ipfsService;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
            _ipfsService = new IpfsService(
                _configuration["IPFS:API:host"],
                int.Parse(_configuration["IPFS:API:port"]),
                _configuration["IPFS:API:protocol"]
            );
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<FileDto>> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<FileDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] FileDto file)
        {
            var filePath = file.Link;

            if (System.IO.File.Exists(filePath)) return BadRequest("Wrong file path");

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var fileName = Guid.NewGuid().ToString();

                var ipfsFile = _ipfsService.Add(fileName, fileStream);

                return Ok(ipfsFile);
            }
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