using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public class FieldStorageInfoCommonData : IFieldStorageInfo
    {
        public short FieldOffsetBits { get; set; }
        public short FieldSizeBits { get; set; }
        public int AdditionalDataSize { get; set; }
        public FieldCompressions StorageType { get; set; }
        public int DefaultValue { get; set; }
        public int UnkOrUnused2 { get; set; }
        public int UnkOrUnused3 { get; set; }
    }
}
