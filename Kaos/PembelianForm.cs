using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Kaos
{
    public partial class PembelianForm : Form
    {
        public static string user;
        public string perlusin;
        public DateTime tgl = DateTime.Now;

        public void inputPenjualan()
        {
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            double total = 0;
            try
            {
                conn.Open();

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    //UPDATE HARGA BELI
                    string test = App.executeScalar("SELECT HargaBeli FROM barang WHERE Kode = '" + dataGridView1[0, i].Value.ToString() + "'").ToString();
                    if (App.stripMoney(dataGridView1[2, i].Value.ToString()) != test)
                    {
                        App.executeNonQuery("UPDATE barang SET HargaBeli = '" + App.stripMoney(dataGridView1[2, i].Value.ToString()) + "' WHERE Kode = '" + dataGridView1[0, i].Value.ToString() + "'");
                    }

                    total += Convert.ToDouble(dataGridView1[4, i].Value) * Convert.ToDouble(App.stripMoney(dataGridView1[2, i].Value.ToString()));

                    //INSERT PEMBELIAN
                    cmd.CommandText = "INSERT INTO pembelian SET Tanggal = '" + tgl.ToShortDateString() + "', Nota = '" + textBox1.Text + "', Kode = '" + dataGridView1[0, i].Value.ToString() + "', Nama = '" + dataGridView1[1, i].Value.ToString() + "', Jumlah ='" + dataGridView1[4, i].Value.ToString() + "', User = '" + label6.Text + "'";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();


                    cmd.CommandText = "UPDATE barang SET Stok = Stok + '" + dataGridView1[4, i].Value.ToString() + "' WHERE Kode = '" + dataGridView1[0, i].Value.ToString() + "'";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }

                //INSERT PEMBELIANCOMPACT
                cmd.CommandText = "INSERT INTO pembeliancompact SET Tanggal = '" + tgl.ToShortDateString() + "', Nota = '" + textBox1.Text + "', User = '" + label6.Text + "', Total ='" + total.ToString() + "', Lunas ='BELUM LUNAS'";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pembelian Berhasil. Terima Kasih");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        public PembelianForm(string user1)
        {
            user = user1;
            InitializeComponent();
        }

        private void PembelianForm_Load(object sender, EventArgs e)
        {
            label6.Text = user;
            App.formatDataGridView(dataGridView1);
            textBox1.Text = tgl.Day.ToString() + tgl.Month.ToString() + tgl.Year.ToString() + "-";
            this.ActiveControl = textBox1;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            DataTable rdr = App.executeReader("SELECT Nama, PerLusin, HargaBeli FROM barang WHERE Kode = '" + textBox2.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    label4.Text = Convert.ToString(row[0]);
                    textBox4.Text = App.stripMoney(Convert.ToString(row[2]));

                    if (Convert.ToString(row[1]) != "")
                    {
                        label5.Text = "Per Lusin: " + Convert.ToString(row[1]);
                        perlusin = Convert.ToString(row[1]);
                    }
                    else
                    {
                        perlusin = "1";
                        label5.Text = "Per Lusin: 1";
                    }

                    textBox3.Text = "1";
                    textBox3.Focus();
                    textBox3.SelectAll();
                }

            }
            else
            {
                label4.Text = "";
                label5.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
            }
        }

        public void clearAllFields()
        {
            textBox2.Text = "";
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addpembelian();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            inputPenjualan();
            App.printPembelian(textBox1.Text, user);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (CariBarangForm cari = new CariBarangForm())
            {
                cari.ShowDialog();

                textBox2.Text = cari.valueFromCari;
                textBox3.Focus();
                // do what ever with result...
            }
        }

        public void addpembelian()
        {
            bool cekduplicate = false;

            if (textBox2.Text != "" && label4.Text != "")
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox2.Text == dataGridView1[0, i].Value.ToString())
                    {
                        cekduplicate = true;
                    }
                }

                if (cekduplicate == false)
                {
                    double jumlahpc;
                    if (radioButton1.Checked == true)
                    {
                        jumlahpc = Convert.ToDouble(textBox3.Text) * 12 / Convert.ToInt32(perlusin);
                    }
                    else
                    {
                        jumlahpc = Convert.ToDouble(textBox3.Text);
                        textBox3.Text = "---";
                    }

                    dataGridView1.Rows.Add(textBox2.Text, label4.Text, textBox4.Text, textBox3.Text, jumlahpc.ToString());
                    textBox2.Text = "";
                    textBox2.Focus();
                }
                else
                {
                    MessageBox.Show("Kode Barang sudah ada di daftar pembelian");
                }
            }

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Jumlah (PC)";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Jumlah (Lusin)";
        }
    }
}

