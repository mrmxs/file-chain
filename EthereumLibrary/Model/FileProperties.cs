using System;
using System.Transactions;

namespace EthereumClassLibrary.Model
{
    public class FileProperties
    {
        private int Id { get; set;}
        public string IPFSHash { get; set; }
        
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public int Owner { get; set; }
        public int[] Editors { get; set; }
        public int[] Viewers { get; set; }

    }
}