using System.Collections.Generic;
using EthereumClassLibrary.Helper;
using EthereumClassLibrary.Model;

namespace EthereumClassLibrary.Service
{
    public interface IFileService
    {
        bool Add();
        bool Edit();
        bool Delete();
        
        FileProperties Get(int id);
        FileProperties Get(string ipfsHash);
        IEnumerable<FileProperties> Get(FileFilter fileFilter);
    }
}