using System.Linq;
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
            Assert.Equal(25, table.ColumnCount);
        }

        [Theory]
        [InlineData(0, new string[] { "25", "-1", "\"\"", "\"\"", "\"\"", "\"\"", "\"Worn Shortsword\"", "0.5", "0", "0", "0", "0", "[0,0,0,0,0,0,0,0,0,0]", "[0,0,0,0,0,0,0,0,0,0]", "1", "0", "0", "3", "18", "1", "1", "0.972", "[0,8192,8388608,0]", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "[0,0]", "0", "0", "0", "0", "2600", "0", "0", "0", "1", "-1", "0", "0", "0", "0", "[0,0,0]", "3", "1", "0", "1", "0", "0", "[0,0,0,0,0,0,0,0,0,0]", "0", "0", "0", "0", "1", "21", "1" })]
        [InlineData(1, new string[] { "35", "-1", "\"\"", "\"\"", "\"\"", "\"\"", "\"Bent Staff\"", "0.3", "0", "0", "0", "0", "[0,0,0,0,0,0,0,0,0,0]", "[0,0,0,0,0,0,0,0,0,0]", "1", "0", "0", "4", "24", "1", "1", "1.0353", "[0,8192,8388608,0]", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "[0,0]", "0", "0", "0", "0", "3600", "0", "0", "0", "1", "-1", "0", "0", "0", "0", "[0,0,0]", "2", "2", "0", "0", "0", "0", "[0,0,0,0,0,0,0,0,0,0]", "0", "0", "0", "0", "1", "17", "1" })]
        public void CanReadFirstTwoLinesOfItemSparse(int line, string[] expectedValues)
        {
            var reader = new Db2ToTableReader(TestFiles.ITEM_SPARSE_DB2_PATH, TestFiles.ITEM_SPARSE_DBD_PATH);
            var table = reader.Read();
            var x = table.ValuesAsJson.ToArray();
            var readed = string.Join(", ", table.ValuesAsJson.Skip(line).First());
            var expected = string.Join(", ", expectedValues.Select(value => value.ToString()).ToArray());

            Assert.Equal(expected, readed);
        }
    }
}