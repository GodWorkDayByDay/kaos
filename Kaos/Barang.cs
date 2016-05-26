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
    public partial class Barang : Form
    {
        public Barang()
        {
            InitializeComponent();
        }


        private void Barang_Load(object sender, EventArgs e)
        {
            App2.DoubleBuffered(dataGridView1, true);
            App.loadTable(dataGridView1, "SELECT * FROM barang");
            App.loadComboBox(comboBox1, "SELECT * FROM merk");
            dataGridView1.CurrentRow.Selected = false;
            this.ActiveControl = textBox9;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            dataGridView1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kode = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            string nama = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string harga = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            string stok = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
            string batas = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
            string ecer = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
            string hargabeli = dataGridView1[6, dataGridView1.CurrentRow.Index].Value.ToString();
            string perlusin = dataGridView1[7, dataGridView1.CurrentRow.Index].Value.ToString();
            string cari = textBox9.Text;

            EditBarang editbarang = new EditBarang(kode, nama, harga, stok, batas, ecer, hargabeli, perlusin, cari);
            editbarang.ShowDialog();


            App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Nama LIKE '%" + textBox9.Text + "%'");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                DialogResult result = MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("DELETE");
                }
                else
                {
                    MessageBox.Show("NOT DELETED");
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            App.loadTable(dataGridView1, "SELECT * FROM barang WHERE Nama LIKE '%" + textBox9.Text + "%'");
        }

        private void Barang_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public void clearAllFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;
        }

        public void insertBarang(string size, MySqlConnection conn, MySqlCommand cmd)
        {
            string nama = comboBox1.Text + " " + textBox2.Text + " " + size;
            string kode = textBox1.Text + size;
            cmd.CommandText = ("INSERT INTO barang SET Kode = '" + kode + "', Nama = '" + nama + "', Harga = '" + App.stripMoney(textBox4.Text) +
                "', Stok = '" + textBox5.Text + "', Batas = '" + textBox6.Text + "', Ecer = '" + textBox7.Text + "', HargaBeli = '" + textBox3.Text + "', PerLusin = '" + textBox8.Text + "'");
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" &&
                textBox2.Text != "" &&
                textBox3.Text != "" &&
                textBox4.Text != "" &&
                textBox5.Text != "" &&
                textBox6.Text != "" &&
                textBox7.Text != "" &&
                textBox8.Text != "" &&
                comboBox1.Text != "")
            {
                if (comboBox2.Text != "" ||
                    checkBox1.Checked != false ||
                    checkBox2.Checked != false ||
                    checkBox3.Checked != false ||
                    checkBox4.Checked != false ||
                    checkBox5.Checked != false ||
                    checkBox6.Checked != false ||
                    checkBox7.Checked != false ||
                    checkBox8.Checked != false ||
                    checkBox9.Checked != false ||
                    checkBox10.Checked != false ||
                    checkBox11.Checked != false)
                {

                    string cek_kode;
                    cek_kode = Convert.ToString(App.executeScalar("SELECT COUNT(*) FROM barang WHERE Kode = '" + textBox1.Text + "'"));
                    if (cek_kode != "")
                    {
                        MySqlConnection conn = new MySqlConnection(App.getConnectionString());
                        MySqlCommand cmd = new MySqlCommand();

                        try
                        {
                            conn.Open();
                            if (comboBox2.Text != "")
                            {
                                insertBarang(comboBox2.Text, conn, cmd);
                            }

                            if (checkBox1.Checked != false)
                            {
                                insertBarang(checkBox1.Text, conn, cmd);
                            }

                            if (checkBox2.Checked != false)
                            {
                                insertBarang(checkBox2.Text, conn, cmd);
                            }

                            if (checkBox3.Checked != false)
                            {
                                insertBarang(checkBox3.Text, conn, cmd);
                            }

                            if (checkBox4.Checked != false)
                            {
                                insertBarang(checkBox4.Text, conn, cmd);
                            }

                            if (checkBox5.Checked != false)
                            {
                                insertBarang(checkBox5.Text, conn, cmd);
                            }

                            if (checkBox6.Checked != false)
                            {
                                insertBarang(checkBox6.Text, conn, cmd);
                            }

                            if (checkBox7.Checked != false)
                            {
                                insertBarang(checkBox7.Text, conn, cmd);
                            }

                            if (checkBox8.Checked != false)
                            {
                                insertBarang(checkBox8.Text, conn, cmd);
                            }

                            if (checkBox9.Checked != false)
                            {
                                insertBarang(checkBox9.Text, conn, cmd);
                            }

                            if (checkBox10.Checked != false)
                            {
                                insertBarang(checkBox10.Text, conn, cmd);
                            }

                            if (checkBox11.Checked != false)
                            {
                                insertBarang(checkBox11.Text, conn, cmd);
                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }


                        clearAllFields();
                        MessageBox.Show("Barang sudah masuk. Terima Kasih.");


                    }
                    else
                    {
                        MessageBox.Show("Kode barang sudah ada.");
                    }


                }
                else
                {
                    MessageBox.Show("Lengkapi data barang.");
                }
            }



        }
    }
}
