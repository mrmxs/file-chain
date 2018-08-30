using System.Threading.Tasks;
using EthereumLibrary.Model;

namespace EthereumLibrary.Service
{
    public interface IEthereumUserService
    {
        Task<IEthereumUser> GetAsyncCall(string login);
        
        Task<bool> AuthenticateAsyncCall(string login, string password);
    }
}