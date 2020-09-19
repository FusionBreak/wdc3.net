using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class SectionHeaderReader : IFileReader<IEnumerable<SectionHeader>>
    {
        private readonly BinaryReader _reader;
        private readonly int _sectionCount;
        public long Position { get; private set; }

        public SectionHeaderReader(BinaryReader reader, int sectionCount)
        {
            _reader = reader;
            _sectionCount = sectionCount;
        }

        public IEnumerable<SectionHeader> Read()
        {
            var output = new List<SectionHeader>();

            for(var currentCount = 0; currentCount < _sectionCount; currentCount++)
            {
                var sectionHeader = new SectionHeader
                {
                    TactKeyHash = _reader.ReadUInt64(),
                    FileOffset = _reader.ReadUInt32(),
                    RecordCount = _reader.ReadUInt32(),
                    StringTableSize = _reader.ReadUInt32(),
                    OffsetRecordsEnd = _reader.ReadUInt32(),
                    IdListSize = _reader.ReadUInt32(),
                    RelationsshipDataSize = _reader.ReadUInt32(),
                    OffsetMapIdCount = _reader.ReadUInt32(),
                    CopyTableCount = _reader.ReadUInt32()
                };

                output.Add(sectionHeader);
            }

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}