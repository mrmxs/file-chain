using System;
using System.Threading.Tasks;
using EthereumLibrary.Model;

namespace EthereumLibrary.Service
{
    public interface IEthereumUserService
    {
        Task<IEthereumUser> GetAsyncCall(string login);
        
        Task<IEthereumUser> AddAsyncCall(string login, string password, string firstName, string lastName, string info);
        
        Task<bool> IsAuthenticatedAsyncCall(string login, string password);
        
        Task<bool> SetNameAsync(string login, string password, string firstName, string lastName, DateTime now);
        
        Task<bool> SetInfoAsync(string login, string password, string requestInfo, DateTime now);
    }
}