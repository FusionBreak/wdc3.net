using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    internal class FieldStructureReader : IFileReader<IEnumerable<FieldStructure>>
    {
        BinaryReader _reader;
        int _totalFieldCount;
        public long Position { get; private set; }

        public FieldStructureReader(BinaryReader reader, int totalFieldCount)
        {
            _reader = reader;
            _totalFieldCount = totalFieldCount;
        }

        public IEnumerable<FieldStructure> Read()
        {
            var output = new List<FieldStructure>();

            for(int currentStructureCount = 0; currentStructureCount < _totalFieldCount; currentStructureCount++)
            {
                var structure = new FieldStructure();

                structure.Size = _reader.ReadInt16();
                structure.Position = _reader.ReadUInt16();

                output.Add(structure);
            }

            return output;
        }
    }
}