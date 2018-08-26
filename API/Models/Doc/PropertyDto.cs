using System;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Models.Doc
{
    public class PropertyDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        
        public static PropertyDto Stub()
        {
            return new PropertyDto
            {
                Id = 4,
                Name = "Test.doc",
                ContentType = new FileExtensionContentTypeProvider().Mappings[".doc"],
                Size = 123,
                CreatedDate = DateTime.Today,
            };
        }
    }
}