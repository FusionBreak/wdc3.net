using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class Db2Reader
    {
        public Db2 ReadFile(string path)
        {
            var fileBuffer = System.IO.File.ReadAllBytes(path);
            Db2 db = new Db2();
            
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));
            HeaderReader headerReader = new HeaderReader(reader);

            db.Header = headerReader.Read();

            return null;
        }
    }
}
