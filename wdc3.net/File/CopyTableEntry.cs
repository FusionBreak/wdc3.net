namespace wdc3.net.File
{
    public class CopyTableEntry : ISizeCalculable
    {
        public uint IdOfNewRow { get; set; }
        public uint IdOfCopiedRow { get; set; }

        public int SizeOf => sizeof(uint) * 2;
    }
}