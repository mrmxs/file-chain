using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Xunit;

namespace Test.SmartContract.IpfsFileStorageContract
{
    public class IpfsFileStorageContractTest
    {
        private const string RcpClientUrl = "http://127.0.0.1:7545";
        private const string SenderAddress = "0xf717d22f1c5e6a91d442e1a7efa7662bda707c5f";
        private const string Password = "";
        private string _contractAddress = "0xb203c9daadcdeb0b6ee0e9e498ef1bbbffe1d402";
        
        private Web3Geth _web3;
        private IpfsFileStorageService _service;
//        private readonly string _abi = File.ReadAllText(@"D:\Dev\DocChain\Test\SmartContract\IpfsFileStorageContract\IpfsFileStorage.abi");
//        private readonly string _byteCode = File.ReadAllText(@"D:\Dev\DocChain\Test\SmartContract\IpfsFileStorageContract\IpfsFileStorage.bin");
        private Contract _contract;

        public IpfsFileStorageContractTest()
        {
            _web3 = new Web3Geth(RcpClientUrl);
            _service = new IpfsFileStorageService(_web3,_contractAddress);
        }
        
        [Fact]
        public async Task T1_DeployContractAndCallFunction()
        {
            // 1. Unclock Account
            var unlockRes = await _web3.Personal.UnlockAccount.SendRequestAsync(SenderAddress, Password, new HexBigInteger(120));
            Assert.True(unlockRes);
            
            // 2. Deploy contract
            var trnsHash = await IpfsFileStorageService.DeployContractAsync(_web3, SenderAddress, new HexBigInteger(1000000));
            
            var mineRes = await _web3.Miner.Start.SendRequestAsync(6);
            Assert.True(mineRes);
            
            // 3. Get contract receipt 
            var receipt =  await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trnsHash);
            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt =  await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trnsHash);
            }

//            // 2. Deploy contract & Get contract receipt 
//            var receipt =
//                await web3.Eth.DeployContract
//                     .SendRequestAndWaitForReceiptAsync(abi, byteCode, senderAddress, new HexBigInteger(900), null, multiplier);

            // 4. Call contract function
            _contractAddress = receipt.ContractAddress;

            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, _contractAddress);

            var fileIndex = await _service.AddIpfsFileToStorageAsyncCall(
                "image/jpeg",       
                125000, 
                "obsjdkfhfowejkj", 
                "testimage.jpeg", 
                "this is test image"
                );
            Assert.NotEqual("0", fileIndex.ToString());
        }

        [Fact]
        public async Task T2_AddFileFunction()
        {
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, _contractAddress);
            
            var fileIndex = await _service.AddIpfsFileToStorageAsyncCall(
                "image/jpeg",       
                125000, 
                "obsjdkfhfowejkj", 
                $"{Guid.NewGuid()}.jpeg", 
                "this is test image"
            );
            Assert.NotEqual("0", fileIndex.ToString());
            
        }

        [Fact]
        public async Task T3_GetFileFunction()
        {
            var fileIndex = 1;
            _contract = _web3.Eth.GetContract(IpfsFileStorageService.Abi, _contractAddress);
            
            var fileContent = await _service.GetIpfsFileAsync(SenderAddress, fileIndex, new HexBigInteger(250000));
            Assert.NotEqual("", fileContent);
            
        }
    }
}