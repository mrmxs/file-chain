using System;
using System.Numerics;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
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

        public Task<IEthereumUser> GetAsyncCall(string login)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateAsyncCall(string login, string password)
        {
            return new Task<bool>(() => false);
            throw new NotImplementedException();
        }
    }
}