using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class HeaderReader
    {
        BinaryReader _reader;
        public long Position { get; private set; }
        public HeaderReader(BinaryReader reader)
        {
            _reader = reader;
        }

        public Header Read()
        {
            var output = new Header();
            output.Magic = _reader.ReadInt32();
            return output;
        }
    }
}