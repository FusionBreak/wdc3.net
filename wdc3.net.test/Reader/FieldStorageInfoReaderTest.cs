using System.IO;
using System.Linq;
using wdc3.net.File;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class FieldStorageInfoReaderTest
    {
        [Fact]
        public void ReadCorrectNumberOfFieldStorageInfos()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(TestFiles.MAP_DB2_PATH);
            var reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 324;
            var fieldStorageInfoReader = new FieldStorageInfoReader(reader, 552);
            var fieldStorageInfos = fieldStorageInfoReader.Read();

            Assert.Equal(23, fieldStorageInfos.Count());
        }

        [Fact]
        public void ReadFieldStorageInfoCorrectly()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(TestFiles.MAP_DB2_PATH);
            var reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 324;
            var fieldStorageInfoReader = new FieldStorageInfoReader(reader, 552);
            var fieldStorageInfos = fieldStorageInfoReader.Read().ToList();

            Assert.IsType<FieldStorageInfo>(fieldStorageInfos[0]);
            Assert.Equal(0, fieldStorageInfos[0].FieldOffsetBits);
            Assert.Equal(32, fieldStorageInfos[0].FieldSizeBits);
            Assert.Equal((uint)0, fieldStorageInfos[0].AdditionalDataSize);

            Assert.IsType<FieldStorageInfoBitPackedIndexed>(fieldStorageInfos[7]);
            Assert.Equal(256, fieldStorageInfos[7].FieldOffsetBits);
            Assert.Equal(3, fieldStorageInfos[7].FieldSizeBits);
            Assert.Equal((uint)16, fieldStorageInfos[7].AdditionalDataSize);

            Assert.IsType<FieldStorageInfoBitPackedIndexedArray>(fieldStorageInfos[22]);
            Assert.Equal(372, fieldStorageInfos[22].FieldOffsetBits);
            Assert.Equal(9, fieldStorageInfos[22].FieldSizeBits);
            Assert.Equal((uint)2240, fieldStorageInfos[22].AdditionalDataSize);
        }
    }
}