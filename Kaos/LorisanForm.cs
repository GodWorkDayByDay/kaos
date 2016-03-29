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

            App.loadTable(dataGridView1, "SELECT Nama, Jumlah FROM lorisan");
            App.formatDataGridView(dataGridView1);
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
                    hitungjumlah = ecer - jumlahlorisan;
                }
            }
            return hitungjumlah;
        }

        public void executeLorisan()
        {
            DataTable lorisantable = App.executeReader("SELECT * FROM lorisan");


            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "LORISAN [KAOS]");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("=========================================");

            foreach (DataRow row in lorisantable.Rows)
            {
                namabarang = row[2].ToString();
                ecer = Convert.ToInt32(App.executeScalar("SELECT Ecer FROM barang WHERE Kode = '" + row[2].ToString() + "'"));
                jumlahbarang = Convert.ToInt32(App.executeScalar("SELECT Stok FROM barang WHERE Kode = '" + row[2].ToString() + "'"));
                jumlahlorisan = Convert.ToInt32(App.executeScalar("SELECT Jumlah FROM Lorisan WHERE Nama = '" + row[2].ToString() + "'"));

                hitungjumlah = hitungLorisan(ecer, jumlahbarang, jumlahlorisan);

                if (hitungjumlah != 0)
                {
                    sb.AppendLine(namabarang + " ... " + hitungjumlah.ToString());
                }

            }


            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\lorisankaos.txt", sb.ToString());

            App.shellCommand("copy c:\\test\\lorisankaos.txt " + App.printer);

            //App.executeNonQuery("DELETE FROM lorisan");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            executeLorisan();
        }
    }
}
