using System.Collections.Generic;
using EthereumLibrary.Helper;
using EthereumLibrary.Model;

namespace EthereumLibrary.Service
{
    public interface IFileService
    {
        bool Add();
        bool Edit();
        bool Delete();
        
        IEthereumFile Get(int id);
        IEthereumFile Get(string ipfsHash);
        IEnumerable<IEthereumFile> Get(FileFilter fileFilter);
    }
}