using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    internal class ColumnDefinitionParser
    {
        public IEnumerable<ColumnDefinition> Parse(DataChunk chunk)
        {
            if(chunk.Content == null)
                throw new Exception();
            else
                foreach(var row in chunk.Content)
                {
                    yield return parseString(row);
                }
        }

        //int<Map::ID> ParentMapID // Lorem Ipsum
        private ColumnDefinition parseString(string input)
        {
            var output = new ColumnDefinition();
            var index = 0;

            var type = getType(input, index);
            output.Type = type.output;
            index = type.lastIndex;

            if(type.lastChar == '<')
            {
                var foreignInformation = getForeignInformation(input);
                output.ForeignTable = foreignInformation.tableName;
                output.ForeignColumn = foreignInformation.columnName;
                index += foreignInformation.tableName.Length + foreignInformation.columnName.Length + "<::>".Length;
            }

            var name = getName(input, index + 1);
            output.Name = name.output;
            output.Verified = !(name.lastChar == '?');
            index = name.lastIndex;

            if(input.Contains("//"))
                output.Comment = input.Split("//")[1].Trim();

            return output;
        }

        private (string output, int lastIndex, char lastChar) getType(string input, int startIndex)
        {
            int index = startIndex;

            while(index < input.Length && (input[index] != ' ' && input[index] != '<'))
            {
                index++;
            }

            return (input[startIndex..index], index, input[index]);
        }

        private (string output, int lastIndex, char lastChar) getName(string input, int startIndex)
        {
            int index = startIndex;

            while(index < input.Length && input[index] != ' ' && input[index] != '?')
            {
                index++;
            }

            return (input[startIndex..index], index, index < input.Length ? input[index] : input[index - 1]);
        }

        private (string tableName, string columnName) getForeignInformation(string input)
        {
            var infos = input.Split('<')[1].Split('>')[0].Split("::");

            return (infos[0], infos[1]);
        }
    }
}