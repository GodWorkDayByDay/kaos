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
            DataTable dt = App.executeReader("SELECT Faktur FROM penjualancompact WHERE Tanggal = '01/03/2016' ORDER BY Faktur DESC LIMIT 5");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0]);

            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridView2.Rows.Clear();

            string faktur = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            DataTable dt = App.executeReader("SELECT Kode, Nama, Jumlah, Harga, Subtotal, User FROM penjualan WHERE Faktur = '" + faktur + "'");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView2.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
            }
        }
    }
}
