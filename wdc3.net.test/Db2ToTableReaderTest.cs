using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace wdc3.net.test
{
    public class Db2ToTableReaderTest
    {
        [Fact]
        public void XXX()
        {
            var reader = new Db2ToTableReader();
            var tabel = reader.Read(@"..\..\..\TestFiles\map.db2", @"..\..\..\TestFiles\Map.dbd", "8.0.1.25976");
        }
    }
}
