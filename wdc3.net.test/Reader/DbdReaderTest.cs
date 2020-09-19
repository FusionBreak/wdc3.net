﻿using System.IO;
using wdc3.net.Reader;
using Xunit;

namespace wdc3.net.test.Reader
{
    public class DbdReaderTest
    {
        [Fact]
        public void ReadsTheWholeFile()
        {
            FileInfo mapDbd = new FileInfo(@"..\..\..\TestFiles\Map.dbd");
            DbdReader reader = new DbdReader();
            var dbd = reader.ReadFile(mapDbd.FullName);
            Assert.True(reader.AllLinesReaded);
        }
    }
}