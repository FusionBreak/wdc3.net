using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wdc3.net.Table;

namespace wdc3.net.native
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            const string db = "ItemSparse";
            var reader = new Db2ToTableReader(@$"D:\Work\wdc3.net\wdc3.net.test\TestFiles\{db.ToLower()}.db2", @$"D:\Work\wdc3.net\wdc3.net.test\TestFiles\{db}.dbd");
            var table = reader.Read();
            var rows = table.GetValues().ToArray();

            foreach(var column in table.ColumnNames)
                dataGridView1.Columns.Add(column, column);

            foreach(var row in rows[0..10])
                dataGridView1.Rows.Add(row.ToArray());

            foreach(var row in rows.Skip(rows.Count() - 10))
                dataGridView1.Rows.Add(row.ToArray());

            dataGridView1.Update();
        }
    }
}