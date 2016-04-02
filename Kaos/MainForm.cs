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
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtv"></param>
        /// <param name="search"></param>

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            

            try
            {
                this.Show();
                if (App.toko == "kaos")
                {
                    this.BackColor = Color.LightBlue;
                    this.Text = "Kaos";
                }
                else if (App.toko == "bh")
                {
                    this.BackColor = Color.Pink;
                    this.BackgroundImage = Kaos.Properties.Resources.batik_rainbow;
                    this.Text = "BH";
                }

                if (App.admin == false)
                {
                    button3.Enabled = false;
                    button7.Enabled = false;
                    button8.Enabled = false;
                    button7.Visible = false;
                    button8.Visible = false;
                }

                this.CenterToScreen();
                App.loadTable(dataGridView1, "SELECT * FROM barang");
                dataGridView1.Columns["HargaBeli"].Visible = false;

                this.ActiveControl = textBox1;

            }
            catch (Exception)
            {
                MessageBox.Show("Password Salah");
            }

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserLoginForm login = new UserLoginForm("Penjualan");
            login.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserLoginForm login = new UserLoginForm("Pembelian");
            login.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Barang barangform = new Barang();
            barangform.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RevisiForm revisi = new RevisiForm();
            revisi.ShowDialog();
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                for (int i = 0; i < (dataGridView1.Rows.Count); i++)
                {
                    if (dataGridView1.Rows[i].Cells["Nama"].Value.ToString().StartsWith(e.KeyChar.ToString(), true, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        dataGridView1.Rows[i].Cells[0].Selected = true;
                        return; // stop looping
                    }
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Nama Like '%" + textBox1.Text + "%'");
                textBox1.Text = "";
            }

            if (e.KeyCode == Keys.F1)
            {
                button1.PerformClick();
            }

            if (e.KeyCode == Keys.F2)
            {
                button2.PerformClick();
            }

            if (e.KeyCode == Keys.F3)
            {
                button3.PerformClick();
            }

            if (e.KeyCode == Keys.F4)
            {
                button4.PerformClick();
            }

            if (e.KeyCode == Keys.F5)
            {
                button7.PerformClick();
            }

            if (e.KeyCode == Keys.F6)
            {
                button8.PerformClick();
            }

            if (e.KeyCode == Keys.F7)
            {
                button6.PerformClick();
            }

            if (e.KeyCode == Keys.F12)
            {
                button5.PerformClick();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            LorisanForm lorisan = new LorisanForm();
            lorisan.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CetakUlangForm cetakulang = new CetakUlangForm();
            cetakulang.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Laporan laporan = new Laporan();
            laporan.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
        }
    }
}