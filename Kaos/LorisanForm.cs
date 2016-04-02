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
    public partial class LorisanForm : Form
    {
        public LorisanForm()
        {
            InitializeComponent();
        }

        DateTime tgl = DateTime.Now;

        private void LorisanForm_Load(object sender, EventArgs e)
        {
            App.loadTable(dataGridView1, "SELECT Sesi, Nama, Jumlah FROM daftarlorisan WHERE Tanggal = '"+tgl.ToShortDateString()+"'");
            App.formatDataGridView(dataGridView1);

            DataTable dt = App.executeReader("SELECT DISTINCT Sesi From daftarlorisan WHERE Tanggal = '" + tgl.ToShortDateString() + "'");
            foreach (DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row[0].ToString());
            }
            comboBox1.SelectedItem = -1;
        }


        int jumlahbarang;
        int jumlahlorisan;
        int ecer;
        int hitungjumlah;
        string namabarang;

        public int hitungLorisan(int ecer, int jumlahbarang, int jumlahlorisan)
        {
            if (jumlahlorisan >= jumlahbarang)
            {
                jumlahlorisan = jumlahbarang;
            }

            if (jumlahlorisan >= ecer)
            {
                hitungjumlah = ecer;
            }
            else
            {
                if (jumlahlorisan == 0)
                {
                    hitungjumlah = 0;
                }
                else
                {
                    hitungjumlah = jumlahlorisan;
                }
            }
            return hitungjumlah;
        }

        public void executeLorisan()
        {
            string sesi = getsesi().ToString();

            DataTable lorisantable = App.executeReader("SELECT * FROM lorisan");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LORISAN [KAOS]");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("Sesi: " + sesi);
            sb.AppendLine("=========================================");


            foreach (DataRow row in lorisantable.Rows)
            {
                namabarang = row[2].ToString();
                ecer = Convert.ToInt32(App.executeScalar("SELECT Ecer FROM barang WHERE Nama = '" + row[2].ToString() + "'"));
                jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Stok FROM barang WHERE Nama = '" + row[2].ToString() + "'"));
                jumlahlorisan = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM Lorisan WHERE Nama = '" + row[2].ToString() + "'"));

                hitungjumlah = hitungLorisan(ecer, jumlahbarang, jumlahlorisan);

                if (hitungjumlah != 0)
                {
                    sb.AppendLine(namabarang + " ... " + hitungjumlah.ToString());

                    App.executeNonQuery("INSERT INTO daftarlorisan SET Tanggal = '" + DateTime.Now.ToShortDateString() + "' , Sesi = '" + sesi + "', Nama = '" + namabarang + "', Jumlah ='" + hitungjumlah.ToString() + "'");
                }

            }


            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\lorisan.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\lorisan.txt " + App.printer);

            App.executeNonQuery("DELETE FROM lorisan");

            this.Close();
        }

        public int getsesi()
        {
            return Convert.ToInt32(App.executeScalar("SELECT Sesi FROM daftarlorisan WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "' ORDER BY Sesi DESC LIMIT 1")) + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            executeLorisan();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Sesi")
            {
                DialogResult result = MessageBox.Show("Cetak Semua Lorisan?", "Cetak", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DataTable lorisantable = App.executeReader("SELECT Sesi, Nama, Jumlah FROM daftarlorisan WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "'");

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LORISAN [KAOS]");
                    sb.AppendLine(Convert.ToChar(27) + "@");
                    sb.AppendLine("Tanggal: " + tgl.ToShortDateString());
                    sb.AppendLine("Sesi: SEMUA");
                    sb.AppendLine("=========================================");


                    foreach (DataRow row in lorisantable.Rows)
                    {
                        sb.AppendLine(row[0].ToString() + ". " + row[1].ToString() + " ... " + row[2].ToString());
                    }


                    sb.AppendLine("-----------------------------------------");
                    sb.AppendLine("");

                    sb.AppendLine(Convert.ToChar(29) + "VA0");


                    System.IO.File.WriteAllText(@"C:\test\lorisan.txt", sb.ToString());

                    App.shellCommand("copy c:\\test\\lorisan.txt " + App.printer);
                }
            }
            else
            {
                    DataTable lorisantable = App.executeReader("SELECT Sesi, Nama, Jumlah FROM daftarlorisan WHERE Tanggal = '" + DateTime.Now.ToShortDateString() + "' AND Sesi = '"+comboBox1.Text+"'");

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LORISAN [KAOS]");
                    sb.AppendLine(Convert.ToChar(27) + "@");
                    sb.AppendLine("Tanggal: " + tgl.ToShortDateString());
                    sb.AppendLine("Sesi: " + comboBox1.Text);
                    sb.AppendLine("=========================================");


                    foreach (DataRow row in lorisantable.Rows)
                    {
                        sb.AppendLine(row[0].ToString() + ". " + row[1].ToString() + " ... " + row[2].ToString());
                    }


                    sb.AppendLine("-----------------------------------------");
                    sb.AppendLine("");

                    sb.AppendLine(Convert.ToChar(29) + "VA0");

                    System.IO.File.WriteAllText(@"C:\test\lorisan.txt", sb.ToString());

                    App.shellCommand("copy c:\\test\\lorisan.txt " + App.printer);
                }
        }

        private void LorisanForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }
    }
}
