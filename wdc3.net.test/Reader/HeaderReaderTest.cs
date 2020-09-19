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
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            HeaderReader headerReader = new HeaderReader(reader);
            var header = headerReader.Read();
            Assert.Equal(860046423, header.Magic);
            Assert.Equal((uint)1025, header.RecordCount);
            Assert.Equal((uint)23, header.FieldCount);
            Assert.Equal((uint)48, header.RecordSize);
            Assert.Equal((uint)55876, header.StringTableSize);
            Assert.Equal((uint)3179597154, header.TableHash);
            Assert.Equal((uint)3667170223, header.LayoutHash);
            Assert.Equal((uint)0, header.MinId);
            Assert.Equal((uint)2297, header.MaxId);
            Assert.Equal((uint)1, header.Locale);
            Assert.Equal((uint)4, header.Flags);
            Assert.Equal((uint)0, header.IdIndex);
            Assert.Equal((uint)23, header.TotalFieldCount);
            Assert.Equal((uint)32, header.BitpackedDataOffset);
            Assert.Equal((uint)0, header.LookUpColumnCount);
            Assert.Equal((uint)552, header.FieldStorageInfoSize);
            Assert.Equal((uint)0, header.CommonDataSize);
            Assert.Equal((uint)2376, header.PalletDataSize);
            Assert.Equal((uint)4, header.SectionCount);
        }
    }
}