using System;
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

        public void AddRow(IEnumerable<Db2Cell> cells) => _rows.Add(cells);

        public void AddRows(IEnumerable<IEnumerable<Db2Cell>> rows)
        {
            foreach(var row in rows)
                AddRow(row);
        }

        public IEnumerable<IEnumerable<object?>> Values
        {
            get
            {
                foreach(var row in _rows)
                {
                    yield return GetRowValues(row);
                }
            }
        }

        private IEnumerable<object?> GetRowValues(IEnumerable<Db2Cell> row)
        {
            foreach(var column in _columns)
            {
                yield return row.First(cell => cell.ColumnName == column.Key).Value;
            }
        }

        public object?[][] ValuesAsArray
        {
            get
            {
                var output = new List<object?[]>();

                foreach(var row in _rows)
                    output.Add(GetRowValuesAsArray(row));

                return output.ToArray();
            }
        }

        private object?[] GetRowValuesAsArray(IEnumerable<Db2Cell> row)
        {
            return GetRowValues(row).ToArray();
        }

        private readonly List<IEnumerable<Db2Cell>> _rows = new();
        private readonly Dictionary<string, Db2ValueTypes> _columns = new();
    }
}