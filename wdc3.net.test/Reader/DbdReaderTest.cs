using System.IO;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class DbdReaderTest
    {
        [Fact]
        public void ReadsTheWholeFile()
        {
            var mapDbd = new FileInfo(TestFiles.MAP_DBD_PATH);
            var reader = new DbdReader();
            var dbd = reader.ReadFile(mapDbd.FullName);
            _ = dbd;
            Assert.True(reader.AllLinesReaded);
        }
    }
}