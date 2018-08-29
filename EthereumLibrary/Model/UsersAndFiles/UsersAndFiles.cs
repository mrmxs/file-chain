using System;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace EthereumLibrary.Model.UsersAndFiles
{
    [FunctionOutput]
    public class AddFileResponseDto
    {
        [Parameter("uint256", "fileindex", 1)] public BigInteger Fileindex { get; set; }

        [Parameter("uint256[]", "arr", 2)] public BigInteger[] Arr { get; set; }
    }

    [FunctionOutput]
    public class UserDto
    {
        [Parameter("bytes32", "login", 1)] 
        public string Login { get; set; }

        [Parameter("bytes32[2]", "firstName", 2)]
        public List<string> FirstName { get; set; }

        [Parameter("bytes32[2]", "lastName", 3)]
        public List<string> LastName { get; set; }

        [Parameter("bytes32[6]", "info", 4)] 
        public List<string> Info { get; set; }

        [Parameter("bool", "isAdmin", 5)]
        public bool IsAdmin { get; set; }

        public ReadableUserDto ToReadable()
        {
            return new ReadableUserDto
            {
                Login = Login,
                FirstName = string.Join<string>("", FirstName.ToArray()),
                LastName = string.Join<string>("", LastName.ToArray()),
                Info = string.Join<string>("", Info.ToArray()),
                IsAdmin = IsAdmin
            };
        }
    }

    [FunctionOutput]
    public class ReadableUserDto
    {
        [Parameter("bytes32", "login", 1)] public string Login { get; set; }

        [Parameter("bytes32[2]", "firstName", 2)]
        public string FirstName { get; set; }

        [Parameter("bytes32[2]", "lastName", 3)]
        public string LastName { get; set; }

        [Parameter("bytes32[6]", "info", 4)] public string Info { get; set; }

        [Parameter("bool", "isAdmin", 5)] public bool IsAdmin { get; set; }
    }

    [FunctionOutput]
    public class GetFilePart1Dto
    {
        [Parameter("bytes32", "mimeType", 1)] public string MimeType { get; set; }

        [Parameter("bytes32[2]", "ipfsHash", 2)]
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
    public class ReadableIpfsFileDto
    {
        [Parameter("bytes32", "mimeType", 1)] public string MimeType { get; set; }

        [Parameter("bytes32[2]", "ipfsHash", 2)]
        public string IpfsHash { get; set; }

        [Parameter("bytes32", "size", 3)] public string Size { get; set; }

        [Parameter("bytes32[3]", "name", 4)] public string Name { get; set; }

        [Parameter("bytes32[6]", "description", 5)]
        public string Description { get; set; }

        [Parameter("uint32", "created", 6)] public DateTime Created { get; set; }

        [Parameter("uint32", "modified", 7)] public DateTime Modified { get; set; }

        public ReadableIpfsFileDto()
        {
        }

        public ReadableIpfsFileDto(GetFilePart1Dto part1, GetFilePart2Dto part2)
        {
            var file = new ReadableIpfsFileDto();

            file.MimeType = part1.MimeType;
            file.IpfsHash = String.Join<string>("", part1.IpfsHash.ToArray());
            file.Size = part1.Size;
            file.Name = String.Join<string>("", part1.Name.ToArray());
            file.Description = String.Join<string>("", part2.Description.ToArray());
            file.Created = new DateTime(part2.Created);
            file.Modified = new DateTime(part2.Modified);
        }
    }
}