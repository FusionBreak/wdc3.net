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
                Sections = _insterter.Sections,
                CommonData = new List<byte>(),
                PalletData = _insterter.PalletData,
            };

            new Db2Writer().WriteFile(db2, path);
        }

        private Header GetHeader()
        {
            var columnInfos = _table.Rows.First().Cells.Select(cell => cell.ColumnInfo).ToArray();

            return new Header()
            {
                Magic = _createInformation.Magic,
                RecordCount = (uint)RecordCount,
                FieldCount = (uint)_table.ColumnCount - 1,
                RecordSize = (uint)RecordSize,
                StringTableSize = (uint)(_insterter.Sections.Select(section => section?.StringData?.Count()).Sum() ?? 0),
                TableHash = _createInformation.TableHash,
                LayoutHash = _createInformation.LayoutHash,
                MinId = _table.Rows.OrderBy(row => row.Id).First().Id,
                MaxId = _table.Rows.OrderBy(row => row.Id).Last().Id,
                Locale = _createInformation.Locale,
                Flags = _createInformation.Flags,
                IdIndex = (ushort)Array.IndexOf(columnInfos, columnInfos.First(info => info?.IsId ?? false)),
                TotalFieldCount = (uint)_table.ColumnCount - 1,
                BitpackedDataOffset = _createInformation.BitpackedDataOffset,
                LookUpColumnCount = _createInformation.LookUpColumnCount,
                FieldStorageInfoSize = _createInformation.FieldStorageInfoSize,
                CommonDataSize = (uint)_insterter.CommonData.Count(),
                PalletDataSize = (uint)_insterter.PalletData.Count(),
                SectionCount = (uint)DetermineAllSectionHeaders().Count(),
            };
        }

        private IEnumerable<SectionHeader> DetermineAllSectionHeaders() => _table.Rows.Select(row => row.DependentSectionHeader).Distinct();
    }
}