using System.Numerics;
using System.Threading.Tasks;
using EthereumLibrary.Helper;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace EthereumLibrary.ContractService
{
    public class IpfsFileStorageService
    {
        public static string Abi => ResourceHelper.Get("IpfsFileStorage.abi");
        public static string ByteCode => ResourceHelper.Get("IpfsFileStorage.bin");

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom, HexBigInteger gas = null,
            HexBigInteger valueAmount = null)
        {
            return web3.Eth.DeployContract.SendRequestAsync(Abi, ByteCode, addressFrom, gas, valueAmount);
        }

        private readonly Web3 _web3;
        private Contract _contract;

        public IpfsFileStorageService(Web3 web3, string address)
        {
            this._web3 = web3;
            this._contract = web3.Eth.GetContract(Abi, address);
        }

        #region Functions
        public Function GetFunctionSetDescription()
        {
            return _contract.GetFunction("setDescription");
        }

        public Function GetFunctionContains()
        {
            return _contract.GetFunction("contains");
        }

        public Function GetFunctionAddIpfsFileToStorage()
        {
            return _contract.GetFunction("addIpfsFileToStorage");
        }

        public Function GetFunctionGetIpfsFile()
        {
            return _contract.GetFunction("getIpfsFile");
        }

        public Function GetFunctionSetName()
        {
            return _contract.GetFunction("setName");
        }
        #endregion

        #region Transaction calls
        public Task<bool> ContainsAsyncCall(BigInteger _index)
        {
            var function = GetFunctionContains();
            return function.CallAsync<bool>(_index);
        }

        /// <summary>
        /// Insert item with file properties to the contract mapping
        /// </summary>
        /// <param name="mimeType">MIME type</param>
        /// <param name="size">File size in bytes</param>
        /// <param name="ipfsHash">Hash of the stored file in IPFS</param>
        /// <param name="name">File name</param>
        /// <param name="description">File description</param>
        /// <returns></returns>
        public Task<BigInteger> AddIpfsFileToStorageAsyncCall(
            string mimeType, BigInteger size, string ipfsHash, string name, string description)
        {
            var function = GetFunctionAddIpfsFileToStorage();
            return function.CallAsync<BigInteger>(mimeType, size, ipfsHash, name, description);
        }

        public Task<string> SetDescriptionAsync(string addressFrom, BigInteger index, string value,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetFunctionSetDescription();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, index, value);
        }

        public Task<string> AddIpfsFileToStorageAsync(string addressFrom, string mimeType, BigInteger size,
            string ipfsHash, string name, string description, HexBigInteger gas = null,
            HexBigInteger valueAmount = null)
        {
            var function = GetFunctionAddIpfsFileToStorage();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, mimeType, size, ipfsHash, name,
                description);
        }

        public Task<string> GetIpfsFileAsync(string addressFrom, BigInteger index, HexBigInteger gas = null,
            HexBigInteger valueAmount = null)
        {
            var function = GetFunctionGetIpfsFile();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, index);
        }

        public Task<string> SetNameAsync(string addressFrom, BigInteger index, string value, HexBigInteger gas = null,
            HexBigInteger valueAmount = null)
        {
            var function = GetFunctionSetName();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, index, value);
        }

        public Task<GetIpfsFileDto> GetIpfsFileAsyncCall(BigInteger _index)
        {
            var function = GetFunctionGetIpfsFile();
            return function.CallDeserializingToObjectAsync<GetIpfsFileDto>(_index);
        }
        #endregion
    }

    [FunctionOutput]
    public class GetIpfsFileDto
    {
        [Parameter("string", "mimeType", 1)] public string MimeType { get; set; }

        [Parameter("uint256", "size", 2)] public BigInteger Size { get; set; }

        [Parameter("string", "ipfsHash", 3)] public string IpfsHash { get; set; }

        [Parameter("string", "name", 4)] public string Name { get; set; }

        [Parameter("string", "description", 5)] public string Description { get; set; }

        [Parameter("address", "owner", 6)] public string Owner { get; set; }

        [Parameter("uint256", "created", 7)] public BigInteger Created { get; set; }

        [Parameter("uint256", "accessed", 8)] public BigInteger Accessed { get; set; }

        [Parameter("uint256", "modified", 9)] public BigInteger Modified { get; set; }
    }
}