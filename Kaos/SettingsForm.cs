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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public void loadUserTable()
        {
            dataGridView1.Rows.Clear();

            DataTable dt = App.executeReader("SELECT * FROM users ORDER BY ID");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0], row[1]);
            }

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            App.formatDataGridView(dataGridView1);
            loadUserTable();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    DialogResult result = MessageBox.Show("Hapus user ini?", "Hapus", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        App.executeNonQuery("DELETE FROM users WHERE ID = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "'");
                        MessageBox.Show("User " + dataGridView1[1,dataGridView1.CurrentRow.Index].Value.ToString()  + " sudah dihapus.");
                        loadUserTable();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                App.executeNonQuery("INSERT INTO users SET ID = '" + textBox1.Text.ToUpper() + "', Name = '" + textBox2.Text + "'");
                loadUserTable();
                MessageBox.Show("User " + textBox2.Text + " berhasil ditambahkan");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
