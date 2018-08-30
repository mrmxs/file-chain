using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EthereumLibrary.Model;

namespace EthereumLibrary.Service
{
    public interface IEthereumFileService
    {
        Task<IEnumerable<IEthereumFile>> GetAsyncCall(
            string ownerLogin, string ownerPassword, IEnumerable<BigInteger> ids);

        Task<IEnumerable<IEthereumFile>> GetAsyncCall(string ownerLogin, string ownerPassword);
       
        Task<IEthereumFile> GetAsyncCall(
            string ownerLogin, string ownerPassword, BigInteger id);

        Task<IEthereumFile> AddAsync(string login, string password, string type, string hash, long size, string name, string description, DateTime created);
        
        Task<IEnumerable<BigInteger>> GetIdsAsyncCall(string ownerLogin, string ownerPassword);
        
        Task<bool> SetNameAsync(string login, string password, BigInteger id, string newName, DateTime modified);
        
        Task<bool> SetDescriptionAsync(string login, string password, BigInteger id, string newDescription, DateTime modified);
        
        Task<bool> DeleteAsync(string login, string password, BigInteger id);
    }
}