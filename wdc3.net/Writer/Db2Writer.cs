using System;
using System.IO;
using System.Linq;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    public class Db2Writer
    {
        private readonly BinaryWriter? _writer;

        public void WriteFile(Db2 db2, string path)
        {
            var fileInfo = new System.IO.FileInfo(path);
            System.IO.Directory.CreateDirectory(fileInfo.Directory?.FullName ?? throw new Exception());

            using(var writer = new BinaryWriter(System.IO.File.Open(path, FileMode.Create)))
            {
                writer.Write(new HeaderSerializer().Serialze(db2.Header ?? throw new NullReferenceException(nameof(Db2.Header))));
                writer.Write(new SectionHeaderSerializer().Serialze(db2.SectionHeaders ?? throw new NullReferenceException(nameof(Db2.SectionHeaders))));
                writer.Write(new FieldStructureSerializer().Serialze(db2.FieldStructures ?? throw new NullReferenceException(nameof(Db2.FieldStructures))));
                writer.Write(new FieldStorageInfoSerializer().Serialze(db2.FieldStorageInfos ?? throw new NullReferenceException(nameof(Db2.FieldStorageInfos))));

                if(db2.PalletData is not null)
                    writer.Write(db2.PalletData.ToArray());

                if(db2.CommonData is not null)
                    writer.Write(db2.CommonData.ToArray());

                writer.Write(new SectionSerializer().Serialze(db2.Sections ?? throw new NullReferenceException(nameof(Db2.Sections))));
            }
        }
    }
}