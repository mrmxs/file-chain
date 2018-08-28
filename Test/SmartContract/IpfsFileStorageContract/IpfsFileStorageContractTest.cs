using System;
using System.IO;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using EthereumLibrary.Helper;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Xunit;

namespace Test.SmartContract.IpfsFileStorageContract
{
    public class IpfsFileStorageContractTest
    {
        private const string RcpClientUrl = "http://127.0.0.1:7545";
        private const string SenderAddress = "0xe24570bde9c5b02015d15cf491b5594f99f4db8c";
        private const string Password = "";

        private Web3Geth _web3;
        private IpfsFileStorageService _service;
        private Contract _contract;
        private readonly string _storedContractAddressPath = @".\SmartContract\IpfsFileStorageContract\contract-address";

        private string ContractAddress
        {
            get => File.Exists(_storedContractAddressPath)
                ? File.ReadAllText(_storedContractAddressPath)
                : "";
            set => File.WriteAllText(_storedContractAddressPath, value);
        }

        public IpfsFileStorageContractTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
        }

        [Fact]
        public async Task T1_DeployContractAndCallFunction()
        {
            // 1. Unclock Account
            var unlockRes =
                await _web3.Personal.UnlockAccount.SendRequestAsync(SenderAddress, Password, new HexBigInteger(120));
            Assert.True(unlockRes);

            // 2. Deploy contract
            var transactionHash =
                await IpfsFileStorageService.DeployContractAsync(_web3, SenderAddress, new HexBigInteger(1000000));

            // 3. Get contract receipt & contractAddress
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            // 4. Call contract function
            ContractAddress = receipt.ContractAddress;

            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);

            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var responce = await _service.ContainsAsyncCall(0);
            Assert.False(responce);
        }

        [Fact]
        public async Task T2_AddFileFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var param = new
            {
                Mime = CastHelper.StringToBytes32("image/jpeg"),
                Hash = CastHelper.StringToBytes32ArrayOf(2, "obsjdkfhfowejkj"),
                Size = CastHelper.StringToBytes32(125000.ToString()),
                Name = CastHelper.StringToBytes32ArrayOf(3, "testimage.jpeg"),
                Description = CastHelper.StringToBytes32ArrayOf(6, "this is test image"),
                Timestamp = (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            // send call to get output value 
            var fileIndex = await _service.AddAsyncCall(
                param.Mime, param.Hash, param.Size, param.Name, param.Description, param.Timestamp);
            Assert.True(fileIndex != 0);

            // send transaction & wait it to be mined
            var transactionHash = await _service.AddAsync(SenderAddress,
                param.Mime, param.Hash, param.Size, param.Name, param.Description, param.Timestamp,
                new HexBigInteger(500000)
            );
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);

            var responce = await _service.ContainsAsyncCall(fileIndex);

            Assert.True(responce);
        }

        [Fact]
        public async Task T3_ContainsFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var responce = await _service.ContainsAsyncCall(1);

            Assert.True(responce);
        }

        [Fact]
        public async Task T4_GetFileFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var fileContent = await _service.GetAsyncCall(2);

            //TODO string conversion
            Assert.Equal("testimage.jpeg", fileContent.ToReadable().Name);
        }

        [Fact]
        public async Task T5_SetNameFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var id = 1;
            var old = await _service.GetAsyncCall(id);
            
            var newName = "new-image-name.jpeg";
            var timestamp = (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            var transactionHash = await _service.SetNameAsync(SenderAddress,
                1, CastHelper.StringToBytes32ArrayOf(3, newName), timestamp,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);
            
            var updated = await _service.GetAsyncCall(id);

            Assert.Equal(newName, updated.ToReadable().Name);
            Assert.Equal(timestamp, updated.Modified);
        }

        [Fact]
        public async Task T6_SetDescriptionFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, ContractAddress);
            _service = new IpfsFileStorageService(_web3, ContractAddress);

            var id = 1;
            var old = await _service.GetAsyncCall(id);
            
            var newDescription = "this image really makes sence";
            var timestamp = (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var transactionHash = await _service.SetDescriptionAsync(SenderAddress,
                1, CastHelper.StringToBytes32ArrayOf(6, newDescription), timestamp,
                new HexBigInteger(500000));
            var receipt = await _service.MineAndGetReceiptAsync(transactionHash);
            
            var updated = await _service.GetAsyncCall(1);

            Assert.Equal(newDescription, updated.ToReadable().Description);
            Assert.Equal(timestamp, updated.Modified);
        }
    }
}