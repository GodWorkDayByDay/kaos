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
    public partial class CetakUlangForm : Form
    {
        public CetakUlangForm()
        {
            InitializeComponent();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            //dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            string tanggal = monthCalendar1.SelectionRange.Start.ToShortDateString();

            if (radioButton1.Checked == true)
            {
                DataTable dt = App.executeReader("SELECT Faktur FROM penjualancompact WHERE Tanggal = '" + tanggal + "'");

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView2.Rows.Add(row[0]);
                }
            }
            else if (radioButton2.Checked == true)
            {
                DataTable dt = App.executeReader("SELECT Nota FROM pembeliancompact WHERE Tanggal = '" + tanggal + "'");

                foreach (DataRow row in dt.Rows)
                {
                    dataGridView2.Rows.Add(row[0]);
                }
            }
            else
            {

            }

        }

        private void CetakUlangForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            App.formatDataGridView(dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string faktur = dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString();
            if (radioButton1.Checked == true)
            {
                App.printPenjualan(faktur, "COPY");
            }
            else if (radioButton2.Checked == true)
            {
                App.printPembelian(faktur, "COPY");
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            string faktur = dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString();
            if (radioButton1.Checked == true)
            {
                App.loadTable(dataGridView1, "SELECT * FROM penjualan WHERE Faktur = '" + faktur + "'");
            }
            else if (radioButton2.Checked == true)
            {
                App.loadTable(dataGridView1, "SELECT * FROM pembelian WHERE Nota = '" + faktur + "'");
            }
            else
            {

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            monthCalendar1.SetDate(DateTime.Now);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            monthCalendar1.SetDate(DateTime.Now);
        }
    }
}
