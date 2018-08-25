using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ipfs;
using IPFSLibrary.Model;

namespace IPFSLibrary.Service
{
    public class IpfsService : IIpfsService
    {
        private string _apiUrl;
        private string _fileEndpoint;


        public IpfsService(string host, int port, string protocol)
        {
            _apiUrl = $"{protocol}://{host}:{port}";
            _fileEndpoint = $"{protocol}://{host}/ipfs/";
            ;
        }

        public IpfsFile Add(string name, Stream source)
        {
            var multiHash = AddAsync(name, source).Result.Hash.ToString();
            
            return new IpfsFile
            {
                Hash = multiHash,
                Url = _fileEndpoint + multiHash,
            };
        }

        public IpfsFile Edit()
        {
            throw new System.NotImplementedException();
        }

        public IpfsFile Get(string hash)
        {
            return new IpfsFile
            {
                Hash = hash,
                Url = _fileEndpoint + hash,
            };
        }

        public IEnumerable<IpfsFile> Get(string[] hashes)
        {
            throw new System.NotImplementedException();
        }

        private async Task<MerkleNode> AddAsync(string name, Stream stream)
        {
            using (var ipfs = new IpfsClient(_apiUrl))
            {
                var inputStream = new IpfsStream(name, stream);

                var merkleNode = await ipfs.Add(inputStream).ConfigureAwait(false);
                //TODO uncomment me in prod
//                var multiHash = ipfs.Pin.Add(merkleNode.Hash.ToString());
                return merkleNode;
            }
        }
    }
}