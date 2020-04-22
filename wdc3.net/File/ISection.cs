using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public interface ISection
    {
        IEnumerable<uint>? IdList { get; set; }
        CopyTableEntry? CopyTable { get; set; }
        OffsetMapEntry? OffsetMap { get; set; }
        RelationshipMapping? RelationshipMap { get; set; }
        IEnumerable<uint>? OffsetMapIdList { get; set; }
    }
}
