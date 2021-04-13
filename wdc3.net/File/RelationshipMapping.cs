using wdc3.net.Helper;

namespace wdc3.net.File
{
    public class RelationshipMapping : ISizeCalculable
    {
        public uint NumEntries { get; set; }
        public uint MinId { get; set; }
        public uint MaxId { get; set; }

        public int SizeOf => sizeof(uint) * 3;
    }
}