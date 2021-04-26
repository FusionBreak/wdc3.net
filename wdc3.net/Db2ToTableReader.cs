using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using wdc3.net.Enums;
using wdc3.net.File;
using wdc3.net.Reader;
using wdc3.net.Table;

namespace wdc3.net
{
    public class Db2ToTableReader
    {
        private readonly FileInfo _db2File;
        private readonly FileInfo _dbdFile;
        private readonly Db2 _db2;
        private readonly Db2Definition _dbd;
        private readonly ColumnInfo[] _columnInfos;
        private readonly RowInfo[] _rowInfos;

        private Header Db2Header => SourceDb2.Header ?? throw new ArgumentNullException(nameof(SourceDb2.Header));
        private IEnumerable<ISection> Sections => SourceDb2.Sections ?? throw new ArgumentNullException(nameof(SourceDb2.Sections));
        private IEnumerable<FieldStructure> FieldStructures => SourceDb2.FieldStructures ?? throw new ArgumentNullException(nameof(SourceDb2.FieldStructures));
        private IEnumerable<IFieldStorageInfo> FieldStorageInfos => SourceDb2.FieldStorageInfos ?? throw new ArgumentNullException(nameof(SourceDb2.FieldStorageInfos));

        private readonly IDb2ValueExtractor _valueExtractor;

        private IEnumerable<byte> PalletData => SourceDb2.PalletData ?? throw new ArgumentNullException(nameof(SourceDb2.PalletData));
        private IEnumerable<byte> CommonData => SourceDb2.CommonData ?? throw new ArgumentNullException(nameof(SourceDb2.CommonData));

        public Db2 SourceDb2 => _db2;

        public Db2Definition SourceDbd => _dbd;

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
            _columnInfos = TableColumnInformationFactory.CreateColumnInformation(_dbd, _db2.Header != null ? _db2.Header.LayoutHash : throw new Exception()).ToArray();
            _rowInfos = RowInfoReader.ReadRowInfos(Sections, _db2.HasOffsetFlag).ToArray();

            _valueExtractor = (_db2.Header.Flags & 1) == 0
                ? new Db2ValueExtractorNoOffsetFlag(PalletData, CommonData, RecordData, RecordStringData, FieldStorageInfos.Sum(info => info.FieldSizeBits), (int)_db2.Header.RecordSize)
                : new Db2ValueExtractorWithOffsetFlag(RecordData);
        }

        public Db2Table Read()
        {
            var output = new Db2Table
            {
                Name = _db2File.Name,
                Locale = ((Locales)Db2Header.Locale).ToString()
            };
            output.AddColumns(ReadColumns(_columnInfos));
            output.AddRows(ReadRows());
            return output;
        }

        private IEnumerable<Db2Row> ReadRows()
        {
            foreach(var rowInfo in _rowInfos)
            {
                yield return new Db2Row(ReadCells(rowInfo).ToArray(), FindSectionHeaderForRow(rowInfo.Id), rowInfo);
                _valueExtractor.NextRow(rowInfo);
            }
        }

        private SectionHeader FindSectionHeaderForRow(uint RowId)
        {
            //var sectionIndex = Sections.TakeWhile(section => section.IdList?.Contains(RowId) ?? throw new NullReferenceException(nameof(ISection.IdList))).Count();
            //return _db2.SectionHeaders?.ElementAt(sectionIndex) ?? throw new NullReferenceException(nameof(Db2.SectionHeaders));

            for(int sectionIndex = 0; sectionIndex < Sections.Count(); sectionIndex++)
            {
                if(Sections.Skip(sectionIndex).First().IdList?.Contains(RowId) ?? throw new NullReferenceException(nameof(ISection.IdList)))
                {
                    return _db2.SectionHeaders?.ElementAt(sectionIndex) ?? throw new NullReferenceException(nameof(Db2.SectionHeaders));
                }
            }
            throw new Exception();
        }

        private IEnumerable<Db2Cell> ReadCells(RowInfo rowInfo)
        {
            yield return CreateCellForId(rowInfo.Id);

            foreach(var columnInfo in _columnInfos)
            {
                if(!columnInfo.IsId)
                {
                    yield return ReadValue(rowInfo, columnInfo);
                }
            }
        }

        private Db2Cell CreateCellForId(uint id) => new Db2Cell()
        {
            ColumnInfo = _columnInfos.Where(col => col.IsId).First(),
            Value = id
        };

        private Db2Cell ReadValue(RowInfo rowInfo, ColumnInfo columnInfo)
        {
            var (_, structure, storageInfo) = GetColumnReadInfos().Where(readInfo => readInfo.Info == columnInfo).First();
            return new Db2Cell()
            {
                Value = _valueExtractor.ExtractValue(structure, storageInfo, columnInfo, rowInfo),
                ColumnInfo = columnInfo,
                FieldStorageInfo = storageInfo,
                FieldStructure = structure
            };
        }

        private IEnumerable<uint> ReadOffsetMapIds()
        {
            foreach(var section in Sections)
                if(section is SectionWithFlag flagSection && flagSection.OffsetMapIdList is not null)
                    foreach(var offsetMapId in flagSection.OffsetMapIdList)
                        yield return offsetMapId;
        }

        private IEnumerable<(string name, Db2ValueTypes type)> ReadColumns(IEnumerable<ColumnInfo> columnInfos)
        {
            foreach(var colInfo in columnInfos)
                yield return colInfo != null && colInfo.Name != null
                    ? (colInfo.Name, colInfo.Type)
                    : throw new Exception();
        }

        private IEnumerable<(ColumnInfo Info, FieldStructure Structure, IFieldStorageInfo StorageInfo)> GetColumnReadInfos()
        {
            var fieldStructures = FieldStructures.ToArray();
            var fieldStorageInfos = FieldStorageInfos.ToArray();
            var columnInfos = _columnInfos.Where(column => !column.IsId).ToArray();

            for(int columnIndex = 0; columnIndex < columnInfos.Length; columnIndex++)
                yield return (columnInfos[columnIndex], fieldStructures[columnIndex], fieldStorageInfos[columnIndex]);
        }
    }
}