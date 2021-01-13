using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using wdc3.net.Enums;
using wdc3.net.File;
using wdc3.net.Reader;
using wdc3.net.Table;

namespace wdc3.net
{
    public class Db2ToTableReader
    {
        private FileInfo _db2File;
        private FileInfo _dbdFile;
        private Db2 _db2;
        private Db2Definition _dbd;
        private IEnumerable<ColumnInfo> _columnInfos;

        private Header Db2Header => _db2.Header ?? throw new ArgumentNullException(nameof(_db2.Header));
        private IEnumerable<ISection> Sections => _db2.Sections ?? throw new ArgumentNullException(nameof(_db2.Sections));
        private IEnumerable<FieldStructure> FieldStructures => _db2.FieldStructures ?? throw new ArgumentNullException(nameof(_db2.FieldStructures));
        private IEnumerable<IFieldStorageInfo> FieldStorageInfos => _db2.FieldStorageInfos ?? throw new ArgumentNullException(nameof(_db2.FieldStorageInfos));

        private Db2ValueExtractor _valueExtractor;

        private IEnumerable<byte> PalletData => _db2.PalletData ?? throw new ArgumentNullException(nameof(_db2.PalletData));
        private IEnumerable<byte> CommonData => _db2.CommonData ?? throw new ArgumentNullException(nameof(_db2.CommonData));

        private IEnumerable<byte> RecordData
        {
            get
            {
                foreach(var section in Sections)
                    if(section is Section defaultSection)
                        foreach(var record in defaultSection.Records ?? throw new Exception())
                            foreach(byte data in record.Data ?? throw new Exception())
                                yield return data;
                    else if(section is SectionWithFlag flagSection)
                        foreach(var data in flagSection.VariableRecordData ?? throw new Exception())
                            yield return data;
            }
        }

        private IEnumerable<byte> RecordStringData
        {
            get
            {
                foreach(var section in Sections)
                    if(section is Section defaultSection)
                        foreach(byte data in defaultSection.StringData ?? throw new Exception())
                            yield return data;
            }
        }

        public Db2ToTableReader(string db2Path, string dbdPath)
        {
            _db2File = new FileInfo(db2Path);
            _dbdFile = new FileInfo(dbdPath);
            _db2 = new Db2Reader().ReadFile(_db2File.FullName);
            _dbd = new DbdReader().ReadFile(_dbdFile.FullName);
            _columnInfos = TableColumnInformationFactory.CreateColumnInformation(_dbd, _db2.Header != null ? _db2.Header.LayoutHash : throw new Exception());
            _valueExtractor = new Db2ValueExtractor(PalletData, CommonData, RecordData, RecordStringData, FieldStorageInfos.Sum(info => info.FieldSizeBits), (int)_db2.Header.RecordSize);
        }

        public Db2Table Read()
        {
            var output = new Db2Table();
            output.Name = _db2File.Name;
            output.Locale = ((Locales)Db2Header.Locale).ToString();
            output.AddColumns(this.readColumns(_columnInfos));
            output.AddRows(readRows());
            return output;
        }

        private IEnumerable<IEnumerable<Db2Cell>> readRows()
        {
            foreach(var id in readIds())
            {
                var row = new List<Db2Cell>() { createCellForId(id) };
                row.AddRange(readCells().Select(cell => cell));
                Console.WriteLine(id);
                yield return row;
                _valueExtractor.NextRow();
            }
        }

        private IEnumerable<Db2Cell> readCells()
        {
            foreach(var col in _columnInfos)
            {
                if(!col.IsId && col.Type != null)
                {
                    yield return new Db2Cell() { ColumnName = col.Name, Value = readValue(col) };
                }
            }
        }

        private Db2Cell createCellForId(uint id) => new Db2Cell() { ColumnName = _columnInfos.Where(col => col.IsId).First().Name, Value = id };

        private object readValue(ColumnInfo columnInfo)
        {
            var (_, structure, storageInfo) = getColumnReadInfos().Where(readInfo => readInfo.Info == columnInfo).First();
            return _valueExtractor.ExtractValue(structure, storageInfo, columnInfo);
        }

        private IEnumerable<uint> readIds()
        {
            foreach(var section in Sections)
                if(section.IdList != null)
                    foreach(var id in section.IdList)
                        yield return id;
        }

        private IEnumerable<(string name, Type type)> readColumns(IEnumerable<ColumnInfo> columnInfos)
        {
            foreach(var colInfo in columnInfos)
                yield return colInfo != null && colInfo.Name != null && colInfo.Type != null
                    ? (colInfo.Name, colInfo.Type)
                    : throw new Exception();
        }

        private IEnumerable<(ColumnInfo Info, FieldStructure Structure, IFieldStorageInfo StorageInfo)> getColumnReadInfos()
        {
            var fieldStructures = FieldStructures.ToArray();
            var fieldStorageInfos = FieldStorageInfos.ToArray();
            var columnInfos = _columnInfos.Where(column => !column.IsId).ToArray();

            for(int columnIndex = 0; columnIndex < columnInfos.Length; columnIndex++)
                yield return (columnInfos[columnIndex], fieldStructures[columnIndex], fieldStorageInfos[columnIndex]);
        }
    }
}