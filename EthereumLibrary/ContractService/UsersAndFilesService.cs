using System.Numerics;
using System.Threading.Tasks;
using EthereumLibrary.Helper;
using EthereumLibrary.Model.UsersAndFiles;
using Nethereum.Contracts;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using UserDto = EthereumLibrary.Model.UsersAndFiles.UserDto;

namespace EthereumLibrary.ContractService
{
   public class UsersAndFilesService : AbstractContractService
   {
       public static string Abi => ResourceHelper.Get("UserToFiles.abi");
       public static string ByteCode => ResourceHelper.Get("UserToFiles.bin");
       public static string LibAbi => ResourceHelper.Get("IpfsFileStorageLibrary.abi");
       public static string LibByteCode => ResourceHelper.Get("IpfsFileStorageLibrary.bin");

        public static Task<string> DeployLibraryAsync(Web3Geth web3, string addressFrom, HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(LibAbi, LibByteCode, addressFrom, gas, valueAmount);
        }
        public static Task<string> DeployContractAsync(Web3Geth web3, string libraryAddress, string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[][] _firstName, byte[][] _lastName, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var byteCode = ByteCode.Replace("__d:/Dev/DocChain/EthereumLibrary/Smar__", libraryAddress.Substring(2));
            
            return web3.Eth.DeployContract.SendRequestAsync(Abi, byteCode, addressFrom, gas, valueAmount , _adminlogin, _adminpassword, _firstName, _lastName, _info);
        }

        private Contract contract;

        public UsersAndFilesService(Web3Geth web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(Abi, address);
        }

