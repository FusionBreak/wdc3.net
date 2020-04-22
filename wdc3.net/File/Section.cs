using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public class Section : ISection
    {
        public IEnumerable<RecordData>? Records { get; set; }
        public IEnumerable<byte>? StringData { get; set; }

        public IEnumerable<uint>? IdList { get; set; }
        public CopyTableEntry? CopyTable { get; set; }
        public OffsetMapEntry? OffsetMap { get; set; }
        public RelationshipMapping? RelationshipMap { get; set; }
        public IEnumerable<uint>? OffsetMapIdList { get; set; }
    }
}
