using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kaos
{
    class App
    {

        public static bool admin = Convert.ToBoolean(Environment.GetCommandLineArgs()[2].ToString());
        public static string printer = Environment.GetCommandLineArgs()[3].ToString();

    public static string getConnectionString()
        {
            string[] settings = System.IO.File.ReadAllLines(@"C:\test\settingskaos.ini");
            MySqlConnectionStringBuilder connstring = new MySqlConnectionStringBuilder();
            connstring.Server = settings[0];
            connstring.UserID = settings[1];
            connstring.Password = settings[2];
            connstring.Database = Environment.GetCommandLineArgs()[1].ToString();

            return connstring.ToString();
        }

        public static DataTable executeReader(string query)

        {
            DataTable results = new DataTable("Results");
            using (MySqlConnection connection = new MySqlConnection(App.getConnectionString()))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    using (MySqlDataReader reader = command.ExecuteReader())
                        results.Load(reader);
                }
            }
            return results;
        }

        public static void executeNonQuery(string query)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }


        public static object executeScalar(string query)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            object result;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                result = cmd.ExecuteScalar();
                //if (result != null){int r = Convert.ToInt32(result);Console.WriteLine("Number of countries in the World database is: " + r);}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = null;
            }

            conn.Close();
            return result;
        }

//        public static MySqlConnection conn = new MySqlConnection(getConnectionString());

        
        public static void loadTable(DataGridView dtv, string search)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            MySqlCommand command1 = new MySqlCommand(search, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            //    foreach (DataRow dr in dt.Rows)
            //  {
            //    dr["Harga"] = ;
            //}

            dtv.DataSource = dt;
            if (dtv.Columns["Harga"] != null) { dtv.Columns["Harga"].DefaultCellStyle.Format = "c"; }
            if (dtv.Columns["HargaBeli"] != null) { dtv.Columns["HargaBeli"].DefaultCellStyle.Format = "c"; }
            if (dtv.Columns["Subtotal"] != null) { dtv.Columns["Subtotal"].DefaultCellStyle.Format = "c"; }
            //           dtv.Columns["Harga"].DefaultCellStyle.Format = "c";
            //         dtv.Columns["HargaBeli"].DefaultCellStyle.Format = "c";
            dtv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void loadComboBox(ComboBox cmbx, string search)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            MySqlCommand command1 = new MySqlCommand(search, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
              {
                cmbx.Items.Add(dr["Merk"]);
            }

//            cmbx.DataSource = dt;
        }

        public static string mysqlcurrency(string str)
        {
            return "CONCAT('Rp', FORMAT(" + str + ", 0))";
        }

        public static string strtomoney(string str)
        {
            //return "Rp" + str.Replace(",", ".");
            try
            {
                return String.Format("{0:C0}", Convert.ToInt32(str));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        public static double moneytodouble(string str)
        {
            try
            {
                str = str.Replace("Rp", "");
                str = str.Replace(".", "");
                return Convert.ToDouble(str);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public static void formatDataGridView(DataGridView dgv)
        {
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;

        }

    }
}
