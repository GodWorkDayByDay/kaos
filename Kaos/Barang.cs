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
    public partial class Barang : Form
    {
        public Barang()
        {
            InitializeComponent();
        }


        private void Barang_Load(object sender, EventArgs e)
        {
            App.loadTable(dataGridView1, "SELECT * FROM barang");
            App.loadComboBox(comboBox1, "SELECT * FROM merk");
            dataGridView1.CurrentRow.Selected = false;
            this.ActiveControl = textBox9;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            dataGridView1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kode = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            string nama = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string harga = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            string stok = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            string batas = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            string ecer = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
            string hargabeli = dataGridView1[6, dataGridView1.CurrentRow.Index].Value.ToString();

            EditBarang editbarang = new EditBarang(kode,nama,harga,stok,batas,ecer,hargabeli);
            editbarang.ShowDialog();
            

    }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                DialogResult result =  MessageBox.Show("Do you want to delete this item?","Delete",MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("DELETE");
                }
                else
                {
                    MessageBox.Show("NOT DELETED");
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Nama LIKE '%"+textBox9.Text+"%'");
        }

        private void Barang_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
