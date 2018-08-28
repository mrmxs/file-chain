using System;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace EthereumLibrary.Model
{
    
    [FunctionOutput]
    public class IpfsFileDto 
    {
        [Parameter("bytes32", "mimeType", 1)]
        public string MimeType {get; set;}

        [Parameter("bytes32[2]", "ipfsHash", 2)]
        public List<string> IpfsHash {get; set;}

        [Parameter("bytes32", "size", 3)]
        public string Size {get; set;}
        
        [Parameter("bytes32[3]", "name", 4)]
        public List<string> Name {get; set;}

        [Parameter("bytes32[6]", "description", 5)]
        public List<string> Description {get; set;}

        [Parameter("uint32", "created", 6)]
        public long Created {get; set;}

        [Parameter("uint32", "modified", 7)]
        public long Modified {get; set;}
        
        public ReadableIpfsFileDto ToReadable()
        {
            return new ReadableIpfsFileDto
            {
                MimeType = MimeType,
                IpfsHash = String.Join<string>("", IpfsHash.ToArray()),
                Size = Size,
                Name = String.Join<string>("", Name.ToArray()),
                Description = String.Join<string>("", Description.ToArray()),
                Created = new DateTime(this.Created),
                Modified = new DateTime(this.Modified),
            };
        }
    }
    
    [FunctionOutput]
    public class ReadableIpfsFileDto 
    {
        [Parameter("bytes32", "mimeType", 1)]
        public string MimeType {get; set;}

        [Parameter("bytes32[2]", "ipfsHash", 2)]
        public string IpfsHash {get; set;}

        [Parameter("bytes32", "size", 3)]
        public string Size {get; set;}

        [Parameter("bytes32[3]", "name", 4)]
        public string Name {get; set;}

        [Parameter("bytes32[6]", "description", 5)]
        public string Description {get; set;}

        [Parameter("uint32", "created", 6)]
        public DateTime Created {get; set;}

        [Parameter("uint32", "modified", 7)]
        public DateTime Modified {get; set;}
    }
}