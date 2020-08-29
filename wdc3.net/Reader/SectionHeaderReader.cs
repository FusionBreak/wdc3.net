using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    internal class SectionHeaderReader : IFileReader<IEnumerable<SectionHeader>>
    {
        BinaryReader _reader;
        int _sectionCount;
        public long Position { get; private set; }

        public SectionHeaderReader(BinaryReader reader, int sectionCount)
        {
            _reader = reader;
            _sectionCount = sectionCount;
        }

        public IEnumerable<SectionHeader> Read()
        {
            var output = new List<SectionHeader>();

            for(int currentCount = 0; currentCount < _sectionCount; currentCount++)
            {
                var sectionHeader = new SectionHeader();

                sectionHeader.TactKeyHash = _reader.ReadUInt64();
                sectionHeader.FileOffset = _reader.ReadUInt32();
                sectionHeader.RecordCount = _reader.ReadUInt32();
                sectionHeader.StringTableSize = _reader.ReadUInt32();
                sectionHeader.OffsetRecordsEnd = _reader.ReadUInt32();
                sectionHeader.IdListSize = _reader.ReadUInt32();
                sectionHeader.RelationsshipDataSize = _reader.ReadUInt32();
                sectionHeader.OffsetMapIdCount = _reader.ReadUInt32();
                sectionHeader.CopyTableCount = _reader.ReadUInt32();

                output.Add(sectionHeader);
            }

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}