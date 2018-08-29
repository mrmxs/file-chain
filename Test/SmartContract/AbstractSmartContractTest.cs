using System.IO;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;

namespace Test.SmartContract
{
    public abstract class AbstractSmartContractTest
    {
        public Web3Geth _web3;
        public readonly string RcpClientUrl = "http://127.0.0.1:7545";
        public HexBigInteger Gas => new HexBigInteger(3000000);
        
        public readonly string SenderAddress = "0x79e64f0ac6f6b2fc9f8e00b2c22eb3b04e497716";
        public readonly string SenderAddress2 = "0xa9aee256679649cee48c974081a4e5bfce48d17d";
        public readonly string Password = "";
        
        public Contract _contract;
        
        protected string StoredContractAddressPath = "";
        protected string ContractAddress
        {
            get => File.Exists(StoredContractAddressPath)
                ? File.ReadAllText(StoredContractAddressPath)
                : "";
            set => File.WriteAllText(StoredContractAddressPath, value);
        }
        
        protected string StoredLibraryAddressPath = "";
        protected string LibraryAddress
        {
            get => File.Exists(StoredLibraryAddressPath)
                ? File.ReadAllText(StoredLibraryAddressPath)
                : "";
            set => File.WriteAllText(StoredLibraryAddressPath, value);
        }
    }
}