        public Function GetFunctionGetFilePart1() {
            return contract.GetFunction("getFilePart1");
        }
        public Function GetFunctionContains() {
            return contract.GetFunction("contains");
        }
        public Function GetFunctionSetAdmin() {
            return contract.GetFunction("setAdmin");
        }
        public Function GetFunctionGetFileIds() {
            return contract.GetFunction("getFileIds");
        }
        public Function GetFunctionSetFileName() {
            return contract.GetFunction("setFileName");
        }
        public Function GetFunctionAddFile() {
            return contract.GetFunction("addFile");
        }
        public Function GetFunctionSetFileDescription() {
            return contract.GetFunction("setFileDescription");
        }
        public Function GetFunctionSetName() {
            return contract.GetFunction("setName");
        }
        public Function GetFunctionSetPasswordAsAdmin() {
            return contract.GetFunction("setPasswordAsAdmin");
        }
        public Function GetFunctionDeleteFile() {
            return contract.GetFunction("deleteFile");
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
        public Function GetFunctionGetFilePart2() {
            return contract.GetFunction("getFilePart2");
        }


        public Task<bool> ContainsAsyncCall(byte[] _login) {
            var function = GetFunctionContains();
            return function.CallAsync<bool>(_login);
        }
        public Task<bool> SetAdminAsyncCall(byte[] _adminlogin, byte[] _adminpassword, byte[] _login, bool _isAdmin) {
            var function = GetFunctionSetAdmin();
            return function.CallAsync<bool>(_adminlogin, _adminpassword, _login, _isAdmin);
        }
        public Task<BigInteger[]> GetFileIdsAsyncCall(byte[] _login, byte[] _password) {
            var function = GetFunctionGetFileIds();
            return function.CallAsync<BigInteger[]>(_login, _password);
        }
        public Task<byte[][]> SetFileNameAsyncCall(byte[] _login, byte[] _password, BigInteger _fileindex, byte[][] _value, int _timestamp) {
            var function = GetFunctionSetFileName();
            return function.CallAsync<byte[][]>(_login, _password, _fileindex, _value, _timestamp);
        }
        public Task<byte[][]> SetFileDescriptionAsyncCall(byte[] _login, byte[] _password, BigInteger _fileindex, byte[][] _value, int _timestamp) {
            var function = GetFunctionSetFileDescription();
            return function.CallAsync<byte[][]>(_login, _password, _fileindex, _value, _timestamp);
        }
        public Task<bool> SetNameAsyncCall(byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName) {
            var function = GetFunctionSetName();
            return function.CallAsync<bool>(_login, _password, _firstName, _lastName);
        }
        public Task<bool> SetPasswordAsAdminAsyncCall(byte[] _adminlogin, byte[] _adminpassword, byte[] _login, byte[] _newPassword) {
            var function = GetFunctionSetPasswordAsAdmin();
            return function.CallAsync<bool>(_adminlogin, _adminpassword, _login, _newPassword);
        }
        public Task<BigInteger[]> DeleteFileAsyncCall(byte[] _login, byte[] _password, BigInteger _fileindex) {
            var function = GetFunctionDeleteFile();
            return function.CallAsync<BigInteger[]>(_login, _password, _fileindex);
        }
        public Task<bool> SetInfoAsyncCall(byte[] _login, byte[] _password, byte[][] _info) {
            var function = GetFunctionSetInfo();
            return function.CallAsync<bool>(_login, _password, _info);
        }
        public Task<bool> AddAsyncCall(byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, byte[][] _info) {
            var function = GetFunctionAdd();
            return function.CallAsync<bool>(_login, _password, _firstName, _lastName, _info);
        }
        public Task<bool> SetPasswordAsyncCall(byte[] _login, byte[] _password, byte[] _newPassword) {
            var function = GetFunctionSetPassword();
            return function.CallAsync<bool>(_login, _password, _newPassword);
        }

        public Task<string> SetAdminAsync(string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[] _login, bool _isAdmin, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetAdmin();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _adminlogin, _adminpassword, _login, _isAdmin);
        }
        public Task<string> SetFileNameAsync(string addressFrom, byte[] _login, byte[] _password, BigInteger _fileindex, byte[][] _value, int _timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetFileName();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _fileindex, _value, _timestamp);
        }
        public Task<string> AddFileAsync(string addressFrom, byte[] _login, byte[] _password, byte[] _mimeType, byte[][] _ipfsHash, byte[] _size, byte[][] _name, byte[][] _description, int timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAddFile();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _mimeType, _ipfsHash, _size, _name, _description, timestamp);
        }
        public Task<string> SetFileDescriptionAsync(string addressFrom, byte[] _login, byte[] _password, BigInteger _fileindex, byte[][] _value, int _timestamp, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetFileDescription();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _fileindex, _value, _timestamp);
        }
        public Task<string> SetNameAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetName();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _firstName, _lastName);
        }
        public Task<string> SetPasswordAsAdminAsync(string addressFrom, byte[] _adminlogin, byte[] _adminpassword, byte[] _login, byte[] _newPassword, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetPasswordAsAdmin();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _adminlogin, _adminpassword, _login, _newPassword);
        }
        public Task<string> DeleteFileAsync(string addressFrom, byte[] _login, byte[] _password, BigInteger _fileindex, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionDeleteFile();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _fileindex);
        }
        public Task<string> SetInfoAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetInfo();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _info);
        }
        public Task<string> AddAsync(string addressFrom, byte[] _login, byte[] _password, byte[][] _firstName, byte[][] _lastName, byte[][] _info, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAdd();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _firstName, _lastName, _info);
        }
        public Task<string> SetPasswordAsync(string addressFrom, byte[] _login, byte[] _password, byte[] _newPassword, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetPassword();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _login, _password, _newPassword);
        }

        public Task<GetFilePart1Dto> GetFilePart1AsyncCall(byte[] _login, byte[] _password, BigInteger _fileindex) {
            var function = GetFunctionGetFilePart1();
            return function.CallDeserializingToObjectAsync<GetFilePart1Dto>(_login, _password, _fileindex);
        }
        public Task<AddFileResponseDto> AddFileAsyncCall(byte[] _login, byte[] _password, byte[] _mimeType, byte[][] _ipfsHash, byte[] _size, byte[][] _name, byte[][] _description, int timestamp) {
            var function = GetFunctionAddFile();
            return function.CallDeserializingToObjectAsync<AddFileResponseDto>(_login, _password, _mimeType, _ipfsHash, _size, _name, _description, timestamp);
        }
        public Task<UserDto> GetAsyncCall(byte[] _login) {
            var function = GetFunctionGet();
            return function.CallDeserializingToObjectAsync<UserDto>(_login);
        }
        public Task<GetFilePart2Dto> GetFilePart2AsyncCall(byte[] _login, byte[] _password, BigInteger _fileindex) {
            var function = GetFunctionGetFilePart2();
            return function.CallDeserializingToObjectAsync<GetFilePart2Dto>(_login, _password, _fileindex);
        }


    }



}

