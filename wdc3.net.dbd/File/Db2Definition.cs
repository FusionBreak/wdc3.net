using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.dbd.File
{
    public class Db2Definition
    {
        public IEnumerable<ColumnDefinition>? ColumnDefinitions { get; set; }
        public IEnumerable<VersionDefinition>? VersionDefinitions { get; set; }
        public string Comment { get; set; }
    }
}
