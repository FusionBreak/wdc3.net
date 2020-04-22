using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public interface IFieldStorageInfo
    {
        ushort FieldOffsetBits { get; set; }
        ushort FieldSizeBits { get; set; }
        uint AdditionalDataSize { get; set; }
        FieldCompressions StorageType { get; set; }
    }
}
