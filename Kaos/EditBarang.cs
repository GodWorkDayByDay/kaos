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
        public EditBarang(string kode, string nama, string harga, string stok, string batas, string ecer, string hargabeli)
        {
            InitializeComponent();
            textBox1.Text = kode;
            textBox2.Text = nama;
            textBox3.Text = harga;
            textBox4.Text = stok;
            textBox5.Text = batas;
            textBox6.Text = ecer;
            textBox7.Text = hargabeli;
        }

        private void EditBarang_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Save changes to this item?", "Save", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                App.executeSql("UPDATE barang SET Nama = '"+textBox2.Text+"' WHERE Kode = '"+textBox1.Text+"'");
                MessageBox.Show("Item changed successfully");
                this.Close();
            }
        }
    }
}
