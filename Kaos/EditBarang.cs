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
    public partial class EditBarang : Form
    {
        string hasilpencarian;

        public EditBarang(string kode, string nama, string harga, string stok, string batas, string ecer, string hargabeli, string perlusin, string cari)
        {
            InitializeComponent();
            textBox1.Text = kode;
            textBox2.Text = nama;
            textBox3.Text = harga;
            textBox4.Text = stok;
            textBox5.Text = batas;
            textBox6.Text = ecer;
            textBox7.Text = hargabeli;
            textBox8.Text = perlusin;
            hasilpencarian = cari;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Save changes to this item?", "Save", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes && checkBox1.Checked == false)
            {
                App.executeNonQuery("UPDATE barang SET Nama = '"+textBox2.Text+"', Harga = '"+textBox3.Text+"', Stok = '"+textBox4.Text+"', Batas = '"+textBox5.Text+"', Ecer = '"+textBox6.Text+"', HargaBeli = '"+textBox7.Text+"', PerLusin = '"+textBox8.Text+"' WHERE Kode = '"+textBox1.Text+"'");
                MessageBox.Show("Item changed successfully");
                this.Close();
            }
            else if (result == DialogResult.Yes && checkBox1.Checked == true)
            {
                App.executeNonQuery("UPDATE barang SET Harga = '" + textBox3.Text + "', Batas = '" + textBox5.Text + "', Ecer = '" + textBox6.Text + "', HargaBeli = '" + textBox7.Text + "', PerLusin = '" + textBox8.Text + "' WHERE Nama LIKE '%" + hasilpencarian + "%'");
                MessageBox.Show("Items changed successfully");
                this.Close();
            }
        }

        private void EditBarang_Load(object sender, EventArgs e)
        {
            textBox9.Text = (App.moneytodouble(textBox3.Text) - App.moneytodouble(textBox7.Text)).ToString();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = (App.moneytodouble(textBox7.Text) + App.moneytodouble(textBox9.Text)).ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.Enabled = false;
                textBox4.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                textBox4.Enabled = true;
            }
        }
    }
}
