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
    public partial class PembelianForm : Form
    {
        public static string user;

        public PembelianForm(string user1)
        {
            user = user1;
            InitializeComponent();
        }

        private void PembelianForm_Load(object sender, EventArgs e)
        {

        }
    }
}
