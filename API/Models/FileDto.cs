using System;
using System.Numerics;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Models
{
    public class FileDto
    {
        public BigInteger? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        
        public static FileDto Stub()
        {
            return new FileDto
            {
                Id = 4,
                Name = "Test.doc",
                Type = new FileExtensionContentTypeProvider().Mappings[".doc"],
                Size = 123,
                Description = "cool text",
                Link = "http://ipfs",
                Created = DateTime.Today,
                Modified = DateTime.Today,
            };
        }
    }
}