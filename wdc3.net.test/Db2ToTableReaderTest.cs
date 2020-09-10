using Xunit;

namespace wdc3.net.test
{
    public class Db2ToTableReaderTest
    {
        [Fact]
        public void XXX()
        {
            //Compare with https://wow.tools/dbc/?dbc=map&build=8.3.7.34872#page=1

            var reader = new Db2ToTableReader(@"..\..\..\TestFiles\map.db2", @"..\..\..\TestFiles\Map.dbd");
            var tabel = reader.Read();
        }
    }
}