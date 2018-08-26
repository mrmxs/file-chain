using System.Collections.Generic;
using EthereumLibrary.Helper;
using EthereumLibrary.Model;

namespace EthereumLibrary.Service
{
    public interface IUserService
    {
        bool Add();
        bool Edit();
        bool Delete();
        
        IEthereumFile Get(int? id, string wallet);
        IEnumerable<IEthereumFile> Get(UserFilter fileFilter);
    }
}