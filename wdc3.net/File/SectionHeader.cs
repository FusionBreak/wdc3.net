namespace wdc3.net.File
{
    public class SectionHeader
    {
        public ulong TactKeyHash { get; set; }
        public uint FileOffset { get; set; }
        public uint RecordCount { get; set; }
        public uint StringTableSize { get; set; }
        public uint OffsetRecordsEnd { get; set; }
        public uint IdListSize { get; set; }
        public uint RelationsshipDataSize { get; set; }
        public uint OffsetMapIdCount { get; set; }
        public uint CopyTableCount { get; set; }
    }
}