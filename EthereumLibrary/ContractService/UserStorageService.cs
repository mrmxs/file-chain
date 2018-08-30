using System.Threading.Tasks;
using EthereumLibrary.Helper;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.Geth;
using UserDto = EthereumLibrary.Model.v1.UserDto;

namespace EthereumLibrary.ContractService
{
   public class UserStorageService : AbstractContractService
   {
       public static string Abi => ResourceHelper.Get("UserStorage.abi");
       public static string ByteCode => ResourceHelper.Get("UserStorage.bin");

        public static Task<string> DeployContractAsync(Web3Geth web3, string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[][] _firstName, byte[][] _lastName, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(Abi, ByteCode, addressFrom, gas, valueAmount , _adminlogin, _adminpassword, _firstName, _lastName, _info);
        }

        private Contract contract;

        public UserStorageService(Web3Geth web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(Abi, address);
        }

        public Function GetFunctionContains() {
            return contract.GetFunction("contains");
        }
        public Function GetFunctionSetAdmin() {
            return contract.GetFunction("setAdmin");
        }
        public Function GetFunctionSetName() {
            return contract.GetFunction("setName");
        }
        public Function GetFunctionSetPasswordAsAdmin() {
            return contract.GetFunction("setPasswordAsAdmin");
        }
        public Function GetFunctionGet() {
            return contract.GetFunction("get");
        }
        public Function GetFunctionSetInfo() {
            return contract.GetFunction("setInfo");
        }
        public Function GetFunctionAdd() {
            return contract.GetFunction("add");
        }
        public Function GetFunctionSetPassword() {
            return contract.GetFunction("setPassword");
        }

        public Task<bool> AddAsyncCall(byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, byte[][] _info) {
            var function = GetFunctionAdd();
            return function.CallAsync<bool>(_login, _password, _firstName, _lastName, _info);
        }
        public Task<string> AddAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAdd();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _firstName, _lastName, _info);
        }
       
        public Task<bool> SetNameAsyncCall(byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName) {
            var function = GetFunctionSetName();
            return function.CallAsync<bool>(_login, _password, _firstName, _lastName);
        }
        public Task<string> SetNameAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetName();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _firstName, _lastName);
        }
       
        public Task<bool> SetInfoAsyncCall(byte[] _login, byte[] _password, byte[][] _info) {
            var function = GetFunctionSetInfo();
            return function.CallAsync<bool>(_login, _password, _info);
        }
        public Task<string> SetInfoAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetInfo();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _info);
        }

        public Task<bool> SetPasswordAsyncCall(byte[] _login, byte[] _password, byte[] _newPassword) {
            var function = GetFunctionSetPassword();
            return function.CallAsync<bool>(_login, _password, _newPassword);
        }
        public Task<string> SetPasswordAsync(string addressFrom, byte[] _login, byte[] _password, byte[] _newPassword, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetPassword();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _newPassword);
        }
       
        public Task<bool> SetPasswordAsAdminAsyncCall(byte[] _adminlogin, byte[] _adminpassword, byte[] _login, byte[] _newPassword) {
            var function = GetFunctionSetPasswordAsAdmin();
            return function.CallAsync<bool>(_adminlogin, _adminpassword, _login, _newPassword);
        }
        public Task<string> SetPasswordAsAdminAsync(string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[] _login, byte[] _newPassword, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetPasswordAsAdmin();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _adminlogin, _adminpassword, _login, _newPassword);
        }
       
        public Task<bool> SetAdminAsyncCall(byte[] _adminlogin, byte[] _adminpassword, byte[] _login, bool _isAdmin) {
            var function = GetFunctionSetAdmin();
            return function.CallAsync<bool>(_adminlogin, _adminpassword, _login, _isAdmin);
        }
        public Task<string> SetAdminAsync(string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[] _login, bool _isAdmin, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetAdmin();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _adminlogin, _adminpassword, _login, _isAdmin);
        }

        public Task<UserDto> GetAsyncCall(byte[] _login) {
            var function = GetFunctionGet();
            return function.CallDeserializingToObjectAsync<UserDto>(_login);
        }

       public Task<bool> ContainsAsyncCall(byte[] _login) {
           var function = GetFunctionContains();
           return function.CallAsync<bool>(_login);
       }
    }
}

