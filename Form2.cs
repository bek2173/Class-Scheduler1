using CLASS_SCHEDULING_SYSTEM.Resources;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CLASS_SCHEDULING_SYSTEM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            SetRoundedPanel(panel5, 20);
        }
        private void SetRoundedPanel(Panel panel, int radius)
        {
            panel.Paint += (sender, e) =>
            {
                using (GraphicsPath path = RoundedRectangle(panel.ClientRectangle, radius))
                using (Region region = new Region(path))
                {
                    panel.Region = region;
                    using (Pen pen = new Pen(Color.LightGray, 1))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };
        }

        private GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4A f4A = new Form4A();
            f4A.Show();
        }
        public string GetSelectedValue(ComboBox combo)
        {
            if (combo != null && combo.SelectedItem != null)
            {
                return combo.SelectedItem.ToString();
            }
            else
            {
                return null; // or some default value or throw an exception, depending on your requirements
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form7A f7 = new Form7A();
            f7.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {


                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                string year = comboBox1.SelectedItem?.ToString();
                string semester = comboBox3.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(semester))
                {
                    MessageBox.Show("Please select both year and semester.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string sqlQuery = "SELECT course_name FROM Course WHERE [batch_year] = @year AND [semester] = @semester";


                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@year", year);
                            command.Parameters.AddWithValue("@semester", semester);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {

                                 this.Hide();
                                 Form3A f3 = new Form3A();
                                 f3.Show();

                                f3.listBox1.Items.Clear(); // Clear existing items in the ListBox

                                while (reader.Read())
                                {
                                    string resultValue = reader.GetString(0); // Assuming the first column is what you want to display
                                    f3.listBox1.Items.Add(resultValue);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close(); // Ensure the connection is closed, even if an exception occurs
                    }
                }
            }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
          
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shared_Data_Class.SelectedComboBox2Item = comboBox2.SelectedItem?.ToString();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shared_Data_Class.SelectedComboBox4Item = comboBox4.SelectedItem?.ToString();

        }
    }
}
