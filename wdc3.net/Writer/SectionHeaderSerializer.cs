using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    internal class SectionHeaderSerializer
    {
        public byte[] Serialze(IEnumerable<SectionHeader> sectionHeaders)
        {
            var output = new List<byte>();

            foreach(var sectionHeader in sectionHeaders)
            {
                output.AddRange(BitConverter.GetBytes(sectionHeader.TactKeyHash));
                output.AddRange(BitConverter.GetBytes(sectionHeader.FileOffset));
                output.AddRange(BitConverter.GetBytes(sectionHeader.RecordCount));
                output.AddRange(BitConverter.GetBytes(sectionHeader.StringTableSize));
                output.AddRange(BitConverter.GetBytes(sectionHeader.OffsetRecordsEnd));
                output.AddRange(BitConverter.GetBytes(sectionHeader.IdListSize));
                output.AddRange(BitConverter.GetBytes(sectionHeader.RelationsshipDataSize));
                output.AddRange(BitConverter.GetBytes(sectionHeader.OffsetMapIdCount));
                output.AddRange(BitConverter.GetBytes(sectionHeader.CopyTableCount));
            }

            return output.ToArray();
        }
    }
}