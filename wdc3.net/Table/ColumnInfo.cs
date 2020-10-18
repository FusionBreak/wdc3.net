using System;

namespace wdc3.net.Table
{
    public record ColumnInfo
    {
        public string? Name { get; init; }
        public Type? Type { get; init; }
        public bool IsId { get; init; }
        public int ArrayLength { get; init; }
    }
}