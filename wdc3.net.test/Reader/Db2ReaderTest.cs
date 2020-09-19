using System.IO;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class Db2ReaderTest
    {
        [Fact]
        public void ReadsTheWholeFile()
        {
            var mapDb2 = new FileInfo(@"..\..\..\TestFiles\map.db2");
            var reader = new Db2Reader();
            var db2 = reader.ReadFile(mapDb2.FullName);
            _ = db2;
            Assert.Equal(mapDb2.Length, reader.BytesReaded);
        }
    }
}