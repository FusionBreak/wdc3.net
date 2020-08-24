namespace wdc3.net.File
{
    public class Header
    {
        public int Magic { get; set; }
        public uint RecordCount { get; set; }
        public uint FieldCount { get; set; }
        public uint RecordSize { get; set; }
        public uint StringTableSize { get; set; }
        public uint TableHash { get; set; }
        public uint LayoutHash { get; set; }
        public uint MinId { get; set; }
        public uint MaxId { get; set; }
        public uint Locale { get; set; }
        public ushort Flags { get; set; }
        public ushort IdIndex { get; set; }
        public uint TotalFieldCount { get; set; }
        public uint BitpackedDataOffset { get; set; }
        public uint LookUpColumnCount { get; set; }
        public uint FieldStorageInfoSize { get; set; }
        public uint CommonDataSize { get; set; }
        public uint PalletDataSize { get; set; }
        public uint SectionCount { get; set; }
    }
}
