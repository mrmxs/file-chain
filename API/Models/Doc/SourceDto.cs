namespace API.Models.Doc
{
    public class SourceDto
    {
        public int? Id { get; set; }
        public byte[] Source { get; set; }
        public string Path { get; set; }
        public string Hash { get; internal set; }

        public static SourceDto Stub()
        {
            return new SourceDto
            {
                Id = 4,
                Source = new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20},
                Hash = "454564hjkhdf454",
            };
        }
    }
}