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
                SectionHeaders = DetermineAllSectionHeaders(),
                FieldStructures = _createInformation.FieldStructures,
                FieldStorageInfos = _createInformation.FieldStorageInfos,
                Sections = new List<Section>(),
                CommonData = null,
                PalletData = null,
            };

            new Db2Writer().WriteFile(db2, path);
        }

        private Header GetHeader()
        {
            return new Header()
            {
                Magic = _createInformation.Magic,
                RecordCount = (uint)_table.Values.Count(),
                FieldCount = (uint)_table.ColumnCount,

                TableHash = _createInformation.TableHash,
                MinId = _table.Rows.OrderBy(row => row.Id).First().Id,
                MaxId = _table.Rows.OrderBy(row => row.Id).Last().Id,
                Locale = _createInformation.Locale,
                Flags = _createInformation.Flags,
                TotalFieldCount = (uint)_table.ColumnCount,

                SectionCount = (uint)DetermineAllSectionHeaders().Count(),
            };
        }

        private IEnumerable<SectionHeader> DetermineAllSectionHeaders() => _table.Rows.Select(row => row.DependentSectionHeader).Distinct();
    }
}