using System;
using System.Collections.Generic;
using System.Linq;
using Nethereum.Hex.HexTypes;

namespace EthereumLibrary.Helper
{
    public static class EnvironmentVariablesHelper
    {
        public static void Clear()
        {
            string[] envvars =
            {
                DOCCHAIN_RCP_CLIENT, DOCCHAIN_GAS, DOCCHAIN_GAS, DOCCHAIN_CONTRACT_ADDRESS, DOCCHAIN_WALLET_ADDRESS,
                DOCCHAIN_WALLET_PASSWORD
            };
            envvars.ToList().ForEach(ev => Environment.SetEnvironmentVariable(ev, ""));
        }

        public const string DOCCHAIN_RCP_CLIENT = "DOCCHAIN_RCP_CLIENT";
        public const string DOCCHAIN_GAS = "DOCCHAIN_GAS";
        public const string DOCCHAIN_LIBRARY_ADDRESS = "DOCCHAIN_LIBRARY_ADDRESS";
        public const string DOCCHAIN_CONTRACT_ADDRESS = "DOCCHAIN_CONTRACT_ADDRESS";
        public const string DOCCHAIN_WALLET_ADDRESS = "DOCCHAIN_WALLET_ADDRESS";
        public const string DOCCHAIN_WALLET_PASSWORD = "DOCCHAIN_WALLET_PASSWORD";

        public const string DefaultRcpClient = "http://127.0.0.1:7545";
        public static readonly string DefaultGas = new HexBigInteger(3000000).HexValue;
        public const string DefaultLibraryAddress = "0x79e64f0ac6f6b2fc9f8e00b2c22eb3b04e497716";
        public const string DefaultContractAddress = "0xa9aee256679649cee48c974081a4e5bfce48d17d";
        public const string DefaultWalletAddress = "0x750b447561a781d06ff8a190adc656b75ed8f422";

        public const string DefaultWalletPassword =
            "0x5ee7f584c60ab80c1cf0fcfec6c464d092c6805e8073bae71932c1c6558361ff";

        public static string RcpClient
        {
            get => Environment.GetEnvironmentVariable(DOCCHAIN_RCP_CLIENT);
            set => Environment.SetEnvironmentVariable(DOCCHAIN_RCP_CLIENT, value);
        }

        public static HexBigInteger Gas
        {
            get => new HexBigInteger(Environment.GetEnvironmentVariable(DOCCHAIN_GAS));
            set => Environment.SetEnvironmentVariable(DOCCHAIN_GAS, value.ToString());
        }

        public static string LibraryAddress
        {
            get => Environment.GetEnvironmentVariable(DOCCHAIN_LIBRARY_ADDRESS);
            set => Environment.SetEnvironmentVariable(DOCCHAIN_LIBRARY_ADDRESS, value);
        }

        public static string ContractAddress
        {
            get => Environment.GetEnvironmentVariable(DOCCHAIN_CONTRACT_ADDRESS);
            set => Environment.SetEnvironmentVariable(DOCCHAIN_CONTRACT_ADDRESS, value);
        }

        public static string WalletAddress
        {
            get => Environment.GetEnvironmentVariable(DOCCHAIN_WALLET_ADDRESS);
            set => Environment.SetEnvironmentVariable(DOCCHAIN_WALLET_ADDRESS, value);
        }

        public static string WalletPassword
        {
            get => Environment.GetEnvironmentVariable(DOCCHAIN_WALLET_PASSWORD);
            set => Environment.SetEnvironmentVariable(DOCCHAIN_WALLET_PASSWORD, value);
        }

        public static string GetNotNull(string envName)
        {
            var defaults = new Dictionary<string, string>();
            defaults.Add(DOCCHAIN_RCP_CLIENT, DefaultRcpClient);
            defaults.Add(DOCCHAIN_GAS, DefaultGas);
            defaults.Add(DOCCHAIN_LIBRARY_ADDRESS, DefaultLibraryAddress);
            defaults.Add(DOCCHAIN_CONTRACT_ADDRESS, DefaultContractAddress);
            defaults.Add(DOCCHAIN_WALLET_ADDRESS, DefaultWalletAddress);
            defaults.Add(DOCCHAIN_WALLET_PASSWORD, DefaultWalletPassword);

            var env = Environment.GetEnvironmentVariable(envName);

            return string.IsNullOrEmpty(env)
                ? defaults[envName]
                : Environment.GetEnvironmentVariable(envName);
        }
    }
}