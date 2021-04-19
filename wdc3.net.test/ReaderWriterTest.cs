using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.Reader;
using wdc3.net.Table;
using wdc3.net.Writer;
using Xunit;

namespace wdc3.net.test
{
    public class ReaderWriterTest
    {
        [Fact]
        public void ReadAndWriteCorrectlyTheSameFile()
        {
            var db2 = new Db2Reader().ReadFile(TestFiles.MAP_DB2_PATH);
            new Db2Writer().WriteFile(db2, TestFiles.MAP_DB2_OUTPUT_PATH);
        }

        [Fact]
        public void ReadAndWriteCorrectlyModified()
        {
            var reader = new Db2ToTableReader(TestFiles.MAP_DB2_PATH, TestFiles.MAP_DBD_PATH);
            var table = reader.Read();
            var writer = new TableToDb2Writer(
                                table,
                                TestFiles.MAP_DBD_PATH,
                                Db2CreateInformationFactory.CreateDb2CreateInformation(reader.SourceDb2)
            );

            writer.Write(TestFiles.MAP_DB2_OUTPUT_PATH);
        }
    }
}