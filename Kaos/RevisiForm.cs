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
    public partial class RevisiForm : Form
    {
        public RevisiForm()
        {
            InitializeComponent();
        }

        private void RevisiForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.formatDataGridView(dataGridView2);

            //            DataTable dt = App.executeReader("SELECT Faktur FROM penjualan WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "'");
            DataTable dt = App.executeReader("SELECT Faktur FROM penjualancompact WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "' AND Bayar = '0'");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0]);

            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult result = MessageBox.Show("Batalkan barang ini?", "REVISI", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    App.executeNonQuery("UPDATE penjualan SET Jumlah = '0' , Subtotal = '0' WHERE Faktur = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "' AND Kode = '" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'");
                    MessageBox.Show("Barang sudah dibatalkan dari penjualan");

                    dataGridView2.Rows.Clear();

                    string faktur = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                    DataTable dt = App.executeReader("SELECT Kode, Nama, Jumlah, Harga, Subtotal, User FROM penjualan WHERE Faktur = '" + faktur + "'");

                    double total = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView2.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                        total += Convert.ToDouble(row[4].ToString());
                    }

                    App.executeNonQuery("UPDATE penjualancompact SET Total = '" + total.ToString() + "' WHERE Faktur = '" + faktur + "'");

                    label1.Text = "Faktur: " + faktur;
                    label2.Text = "Total: " + App.strtomoney(total.ToString());

                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            string faktur = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            DataTable dt = App.executeReader("SELECT Kode, Nama, Jumlah, Harga, Subtotal, User FROM penjualan WHERE Faktur = '" + faktur + "'");

            double total = 0;
            foreach (DataRow row in dt.Rows)
            {
                dataGridView2.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                total += Convert.ToDouble(row[4].ToString());
            }

            label1.Text = "Faktur: " + faktur;
            label2.Text = "Total: " + App.strtomoney(total.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            App.printPenjualan(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString(), "REVISI");
            this.Close();
        }

        private void RevisiForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
