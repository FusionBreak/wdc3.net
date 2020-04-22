using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class HeaderReader : IFileReader<Header>
    {
        BinaryReader _reader;
        public long Position { get; private set; }
        public HeaderReader(BinaryReader reader)
        {
            _reader = reader;
        }

        public Header Read()
        {
            var output = new Header();

            output.Magic = _reader.ReadInt32();
            output.RecordCount = _reader.ReadUInt32();
            output.FieldCount = _reader.ReadUInt32();
            output.RecordSize = _reader.ReadUInt32();
            output.StringTableSize = _reader.ReadUInt32();
            output.TableHash = _reader.ReadUInt32();
            output.LayoutHash = _reader.ReadUInt32();
            output.MinId = _reader.ReadUInt32();
            output.MaxId = _reader.ReadUInt32();
            output.Locale = _reader.ReadUInt32();
            output.Flags = _reader.ReadUInt16();
            output.IdIndex = _reader.ReadUInt16();
            output.TotalFieldCount = _reader.ReadUInt32();
            output.BitpackedDataOffset = _reader.ReadUInt32();
            output.LookUpColumnCount = _reader.ReadUInt32();
            output.FieldStorageInfoSize = _reader.ReadUInt32();
            output.CommonDataSize = _reader.ReadUInt32();
            output.PalletDataSize = _reader.ReadUInt32();
            output.SectionCount = _reader.ReadUInt32();

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}