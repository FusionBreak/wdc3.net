using System.Collections.Generic;

namespace wdc3.net.File
{
    public class Db2
    {
        public Header? Header { get; set; }
        public IEnumerable<SectionHeader>? SectionHeaders { get; set; }
        public IEnumerable<FieldStructure>? FieldStructures { get; set; }
        public IEnumerable<IFieldStorageInfo>? FieldStorageInfos { get; set; }
        public IEnumerable<byte>? PalletData { get; set; }
        public IEnumerable<byte>? CommonData { get; set; }
        public IEnumerable<ISection>? Sections { get; set; }
    }
}