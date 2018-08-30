using System;
using System.Numerics;

namespace EthereumLibrary.Model
{
    public interface IEthereumFile
    {
        BigInteger Id { get; set; }
        string MimeType { get; set; }
        string IpfsHash { get; set; }
        string Size { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
    }
}