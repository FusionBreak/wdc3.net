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
            Assert.Equal(1025, header.RecordCount);
            Assert.Equal(23, header.FieldCount);
            Assert.Equal(48, header.RecordSize);
            Assert.Equal(55876, header.StringTableSize);
            Assert.Equal(3179597154, header.TableHash);
            Assert.Equal(3667170223, header.LayoutHash);
            Assert.Equal(0, header.MinId);
            Assert.Equal(2297, header.MaxId);
            Assert.Equal(1, header.Locale);
            Assert.Equal(4, header.Flags);
            Assert.Equal(0, header.IdIndex);
            Assert.Equal(23, header.TotalFieldCount);
            Assert.Equal(32, header.BitpackedDataOffset);
            Assert.Equal(552, header.FieldStorageInfoSize);
            Assert.Equal(0, header.CommonDataSize);
            Assert.Equal(2376, header.PalletDataSize);
            Assert.Equal(4, header.SectionCount);
        }
    }
}