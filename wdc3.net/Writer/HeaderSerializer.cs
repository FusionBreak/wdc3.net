using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    internal class HeaderSerializer
    {
        public byte[] Serialze(Header header)
        {
            var output = new List<byte>();

            output.AddRange(BitConverter.GetBytes(header.Magic));
            output.AddRange(BitConverter.GetBytes(header.RecordCount));
            output.AddRange(BitConverter.GetBytes(header.FieldCount));
            output.AddRange(BitConverter.GetBytes(header.RecordSize));
            output.AddRange(BitConverter.GetBytes(header.StringTableSize));
            output.AddRange(BitConverter.GetBytes(header.TableHash));
            output.AddRange(BitConverter.GetBytes(header.LayoutHash));
            output.AddRange(BitConverter.GetBytes(header.MinId));
            output.AddRange(BitConverter.GetBytes(header.MaxId));
            output.AddRange(BitConverter.GetBytes(header.Locale));
            output.AddRange(BitConverter.GetBytes(header.Flags));
            output.AddRange(BitConverter.GetBytes(header.IdIndex));
            output.AddRange(BitConverter.GetBytes(header.TotalFieldCount));
            output.AddRange(BitConverter.GetBytes(header.BitpackedDataOffset));
            output.AddRange(BitConverter.GetBytes(header.LookUpColumnCount));
            output.AddRange(BitConverter.GetBytes(header.FieldStorageInfoSize));
            output.AddRange(BitConverter.GetBytes(header.CommonDataSize));
            output.AddRange(BitConverter.GetBytes(header.PalletDataSize));
            output.AddRange(BitConverter.GetBytes(header.SectionCount));

            return output.ToArray();
        }
    }
}