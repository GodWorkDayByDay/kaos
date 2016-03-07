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
    public partial class Laporan : Form
    {
        public void loadLaporanPenjualan(DataGridView dgv, string tanggal)
        {
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            DataTable rdr = App.executeReader("SELECT * FROM penjualan WHERE Tanggal = '" + tanggal + "'");

            double subtotal = 0;


            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[0], row[1], row[2], row[3], row[4], App.strtomoney(row[5].ToString()), App.strtomoney(row[6].ToString()), row[7]);
            }

            string lastfaktur = "";
            try
            {
                lastfaktur = dataGridView1[1, 0].Value.ToString();
            }
            catch (Exception)
            {
            }
             
            for (int i = 1; i < dataGridView1.RowCount; i++)
            {
                subtotal += App.moneytodouble(dataGridView1[6, i].Value.ToString());
                if (lastfaktur == dataGridView1[1, i].Value.ToString())
                {
                    dataGridView1.Rows[i].Cells[1].Value = "";
                }
                else
                {
                    lastfaktur = dataGridView1.Rows[i].Cells[1].Value.ToString();
                }

            }

            label2.Text = "TOTAL: " + App.strtomoney(subtotal.ToString());

            //dataGridView1.Rows.Add("", "", "", "", "", "TOTAL:", App.strtomoney(subtotal.ToString()), "");
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.Font =  new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.Font = new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;

            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //dataGridView1[0, dataGridView1.RowCount - 1].Selected = true;

        }

        public void loadLaporanPembelian(DataGridView dgv, string tanggal)
        {
            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            DataTable rdr = App.executeReader("SELECT * FROM pembelian WHERE Tanggal = '" + tanggal + "'");

            double subtotal = 0;

            foreach (DataRow row in rdr.Rows)
            {
                dgv.Rows.Add(row[0], row[1], row[2], row[3], row[4], App.strtomoney(row[5].ToString()), App.strtomoney(row[6].ToString()), row[7]);
            }

            for (int i = 1; i < dataGridView1.RowCount; i++)
            {
                subtotal += App.moneytodouble(dataGridView1[6, i].Value.ToString());
                if (dataGridView1[1, i - 1].Value.ToString() == dataGridView1[1, i].Value.ToString() || dataGridView1[1, i - 1].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].Cells[1].Value = "";
                }

            }
            //dataGridView1.Rows.Add("", "", "", "", "", "TOTAL:", App.strtomoney(subtotal.ToString()), "");
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.Font =  new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.Font = new Font("Arial", 12, FontStyle.Bold);
            //dataGridView1[5, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;
            //dataGridView1[6, dataGridView1.RowCount - 1].Style.ForeColor = Color.Red;

            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //dataGridView1[0, dataGridView1.RowCount - 1].Selected = true;
            label2.Text = "TOTAL: " + App.strtomoney(subtotal.ToString());
        }

        public Laporan()
        {
            InitializeComponent();
        }

        private void Laporan_Load(object sender, EventArgs e)
        {
            DateTime tgl = DateTime.Now;
            //App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Tanggal = '" + tgl.ToShortDateString() + "'");
            loadLaporanPenjualan(dataGridView1, tgl.ToShortDateString());
            label1.Text = "Tanggal: " + tgl.ToShortDateString();

        }



        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dataGridView1.Rows.Clear();
            loadLaporanPenjualan(dataGridView1, monthCalendar1.SelectionRange.Start.ToShortDateString());
            label1.Text = "Tanggal: " + monthCalendar1.SelectionRange.Start.ToShortDateString();
            //            App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Tanggal = '"+ monthCalendar1.SelectionRange.Start.ToShortDateString() + "'");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            dataGridView2.Rows.Clear();
            loadLaporanPenjualan(dataGridView2, monthCalendar2.SelectionRange.Start.ToShortDateString());
            label2.Text = "Tanggal: " + monthCalendar2.SelectionRange.Start.ToShortDateString();
            //            App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Tanggal = '"+ monthCalendar1.SelectionRange.Start.ToShortDateString() + "'");
        }
    }
}
