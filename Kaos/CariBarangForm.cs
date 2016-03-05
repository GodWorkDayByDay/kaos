using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kaos
{
    public partial class CariBarangForm : Form
    {
        public string valueFromCari;

        public CariBarangForm()
        {
            InitializeComponent();
        }

        private void CariBarangForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);

            DataTable table = App.executeReader("SELECT Kode, Nama, Stok, Harga FROM barang");
            foreach (DataRow row in table.Rows)
            {
                dataGridView1.Rows.Add(row[0], row[1], row[2], App.strtomoney(row[3].ToString()));
            }
            this.ActiveControl = textBox1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DataTable table = App.executeReader("SELECT Kode, Nama, Stok, Harga FROM barang WHERE Nama LIKE '%"+textBox1.Text+"%'");
            foreach (DataRow row in table.Rows)
            {
                dataGridView1.Rows.Add(row[0], row[1], row[2], App.strtomoney(row[3].ToString()));
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            valueFromCari = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                valueFromCari = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                this.Close();
            }
        }
    }
}
