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

            dataGridView1.DataSource = table.DataTable;
            dataGridView1.Update();
        }
    }
}