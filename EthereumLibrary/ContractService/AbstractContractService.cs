using System.Threading;
using System.Threading.Tasks;
using Nethereum.Geth;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace EthereumLibrary.ContractService
{
    public abstract class AbstractContractService
    {
        protected Web3Geth web3;

        public async Task<TransactionReceipt> MineAndGetReceiptAsync(string transactionHash)
        {
            return await MineAndGetReceiptAsync(web3, transactionHash);
        }
        
        public static async Task<TransactionReceipt> MineAndGetReceiptAsync(Web3Geth web3, string transactionHash){

            var miningResult = await web3.Miner.Start.SendRequestAsync(6);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while(receipt == null){
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }
            miningResult = await web3.Miner.Stop.SendRequestAsync();
            
            return receipt;
        }
    }
}