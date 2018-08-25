using System.Collections.Generic;
using System.IO;
using IPFSLibrary.Model;

namespace IPFSLibrary.Service
{
    public interface IIpfsService
    {
        IpfsFile Add(string name, Stream source);
        IpfsFile Edit();
        
        IpfsFile Get(string hash);
        IEnumerable<IpfsFile> Get(string[] hashes);
    }
}