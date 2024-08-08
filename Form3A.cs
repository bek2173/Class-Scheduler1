using CLASS_SCHEDULING_SYSTEM.Resources;
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

namespace CLASS_SCHEDULING_SYSTEM
{
    public partial class Form3A : Form
    {
        public Form3A()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedCourseName = listBox1.SelectedItem.ToString();

                // Call the stored procedure to get the course code
                string courseCode = GetCourseCodeByCourseName(selectedCourseName);

                // Call another method to get a list of Instructor IDs based on the course code
                List<string> instructorIDs = GetInstructorIDsByCourseCode(courseCode);

                // Display the retrieved Instructor IDs in listBox2
                listBox2.Items.Clear();


                // Loop through each Instructor ID and get the corresponding Instructor Name
                foreach (string instructorID in instructorIDs)
                {
                    // Call another method to get the Instructor Name based on the Instructor ID
                    string instructorName = GetInstructorNameByID(instructorID);

                    // Add the Instructor Name to listBox3
                    listBox2.Items.Add(instructorName);
                }
            }



            string GetCourseCodeByCourseName(string courseName)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("getCourseIDByCourseName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@InputCourseName", courseName);

                        object result = command.ExecuteScalar();

                        return result != null ? result.ToString() : string.Empty;
                    }
                }
            }



            // Method to get a list of Instructor IDs based on the course code
            List<string> GetInstructorIDsByCourseCode(string courseCode)
            {
                List<string> instructorIDs = new List<string>();

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("getInstructorIDsByCourseCode", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@coursecode", courseCode);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Assuming the result is a list of strings (Instructor IDs)
                                string instructorID = reader["Instructor_ID"].ToString();
                                instructorIDs.Add(instructorID);
                            }
                        }
                    }
                }


                return instructorIDs;
            }


            // Method to get the Instructor Name based on the Instructor ID
            string GetInstructorNameByID(string instructorID)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("getInstructorNameByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@InstructorID", instructorID);

                        object result = command.ExecuteScalar();

                        return result != null ? result.ToString() : string.Empty;
                    }
                }


            }
    }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";


                    string selectedCourseName = listBox1.SelectedItem.ToString();
                    string instructorName = listBox2.SelectedItem.ToString();


                     (string Section, string GC_year) GetSelectedValues()
                    {
                       // Access the selected values directly from SharedData
                       string section = Shared_Data_Class.SelectedComboBox2Item;
                       string GC_year = Shared_Data_Class.SelectedComboBox4Item;

                         // Return a tuple with both values
                       return (section, GC_year);
                    }

                     string courseCode = GetCourseCodeByCourseName(selectedCourseName);
                     string InstID = GetInsIDByInsName(instructorName);

                     var selectedValues = GetSelectedValues();
                    // Get BatchID based on provided Section
                    string batchId = GetBatchId(connectionString, selectedValues.Section , selectedValues.GC_year);

            string GetCourseType(string ConnectionString, string coCode)
            {
                string courseType = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Your SQL query with a parameter for the course code
                    string query = "SELECT course_type FROM Course_pref WHERE course_code = @CourseCode";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameter for the course code
                        command.Parameters.AddWithValue("@CourseCode", courseCode);

                        // Execute the query
                        object result = command.ExecuteScalar();

                        // Check if the result is not null
                        if (result != null && result != DBNull.Value)
                        {
                            // Convert the result to string (assuming CourseType is a string)
                            courseType = result.ToString();
                        }
                    }
                }

                return courseType;
            }

            String CT = GetCourseType(connectionString, courseCode);

            if (batchId != null)
                    {
                        // Select Room from BatchRoom based on BatchID and GCYear
                        int roomId = GetRoomId(connectionString, batchId);

                        if (roomId != -1)
                        {

                    InsertCourseInstructor(connectionString, courseCode, selectedValues.GC_year, batchId, roomId, InstID,CT);
                        }
                        else
                        {
                    MessageBox.Show("Room Not Assigned for the Batch");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No Batch found for Section {selectedValues.Section}");
                    }


            string GetBatchId(string ConnectionString, string Sec, string GC)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SELECT Batch_ID FROM Batch WHERE Section = @Section AND GC_year=@gcy", connection))
                        {
                            command.Parameters.AddWithValue("@Section", Sec);
                            command.Parameters.AddWithValue("@gcy", GC);

                        object result = command.ExecuteScalar();

                        // Check if the result is not null before calling ToString()
                        // Handle DBNull.Value appropriately
                        return result != null && result != DBNull.Value ? result.ToString() : null;
                    }
                    }
                }

                int GetRoomId(string ConnectionString, string bId)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SELECT Room_no FROM Batch_room WHERE Batch_ID = @BatchID ", connection))
                        {
                            command.Parameters.AddWithValue("@BatchID", bId);
                            var result = command.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : -1;
                        }
                    }
                }

            string GetCourseCodeByCourseName(string courseName)
            {
                string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("getCourseIDByCourseName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@InputCourseName", courseName);

                        object result = command.ExecuteScalar();

                        return result != null ? result.ToString() : string.Empty;
                    }
                }
            }

            string GetInsIDByInsName(string Name)
            {
                string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SCHOOL\Documents\Schedule.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("getInsIDByInsName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@InputInsName", Name);

                        object result = command.ExecuteScalar();

                        return result != null ? result.ToString() : string.Empty;
                    }
                }
            }

            void InsertCourseInstructor(string ConnectionString, string CourseCode, string GC_Year, string BatchId,int  roomId,string InstId, string Ct)
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Course_instructor (course_code, GC_year, Batch_Id, Room_no, Instructor_ID, course_type) " +
                                 "VALUES (@CourseCode, @GC_Year, @BatchId, @RoomId, @InstructorId, @courseT)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CourseCode", CourseCode);
                        command.Parameters.AddWithValue("@GC_Year", GC_Year);
                        command.Parameters.AddWithValue("@BatchId", BatchId);
                        command.Parameters.AddWithValue("@RoomId", roomId);
                        command.Parameters.AddWithValue("@InstructorId", InstId);
                        command.Parameters.AddWithValue("@courseT", Ct);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }


            }



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
            Form3A f3 = new Form3A();
            f3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4A f4 = new Form4A();
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
            Form6A f6 = new Form6A();
            f6.Show();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
