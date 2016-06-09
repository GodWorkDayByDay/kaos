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
    public partial class Barcode : Form
    {
        public Barcode(string kode, string nama, string harga)
        {
            InitializeComponent();
            label3.Text = kode;

            if (nama.Length > 26)
            {
                label4.Text = nama.Substring(0, 26);
            }
            else
            {
                label4.Text = nama;
            }
            label5.Text = App.strtomoney(harga);
        }

        private void Barcode_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string printerbarcode;
                if (radioButton1.Checked)
                {
                    printerbarcode = "\\\\majubabyshop-pc\\argox";
                }
                else
                {
                    printerbarcode = "\\\\bh3-pc\\argox";
                }

                int lines = Convert.ToInt32(textBox1.Text) / 3;
//                MessageBox.Show(lines.ToString());
                App.printBarcode(label3.Text, label4.Text, label5.Text, lines.ToString(), printerbarcode);
                Close();
            }
        }
    }
}
