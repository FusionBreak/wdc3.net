using System.Collections.Generic;

namespace wdc3.net.File
{
    public interface ISection
    {
        IEnumerable<uint>? IdList { get; set; }
        IEnumerable<CopyTableEntry>? CopyTable { get; set; }
        IEnumerable<OffsetMapEntry>? OffsetMap { get; set; }
        RelationshipMapping? RelationshipMap { get; set; }
        IEnumerable<uint>? OffsetMapIdList { get; set; }
    }
}