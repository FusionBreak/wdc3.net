using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.Reader;
using wdc3.net.Writer;
using Xunit;

namespace wdc3.net.test
{
    public class ReaderWriterTest
    {
        [Fact]
        public void ReadAndWriteCorrectly()
        {
            var db2 = new Db2Reader().ReadFile(TestFiles.MAP_DB2_PATH);
            new Db2Writer().WriteFile(db2, TestFiles.MAP_DB2_OUTPUT_PATH);
        }
    }
}