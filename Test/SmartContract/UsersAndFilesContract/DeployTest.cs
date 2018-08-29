using System.IO;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Xunit;
using Stub = Test.SmartContract.Stubs.UserStorageContractTestStub;

namespace Test.SmartContract.UsersAndFilesContract
{
    public class DeployTest
    {
        private const string RcpClientUrl = "http://127.0.0.1:7545";
        private const string SenderAddress = "0x2042a043c2b9c56906ae1dbbda5b8b0d6f3ee2c3";
        private const string SenderAddress2 = "0xa9aee256679649cee48c974081a4e5bfce48d17d";
        private const string Password = "";

        private Web3Geth _web3;
        private UsersAndFilesService _service;
        private Contract _contract;
        private readonly string _storedContractAddressPath = @".\SmartContract\UsersAndFilesContract\contract-address";
        private readonly string _storedLibraryAddressPath = @".\SmartContract\UsersAndFilesContract\library-address";

        private string LibraryAddress
        {
            get => File.Exists(_storedLibraryAddressPath)
                ? File.ReadAllText(_storedLibraryAddressPath)
                : "";
            set => File.WriteAllText(_storedLibraryAddressPath, value);
        }
        private string ContractAddress
        {
            get => File.Exists(_storedContractAddressPath)
                ? File.ReadAllText(_storedContractAddressPath)
                : "";
            set => File.WriteAllText(_storedContractAddressPath, value);
        }

        public DeployTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
        }


        /// <summary>
        /// Deploy contract. When creating, add admin user.
        /// Save contractAddress. 
        /// Check that admin exist in user mapping
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T1_DeployLibrary()
        {
            // 1. Unclock Account
            var unlockRes = await _web3.Personal.UnlockAccount.SendRequestAsync(
                SenderAddress, Password, new HexBigInteger(120));
            Assert.True(unlockRes);

            // 2. Deploy library
            var transactionHash =
                await UsersAndFilesService.DeployLibraryAsync(_web3,
                    SenderAddress, new HexBigInteger(60000000000));
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);

            LibraryAddress = receipt.ContractAddress;
            
            Assert.True(!string.IsNullOrEmpty(transactionHash));
            Assert.NotNull(receipt);
        }

        /// <summary>
        /// Deploy contract. When creating, add admin user.
        /// Save contractAddress. 
        /// Check that admin exist in user mapping
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task T2_DeployContract()
        {
            // 1. Unclock Account
            var unlockRes = await _web3.Personal.UnlockAccount.SendRequestAsync(
                SenderAddress, Password, new HexBigInteger(120));
            Assert.True(unlockRes);

            // 2. Deploy contract
            // Get contract receipt & contractAddress, save contractAdress to file
            var transactionHash =
                await UsersAndFilesService.DeployContractAsync(_web3,
                    LibraryAddress,
                    SenderAddress,
                    Stub.AdminLogin,
                    Stub.AdminPassword,
                    Stub.FirstName1,
                    Stub.LastName1,
                    Stub.Info1,
                    new HexBigInteger(60000000000));
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);

            ContractAddress = receipt.ContractAddress;
            
            Assert.True(!string.IsNullOrEmpty(transactionHash));
            Assert.NotNull(receipt);
        }
    }
}