using System.Collections.Generic;
using IPFSClassLibrary.Model;

namespace IPFSClassLibrary.Service
{
    public interface IFileService
    {
        bool Add();
        bool Edit();
        
        File Get(string hash);
        IEnumerable<File> Get(string[] hashes);
    }
}