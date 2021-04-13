using System.Collections.Generic;
using System.Linq;
using wdc3.net.Helper;

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

                size += IdList.Count() * sizeof(uint);

                foreach(var entry in CopyTable)
                    size += entry.SizeOf;

                foreach(var entry in OffsetMap)
                    size += entry.SizeOf;

                size += RelationshipMap.SizeOf;

                size += OffsetMapIdList.Count() * sizeof(uint);

                return size;
            }
        }

        public int SizeOf => SizeOf + VariableRecordData.Count();
    }
}