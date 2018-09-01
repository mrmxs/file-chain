using System;
using System.Numerics;
using System.Threading.Tasks;
using API.Models;
using API.Utils;
using EthereumLibrary.Model;
using EthereumLibrary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nethereum.Geth;
using UserDto = API.Models.UserDto;
using EV = EthereumLibrary.Helper.EnvironmentVariablesHelper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        private Web3Geth _web3;
        private string _walletAddress;
        private string _contractAddress;
        private BigInteger _gas;

        private IEthereumUserService _ethereumUserService;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;

            _web3 = new Web3Geth(_configuration["Ethereum:RPCServer"] as string);
            _contractAddress = EV.ContractAddress;
            _walletAddress = EV.WalletAddress;
            _gas = long.Parse(_configuration["Ethereum:Gas"]);
            _ethereumUserService = new EthereumUserService(_web3, _contractAddress, _walletAddress, _gas);
        }

        /// <summary>
        ///  Registation
        ///  POST api/user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserDto request)
        {
            if (string.IsNullOrEmpty(request.Login)
                || string.IsNullOrEmpty(request.Password)
                || string.IsNullOrEmpty(request.FirstName)
                || string.IsNullOrEmpty(request.LastName))
                return BadRequest(Errors.REQUIRED_FIELDS_ARE_MISSING);

            try
            {
                var user = await _ethereumUserService.AddAsyncCall(
                    request.Login, request.Password, request.FirstName, request.LastName, request.Info);

                return Ok(ConvertToDto(user));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("LOGIN ALREADY EXISTS"))
                    return BadRequest(Errors.LOGIN_ALREADY_EXISTS);

                return StatusCode(500, new ErrorDto($"{e.Message}\n{e.StackTrace}"));
            }
        }

        /// <summary>
        ///  Login
        ///  DELETE api/user
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<bool>> Delete()
        {
            try
            {
                var login = Request.Headers["X-Login"];
                var password = Request.Headers["X-Token"];

                var auth = await _ethereumUserService.IsAuthenticatedAsyncCall(login, password);

                if (!auth) return BadRequest(Errors.WRONG_CREDENTIALS);

                return Ok();
            }
            catch (Exception e)
            {
                return HandleEthereumError(e.Message, e.StackTrace);
            }
        }

        /// <summary>
        ///  GET api/user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<UserProfileDto>> Get()
        {
            try
            {
                var login = Request.Headers["X-Login"].ToString();
                var password = Request.Headers["X-Token"].ToString();

                var auth = await _ethereumUserService.IsAuthenticatedAsyncCall(login, password);

                if (!auth) return BadRequest(Errors.WRONG_CREDENTIALS);

                var user = await _ethereumUserService.GetAsyncCall(login);

                var userDto = ConvertToDto(user);
                var walletBalance = await _web3.Eth.GetBalance.SendRequestAsync(_walletAddress);
                var walletDto = user.IsAdmin
                                || !user.IsAdmin // TODO THIS IS CRUTCH TO GET WALLET - FIX & DELETE ME 
                    ? new WalletDto(_walletAddress, walletBalance)
                    : null;

                return Ok(new UserProfileDto
                {
                    user = userDto,
                    wallet = walletDto
                });
            }
            catch (Exception e)
            {
                return HandleEthereumError(e.Message, e.StackTrace);
            }
        }

        private ObjectResult HandleEthereumError(string message, string stacktrace = null)
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

            return StatusCode(500, new ErrorDto($"{message}\n{stacktrace}"));
        }

        /// <summary>
        ///  PUT api/user
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [HasHeader("X-Login,X-Token")]
        public async Task<ActionResult<UserDto>> Put([FromBody] UserDto request)
        {
            var login = Request.Headers["X-Login"];
            var password = Request.Headers["X-Token"];

            if ((string.IsNullOrEmpty(request.FirstName)
                 || string.IsNullOrEmpty(request.LastName))
                && string.IsNullOrEmpty(request.Info))
                return BadRequest(Errors.REQUIRED_FIELDS_ARE_MISSING);

            var auth = await _ethereumUserService.IsAuthenticatedAsyncCall(login, password);
            if (!auth) return BadRequest(Errors.WRONG_CREDENTIALS);

            try
            {
                if (!string.IsNullOrEmpty(request.FirstName) && !string.IsNullOrEmpty(request.LastName))
                    await _ethereumUserService.SetNameAsync(
                        login, password, request.FirstName, request.LastName, DateTime.Now);

                if (!string.IsNullOrEmpty(request.Info))
                    await _ethereumUserService.SetInfoAsync(login, password,
                        request.Info, DateTime.Now);

                var user = await _ethereumUserService.GetAsyncCall(login);

                return Ok(ConvertToDto(user));
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

                return StatusCode(500, new ErrorDto(e.Message));
            }
        }

        private UserDto ConvertToDto(IEthereumUser user)
        {
            return new UserDto
            {
                Login = user.Login,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Info = user.Info,
                IsAdmin = false
            };
        }
    }
}