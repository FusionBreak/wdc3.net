using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class TableColumnInformationFactory
    {
        public static IEnumerable<ColumnInfo> CreateColumnInformation(Db2Definition dbd, uint hexLayoutHash)
        {
            var columnDefinitions = dbd.ColumnDefinitions;
            var definitions = dbd.GetVersionDefinition(hexLayoutHash)?.Definitions;

            if(definitions == null || columnDefinitions == null)
                throw new Exception();

            foreach(var definition in definitions)
            {
                var columnDef = columnDefinitions.Where(colDef => colDef.Name == definition.Name).FirstOrDefault();

                yield return columnDef is not null && columnDef.Type is not null
                ? new ColumnInfo()
                {
                    Name = columnDef.Name,
                    Type = TableTypeParser.Parse(columnDef.Type, definition.IsSigned, definition.Size, definition.ArrayLength > 0),
                    IsId = definition.IsId,
                    ArrayLength = definition.ArrayLength
                }
                : throw new Exception();
            }
        }
    }
}