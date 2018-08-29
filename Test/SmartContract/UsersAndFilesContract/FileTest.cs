using System;
using System.IO;
using System.Numerics;
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
    public class FileTest
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

        public FileTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
        }
        
        
        /// <summary>
        /// Get File
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T4_GetFileFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new UsersAndFilesService(_web3, ContractAddress);

            var fileContent = await _service.GetFilePart1AsyncCall(
                Stub.AdminLogin,Stub.AdminPassword,new BigInteger(1));

            Assert.Equal("jpeg", fileContent.MimeType);
        }

    }
}