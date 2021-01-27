using System;
using wdc3.net.Enums;

namespace wdc3.net.Table
{
    public record ColumnInfo
    {
        public string? Name { get; init; }
        public Db2ValueTypes Type { get; init; }
        public bool IsId { get; init; }
        public int ArrayLength { get; init; }
        public int Size { get; init; }
        public bool IsSigned { get; init; }
    }
}