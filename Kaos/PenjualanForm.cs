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
    public partial class PenjualanForm : Form
    {
        bool retur = false;

        public string getFaktur(DateTime tgl)
        {
            string tanggal = tgl.Day.ToString();
            string bulan = tgl.Month.ToString();
            string tahun = tgl.Year.ToString();

            if (tanggal.Length < 2)
            {
                tanggal = "0" + tanggal;
            }

            if (bulan.Length < 2)
            {
                bulan = "0" + bulan;
            }

            tahun = tahun.Substring(2);

            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            object result;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select COUNT(*) FROM penjualancompact WHERE Tanggal = '" + tgl.ToShortDateString() + "'", conn);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                result = null;
            }
            conn.Close();

            string nomor = "";
            string urut;

            if (Convert.ToString(result) != "0")
            {
                urut = Convert.ToString(Convert.ToUInt32(result) + 1);
            }
            else
            {
                urut = "1";
            }

            if (urut.Length == 1)
            {
                nomor = "000" + urut;
            }
            else if (urut.Length == 2)
            {
                nomor = "00" + urut;
            }
            else if (urut.Length == 3)
            {
                nomor = "0" + urut;
            }
            else if (urut.Length == 4)
            {
                nomor = urut;
            }

            return tanggal + bulan + tahun + nomor;
        }

        public static string user;

        public void inputPenjualan()
        {
            int jumlah = 0;
 
            if (label8.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {

                if (textBox2.Text == "0" || textBox2.Text == "")
                {
                    jumlah = 1;
                }
                else
                {
                    jumlah = Convert.ToInt32(textBox2.Text);
                }

                //RETUR
                if (retur == true)
                {
                    jumlah = jumlah * -1;

                }



                bool newitem = true;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox1.Text == dataGridView1[0, i].Value.ToString())
                    {
                        newitem = false;
                        dataGridView1[2, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value.ToString()) + jumlah);
                        dataGridView1[4, i].Value = App.strtomoney(Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value.ToString()) * App.moneytodouble(dataGridView1[3, i].Value.ToString())));
                    }

                }

                if (newitem == true)
                {
                    dataGridView1.Rows.Add(textBox1.Text, label8.Text, jumlah.ToString(), textBox3.Text, App.strtomoney((Convert.ToDouble(jumlah) * App.moneytodouble(textBox3.Text)).ToString()));
                }

                calculateTotalQty();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";

                dataGridView1.ClearSelection();

                textBox1.Focus();
            }


        }

        public void calculateTotalQty()
        {
            int qty = 0;
            double total = 0;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                qty += App.cInt(dataGridView1[2, i].Value.ToString());
                total += App.moneytodouble(dataGridView1[4, i].Value.ToString());
            }
            label5.Text = "Total: " + App.strtomoney(total.ToString());
            label6.Text = "Qty: " + qty.ToString();
        }


        public PenjualanForm(string user1)
        {
            user = user1;
            InitializeComponent();
        }

        private void PenjualanForm_Load(object sender, EventArgs e)
        {
            DateTime tgl = DateTime.Now;
            label1.Text = getFaktur(tgl);
            label4.Text = user;
            this.CenterToScreen();
            App2.DoubleBuffered(dataGridView1, true);
            App.formatDataGridView(dataGridView1);
            this.ActiveControl = textBox1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable rdr = App.executeReader("SELECT Nama, Harga From barang WHERE Kode = '" + textBox1.Text + "'");

            if (rdr.Rows.Count != 0)
            {
                foreach (DataRow row in rdr.Rows)
                {
                    label8.Text = Convert.ToString(row[0]);
                    textBox2.Text = "1";
                    textBox3.Text = App.strtomoney(row[1].ToString());
                }

            }
            else
            {
                label8.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.F7)
            {
                button2.PerformClick();
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                textBox3.Focus();
                textBox3.SelectAll();
            }

            if (e.KeyCode == Keys.Left)
            {
                textBox1.Focus();
                textBox1.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                inputPenjualan();
            }

            if (e.KeyCode == Keys.F7)
            {
                button2.PerformClick();
            }

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                textBox2.Focus();
                textBox2.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                inputPenjualan();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (CariBarangForm cari = new CariBarangForm())
            {
                cari.ShowDialog();

                textBox1.Text = cari.valueFromCari;
                textBox2.Focus();
                // do what ever with result...
            }

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    int x = Convert.ToInt32(textBox2.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Masukkan jumlah angka saja jangan huruf!");
                textBox2.Text = "";
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                calculateTotalQty();
            }
        }

        public void addLorisan(string kode, string jumlah)
        {
            DateTime tgl = DateTime.Now;
            if (App.executeScalar("SELECT Jumlah FROM lorisan WHERE Nama = '" + kode + "' LIMIT 1") == null)
            {
                App.executeNonQuery("INSERT INTO lorisan SET Tanggal = '" + tgl.ToShortDateString() + "', Sesi = '0', Nama = '" + kode + "', Jumlah = '" + jumlah + "'");
            }
            else
            {
                App.executeNonQuery("UPDATE lorisan SET Jumlah = Jumlah + '" + jumlah + "' WHERE Nama = '" + kode + "'");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount != 0)
            {
                DateTime tgl = DateTime.Now;
                MySqlConnection conn = new MySqlConnection(App.getConnectionString());
                MySqlCommand cmd = new MySqlCommand();

                string lastfaktur = getFaktur(tgl);

                try
                {
                    conn.Open();
                    cmd.Connection = conn;

                    string kode;
                    string nama;
                    string jumlah;
                    string harga;
                    string subtotal;
                    double total = 0;
                    double laba = 0;
                    double labatotal = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        kode = dataGridView1[0, i].Value.ToString();
                        nama = dataGridView1[1, i].Value.ToString();
                        jumlah = dataGridView1[2, i].Value.ToString();
                        harga = App.stripMoney(dataGridView1[3, i].Value.ToString());
                        subtotal = App.stripMoney(dataGridView1[4, i].Value.ToString());

                        laba = (Convert.ToDouble(harga) - Convert.ToDouble(App.executeScalar("SELECT HargaBeli FROM barang WHERE Kode = '" + kode + "'").ToString())) * Convert.ToDouble(jumlah);


                        labatotal += laba;

                        cmd.CommandText = "INSERT INTO penjualan SET Tanggal='" + tgl.ToShortDateString() + "', Faktur='" + lastfaktur + "',Kode='" + kode + "',Nama='" + nama + "',Jumlah='" + jumlah + "',Harga='" + harga + "',Subtotal='" + subtotal + "',Laba='" + laba + "', User='" + user + "'";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE barang SET Stok = Stok - '" + jumlah + "' WHERE Kode = '" + kode + "'";
                        cmd.ExecuteNonQuery();

                        addLorisan(nama, jumlah);

                        total += App.cDouble(App.stripMoney(dataGridView1[4, i].Value.ToString()));
                    }

                    cmd.CommandText = "INSERT INTO penjualancompact SET Tanggal='" + tgl.ToShortDateString() + "', Faktur='" + lastfaktur + "',Total='" + total + "',Laba='" + labatotal + "', Bayar='0', User='" + user + "'";
                    cmd.ExecuteNonQuery();


                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


                App.printPenjualan(lastfaktur, user);


                this.Close();

            }
            else
            {
                MessageBox.Show("Penjualan masih kosong!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (retur == false)
            {
                retur = true;
                this.BackColor = Color.Yellow;
                MessageBox.Show("Hati-hati retur barang!");
                textBox1.Focus();
            }
            else
            {
                retur = false;
                this.BackColor = Control.DefaultBackColor;
                MessageBox.Show("Kembali ke mode penjualan!");
                textBox1.Focus();
            }
        }
    }
}
