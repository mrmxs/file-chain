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
    public class DeployTest : AbstractSmartContractTest
    {
        private UsersAndFilesService _service;

        public DeployTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
            StoredContractAddressPath = @".\SmartContract\UsersAndFilesContract\contract-address";
            StoredLibraryAddressPath = @".\SmartContract\UsersAndFilesContract\library-address";
        }

        /// <summary>
        /// Deploy contract. When creating, add admin user.
        /// Save contractAddress. 
        /// Check that admin exist in user mapping
        ///
        /// "gasUsed": "0xc1806" -> 792582
        /// 
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
                    SenderAddress, Gas);
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);

            LibraryAddress = receipt.ContractAddress;
            
            Assert.True(!string.IsNullOrEmpty(transactionHash));
            Assert.NotNull(receipt);
        }

        /// <summary>
        /// Deploy contract. When creating, add admin user.
        /// Save contractAddress. 
        /// Check that admin exist in user mapping
        /// 
        /// "gasUsed": "0x28db7d" -> 2677629
        /// 
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
                    Gas);
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);

            ContractAddress = receipt.ContractAddress;
            
            Assert.True(!string.IsNullOrEmpty(transactionHash));
            Assert.NotNull(receipt);
        }
    }
}