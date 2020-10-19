using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.Reader;
using wdc3.net.Table;
using Xunit;

namespace wdc3.net.test.Table
{
    public class TableColumnInformationFactoryTest
    {
        [Fact]
        public void CreateCorrectColumnInformations()
        {
            var colInfos = TableColumnInformationFactory.CreateColumnInformation(
                new DbdReader().ReadFile(new FileInfo(TestFiles.MAP_DBD_PATH).FullName),
                3667170223);
            Assert.Equal(24, colInfos.Count());
            Console.WriteLine();
        }
    }
}