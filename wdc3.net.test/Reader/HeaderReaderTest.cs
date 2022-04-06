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
            var fileBuffer = System.IO.File.ReadAllBytes(TestFiles.MAP_DB2_PATH);
            var reader = new BinaryReader(new MemoryStream(fileBuffer));
            var headerReader = new HeaderReader(reader);
            var header = headerReader.Read();
            Assert.Equal(860046423, header.Magic);
            Assert.Equal((uint)1085, header.RecordCount);
            Assert.Equal((uint)24, header.FieldCount);
            Assert.Equal((uint)52, header.RecordSize);
            Assert.Equal((uint)58419, header.StringTableSize);
            Assert.Equal(3179597154, header.TableHash);
            Assert.Equal((uint)2034192014, header.LayoutHash);
            Assert.Equal((uint)0, header.MinId);
            Assert.Equal((uint)2453, header.MaxId);
            Assert.Equal((uint)1, header.Locale);
            Assert.Equal((uint)4, header.Flags);
            Assert.Equal((uint)0, header.IdIndex);
            Assert.Equal((uint)24, header.TotalFieldCount);
            Assert.Equal((uint)36, header.BitpackedDataOffset);
            Assert.Equal((uint)0, header.LookUpColumnCount);
            Assert.Equal((uint)576, header.FieldStorageInfoSize);
            Assert.Equal((uint)0, header.CommonDataSize);
            Assert.Equal((uint)2552, header.PalletDataSize);
            Assert.Equal((uint)4, header.SectionCount);
        }
    }
}