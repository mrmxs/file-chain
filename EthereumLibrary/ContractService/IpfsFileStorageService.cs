using System.Threading.Tasks;
using System.Numerics;
using EthereumLibrary.Helper;
using EthereumLibrary.Model.v1;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.Geth;

namespace EthereumLibrary.ContractService
{
   public class IpfsFileStorageService : AbstractContractService
   {
        public static string Abi => ResourceHelper.Get("IpfsFileStorage.abi");
        public static string ByteCode => ResourceHelper.Get("IpfsFileStorage.bin");

        public static Task<string> DeployContractAsync(Web3Geth web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(Abi, ByteCode, addressFrom, gas, valueAmount );
        }

        private Contract contract;

        public IpfsFileStorageService(Web3Geth web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(Abi, address);
        }

        public Function GetFunctionAdd() {
            return contract.GetFunction("add");
        }
        public Function GetFunctionGet() {
            return contract.GetFunction("get");
        }
        public Function GetFunctionSetName() {
            return contract.GetFunction("setName");
        }
        public Function GetFunctionSetDescription() {
            return contract.GetFunction("setDescription");
        }
        public Function GetFunctionContains() {
            return contract.GetFunction("contains");
        }

        public Task<BigInteger> AddAsyncCall(byte[] _mimeType, byte[][] _ipfsHash, byte[] _size, byte[][] _name, byte[][] _description, int timestamp) {
            var function = GetFunctionAdd();
            return function.CallAsync<BigInteger>(_mimeType, _ipfsHash, _size, _name, _description, timestamp);
        }
        public Task<string> AddAsync(string addressFrom, byte[] _mimeType, byte[][] _ipfsHash, byte[] _size, byte[][] _name, byte[][] _description, int timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAdd();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _mimeType, _ipfsHash, _size, _name, _description, timestamp);
        }
       
        public Task<string> SetNameAsync(string addressFrom, BigInteger _index, byte[][] _value, int timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetName();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _index, _value, timestamp);
        }
       
        public Task<string> SetDescriptionAsync(string addressFrom, BigInteger _index, byte[][] _value, int timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetDescription();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _index, _value, timestamp);
        }
       
        public Task<IpfsFileDto> GetAsyncCall(BigInteger _index) {
            var function = GetFunctionGet();
            return function.CallDeserializingToObjectAsync<IpfsFileDto>(_index);
        }

        public Task<bool> ContainsAsyncCall(BigInteger _index) {
            var function = GetFunctionContains();
            return function.CallAsync<bool>(_index);
        }
    }

}

