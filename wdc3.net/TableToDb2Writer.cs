using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.File;
using wdc3.net.Reader;
using wdc3.net.Table;
using wdc3.net.Writer;

namespace wdc3.net
{
    public class TableToDb2Writer
    {
        private Db2Table _table;
        private readonly Db2Definition _dbd;
        private readonly Db2CreateInformation _createInformation;

        public TableToDb2Writer(Db2Table table, string dbdPath, Db2CreateInformation createInformation)
        {
            _table = table;
            _dbd = new DbdReader().ReadFile(dbdPath);
            _createInformation = createInformation;
        }

        public void Write(string path)
        {
            var db2 = new Db2()
            {
                Header = GetHeader(),
                //SectionHeaders = new List<SectionHeader>(),
                //FieldStructures = new List<FieldStructure>(),
                //FieldStorageInfos = new List<FieldStorageInfoCommonData>(),
                //Sections = new List<Section>()
            };

            new Db2Writer().WriteFile(db2, path);
        }

        private Header GetHeader()
        {
            return new Header()
            {
                Magic = _createInformation.Magic,
                RecordCount = (uint)_table.Values.Count(),
                FieldCount = (uint)_table.ColumnCount
            };
        }
    }
}