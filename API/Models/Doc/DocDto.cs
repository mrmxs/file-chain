namespace API.Models.Doc
{
    public class DocDto
    {
        private int? Id { get; set;}
        public SourceDto Source { get; set; }
        public PropertyDto Properties { get; set; }
        public AccessDto Access { get; set; }

        public static DocDto Stub()
        {
            return new DocDto
            {
                Id = 4,
                Source = SourceDto.Stub(),
                Properties = PropertyDto.Stub(),
                Access = AccessDto.Stub(),
            };
        }
    }
}