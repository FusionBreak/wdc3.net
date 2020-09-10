using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.Table
{
    public class ColumnInfo
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }
        public bool IsId { get; set; }
    }
}
