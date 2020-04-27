using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class Db2Reader
    {
        public long BytesReaded { get; private set; }
        public Db2 ReadFile(string path)
        {
            var fileBuffer = System.IO.File.ReadAllBytes(path);
            Db2 db = new Db2();
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));

            HeaderReader headerReader = new HeaderReader(reader);
            db.Header = headerReader.Read();

            SectionHeaderReader sectionHeaderReader = new SectionHeaderReader(reader, (int)db.Header.SectionCount); 
            db.SectionHeaders = sectionHeaderReader.Read();

            FieldStructureReader fieldStructureReader = new FieldStructureReader(reader, (int)db.Header.TotalFieldCount);
            db.FieldStructures = fieldStructureReader.Read();

            FieldStorageInfoReader fieldStorageInfoReader = new FieldStorageInfoReader(reader, (int)db.Header.FieldStorageInfoSize);
            db.FieldStorageInfos = fieldStorageInfoReader.Read();

            db.PalletData = ReadPalletData(reader, (int)db.Header.PalletDataSize);
            db.CommonData = ReadCommonData(reader, (int)db.Header.CommonDataSize);

            SectionReader sectionReader = new SectionReader(reader, db.SectionHeaders, db.Header.Flags, db.Header.RecordSize);
            db.Sections = sectionReader.Read();

            BytesReaded = reader.BaseStream.Position;

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
