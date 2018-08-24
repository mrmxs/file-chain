using System;

namespace IPFSClassLibrary.Model
{
    public class File
    {
        public string IpfsHash { get; set; }
        public byte[] Source { get; set; }
        public string Url { get; set; }
    }
}