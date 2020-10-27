﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace wdc3.net.Table
{
    public class Db2Table
    {
        public string? Name { get; set; }
        public string? Locale { get; set; }

        public void AddColumn(string name, Type type) => _columns.Add(name, type);

        public void AddColumnRange(IEnumerable<(string name, Type type)> infos)
        {
            foreach(var (name, type) in infos)
                AddColumn(name, type);
        }

        public IEnumerable<string> ColumnNames => _columns.Keys;

        public int ColumnCount => _columns.Count;

        public void AddRow(IEnumerable<Db2Cell> cells) => _rows.Add(cells);

        public IEnumerable<IEnumerable<object?>> GetValues()
        {
            foreach(var row in _rows)
            {
                yield return getRowValues(row);
            }
        }

        private IEnumerable<object?> getRowValues(IEnumerable<Db2Cell> row)
        {
            foreach(var column in _columns)
            {
                yield return row.First(cell => cell.ColumnName == column.Key).Value; //Convert.ChangeType(row.First(cell => cell.ColumnName == column.Key).Value, column.Value);
            }
        }

        private readonly List<IEnumerable<Db2Cell>> _rows = new List<IEnumerable<Db2Cell>>();
        private readonly Dictionary<string, Type> _columns = new Dictionary<string, Type>();
    }
}