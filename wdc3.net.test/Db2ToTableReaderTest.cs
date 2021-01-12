using Xunit;

namespace wdc3.net.test
{
    public class Db2ToTableReaderTest
    {
        [Fact]
        public void ReadsMap()
        {
            //Compare with https://wow.tools/dbc/?dbc=map&build=8.3.7.34872#page=1

            var reader = new Db2ToTableReader(TestFiles.MAP_DB2_PATH, TestFiles.MAP_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsMap902()
        {
            var reader = new Db2ToTableReader(TestFiles.MAP_9_DB2_PATH, TestFiles.MAP_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsItem()
        {
            var reader = new Db2ToTableReader(TestFiles.ITEM_DB2_PATH, TestFiles.ITEM_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsItemSparse()
        {
            var reader = new Db2ToTableReader(TestFiles.ITEM_SPARSE_DB2_PATH, TestFiles.ITEM_SPARSE_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsLocale()
        {
            var reader = new Db2ToTableReader(TestFiles.LOCALE_DB2_PATH, TestFiles.LOCALE_DBD_PATH);
            var table = reader.Read();
            _ = table;
        }

        [Fact]
        public void ReadsTextureFileData()
        {
            var reader = new Db2ToTableReader(TestFiles.TEXTURE_FILE_DATA_DB2_PATH, TestFiles.TEXTURE_FILE_DATA_DBD_PATH);
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