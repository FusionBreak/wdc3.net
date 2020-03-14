using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    class SectionHeader
    {
        public long TactKeyHash { get; set; }
        public int FileOffset { get; set; }
        public int RecordCount { get; set; }
        public int StringTableSize { get; set; }
        public int OffsetRecordsEnd { get; set; }
        public int IdListSize { get; set; }
        public int RelationsshipDataSize { get; set; }
        public int OffsetMapIdCount { get; set; }
        public int CopyTableCount { get; set; }
    }
}
