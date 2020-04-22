using System;
using System.IO;
using wdc3.net.Reader;
using Xunit;


namespace wdc3.net.test.Reader
{
    public class HeaderReaderTest
    {
        [Fact]
        public void ReadHeaderCorrectly()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(@"..\..\..\TestFiles\map.db2");
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            HeaderReader headerReader = new HeaderReader(reader);
            var header = headerReader.Read();
            Assert.Equal(860046423, header.Magic);
        }
    }
}