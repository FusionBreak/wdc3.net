namespace wdc3.net.File
{
    public class DefinitionInfo
    {
        public int Size { get; set; }
        public int ArrayLength { get; set; }
        public string? Name { get; set; }
        public bool IsId { get; set; }
        public bool IsRelation { get; set; }
        public bool IsNonInline { get; set; }
        public bool IsSigned { get; set; }
        public string? Comment { get; set; }
    }
}