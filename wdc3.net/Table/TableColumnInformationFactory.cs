using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class TableColumnInformationFactory
    {
        public IEnumerable<ColumnInfo> CreateColumnInformation(Db2Definition dbd, uint hexLayoutHash)
        {
            var output = new List<ColumnInfo>();

            var typeParser = new TableTypeParser();
            var columnDefinitions = dbd.ColumnDefinitions;
            var definitions = dbd.GetVersionDefinition(hexLayoutHash).Definitions;

            if(columnDefinitions == null)
                throw new Exception();

            foreach(var colDef in columnDefinitions)
            {
                if(colDef.Name != null && colDef.Type != null)
                {
                    DefinitionInfo? definition = null;

                    if(definitions != null)
                        definition = definitions.Where(def => def.Name == colDef.Name).FirstOrDefault();

                    if(definition != null)
                    {
                        if(definition.ArrayLength > 0)
                        {
                            for(int i = 0; i < definition.ArrayLength; i++)
                            {
                                output.Add(new ColumnInfo() {
                                    Name = $"{colDef.Name}[{i}]",
                                    Type = typeParser.Parse(colDef.Type, definition.IsSigned, definition.Size),
                                    IsId = definition.IsId
                                });
                            }
                        }
                        else
                            output.Add(new ColumnInfo()
                            {
                                Name = colDef.Name,
                                Type = typeParser.Parse(colDef.Type, definition.IsSigned, definition.Size),
                                IsId = definition.IsId
                            });
                    }
                    else
                        output.Add(new ColumnInfo()
                        {
                            Name = colDef.Name,
                            Type = typeParser.Parse(colDef.Type),
                            IsId = false
                        });
                }
            }

            return output;
        }
    }
}