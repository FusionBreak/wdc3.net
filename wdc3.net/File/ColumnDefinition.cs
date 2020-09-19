namespace wdc3.net.File
{
    public class ColumnDefinition
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool Verified { get; set; }
        public string? Comment { get; set; }
        public string? ForeignTable { get; set; }
        public string? ForeignColumn { get; set; }
    }
}