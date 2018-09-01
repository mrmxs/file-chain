using System;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace EthereumLibrary.Model
{
    [FunctionOutput]
    public class GetFilePart1Dto
    {
        [Parameter("bytes32", "mimeType", 1)] public string MimeType { get; set; }

        [Parameter("bytes32[6]", "ipfsHash", 2)]
        public List<string> IpfsHash { get; set; }

        [Parameter("bytes32", "size", 3)] public string Size { get; set; }

        [Parameter("bytes32[3]", "name", 4)] public List<string> Name { get; set; }
    }

    [FunctionOutput]
    public class GetFilePart2Dto
    {
        [Parameter("bytes32[6]", "description", 1)]
        public List<string> Description { get; set; }

        [Parameter("uint32", "created", 2)] public int Created { get; set; }

        [Parameter("uint32", "modified", 3)] public int Modified { get; set; }
    }


    [FunctionOutput]
    public class ReadableIpfsFileDto : IEthereumFile
    {
        public BigInteger Id { get; set; }
        public string MimeType { get; set; }
        public string IpfsHash { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public ReadableIpfsFileDto()
        {
        }

        public ReadableIpfsFileDto(BigInteger id, GetFilePart1Dto part1, GetFilePart2Dto part2)
        {
            Id = id;
            MimeType = part1.MimeType;
            IpfsHash = String.Join<string>("", part1.IpfsHash.ToArray());
            Size = part1.Size;
            Name = String.Join<string>("", part1.Name.ToArray());
            Description = String.Join<string>("", part2.Description.ToArray());
            Created = DateTimeOffset.FromUnixTimeMilliseconds(part2.Created).UtcDateTime;
            Modified = DateTimeOffset.FromUnixTimeMilliseconds(part2.Modified).UtcDateTime;
        }
    }
}