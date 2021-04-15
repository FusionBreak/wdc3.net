using System.IO;
using System.Linq;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class FieldStructureReaderTest
    {
        [Fact]
        public void ReadCorrectNumberOfFieldStructures()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(TestFiles.MAP_DB2_PATH);
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 232;
            FieldStructureReader fieldStructureReader = new FieldStructureReader(reader, 23);
            var fieldStructures = fieldStructureReader.Read();

            Assert.Equal(23, fieldStructures.Count());
        }

        [Fact]
        public void ReadFieldStructuresCorrectly()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(TestFiles.MAP_DB2_PATH);
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 232;
            FieldStructureReader fieldStructureReader = new FieldStructureReader(reader, 23);
            var fieldStructures = fieldStructureReader.Read().ToList();

            Assert.Equal(0, fieldStructures[0].Size);
            Assert.Equal(0, fieldStructures[0].Position);

            Assert.Equal(0, fieldStructures[6].Size);
            Assert.Equal(24, fieldStructures[6].Position);

            Assert.Equal(32, fieldStructures[22].Size);
            Assert.Equal(36, fieldStructures[22].Position);
        }
    }
}