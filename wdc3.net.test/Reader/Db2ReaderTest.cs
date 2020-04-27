using System;
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
            FileInfo mapDb2 = new FileInfo(@"..\..\..\TestFiles\map.db2");
            Db2Reader reader = new Db2Reader();
            reader.ReadFile(mapDb2.FullName);
            Assert.Equal(mapDb2.Length, reader.BytesReaded);
        }
    }
}