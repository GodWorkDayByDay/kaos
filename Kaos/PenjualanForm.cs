﻿using System;
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

        public double total;
        public int qty;

        public void inputPenjualan()
        {
            if (label8.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (textBox2.Text == "0" || textBox2.Text == "")
                {
                    textBox2.Text = "1";
                }

                bool newitem = true;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox1.Text == dataGridView1[0, i].Value.ToString())
                    {
                        newitem = false;
                        dataGridView1[2, i].Value = Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value.ToString()) + Convert.ToInt32(textBox2.Text));
                        dataGridView1[4, i].Value = App.strtomoney(Convert.ToString(Convert.ToInt32(dataGridView1[2, i].Value.ToString()) * App.moneytodouble(dataGridView1[3, i].Value.ToString())));
                    }
                    
                }

                if (newitem == true)
                {
                    dataGridView1.Rows.Add(textBox1.Text, label8.Text, textBox2.Text, textBox3.Text, App.strtomoney((Convert.ToDouble(textBox2.Text) * App.moneytodouble(textBox3.Text)).ToString()));
                }

                total += ((Convert.ToDouble(textBox2.Text) * App.moneytodouble(textBox3.Text)));
                label5.Text = "Total: " + App.strtomoney(total.ToString());

                qty += Convert.ToInt32(textBox2.Text);
                label6.Text = "Qty: " + qty.ToString();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }

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
                inputPenjualan();
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

      

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
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
    }
}