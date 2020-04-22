using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class HeaderReader
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
            output.RecordCount = _reader.ReadInt32();
            output.FieldCount = _reader.ReadInt32();
            output.RecordSize = _reader.ReadInt32();
            output.StringTableSize = _reader.ReadInt32();
            output.TableHash = _reader.ReadUInt32();
            output.LayoutHash = _reader.ReadUInt32();
            output.MinId = _reader.ReadInt32();
            output.MaxId = _reader.ReadInt32();
            output.Locale = _reader.ReadInt32();
            output.Flags = _reader.ReadInt16();
            output.IdIndex = _reader.ReadInt16();
            output.TotalFieldCount = _reader.ReadInt32();
            output.BitpackedDataOffset = _reader.ReadInt32();
            output.LookUpColumnCount = _reader.ReadInt32();
            output.FieldStorageInfoSize = _reader.ReadInt32();
            output.CommonDataSize = _reader.ReadInt32();
            output.PalletDataSize = _reader.ReadInt32();
            output.SectionCount = _reader.ReadInt32();

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}