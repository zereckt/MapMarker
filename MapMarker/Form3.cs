using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection("Server = localhost; Port = 5432; User Id = postgres; Password = 123; Database = postgres;");
            string command = "SELECT * FROM data2018";
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command, connection);
            DataSet dataset = new DataSet();
            try
            {
                connection.Open();
                adapter.Fill(dataset);
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(dataset.Tables[0].Rows[0]["latitude"].ToString());
        }
    }
}
