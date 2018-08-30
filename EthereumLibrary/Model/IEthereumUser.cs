using System;

namespace EthereumLibrary.Model
{
    public interface IEthereumUser
    {
        string Login { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Info { get; set; }
        bool IsAdmin { get; set; }
    }
}