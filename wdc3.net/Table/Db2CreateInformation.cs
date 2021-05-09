using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2CreateInformation
    {
        public int Magic { get; set; }
        public uint TableHash { get; set; }
        public uint LayoutHash { get; set; }
        public uint Locale { get; set; }
        public ushort Flags { get; set; }
        public uint FieldStorageInfoSize { get; set; }

        public IEnumerable<FieldStructure>? FieldStructures { get; set; }
        public IEnumerable<IFieldStorageInfo>? FieldStorageInfos { get; set; }
    }
}