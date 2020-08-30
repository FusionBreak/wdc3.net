using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wdc3.net.dbd.File;
using wdc3.net.dbd.Reader;
using wdc3.net.Reader;
using wdc3.net.Table;

namespace wdc3.net
{
    public class Db2ToTableReader
    {
        public Db2Table Read(string db2Path, string dbdPath, string buildString)
        {
            var output = new Db2Table();

            var db2 = new Db2Reader().ReadFile(db2Path);
            var dbd = new DbdReader().ReadFile(dbdPath);

            var columnInformations = new TableColumnInformationFactory().CreateColumnInformation(dbd, buildString);

            foreach(var colInfo in columnInformations)
                output.AddColumn(colInfo.Name, colInfo.Type);


            return output;
        }
    }
}
