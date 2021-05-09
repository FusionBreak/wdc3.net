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
        private readonly Db2ValueInserterNoOffsetFlag _insterter;

        private decimal RecordSize => Math.Ceiling((decimal)(_table.Rows.First().Cells?.Select(cell => cell.FieldStorageInfo?.FieldSizeBits).Sum(x => x) ?? 0) / 8);
        private int RecordCount => _table.Values.Count();
        private decimal RecordDataSize => RecordSize * RecordCount;

        public TableToDb2Writer(Db2Table table, string dbdPath, Db2CreateInformation createInformation)
        {
            _table = table;
            _insterter = new Db2ValueInserterNoOffsetFlag(RecordDataSize, RecordSize);
            _dbd = new DbdReader().ReadFile(dbdPath);
            _createInformation = createInformation;

            foreach(var row in _table.Rows)
                _insterter.ProcessRow(row);
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
                CommonData = new List<byte>(),
                PalletData = _insterter.PalletData,
            };

            new Db2Writer().WriteFile(db2, path);
        }

        private Header GetHeader()
        {
            return new Header()
            {
                Magic = _createInformation.Magic,
                RecordCount = (uint)RecordCount,
                FieldCount = (uint)_table.ColumnCount,
                RecordSize = (uint)RecordSize,
                TableHash = _createInformation.TableHash,
                LayoutHash = _createInformation.LayoutHash,
                MinId = _table.Rows.OrderBy(row => row.Id).First().Id,
                MaxId = _table.Rows.OrderBy(row => row.Id).Last().Id,
                Locale = _createInformation.Locale,
                Flags = _createInformation.Flags,
                TotalFieldCount = (uint)_table.ColumnCount,

                PalletDataSize = (uint)_insterter.PalletData.Count(),
                SectionCount = (uint)DetermineAllSectionHeaders().Count(),
            };
        }

        private IEnumerable<SectionHeader> DetermineAllSectionHeaders() => _table.Rows.Select(row => row.DependentSectionHeader).Distinct();
    }
}