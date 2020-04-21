using System;
using System.IO;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class Db2ReaderTest
    {
        [Fact]
        public void XXX()
        {
            Db2Reader reader = new Db2Reader();
            reader.ReadFile(@"..\..\..\TestFiles\map.db2");
        }
    }
}