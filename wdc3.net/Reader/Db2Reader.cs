using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class Db2Reader
    {
        public long BytesReaded => _reader != null ? _reader.BaseStream.Position : 0;

        private byte[]? _fileBuffer;
        private BinaryReader? _reader;

        public Db2 ReadFile(string path)
        {
            _fileBuffer = System.IO.File.ReadAllBytes(path);
            _reader = new BinaryReader(new MemoryStream(_fileBuffer));

            var db = new Db2();

            var headerReader = new HeaderReader(_reader);
            db.Header = headerReader.Read();

            var sectionHeaderReader = new SectionHeaderReader(_reader, (int)db.Header.SectionCount);
            db.SectionHeaders = sectionHeaderReader.Read();

            var fieldStructureReader = new FieldStructureReader(_reader, (int)db.Header.TotalFieldCount);
            db.FieldStructures = fieldStructureReader.Read();

            var fieldStorageInfoReader = new FieldStorageInfoReader(_reader, (int)db.Header.FieldStorageInfoSize);
            db.FieldStorageInfos = fieldStorageInfoReader.Read();

            db.PalletData = ReadPalletData(_reader, (int)db.Header.PalletDataSize);
            db.CommonData = ReadCommonData(_reader, (int)db.Header.CommonDataSize);

            var sectionReader = new SectionReader(_reader, db.SectionHeaders, db.Header.Flags, db.Header.RecordSize);
            db.Sections = sectionReader.Read();

            return db;
        }

        public IEnumerable<byte> ReadPalletData(BinaryReader reader, int palletDataSize)
        {
            var output = new List<byte>();

            for(int currentPalletData = 0; currentPalletData < palletDataSize; currentPalletData++)
                output.Add(reader.ReadByte());

            return output;
        }

        public IEnumerable<byte> ReadCommonData(BinaryReader reader, int commonDataSize)
        {
            var output = new List<byte>();

            for(int currentCommonData = 0; currentCommonData < commonDataSize; currentCommonData++)
                output.Add(reader.ReadByte());

            return output;
        }
    }
}