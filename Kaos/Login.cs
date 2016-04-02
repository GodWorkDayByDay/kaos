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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public int tries = 0;

        private void Login_Load(object sender, EventArgs e)
        {
            if (App.toko == "kaos")
            {
                pictureBox1.Image = Kaos.Properties.Resources.splash;
                pictureBox1.Height = 500;
                pictureBox1.Width = 372;
                this.Height = 586;
                this.Width = 407;
                textBox1.Width = 372;
                textBox1.Location = new Point(9, 518);
            }
            else if (App.toko == "bh")
            {
                pictureBox1.Image = Kaos.Properties.Resources.Underwear_Etsuko;
            }

            if (App.admin == false)
            {
                textBox1.Enabled = false;
                textBox1.Visible = false;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Second % 4 == 0)
            {
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.ToUpper() == "MAREMA168" || textBox1.Text.ToUpper() == "314159")
                {
                    this.Close();
                }
                else if (textBox1.Text.ToUpper() == "KAOS" || textBox1.Text.ToUpper() == "BH")
                {
                    App.admin = false;
                    this.Close();
                }
                else
                {
                    tries += 1;
                    textBox1.Text = "";
                    if (tries > 2)
                    {
                        System.Windows.Forms.Application.Exit();
                    }

                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                System.Windows.Forms.Application.Exit();
            }

        }
    }
}
