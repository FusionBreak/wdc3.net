using System;
using System.Collections.Generic;
using System.Text;

namespace wdc3.net.File
{
    public class Header
    {
        public int Magic { get; set; }
        public int RecordCount { get; set; }
        public int FieldCount { get; set; }
        public int RecordSize { get; set; }
        public int StringTableSize { get; set; }
        public int TabeHash { get; set; }
        public int LayoutHash { get; set; }
        public int MinId { get; set; }
        public int MaxId { get; set; }
        public int Locale { get; set; }
        public short Flags { get; set; }
        public short IdIndex { get; set; }
        public int TotalFieldCount { get; set; }
        public int BitpackedDataOffset { get; set; }
        public int LookUpColumnCount { get; set; }
        public int FieldStorageInfoSize { get; set; }
        public int CommonDataSize { get; set; }
        public int PalletDataSize { get; set; }
        public int SectionCount { get; set; }
    }
}
