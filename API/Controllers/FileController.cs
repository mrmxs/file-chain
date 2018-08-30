using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using API.Models;
using API.Utils;
using EthereumLibrary.Model;
using EthereumLibrary.Service;
using IPFSLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nethereum.Geth;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IConfiguration _configuration;
        private IIpfsService _ipfsService;

        private IEthereumFileService _ethereumFileService;
        private IEthereumUserService _ethereumUserService;

        private string _walletAddress;
        private int _gas;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;

            _ipfsService = new IpfsService(
                _configuration["IPFS:API:host"],
                int.Parse(_configuration["IPFS:API:port"]),
                _configuration["IPFS:API:protocol"]
            );

            var web3 = new Web3Geth(_configuration["Ethereum:RPCServer"] as string);
            var contractAddress = _configuration["Ethereum:ContractAddress"] as string;
            var walletAddress = _configuration["Ethereum:Wallet:Address"] as string;
            var gas = long.Parse(_configuration["Ethereum:Gas"]);

            _ethereumFileService = new EthereumFileService(web3, contractAddress, walletAddress, gas);
            _ethereumUserService = new EthereumUserService(web3, contractAddress, walletAddress, gas);
        }


        /// <summary>
        /// GET api/file 
        /// GET api/file?type=image
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<IEnumerable<FileDto>>> Get([FromQuery(Name = "type")] string type)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            var files = await _ethereumFileService.GetAsyncCall(login, password);

            // todo filter by types

            return Ok(files.Select(ConvertToDto));
        }

        // GET api/file/5
        [HttpGet("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Get(BigInteger id)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            var file = await _ethereumFileService.GetAsyncCall(login, password, id);

            return Ok(ConvertToDto(file));
        }

        // POST api/file
        [HttpPost]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Post([FromBody] FileDto request)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            var auth = await _ethereumUserService.IsAuthenticatedAsyncCall(login, password);
            if (!auth) return BadRequest(Errors.WRONG_CREDENTIALS);

            var filePath = request.Link;

            if (!System.IO.File.Exists(filePath))
                return BadRequest(Errors.FILE_DOES_NOT_EXISTS);

            var temp = new FileDto
            {
                Name = Path.GetFileName(filePath),
                // Type = "image/jpeg", // set later
                Size = new FileInfo(filePath).Length,
                Description = request.Description,
                // Link = "", // set later
                Created = DateTime.Today,
                // Modified, // not needed
            };

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    temp.Type = MimeType.GetMimeType(fileBytes.Take(50).ToArray(), filePath);
                }
                
                var ipfsFile = _ipfsService.Add(temp.Name, fileStream);
                temp.Link = ipfsFile.Hash;
            }

            var etherFile = await _ethereumFileService.AddAsync(
                login,
                password,
                temp.Type,
                temp.Link,
                temp.Size,
                temp.Name,
                temp.Description,
                temp.Created);

            return Ok(ConvertToDto(etherFile));
        }

        // PUT api/file/5
        [HttpPut("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Put([FromBody] FileDto request)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            if (request.Id == null || (request.Name == "" && request.Description == ""))
                return BadRequest("Required fields are missing");

            try
            {
                var ids = await _ethereumFileService.GetIdsAsyncCall(login, password);

                if (!ids.Contains(request.Id.Value))
                    return StatusCode(403, Errors.INSUFFICIENT_PRIVILEGES);

                if (request.Name != "")
                    await _ethereumFileService.SetNameAsync(login, password, request.Id.Value, request.Name, DateTime.Now);
                
                if (request.Description != "")
                    await _ethereumFileService.SetDescriptionAsync(login, password, request.Id.Value, request.Description, DateTime.Now);

                var file = await _ethereumFileService.GetAsyncCall(login, password, request.Id.Value);
                
                return Ok(ConvertToDto(file));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("LOGIN DOESN'T EXIST"))
                    return BadRequest(Errors.WRONG_CREDENTIALS);

                if (e.Message.Contains("WRONG CREDENTIALS"))
                    return BadRequest(Errors.WRONG_CREDENTIALS);

                //only owner can edit
                if (e.Message.Contains("INSUFFICIENT PRIVILEGES"))
                    return StatusCode(403, Errors.INSUFFICIENT_PRIVILEGES);

                return StatusCode(500);
            }
        }

        // DELETE api/file/5
        [HttpDelete("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult> Delete(BigInteger id)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            try
            {
                var ids = await _ethereumFileService.DeleteAsync(login, password, id);
                return Ok(new SuccessDto($"File #{id} was deleted"));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("LOGIN DOESN'T EXIST"))
                    return BadRequest(Errors.WRONG_CREDENTIALS);

                if (e.Message.Contains("WRONG CREDENTIALS"))
                    return BadRequest(Errors.WRONG_CREDENTIALS);

                if (e.Message.Contains("NOT EXISTING INDEX"))
                    return BadRequest(Errors.INDEX_DOES_NOT_EXISTS);

                //only owner can delete
                if (e.Message.Contains("INSUFFICIENT PRIVILEGES"))
                    return StatusCode(403, Errors.INSUFFICIENT_PRIVILEGES);

                return StatusCode(500);
            }
        }

        private FileDto ConvertToDto(IEthereumFile file)
        {
            return new FileDto
            {
                Id = file.Id,
                Name = file.Name,
                Type = file.MimeType,
                Size = long.Parse(file.Size),
                Description = file.Description,
                Link = _ipfsService.Get(file.IpfsHash).Url,
                Created = file.Created,
                Modified = file.Modified,
            };
        }
    }
}