﻿using System;
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
        private FileInfo Db2File { get; set; }
        private FileInfo DbdFile { get; set; }
        private Db2 Db2 { get; set; }
        private Db2Definition Dbd { get; set; }

        private uint MinRowId => Db2.Header!.MinId;
        //private uint MaxRowId => Db2.Header!.MaxId;
        //private bool AllRowsReaded => _currentRowId >= MaxRowId;

        private uint _currentRowId;

        public Db2ToTableReader(string db2Path, string dbdPath)
        {
            Db2File = new FileInfo(db2Path);
            DbdFile = new FileInfo(dbdPath);
            Db2 = new Db2Reader().ReadFile(Db2File.FullName);
            Dbd = new DbdReader().ReadFile(DbdFile.FullName);
            _currentRowId = MinRowId;
        }

        public Db2Table Read()
        {
            var output = new Db2Table();

            if(Db2.Header == null)
                throw new Exception();

            output.Name = Db2File.Name;
            output.Locale = ((Locales)Db2.Header.Locale).ToString();

            IEnumerable<ColumnInfo>? colInfos = TableColumnInformationFactory.CreateColumnInformation(Dbd, Db2.Header.LayoutHash);

            output.AddColumnRange(this.readColumns(colInfos));

            if(Db2.Sections == null)
                throw new Exception();

            foreach(var section in Db2.Sections)
            {
                if(section.IdList == null)
                    throw new Exception();

                foreach(var id in section.IdList)
                {
                    _currentRowId = id;
                    var row = new List<Db2Cell>
                    {
                        new Db2Cell() { ColumnName = colInfos.Where(col => col.IsId).First().Name, Value = _currentRowId }
                    };

                    foreach(var col in colInfos)
                    {
                        if(!col.IsId)
                        {
                            row.Add(col.Type == typeof(string)
                                ? new Db2Cell() { ColumnName = col.Name, Value = "Lorem Ipsum" }
                                : new Db2Cell() { ColumnName = col.Name, Value = null });
                        }
                    }

                    output.AddRow(row);
                }
            }

            return output;
        }

        private IEnumerable<(string name, Type type)> readColumns(IEnumerable<ColumnInfo> columnInfos)
        {
            foreach(var colInfo in columnInfos)
                    yield return colInfo != null && colInfo.Name != null && colInfo.Type != null
                        ? (colInfo.Name, colInfo.Type)
                        : throw new Exception();
        }
    }
}