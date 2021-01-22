using System.Collections.Generic;

namespace wdc3.net.File
{
    public class Section : ISection
    {
        public IEnumerable<RecordData>? Records { get; set; }
        public IEnumerable<byte>? StringData { get; set; }

        public IEnumerable<uint>? IdList { get; set; }
        public IEnumerable<CopyTableEntry>? CopyTable { get; set; }
        public IEnumerable<OffsetMapEntry>? OffsetMap { get; set; }
        public RelationshipMapping? RelationshipMap { get; set; }
        public IEnumerable<uint>? OffsetMapIdList { get; set; }
    }
}