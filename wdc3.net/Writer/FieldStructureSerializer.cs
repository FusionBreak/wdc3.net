using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    internal class FieldStructureSerializer
    {
        public byte[] Serialze(IEnumerable<FieldStructure> fieldStructures)
        {
            var output = new List<byte>();

            foreach(var fieldStructure in fieldStructures)
            {
                output.AddRange(BitConverter.GetBytes(fieldStructure.Size));
                output.AddRange(BitConverter.GetBytes(fieldStructure.Position));
            }

            return output.ToArray();
        }
    }
}