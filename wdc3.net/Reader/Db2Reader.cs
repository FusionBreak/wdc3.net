using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
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

            Db2 db = new Db2();

            HeaderReader headerReader = new HeaderReader(_reader);
            db.Header = headerReader.Read();

            SectionHeaderReader sectionHeaderReader = new SectionHeaderReader(_reader, (int)db.Header.SectionCount); 
            db.SectionHeaders = sectionHeaderReader.Read();

            FieldStructureReader fieldStructureReader = new FieldStructureReader(_reader, (int)db.Header.TotalFieldCount);
            db.FieldStructures = fieldStructureReader.Read();

            FieldStorageInfoReader fieldStorageInfoReader = new FieldStorageInfoReader(_reader, (int)db.Header.FieldStorageInfoSize);
            db.FieldStorageInfos = fieldStorageInfoReader.Read();

            db.PalletData = ReadPalletData(_reader, (int)db.Header.PalletDataSize);
            db.CommonData = ReadCommonData(_reader, (int)db.Header.CommonDataSize);

            SectionReader sectionReader = new SectionReader(_reader, db.SectionHeaders, db.Header.Flags, db.Header.RecordSize);
            db.Sections = sectionReader.Read();

            return db;
        }

        public IEnumerable<byte> ReadPalletData(BinaryReader reader, int palletDataSize)
        {
            List<byte> output = new List<byte>();

            for(int currentPalletData = 0; currentPalletData < palletDataSize; currentPalletData++)
                output.Add(reader.ReadByte());
            
            return output;
        }

        public IEnumerable<byte> ReadCommonData(BinaryReader reader, int commonDataSize)
        {
            List<byte> output = new List<byte>();

            for(int currentCommonData = 0; currentCommonData < commonDataSize; currentCommonData++)
                output.Add(reader.ReadByte());

            return output;
        }
    }
}
