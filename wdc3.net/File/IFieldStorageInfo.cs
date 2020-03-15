using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public interface IFieldStorageInfo
    {
        public short FieldOffsetBits { get; set; }
        public short FieldSizeBits { get; set; }
        public int AdditionalDataSize { get; set; }
        public FieldCompressions StorageType { get; set; }
    }
}
