using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.Enums;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class DbdReader
    {
        public bool AllLinesReaded => _currentLineNumber >= _allLines?.Length;
        private string[]? _allLines;
        private int _currentLineNumber;
        private string? CurrentLine => !AllLinesReaded ? _allLines?[_currentLineNumber] : "";

        private bool CurrentLineStartsWithDataChunk
            => CurrentLine != null
                && (
                    CurrentLine.StartsWith(DataChunkNames.BUILD)
                    || CurrentLine.StartsWith(DataChunkNames.COLUMNS)
                    || CurrentLine.StartsWith(DataChunkNames.COMMENT)
                    || CurrentLine.StartsWith(DataChunkNames.LAYOUT)
                );

        private List<DataChunk> _chunks = new List<DataChunk>();

        public Db2Definition ReadFile(string path)
        {
            var output = new Db2Definition();
            var versions = new List<VersionDefinition>();
            _allLines = System.IO.File.ReadAllLines(path);

            if(CurrentLine == null)
                throw new Exception("Empty line.");

            _chunks = readAllLinesToChunks().ToList();

            foreach(var chunk in _chunks)
            {
                switch(chunk.Name)
                {
                    case DataChunkNames.COLUMNS:
                        output.ColumnDefinitions = new ColumnDefinitionParser().Parse(chunk).ToList();
                        break;

                    case DataChunkNames.LAYOUT:
                        versions.Add(new VersionDefinitionParser().ParseLayout(chunk));
                        break;

                    case DataChunkNames.BUILD:
                        versions.Add(new VersionDefinitionParser().ParseBuild(chunk));
                        break;

                    case DataChunkNames.COMMENT:
                        output.Comment = chunk.Parameters.First();
                        break;

                    default:
                        break;
                }
            }

            output.VersionDefinitions = versions;

            return output;
        }

        private IEnumerable<DataChunk> readAllLinesToChunks()
        {
            while(!AllLinesReaded)
            {
                if(CurrentLineStartsWithDataChunk)
                {
                    var header = readChunkHeader();
                    var chunk = new DataChunk
                    {
                        Name = header.Name,
                        Parameters = header.Parameters,
                        Content = readChunkContent().ToList()
                    };
                    yield return chunk;
                }
                else
                {
                    _currentLineNumber++;
                }
            }
        }

        private (string? Name, IEnumerable<string>? Parameters) readChunkHeader()
        {
            var output = (
                CurrentLine?.Split(' ')[0],
                CurrentLine?.Split(' ')[1..]
                            .Select(content => content.Trim().Replace(",", ""))
            );

            _currentLineNumber++;

            return output;
        }

        private IEnumerable<string> readChunkContent()
        {
            while(CurrentLine != null && CurrentLine != "")
            {
                yield return CurrentLine;
                _currentLineNumber++;
            }
        }
    }
}