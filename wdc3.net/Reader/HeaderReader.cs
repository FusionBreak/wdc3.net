using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class HeaderReader : IFileReader<Header>
    {
        private readonly BinaryReader _reader;
        public long Position { get; private set; }

        public HeaderReader(BinaryReader reader) => _reader = reader;

        public Header Read()
        {
            var output = new Header
            {
                Magic = _reader.ReadInt32(),
                RecordCount = _reader.ReadUInt32(),
                FieldCount = _reader.ReadUInt32(),
                RecordSize = _reader.ReadUInt32(),
                StringTableSize = _reader.ReadUInt32(),
                TableHash = _reader.ReadUInt32(),
                LayoutHash = _reader.ReadUInt32(),
                MinId = _reader.ReadUInt32(),
                MaxId = _reader.ReadUInt32(),
                Locale = _reader.ReadUInt32(),
                Flags = _reader.ReadUInt16(),
                IdIndex = _reader.ReadUInt16(),
                TotalFieldCount = _reader.ReadUInt32(),
                BitpackedDataOffset = _reader.ReadUInt32(),
                LookUpColumnCount = _reader.ReadUInt32(),
                FieldStorageInfoSize = _reader.ReadUInt32(),
                CommonDataSize = _reader.ReadUInt32(),
                PalletDataSize = _reader.ReadUInt32(),
                SectionCount = _reader.ReadUInt32()
            };

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}