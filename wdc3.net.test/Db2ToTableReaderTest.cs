using Xunit;

namespace wdc3.net.test
{
    public class Db2ToTableReaderTest
    {
        [Fact]
        public void XXX()
        {
            //Compare with https://wow.tools/dbc/?dbc=map&build=8.3.7.34872#page=1

            var reader = new Db2ToTableReader(TestFiles.MAP_DB2_PATH, TestFiles.MAP_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsCorrectNumberOfColumns()
        {
            var reader = new Db2ToTableReader(TestFiles.MAP_DB2_PATH, TestFiles.MAP_DBD_PATH);
            var table = reader.Read();
            Assert.Equal(24, table.ColumnCount);
        }
    }
}