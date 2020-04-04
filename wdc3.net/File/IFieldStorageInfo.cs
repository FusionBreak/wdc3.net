using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public interface IFieldStorageInfo
    {
        short FieldOffsetBits { get; set; }
        short FieldSizeBits { get; set; }
        int AdditionalDataSize { get; set; }
        FieldCompressions StorageType { get; set; }
    }
}
