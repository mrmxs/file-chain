using System.Collections.Generic;
using IPFSClassLibrary.Model;
using IPFSClassLibrary.Service;

namespace IPFSClassLibrary.Service
{
    public class FileService : IFileService
    {
//        private string _host;
//        private string _port;
//        private string _protocol;
        
        

        public FileService(string host, int port, string protocol)
        {
//            _host = host;
//            _port = port;
//            _protocol = protocol;
        }

        public bool Add()
        {
            throw new System.NotImplementedException();
        }

        public bool Edit()
        {
            throw new System.NotImplementedException();
        }

        public File Get(string hash)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<File> Get(string[] hashes)
        {
            throw new System.NotImplementedException();
        }
    }
}