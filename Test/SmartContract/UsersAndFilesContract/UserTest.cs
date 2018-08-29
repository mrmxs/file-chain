using System;
using System.IO;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using EthereumLibrary.Helper;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Xunit;
using Stub = Test.SmartContract.Stubs.UserStorageContractTestStub;

namespace Test.SmartContract.UsersAndFilesContract
{
    public class UserTest
    {
        private const string RcpClientUrl = "http://127.0.0.1:7545";
        private const string SenderAddress = "0xe108b9f29929287fa7522daecea7b79d8ed6fdd6";
        private const string SenderAddress2 = "0xa9aee256679649cee48c974081a4e5bfce48d17d";
        private const string Password = "";

        private Web3Geth _web3;
        private UsersAndFilesService _service;
        private Contract _contract;
        private readonly string _storedContractAddressPath = @".\SmartContract\UsersAndFilesContract\contract-address";

        private string ContractAddress
        {
            get => File.Exists(_storedContractAddressPath)
                ? File.ReadAllText(_storedContractAddressPath)
                : "";
            set => File.WriteAllText(_storedContractAddressPath, value);
        }

        public UserTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
        }

        /// <summary>
        /// Contains existing user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T2_ContainsFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var responce = await _service.ContainsAsyncCall(Stub.AdminLogin);

            Assert.True(responce);
        }

        /// <summary>
        /// Add new user to storage
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T3_AddFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var param = new
            {
                Login = Stub.UserLogin1,
                Password = Stub.UserPassword1,
                FirstName = Stub.FirstName1,
                LastName = Stub.LastName1,
                Info = Stub.Info1,
            };

            // send call to get output value 
            var result = await _service.AddAsyncCall(
                param.Login, param.Password, param.FirstName, param.LastName, param.Info);
            Assert.True(result);

            // send transaction & wait it to be mined
            var transactionHash = await _service.AddAsync(SenderAddress,
                param.Login, param.Password, param.FirstName, param.LastName, param.Info,
                new HexBigInteger(500000)
            );
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            var responce = await _service.ContainsAsyncCall(Stub.UserLogin1);

            Assert.True(responce);
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T4_GetFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var user = await _service.GetAsyncCall(Stub.UserLogin1);

            Assert.Equal(
                CastHelper.Bytes32ArrayToString(Stub.FirstName1),
                user.ToReadable().FirstName);
            Assert.False(user.IsAdmin);
        }


        /// <summary>
        /// Change username
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T5_SetNameFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var old = await _service.GetAsyncCall(Stub.UserLogin1);

            var transactionHash = await _service.SetNameAsync(SenderAddress,
                Stub.UserLogin1,
                Stub.UserPassword1,
                Stub.FirstName2,
                Stub.LastName2,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            var updated = await _service.GetAsyncCall(Stub.UserLogin1);

            Assert.Equal(
                CastHelper.Bytes32ArrayToString(Stub.FirstName2),
                updated.ToReadable().FirstName);
            Assert.Equal(
                CastHelper.Bytes32ArrayToString(Stub.LastName2),
                updated.ToReadable().LastName);
        }


        /// <summary>
        /// Change info
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T6_SetInfoFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var old = await _service.GetAsyncCall(Stub.UserLogin1);

            var transactionHash = await _service.SetInfoAsync(SenderAddress,
                Stub.UserLogin1,
                Stub.UserPassword1,
                Stub.Info2,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            var updated = await _service.GetAsyncCall(Stub.UserLogin1);

            Assert.Equal(
                CastHelper.Bytes32ArrayToString(Stub.Info2),
                updated.ToReadable().Info);
        }


        /// <summary>
        /// Change password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T7_SetPasswordFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            // trying change password, should be Ok  
            var transactionHash = await ChangePassFromTo(Stub.UserPassword1, Stub.UserPassword2);
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            // trying to execute function with using of new password, should be Ok  
            transactionHash = await ChangePassFromTo(Stub.UserPassword2, Stub.UserPassword1); // switched to 1 password
            receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            Assert.False(string.IsNullOrEmpty(transactionHash));

        }

        private async Task<string> ChangePassFromTo(byte[] oldPassword, byte[] newPassword)
        {
            return await _service.SetPasswordAsync(SenderAddress,
                Stub.UserLogin1, oldPassword, newPassword,
                new HexBigInteger(500000));
        }


        /// <summary>
        /// Admin changes pasword to another user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T8_SetPasswordAsAdminFunction_Accessed()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            // 1. user is admin
            var user = await _service.GetAsyncCall(Stub.AdminLogin);
            Assert.True(user.IsAdmin);

            // 2. admin trying change password, should be Ok  
            var transactionHash = await _service.SetPasswordAsAdminAsync(SenderAddress,
                Stub.AdminLogin, Stub.AdminPassword, Stub.UserLogin1, Stub.UserPassword2,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            Assert.False(string.IsNullOrEmpty(transactionHash));

            // 3. user tries to execute function with using of new password
            transactionHash = await ChangePassFromTo(Stub.UserPassword2, Stub.UserPassword1); // switched to 1 password
            receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            Assert.False(string.IsNullOrEmpty(transactionHash));
        }


        /// <summary>
        /// Not admins cannot change password to other users
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T9_SetPasswordAsAdminFunction_Denied()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            // 1. user is not admin
            var user = await _service.GetAsyncCall(Stub.UserLogin1);
            Assert.False(user.IsAdmin);

            // 2. not admin user trying change password of the another user
            // should be exception 
            
            Assert.Throws<AggregateException>(() => INSUFFICIENT_PRIVILEGES().Wait());
        }

        private async Task INSUFFICIENT_PRIVILEGES()
        {
            var transactionHash = await _service.SetPasswordAsAdminAsync(SenderAddress,
                Stub.UserLogin1, Stub.UserPassword1, Stub.AdminLogin, Stub.UserPassword2,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);
        }
        

        /// <summary>
        /// Set/unset admin role
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T10_SetAdminFunction()
        {
            _contract = _web3.Eth.GetContract(UsersAndFilesService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            // 1. user is not admin
            var user = await _service.GetAsyncCall(Stub.UserLogin1);
            Assert.False(user.IsAdmin);

            // 2. admin grants user admin privilege
            var transactionHash = await SetAdmin(Stub.UserLogin1, true);
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            // 3. user is admin
            user = await _service.GetAsyncCall(Stub.UserLogin1);
            Assert.True(user.IsAdmin);

            // switch back to not admin
            transactionHash = await SetAdmin(Stub.UserLogin1, false);
            receipt = await _service.MineAndGetReceiptAsync(transactionHash);
        }

        private async Task<string> SetAdmin(byte[] user, bool isAdmin)
        {
            return await _service.SetAdminAsync(SenderAddress,
                Stub.AdminLogin, Stub.AdminPassword, user, isAdmin,
                new HexBigInteger(500000));
        }


        /// <summary>
        /// Access from other wallets denied
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T11_AccessFromOtherWalletDenied()
        {
        }
    }
}