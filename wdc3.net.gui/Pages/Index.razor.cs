using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wdc3.net.Table;

namespace wdc3.net.gui.Pages
{
    public partial class Index : ComponentBase
    {
        public Db2Table Table { get; set; }

        protected override void OnInitialized()
        {
            var reader = new Db2ToTableReader(@"D:\Work\wdc3.net\wdc3.net.test\TestFiles\map.db2", @"D:\Work\wdc3.net\wdc3.net.test\TestFiles\Map.dbd");
            Table = reader.Read();
        }
    }
}
