using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    internal class ColumnDefinitionParser
    {
        public static IEnumerable<ColumnDefinition> Parse(DataChunk chunk)
        {
            if(chunk.Content == null)
            {
                throw new Exception();
            }
            else
            {
                foreach(var row in chunk.Content)
                {
                    yield return parseString(row);
                }
            }
        }

        //int<Map::ID> ParentMapID // Lorem Ipsum
        private static ColumnDefinition parseString(string input)
        {
            var output = new ColumnDefinition();
            var index = 0;

            var type = getType(input, index);
            output.Type = type.output;
            index = type.lastIndex;

            if(type.lastChar == '<')
            {
                var (tableName, columnName) = getForeignInformation(input);
                output.ForeignTable = tableName;
                output.ForeignColumn = columnName;
                index += tableName.Length + columnName.Length + "<::>".Length;
            }

            var name = getName(input, index + 1);
            output.Name = name.output;
            output.Verified = !(name.lastChar == '?');

            if(input.Contains("//"))
                output.Comment = input.Split("//")[1].Trim();

            return output;
        }

        private static (string output, int lastIndex, char lastChar) getType(string input, int startIndex)
        {
            var index = startIndex;

            while(index < input.Length && (input[index] != ' ' && input[index] != '<'))
            {
                index++;
            }

            return (input[startIndex..index], index, input[index]);
        }

        private static (string output, int lastIndex, char lastChar) getName(string input, int startIndex)
        {
            var index = startIndex;

            while(index < input.Length && input[index] != ' ' && input[index] != '?')
            {
                index++;
            }

            return (input[startIndex..index], index, index < input.Length ? input[index] : input[index - 1]);
        }

        private static (string tableName, string columnName) getForeignInformation(string input)
        {
            var infos = input.Split('<')[1].Split('>')[0].Split("::");

            return (infos[0], infos[1]);
        }
    }
}