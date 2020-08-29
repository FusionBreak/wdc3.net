using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.dbd.File
{
    public class ColumnDefinition
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool Verified { get; set; }
        public string? Comment { get; set; }
        public string? ForeignTable { get; set; }
        public string? ForeignColumn { get; set; }
    }
}
