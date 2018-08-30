using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace EthereumLibrary.Model.v1
{
    [FunctionOutput]
    public class UserDto 
    {
        [Parameter("bytes32", "login", 1)]
        public string Login {get; set;}

        [Parameter("bytes32[2]", "firstName", 2)]
        public List<string> FirstName {get; set;}

        [Parameter("bytes32[2]", "lastName", 3)]
        public List<string> LastName {get; set;}

        [Parameter("bytes32[6]", "info", 4)]
        public List<string> Info {get; set;}

        [Parameter("bool", "isAdmin", 5)]
        public bool IsAdmin {get; set;}
        
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
    
        [FunctionOutput]
        public class ReadableUserDto 
        {
            [Parameter("bytes32", "login", 1)]
            public string Login {get; set;}

            [Parameter("bytes32[2]", "firstName", 2)]
            public string FirstName {get; set;}

            [Parameter("bytes32[2]", "lastName", 3)]
            public string LastName {get; set;}

            [Parameter("bytes32[6]", "info", 4)]
            public string Info {get; set;}

            [Parameter("bool", "isAdmin", 5)]
            public bool IsAdmin {get; set;}
        }
    }
}