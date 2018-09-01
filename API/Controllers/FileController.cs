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
using EV = EthereumLibrary.Helper.EnvironmentVariablesHelper;

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

            //todo get secure info from Environment Variables 
            var web3 = new Web3Geth(_configuration["Ethereum:RPCServer"] as string);
            var contractAddress = EV.ContractAddress;
            var walletAddress = EV.WalletAddress;
            var gas = long.Parse(_configuration["Ethereum:Gas"]);

            _ethereumFileService = new EthereumFileService(web3, contractAddress, walletAddress, gas);
            _ethereumUserService = new EthereumUserService(web3, contractAddress, walletAddress, gas);
        }


        /// <summary>
        /// GET api/file 
        /// GET api/file?type=image
        /// 
        /// Known file types:
        /// "image", "audio", "video", "document", "archive"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<IEnumerable<FileDto>>> Get([FromQuery(Name = "type")] string type)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            IEnumerable<IEthereumFile> files;
            try
            {
                files = await _ethereumFileService.GetAsyncCall(login, password);
            }
            catch (Exception e)
            {
                return HandleEthereumError(e.Message);
            }

            var filtered = (string.IsNullOrEmpty(type) || !MimeType.knownTypes.Contains(type))
                ? files
                : files.Where(file =>MimeType.IsCategory(file.MimeType, type.ToLower()));

            return Ok(filtered.Select(ConvertToDto));
        }

        // GET api/file/5
        [HttpGet("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Get(long id)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            try
            {
                var file = await _ethereumFileService.GetAsyncCall(login, password, id);
                return Ok(ConvertToDto(file));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("invalid opcode"))return NotFound();
                        
                return HandleEthereumError(e.Message);
            }
        }

        // POST api/file
        [HttpPost]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Post([FromBody] FileDto request)
        {
            if (string.IsNullOrEmpty(request.Link))
                return BadRequest(Errors.REQUIRED_FIELDS_ARE_MISSING);

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
                Size = new FileInfo(filePath).Length,
                Description = request.Description ?? "",
                Created = DateTime.Now,
                // Type = "image/jpeg", // set later
                // Link = "",           // set later
                // Modified,            // not needed
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

            try
            {
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
            catch (Exception e)
            {
                return HandleEthereumError(e.Message);
            }
        }

        // PUT api/file/5
        [HttpPut("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<FileDto>> Put([FromBody] FileDto request)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];
            BigInteger id;
            try
            {
                id = new BigInteger(long.Parse(RouteData.Values["id"].ToString()));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (request.Name == "" && request.Description == "")
                return BadRequest(Errors.REQUIRED_FIELDS_ARE_MISSING);
            
            try
            {
                var ids = await _ethereumFileService.GetIdsAsyncCall(login, password);

                if (!ids.Contains(id))
                    return StatusCode(403, Errors.INSUFFICIENT_PRIVILEGES);

                if (!string.IsNullOrEmpty(request.Name))
                    await _ethereumFileService.SetNameAsync(
                        login, password, id, request.Name, DateTime.Now);

                if (!string.IsNullOrEmpty(request.Description))
                    await _ethereumFileService.SetDescriptionAsync(
                        login, password, id, request.Description, DateTime.Now);

                var file = await _ethereumFileService.GetAsyncCall(login, password, id);

                return Ok(ConvertToDto(file));
            }
            catch (Exception e)
            {
                return HandleEthereumError(e.Message);
            }
        }

        // DELETE api/file/5
        [HttpDelete("{id}")]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult> Delete(long id)
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
                return HandleEthereumError(e.Message);
            }
        }

        private ObjectResult HandleEthereumError(string message)
        {
            if (message.Contains("LOGIN DOESN'T EXIST"))
                return BadRequest(Errors.WRONG_CREDENTIALS);

            if (message.Contains("WRONG CREDENTIALS"))
                return BadRequest(Errors.WRONG_CREDENTIALS);

            if (message.Contains("NOT EXISTING INDEX"))
                return BadRequest(Errors.INDEX_DOES_NOT_EXISTS);

            //only owner can edit
            if (message.Contains("INSUFFICIENT PRIVILEGES"))
                return StatusCode(403, Errors.INSUFFICIENT_PRIVILEGES);

            return StatusCode(500, new ErrorDto(message));
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