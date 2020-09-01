namespace wdc3.net.File
{
    public class FieldStorageInfoCommonData : IFieldStorageInfo
    {
        public ushort FieldOffsetBits { get; set; }
        public ushort FieldSizeBits { get; set; }
        public uint AdditionalDataSize { get; set; }
        public FieldCompressions StorageType { get; set; }
        public uint DefaultValue { get; set; }
        public uint UnkOrUnused2 { get; set; }
        public uint UnkOrUnused3 { get; set; }
    }
}