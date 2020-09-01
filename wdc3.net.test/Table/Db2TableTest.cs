using System;
using System.Collections.Generic;
using System.Text;
using wdc3.net.Table;
using Xunit;

namespace wdc3.net.test.Table
{
    public class Db2TableTest
    {
        [Fact]
        public void XXX()
        {
            var table = new Db2Table();
            table.Name = "Testtable";
            table.Locale = "xx-XX";

            table.AddColumn("First", typeof(string));
            table.AddColumn("Second", typeof(int));

            table.AddRow(new List<Db2Cell>() {
                new Db2Cell()
                {
                    ColumnName = "First",
                    Value = "Lorem"
                },
                new Db2Cell()
                {
                    ColumnName = "Second",
                    Value = 12
                },
            });

            table.AddRow(new List<Db2Cell>() {
                new Db2Cell()
                {
                    ColumnName = "First",
                    Value = "Ipsum"
                },
                new Db2Cell()
                {
                    ColumnName = "Second",
                    Value = 99
                },
            });

            foreach(var a in table.GetValues())
            {
                foreach(var b in a)
                {
                    var value = b;
                    var type = b.GetType();
                }
            }
        }
    }
}