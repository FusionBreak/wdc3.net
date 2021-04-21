using System.Collections.Generic;
using System.Data;
using System.Linq;
using wdc3.net.Enums;

namespace wdc3.net.Table
{
    public class Db2Table
    {
        public string? Name { get; set; }
        public string? Locale { get; set; }

        public DataTable DataTable
        {
            get
            {
                DataTable data = new DataTable();

                foreach(var column in ColumnNames)
                    data.Columns.Add(column);

                foreach(var row in ValuesAsArray)
                    data.Rows.Add(row);

                return data;
            }
        }

        public void AddColumn(string name, Db2ValueTypes type) => _columns.Add(name, type);

        public void AddColumns(IEnumerable<(string name, Db2ValueTypes type)> infos)
        {
            foreach(var (name, type) in infos)
                AddColumn(name, type);
        }

        public IEnumerable<string> ColumnNames => _columns.Keys;

        public int ColumnCount => _columns.Count;

        public void AddRow(Db2Row row) => _rows.Add(row);

        public void AddRows(IEnumerable<Db2Row> rows) => _rows.AddRange(rows);

        public IEnumerable<IEnumerable<object?>> Values => _rows.Select(row => GetRowValues(row));

        private IEnumerable<object?> GetRowValues(Db2Row row) => row.Cells.Select(cell => cell.Value);

        public object?[][] ValuesAsArray => _rows.Select(row => GetRowValues(row).ToArray()).ToArray();

        public List<Db2Row> Rows => _rows;

        private object?[] GetRowValuesAsArray(Db2Row row) => GetRowValues(row).ToArray();

        private readonly List<Db2Row> _rows = new();
        private readonly Dictionary<string, Db2ValueTypes> _columns = new();
    }
}