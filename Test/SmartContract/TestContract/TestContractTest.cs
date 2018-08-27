using System.Threading;
using System.Threading.Tasks;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Xunit;

namespace Test.SmartContract.TestContract
{
    public class TestContractTest
    {
        [Fact]
        public async Task DeployContractAndCallFunction()
        {
            var rcpClientUrl = "http://127.0.0.1:7545";
            var senderAddress = "0xf717d22f1c5e6a91d442e1a7efa7662bda707c5f";
            var password = "";
            var abi = @"[{'constant':true,'inputs':[{'name':'val','type':'int256'}],'name':'multiply','outputs':[{'name':'d','type':'int256'}],'payable':false,'stateMutability':'view','type':'function'},{'inputs':[{'name':'multiplier','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";
            var byteCode = "0x608060405234801561001057600080fd5b506040516020806100d08339810160405251600055609d806100336000396000f300608060405260043610603e5763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416631df4f14481146043575b600080fd5b348015604e57600080fd5b506058600435606a565b60408051918252519081900360200190f35b60005402905600a165627a7a723058203474d84d108f129427cda33039c16b8d52b93ffd29e99251795211a360e894bf0029";
            int multiplier = 7;
            
            
            var web3 = new Web3Geth(rcpClientUrl);
           
            // 1. Unclock Account
            var unlockRes =
                await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, new HexBigInteger(120));
            Assert.True(unlockRes);
            
            // 2. Deploy contract
            var trnsHash = await web3.Eth.DeployContract
                .SendRequestAsync(abi, byteCode, senderAddress,  new HexBigInteger(250000),  multiplier);
            
            var mineRes = await web3.Miner.Start.SendRequestAsync(6);
            Assert.True(mineRes);
            
            // 3. Get contract receipt 
            var receipt =  await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trnsHash);
            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt =  await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trnsHash);
            }

//            // 2. Deploy contract & Get contract receipt 
//            var receipt =
//                await web3.Eth.DeployContract
//                     .SendRequestAndWaitForReceiptAsync(abi, byteCode, senderAddress, new HexBigInteger(900), null, multiplier);

            // 4. Call contract function
            var contractAddress = receipt.ContractAddress;

            var contract = web3.Eth.GetContract(abi, contractAddress);

            var multiplyFunction = contract.GetFunction("multiply");

            var result = await multiplyFunction.CallAsync<int>(7);

            Assert.Equal(49, result);
        }
    }
}