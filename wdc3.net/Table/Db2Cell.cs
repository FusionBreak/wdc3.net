using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2Cell
    {
        public string? ColumnName => ColumnInfo?.Name;
        public bool? IsId => ColumnInfo?.IsId;
        public object? Value { get; set; }
        public FieldStructure? FieldStructure { get; set; }
        public IFieldStorageInfo? FieldStorageInfo { get; set; }
        public ColumnInfo? ColumnInfo { get; set; }
    }
}