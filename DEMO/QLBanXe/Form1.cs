using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanXe
{
    public partial class Form1 : Form
    {
        string ConnectionString = "Data Source = DESKTOP-6TE1KM3; Initial Catalog = DBQLXe; Integrated Security = True";
        SqlConnection conn = null;
        SqlDataAdapter dataAdapter = null;
        DataTable dataTable = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    conn = new SqlConnection(ConnectionString);
            //    dataAdapter = new SqlDataAdapter("exec sp_SortBeDenLon", conn);
            //    dataTable = new DataTable();
            //    dataTable.Clear();
            //    dataAdapter.Fill(dataTable);
            //    dataGridView1.DataSource = dataTable;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Thai Bai");
            //}

        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source = DESKTOP-6TE1KM3; Initial Catalog = DBQLXe;"+"User ID="+textBox1.Text+";Password="+textBox2.Text+";Integrated Security = True";
            MessageBox.Show(ConnectionString);
            try
            {
                conn = new SqlConnection(ConnectionString);
                dataAdapter = new SqlDataAdapter("exec sp_SortBeDenLon", conn);
                dataTable = new DataTable();
                dataTable.Clear();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("Thai Bai");
            }
        }
    }
}
