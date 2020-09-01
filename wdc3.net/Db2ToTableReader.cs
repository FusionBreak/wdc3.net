using System;
using System.IO;
using wdc3.net.dbd.Reader;
using wdc3.net.Enums;
using wdc3.net.Reader;
using wdc3.net.Table;

namespace wdc3.net
{
    public class Db2ToTableReader
    {
        public Db2Table Read(string db2Path, string dbdPath)
        {
            var output = new Db2Table();

            var db2File = new FileInfo(db2Path);
            var dbdFile = new FileInfo(dbdPath);
            var db2 = new Db2Reader().ReadFile(db2File.FullName);
            var dbd = new DbdReader().ReadFile(dbdFile.FullName);

            if(db2.Header == null)
                throw new Exception();

            output.Name = db2File.Name;
            output.Locale = ((Locales)db2.Header.Locale).ToString();

            foreach(var colInfo in new TableColumnInformationFactory().CreateColumnInformation(dbd, db2.Header.LayoutHash))
                output.AddColumn(colInfo.Name, colInfo.Type);

            return output;
        }
    }
}