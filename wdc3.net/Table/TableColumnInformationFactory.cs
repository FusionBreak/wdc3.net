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

            if(columnDefinitions == null)
                throw new Exception();

            foreach(var colDef in columnDefinitions)
            {
                if(colDef.Name != null && colDef.Type != null)
                {
                    var definition = definitions?.Where(def => def.Name == colDef.Name).FirstOrDefault();

                    yield return definition != null
                        ? new ColumnInfo()
                        {
                            Name = colDef.Name,
                            Type = TableTypeParser.Parse(colDef.Type, definition.IsSigned, definition.Size, definition.ArrayLength > 0),
                            IsId = definition.IsId,
                            ArrayLength = definition.ArrayLength
                        }
                        : new ColumnInfo()
                        {
                            Name = colDef.Name,
                            Type = TableTypeParser.Parse(colDef.Type),
                            IsId = false
                        };
                }
            }
        }
    }
}