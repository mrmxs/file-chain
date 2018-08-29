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
    public class FileTest : AbstractSmartContractTest
    {
        private UsersAndFilesService _service;

        public FileTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
            StoredContractAddressPath = @".\SmartContract\UsersAndFilesContract\contract-address";
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