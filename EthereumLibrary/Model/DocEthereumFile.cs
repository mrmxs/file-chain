using System;

namespace EthereumLibrary.Model
{
    public class DocEthereumFile : IEthereumFile
    {
        public int Id { get; set; }
        public string IpfsHash { get; set; }
        public string ContentType { get; set; }
        public int Owner { get; set; }
        public int[] Editors { get; set; }
        public int[] Viewers { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        
        public string[] Properties { get; set; }

        public string[] PropertiesList => new[] { "Name", "Size" };
    }
}