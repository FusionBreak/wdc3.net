namespace wdc3.net.File
{
    public class OffsetMapEntry : ISizeCalculable
    {
        public uint Offset { get; set; }
        public ushort Size { get; set; }

        public int SizeOf => sizeof(uint) + sizeof(ushort);
    }
}