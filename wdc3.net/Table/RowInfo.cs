namespace wdc3.net.Table
{
    public record RowInfo
    {
        public uint Id { get; init; }
        public uint? OffsetMapId { get; init; }
        public uint? Offset { get; init; }
        public ushort? Size { get; init; }
        public bool HasOffset => Offset is not null;
    }
}