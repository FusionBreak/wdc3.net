using System.Linq;
using System;
using System.IO;
using wdc3.net.Reader;
using Xunit;
using System.Collections.Generic;
using System.Collections;
using wdc3.net.File;

namespace wdc3.net.test.Reader
{
    public class SectionHeaderReaderTest
    {
        [Fact]
        public void ReadCorrectNumberOfSectionHeaders()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(@"..\..\..\TestFiles\map.db2");
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 72;
            SectionHeaderReader sectionHeaderReader = new SectionHeaderReader(reader, 4);
            var sectionHeaders = sectionHeaderReader.Read();

            Assert.Equal(4, sectionHeaders.Count());
        }

        [Fact]
        public void ReadSectionHeadersCorrectly()
        {
            var fileBuffer = System.IO.File.ReadAllBytes(@"..\..\..\TestFiles\map.db2");
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            reader.BaseStream.Position = 72;
            SectionHeaderReader sectionHeaderReader = new SectionHeaderReader(reader, 4);
            var sectionHeaders = sectionHeaderReader.Read().ToList();
            
            for (int i = 0; i < TestData.Count; i++)
            {
                Assert.Equal(TestData[i].TactKeyHash, sectionHeaders[i].TactKeyHash);
                Assert.Equal(TestData[i].FileOffset, sectionHeaders[i].FileOffset);
                Assert.Equal(TestData[i].RecordCount, sectionHeaders[i].RecordCount);
                Assert.Equal(TestData[i].StringTableSize, sectionHeaders[i].StringTableSize);
                Assert.Equal(TestData[i].OffsetRecordsEnd, sectionHeaders[i].OffsetRecordsEnd);
                Assert.Equal(TestData[i].IdListSize, sectionHeaders[i].IdListSize);
                Assert.Equal(TestData[i].RelationsshipDataSize, sectionHeaders[i].RelationsshipDataSize);
                Assert.Equal(TestData[i].OffsetMapIdCount, sectionHeaders[i].OffsetMapIdCount);
                Assert.Equal(TestData[i].CopyTableCount, sectionHeaders[i].CopyTableCount);
            }
        }

        public static readonly List<SectionHeader> TestData = new List<SectionHeader>()
        {
            new SectionHeader() {
                TactKeyHash = 0,
                FileOffset = 3252,
                RecordCount = 1022,
                StringTableSize = 55803,
                OffsetRecordsEnd = 0,
                IdListSize = 4088,
                RelationsshipDataSize = 0,
                OffsetMapIdCount = 0,
                CopyTableCount = 0
            },
            new SectionHeader() {
                TactKeyHash = 1725015195270896445,
                FileOffset = 112199,
                RecordCount = 1,
                StringTableSize = 24,
                OffsetRecordsEnd = 0,
                IdListSize = 4,
                RelationsshipDataSize = 0,
                OffsetMapIdCount = 0,
                CopyTableCount = 0
            },
            new SectionHeader() {
                TactKeyHash = 9485332275027175826,
                FileOffset = 112275,
                RecordCount = 1,
                StringTableSize = 21,
                OffsetRecordsEnd = 0,
                IdListSize = 4,
                RelationsshipDataSize = 0,
                OffsetMapIdCount = 0,
                CopyTableCount = 0
            },
            new SectionHeader() {
                TactKeyHash = 15074942342469000434,
                FileOffset = 112348,
                RecordCount = 1,
                StringTableSize = 28,
                OffsetRecordsEnd = 0,
                IdListSize = 4,
                RelationsshipDataSize = 0,
                OffsetMapIdCount = 0,
                CopyTableCount = 0
            },
        };
    }
}