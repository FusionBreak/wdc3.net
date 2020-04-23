using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public class FieldStorageInfoBitPacked : IFieldStorageInfo
    {
        public ushort FieldOffsetBits { get; set; }
        public ushort FieldSizeBits { get; set; }
        public uint AdditionalDataSize { get; set; }
        public FieldCompressions StorageType { get; set; }
        public uint BitpackingSizeBits { get; set; }
        public uint BitpackingOffsetBits { get; set; }
        public uint Flags { get; set; }
    }
}
