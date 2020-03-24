using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public class SectionWithFlag : ISection
    {
        public IEnumerable<byte> VariableRecordData { get; set; }

        public IEnumerable<int> IdList { get; set; }
        public CopyTableEntry CopyTable { get; set; }
        public OffsetMapEntry OffsetMap { get; set; }
        public RelationshipMapping RelationshipMap { get; set; }
        public IEnumerable<int> OffsetMapIdList { get; set; }
    }
}
