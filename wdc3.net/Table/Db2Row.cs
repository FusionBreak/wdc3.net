using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2Row
    {
        public uint Id => RowInfo.Id;
        public Db2Cell[] Cells { get; init; }
        public SectionHeader DependentSectionHeader { get; init; }
        public RowInfo RowInfo { get; init; }

        public Db2Row(Db2Cell[] cells, SectionHeader dependentSectionHeader, RowInfo rowInfo)
        {
            Cells = cells;
            DependentSectionHeader = dependentSectionHeader;
            RowInfo = rowInfo;
        }
    }
}