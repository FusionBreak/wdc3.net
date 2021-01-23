using System.Collections.Generic;
using wdc3.net.Table;
using Xunit;

namespace wdc3.net.test.Table
{
    public class Db2TableTest
    {
        [Fact]
        public void XXX()
        {
            var table = new Db2Table
            {
                Name = "Testtable",
                Locale = "xx-XX"
            };

            table.AddColumn("First", Db2ValueTypes.Text);
            table.AddColumn("Second", Db2ValueTypes.Number);

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
                    _ = value;
                    _ = type;
                }
            }
        }
    }
}