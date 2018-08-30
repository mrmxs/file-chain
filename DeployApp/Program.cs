using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EthereumLibrary.ContractService;
using EthereumLibrary.Helper;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using EV = EthereumLibrary.Helper.EnvironmentVariablesHelper;

namespace DeployApp
{
    class Program
    {
        private static Web3Geth _web3;
        private static HexBigInteger _gas;
        private static string[] admin;

        private static IDictionary<string, string> _dic;

        static void Main(string[] args)
        {
            Console.WriteLine("============ Deploy Contract App ===============");
            _dic = args.Aggregate(new Dictionary<string, string>(),
                (dic, arg) =>
                {
                    dic.Add(arg.Split("=")[0], arg.Split("=")[1]);
                    return dic;
                });

            //EV.Clear();
            EV.WalletAddress = _dic["-wa"];
            EV.WalletPassword = _dic["-wp"];
            if (_dic.ContainsKey("-rcp")) EV.RcpClient = _dic["-rcp"];
            if (_dic.ContainsKey("-lib")) EV.LibraryAddress = _dic["--lib"];
            if (_dic.ContainsKey("-gas")) EV.Gas = new HexBigInteger(_dic["-gas"]);
            
            
            _web3 = new Web3Geth(EV.GetNotNull(EV.DOCCHAIN_RCP_CLIENT));
            _gas = new HexBigInteger(EV.GetNotNull(EV.DOCCHAIN_GAS));
            admin = new[]
            {
                _dic["-al"], _dic["-ap"], _dic["-afn"], _dic["-aln"], _dic.ContainsKey("-ai") ? _dic["-ai"] : " "
            };


            Console.WriteLine("\n\n------------ Library Deploying ------------------\n");
            if (_dic.ContainsKey("--lib"))
                Console.WriteLine("\nUse deployed library\n" +
                                  $">>> Library Address: {EV.LibraryAddress}");
            else DeployLibrary().Wait();
            
            Console.WriteLine("\n\n------------ Contract Deploying ------------------\n");
            DeployContract().Wait();

            Console.WriteLine("\n==================== Done ======================");
            Console.ReadKey();
        }

        public static async Task DeployLibrary()
        {
            // 1. Unclock Account
            var unlockTime = new HexBigInteger(120);
            Console.WriteLine($"Unlock account for {unlockTime.Value}s\n" +
                              $"  address: {EV.WalletAddress}\n" +
                              $"  pass:    {EV.WalletPassword}\n\n");
            var unlockRes = await _web3.Personal.UnlockAccount.SendRequestAsync(
                EV.WalletAddress, EV.WalletPassword, unlockTime);

            // 2. Deploy library
            Console.WriteLine("Create transaction to deploy library\n" +
                              $"  gas: {_gas.Value}\n" +
                              $"  . . . Getting hash . . .");
            var transactionHash =
                await UsersAndFilesService.DeployLibraryAsync(_web3, EV.WalletAddress, _gas);
            Console.WriteLine($"  hash: {transactionHash}");

            // 3. Mine transaction
            Console.WriteLine("\nMine transaction\n" +
                              "  . . . Getting receipt . . .");
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);
            Console.WriteLine($"  gas used: {receipt.GasUsed.Value}");

            EV.LibraryAddress = receipt.ContractAddress;
            Console.WriteLine($"\n>>> Library Address: {receipt.ContractAddress}");
        }

        public static async Task DeployContract()
        {
            // 1. Unclock Account
            var unlockTime = new HexBigInteger(120);
            Console.WriteLine($"Unlock account for {unlockTime.Value}s\n" +
                              $"  address: {EV.WalletAddress}\n" +
                              $"  pass: {EV.WalletPassword}\n\n");
            var unlockRes = await _web3.Personal.UnlockAccount.SendRequestAsync(
                EV.WalletAddress, EV.WalletPassword, unlockTime);

            // 2. Deploy contract
            // Get contract receipt & contractAddress, save contractAdress to file
            Console.WriteLine("\nCreate transaction to deploy contract\n" +
                              $"  gas: {_gas.Value}\n" +
                              $"  library address: {EV.LibraryAddress}");

            var adminBytes = new
            {
                login = CastHelper.StringToBytes32(admin[0]),
                password = CastHelper.StringToBytes32(admin[1]),
                firstName = CastHelper.StringToBytes32ArrayOf2(admin[2]),
                lastName = CastHelper.StringToBytes32ArrayOf2(admin[3]),
                info = CastHelper.StringToBytes32ArrayOf6(admin[4]),
            };

            Console.WriteLine($"\n  Admin:\n" +
                              $"    login:    {admin[0]}\n" +
                              $"    password: {admin[1]}\n" +
                              $"    name:     {admin[2]} {admin[3]}\n" +
                              $"    info:     {admin[4]}");
            Console.WriteLine("\n  . . . Getting hash . . .");

            var transactionHash =
                await UsersAndFilesService.DeployContractAsync(_web3,
                    EV.LibraryAddress,
                    EV.WalletAddress,
                    adminBytes.login,
                    adminBytes.password,
                    adminBytes.firstName,
                    adminBytes.lastName,
                    adminBytes.info,
                    _gas);
            Console.WriteLine($"  hash: {transactionHash}");

            // 3. Mine transaction
            Console.WriteLine("\nMine transaction\n" +
                              "  . . . Getting receipt . . .");
            var receipt = await UsersAndFilesService.MineAndGetReceiptAsync(_web3, transactionHash);
            Console.WriteLine($"  gas used: {receipt.GasUsed.Value}");

            EV.ContractAddress = receipt.ContractAddress;

            Console.WriteLine($"\n>>>\n" +
                              $">>> Contract Address: {receipt.ContractAddress}\n" +
                              $">>>");
        }
    }
}