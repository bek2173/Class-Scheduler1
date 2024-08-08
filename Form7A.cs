using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CLASS_SCHEDULING_SYSTEM.Resources;
namespace CLASS_SCHEDULING_SYSTEM
{
    public partial class Form7A : Form
    {
        public string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=class_schedule;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Form7A()
        {
            InitializeComponent();
            this.Load += Form7A_Load;

        }
        private void Form7A_Load(object sender, EventArgs e)
        {

            string year = Share.SelectedComboBox1Item;
            string semester = Share.SelectedComboBox3Item;
            string section = Share.SelectedComboBox2Item;
            string gcyear = Share.SelectedComboBox4Item;




            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM Batch WHERE[batch_year] = @year AND [section] =@sec AND [GC_year] = @gcyear";
                string batchid, query4;

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@semester", semester);
                    command.Parameters.AddWithValue("@sec", section);
                    command.Parameters.AddWithValue("@gcyear", gcyear);
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batchid = reader["Batch_ID"].ToString();
                            
                                
                            query4 = "SELECT * FROM Program p where Batch_ID =@bid INNER JOIN (SELECT * FROM Course c WHERE batch_year =@year AND semester = @semester) AS SubqueryAlias" +
                         "ON p.course_code = SubqueryAlias.course_code";


                            using (SqlCommand command2 = new SqlCommand(query4, connection))
                            {
                                command2.Parameters.AddWithValue("@year", year);
                                command2.Parameters.AddWithValue("@semester", semester);
                                command2.Parameters.AddWithValue("@bid", batchid);


                                using (SqlDataAdapter adapter2 = new SqlDataAdapter(command2))
                                {
                                    DataTable dataTable1 = new DataTable();
                                    adapter2.Fill(dataTable1);
                                    dataGridView1.AutoGenerateColumns = true;
                                    dataGridView1.DataSource = dataTable1;
                                    dataGridView1.Refresh();

                                }
                            }
                        }
                    }
                }
            }
        }
                       
                    
             
         
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form4 f4 = new Form4();
            f4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form5A f5 = new Form5A();
            f5.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form6 f6 = new Form6();
            f6.Show();
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public DataGridView Getgrid()
        {
            return dataGridView1;
        }
    }
}
