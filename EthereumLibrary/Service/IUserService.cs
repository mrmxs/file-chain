using System.Collections.Generic;
using EthereumClassLibrary.Helper;
using EthereumClassLibrary.Model;

namespace EthereumClassLibrary.Service
{
    public interface IUserService
    {
        bool Add();
        bool Edit();
        bool Delete();
        
        FileProperties Get(int? id, string wallet);
        IEnumerable<FileProperties> Get(UserFilter fileFilter);
    }
}