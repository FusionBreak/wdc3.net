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
            var fileBytes = System.IO.File.ReadAllBytes(path);
            Db2 db = new Db2();
            //db.Header

            return null;
        }
    }
}
