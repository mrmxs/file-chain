using System;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using EthereumLibrary.Helper;
using EthereumLibrary.Model;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;

namespace EthereumLibrary.Service
{
    public class EthereumUserService : IEthereumUserService
    {
        private UsersAndFilesService _contractService;
        private string _walletAddress;
        private HexBigInteger _gas;

        public EthereumUserService(Web3Geth web3, string address, string walletAddress, BigInteger gas)
        {
            _contractService = new UsersAndFilesService(web3, address);
            _walletAddress = walletAddress;
            _gas = new HexBigInteger(gas);
        }

        public async Task<IEthereumUser> GetAsyncCall(string login)
        {
            var user = await _contractService.GetAsyncCall(CastHelper.StringToBytes32(login));

            return user.ToReadable();
        }

        public async Task<bool> IsAuthenticatedAsyncCall(string login, string password)
        {
            var byteLogin = CastHelper.StringToBytes32(login);
            var bytePassword = CastHelper.StringToBytes32(password);

            try
            {
                var responce = await _contractService.GetFileIdsAsyncCall(byteLogin, bytePassword);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEthereumUser> AddAsyncCall(string login, string password, string firstName, string lastName,
            string info)
        {
            var param = new
            {
                Login = CastHelper.StringToBytes32(login),
                Password = CastHelper.StringToBytes32(password),
                FirstName = CastHelper.ToUserNameType(firstName),
                LastName = CastHelper.ToUserNameType(lastName),
                Info = CastHelper.ToDescriptionType(info ?? ""),
            };

            // send call to get output value 
            var result = await _contractService.AddAsyncCall(
                param.Login, param.Password, param.FirstName, param.LastName, param.Info);

            // send transaction & wait it to be mined
            var transactionHash = await _contractService.AddAsync(_walletAddress,
                param.Login, param.Password, param.FirstName, param.LastName, param.Info,
                _gas
            );
            var receipt = await _contractService.MineAndGetReceiptAsync(transactionHash);

            var user = await _contractService.GetAsyncCall(param.Login);

            return user.ToReadable();
        }

        public async Task<bool> SetNameAsync(string login, string password, string firstName, string lastName,
            DateTime now)
        {
            var param = new
            {
                Login = CastHelper.StringToBytes32(login),
                Password = CastHelper.StringToBytes32(password),
                FirstName = CastHelper.ToUserNameType(firstName),
                LastName = CastHelper.ToUserNameType(lastName),
            };

            var transactionHash = await _contractService.SetNameAsync(
                _walletAddress, param.Login, param.Password, param.FirstName, param.LastName, _gas);

            var receipt = await _contractService.MineAndGetReceiptAsync(transactionHash);

            return true;
        }

        public async Task<bool> SetInfoAsync(string login, string password, string info, DateTime now)
        {
            var param = new
            {
                Login = CastHelper.StringToBytes32(login),
                Password = CastHelper.StringToBytes32(password),
                Info = CastHelper.ToDescriptionType(info),
            };

            var transactionHash = await _contractService.SetInfoAsync(
                _walletAddress, param.Login, param.Password, param.Info, _gas);

            var receipt = await _contractService.MineAndGetReceiptAsync(transactionHash);

            return true;
        }
    }
}