using System.Collections.Generic;

namespace wdc3.net.File
{
    public class SectionWithFlag : ISection
    {
        public IEnumerable<byte>? VariableRecordData { get; set; }

        public IEnumerable<uint>? IdList { get; set; }
        public CopyTableEntry? CopyTable { get; set; }
        public OffsetMapEntry? OffsetMap { get; set; }
        public RelationshipMapping? RelationshipMap { get; set; }
        public IEnumerable<uint>? OffsetMapIdList { get; set; }
    }
}