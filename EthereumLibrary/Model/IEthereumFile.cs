using System;

namespace EthereumLibrary.Model
{
    public interface IEthereumFile
    {
        int Id { get; set; }
        string IpfsHash { get; set; }
        string ContentType { get; set; }

        int Owner { get; set; }
        int[] Editors { get; set; }
        int[] Viewers { get; set; }

        bool IsDeleted { get; set; }

        DateTime CreatedDate { get; set; }
        DateTime? EditedDate { get; set; }
        DateTime? DeletedDate { get; set; }

        string[] PropertiesList { get; }
        string[] Properties { get; set; }
    }
}