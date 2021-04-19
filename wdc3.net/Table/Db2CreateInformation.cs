using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wdc3.net.Table
{
    public class Db2CreateInformation
    {
        public int Magic { get; set; }
        public uint TableHash { get; set; }
        public uint LayoutHash { get; set; }
        public uint Locale { get; set; }
        public ushort Flags { get; set; }
    }
}