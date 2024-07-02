using System;
using System.Drawing;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class DataConfiguration : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        SQLiteConnection conn;
        public string activity_view_text = "";

        public DataConfiguration()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void user_view_common_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void user_view_common_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void user_view_common_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void shiftUp_Click(object sender, EventArgs e)
        {
            ConfirmWithPassword view = new ConfirmWithPassword();
            if (view.ShowDialog() == DialogResult.OK)
            {
                string updatecQuery = "INSERT INTO configuration_activity_log (active_user,activity_category,activity_status,activity_date_time)" +
                                    "VALUES (@auser,@acat,@astatus,@adate)";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    SoftwareUserData _dataSetter = new SoftwareUserData();
                    updatec.Parameters.AddWithValue("@auser", _dataSetter.getLoggedUser());
                    updatec.Parameters.AddWithValue("@acat", "Grade");
                    updatec.Parameters.AddWithValue("@astatus", "ShiftUp");
                    updatec.Parameters.AddWithValue("@adate", DateTime.Now.ToLongDateString());
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter("@>>> Success Updated.@>>> Grade -> ShiftUp @");
                    activity_view_text_setter("@>>> Row Count.@>>> Updated -> " + updateGrade(1) + " Rows @");
                    new ActivityLogDataFetch("System Data Settings", "System Grade Shifted (+1)");
                }
            }
            else
            {
                activity_view_text_setter("@>>> Error Update.@>>> Password -> Unset @");
            }
        }

        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@", System.Environment.NewLine);
            activity_view_text += receiveText;
            terminal.Text = activity_view_text;
            terminal.SelectionStart = terminal.Text.Length;
            terminal.ScrollToCaret();
        }

        private void DataConfiguration_Load(object sender, EventArgs e)
        {
            timer1.Start();
            fetchFineVal();
        }

        public void fetchFineVal()
        {
            Directory.CreateDirectory("Data");
            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Data/" + "_sys-data.txt"))
            {
                int y = 0;
                Int32.TryParse(sr.ReadLine(), out y);
                label9.Text = "Fine value/day : " + y.ToString() + ".00 LKR";
            }
        }

        private void shiftDown_Click(object sender, EventArgs e)
        {
            ConfirmWithPassword view = new ConfirmWithPassword();
            if (view.ShowDialog() == DialogResult.OK)
            {
                string updatecQuery = "INSERT INTO configuration_activity_log (active_user,activity_category,activity_status,activity_date_time)" +
                                    "VALUES (@auser,@acat,@astatus,@adate)";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    SoftwareUserData _dataSetter = new SoftwareUserData();
                    updatec.Parameters.AddWithValue("@auser", _dataSetter.getLoggedUser());
                    updatec.Parameters.AddWithValue("@acat", "Grade");
                    updatec.Parameters.AddWithValue("@astatus", "ShiftDown");
                    updatec.Parameters.AddWithValue("@adate", DateTime.Now.ToLongDateString());
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter("@>>> Success Updated.@>>> Grade -> ShiftUp @");
                    activity_view_text_setter("@>>> Row Count.@>>> Updated -> " + updateGrade(-1) + " Rows @");
                    new ActivityLogDataFetch("System Data Settings", "System Grade Shifted (-1)");
                }
            }
            else
            {
                activity_view_text_setter("@>>> Error Update.@>>> Password -> Unset @");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            label5.Text = data.getLastActivityLogDate();
            label6.Text = data.getLastActivityLogCategory() +" "+ data.getLastActivityLogStatus();
        }

        private int updateGrade(int n)
        {
            int returnVal = 0;
            int suspendedCount = 0;
            try
            {
                string data_fetch = "SELECT * FROM library_user";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        if (!dt_data_fetch.Rows[xx]["user_grade"].ToString().Equals("None"))
                        {
                            if (n.Equals(1))
                            {
                                if (!dt_data_fetch.Rows[xx]["user_grade"].ToString().Equals("13"))
                                {
                                    string userId = dt_data_fetch.Rows[xx]["user_id"].ToString();
                                    string userGrade = dt_data_fetch.Rows[xx]["user_grade"].ToString();
                                    string updatecQuery = "UPDATE library_user SET user_grade = @updateGrade WHERE user_id = @affectId";
                                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                                    {
                                        updatec.Parameters.AddWithValue("@updateGrade", Int32.Parse(userGrade) + (n));
                                        updatec.Parameters.AddWithValue("@affectId", userId);
                                        updatec.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    string userId = dt_data_fetch.Rows[xx]["user_id"].ToString();
                                    string userGrade = dt_data_fetch.Rows[xx]["user_grade"].ToString();
                                    string updatecQuery = "UPDATE library_user SET user_state = @updateState WHERE user_id = @affectId";
                                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                                    {
                                        updatec.Parameters.AddWithValue("@updateState", "DeActive");
                                        updatec.Parameters.AddWithValue("@affectId", userId);
                                        updatec.ExecuteNonQuery();
                                        suspendedCount++;
                                    }
                                }
                            }
                            else
                            {
                                string userId = dt_data_fetch.Rows[xx]["user_id"].ToString();
                                string userGrade = dt_data_fetch.Rows[xx]["user_grade"].ToString();
                                string updatecQuery = "UPDATE library_user SET user_grade = @updateGrade WHERE user_id = @affectId";
                                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                                {
                                    updatec.Parameters.AddWithValue("@updateGrade", Int32.Parse(userGrade) + (n));
                                    updatec.Parameters.AddWithValue("@affectId", userId);
                                    updatec.ExecuteNonQuery();
                                }
                            }

                        }
                        returnVal = xx;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            
            activity_view_text_setter("@>>> Suspended User Count.@>>> Updated -> " + suspendedCount + " Rows @");
            return returnVal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int _converted = 0;
            if(int.TryParse(textBox1.Text, out _converted))
            {
                Directory.CreateDirectory("Data");
                File.WriteAllText(Environment.CurrentDirectory + "/Data/" + "_sys-data.txt", String.Empty);
                using (StreamWriter sr = File.AppendText(Environment.CurrentDirectory + "/Data/" + "_sys-data.txt"))
                {
                    sr.WriteLine(textBox1.Text);
                    sr.Close();
                    activity_view_text_setter("@>>> Success Updated.@>>> Fine Value -> "+ textBox1.Text + " LKR @");
                    new ActivityLogDataFetch("System Data Settings", "Fine Value Updated (" + textBox1.Text + "])");
                    fetchFineVal();
                }
            }
            else
            {
                activity_view_text_setter("@>>> Error.@>>> Value is not a number -> " + textBox1.Text + " @");
            }
        }
    }
}
