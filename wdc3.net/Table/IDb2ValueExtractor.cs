using wdc3.net.File;

namespace wdc3.net.Table
{
    public interface IDb2ValueExtractor
    {
        public object ExtractValue(FieldStructure fieldStructure, IFieldStorageInfo fieldStorageInfo, ColumnInfo columnInfo, RowInfo rowInfo);

        public void NextRow(RowInfo rowInfo);
    }
}