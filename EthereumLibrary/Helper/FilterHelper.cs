namespace EthereumClassLibrary.Helper
{
    public class FilterHelper
    {
        
        
        
    }

    public class FileFilter
    {
        public int? Id { get; set; }
        public string IpfsHash { get; set; }
        public int? Owner { get; set; }
        public int[] Viewers { get; set; }
        public int[] Editors { get; set; }
    }

    public class UserFilter
    {
        public int? Id { get; set; }
        public string Wallet { get; set; }
        public string Name { get; set; }
    }
}