using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace API.Models
{
    public class WalletDto
    {
        public WalletDto(string address, HexBigInteger balance)
        {
            Address = address;
            _wei = balance;
        }

        private readonly BigInteger _wei;
        
        public string Address { get; }
        public string Wei => _wei.ToString();
        public string Ether => Web3.Convert.FromWei(_wei).ToString();
    }
}