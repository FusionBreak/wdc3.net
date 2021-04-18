using System.Collections.Generic;
using System.Linq;

namespace wdc3.net.File
{
    public class SectionWithFlag : ISection, ISizeCalculable
    {
        public IEnumerable<byte>? VariableRecordData { get; set; }

        public IEnumerable<uint>? IdList { get; set; }
        public IEnumerable<CopyTableEntry>? CopyTable { get; set; }
        public IEnumerable<OffsetMapEntry>? OffsetMap { get; set; }
        public RelationshipMapping? RelationshipMap { get; set; }
        public IEnumerable<uint>? OffsetMapIdList { get; set; }

        public int SizeOfWithoutVariableRecordData
        {
            get
            {
                var size = 0;

                size += (IdList is not null ? IdList.Count() : 0) * sizeof(uint);

                foreach(var entry in CopyTable is not null ? CopyTable : new CopyTableEntry[0])
                    size += entry.SizeOf;

                foreach(var entry in OffsetMap is not null ? OffsetMap : new OffsetMapEntry[0])
                    size += entry.SizeOf;

                size += RelationshipMap is not null ? RelationshipMap.SizeOf : 0;

                size += (OffsetMapIdList is not null ? OffsetMapIdList.Count() : 0) * sizeof(uint);

                return size;
            }
        }

        public int SizeOf => SizeOfWithoutVariableRecordData + (VariableRecordData is not null ? VariableRecordData.Count() : 0);
    }
}