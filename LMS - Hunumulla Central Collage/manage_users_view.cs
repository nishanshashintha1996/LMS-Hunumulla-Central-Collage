using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class manage_users_view : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public string get_user_id = "";
        public string get_user_registration_number = "";
        public string get_user_fname = "";
        public string get_user_lname = "";
        public string get_user_grade = "";
        public string get_contact_number = "";
        public string get_user_address = "";
        public string get_user_web_id = "";
        public string get_user_type = "";
        public string librarian_id = "";
        public string librarian_name = "";
        public string librarian_email = "";
        public string swstateD = "";
        public string librarian_username = "";
        public string last_login = "";
        public string librarian_password = "";
        public string state = "";
        public string activity_view_text = "";
        SQLiteConnection conn;

        public manage_users_view(string id, string swstate)
        {
            swstateD = swstate;
            librarian_id = id;
            string q = "SELECT * FROM librarian_user";
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("librarian_id=" + librarian_id);
                foreach (DataRow row in dr)
                {
                    librarian_name = row["librarian_name"].ToString();
                    librarian_email = row["librarian_email"].ToString();
                    librarian_username = row["librarian_username"].ToString();
                    last_login = row["last_login"].ToString();
                    librarian_password = row["password"].ToString();
                    state = row["state"].ToString();
                }
            }
            InitializeComponent();
        }

        private void manage_users_view_Load(object sender, EventArgs e)
        {
            new ActivityLogDataFetch("Manage Users", "Open Manage User Panel");
            timer2.Start();
            timer3.Start();
            loading_view view = new loading_view();
            view.ShowDialog();
            this.WindowState = FormWindowState.Maximized;
            user_name_lable.Text = librarian_name;
            timer1.Start();
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            date.Text = DateTime.Now.ToLongDateString();
            student_panel_01.Hide();
            student_panel_02.Hide();
            teacher_panel_01.Hide();
            teacher_panel_02.Hide();
            nun_ac_panel_01.Hide();
            nun_ac_panel_02.Hide();
            empty_panel.Show();
            activity_view_text_setter("Hunumulla Central Collage LMS.@Version 1.3.0 @");

            //main views
            add_user_main_view_panel.Hide();
            edit_user_main_view_panel.Hide();

            //Loading Search Field Data
            string sql_query = "SELECT user_registration_number,user_fname,user_lname,user_grade,contact_number,user_type,user_registed_date FROM library_user";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            search_result_view.DataSource = dtable;

            student_user_count.Text = static_counter("Student");
            teacher_user_count.Text = static_counter("Teacher");
            non_academic_user_count.Text = static_counter("Non Academic");
            active_user_count.Text = static_counter_all("Active");
            removed_user_count.Text = static_counter_all("DeActive");
            
            string[] names = { "Student", "Teacher", "Non Academic"};
            string[] values = { static_counter("Student"), static_counter("Teacher"), static_counter("Non Academic") };
            fillChart(names,values,"Users Of the System");
        }

        private void fillChart(string[] names, string[] values, string title)
        {
            //chart title
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart1.Titles.Add(title);
            chart1.Series.Add("UserCount");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            chart1.Series[0]["PieLabelStyle"] = "Disabled";
            if (names.Length.ToString().Equals(values.Length.ToString()))
            {
                for (int i = 0; i < names.Length; i++)
                {
                    chart1.Series["UserCount"].Points.AddXY(names[i], values[i]);
                }
            }
        }

        public string static_counter(string value)
        {
            string sql_query = "SELECT * FROM library_user WHERE user_type = @value AND user_state = 'Active'";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            search_result_view.DataSource = dtable;
            return dtable.Rows.Count.ToString();
        }


        public string static_counter_all(string value)
        {
            string sql_query = "SELECT * FROM library_user WHERE user_state = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            search_result_view.DataSource = dtable;
            return dtable.Rows.Count.ToString();
        }

        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@",System.Environment.NewLine);
            activity_view_text += receiveText;
            activity_view.Text = activity_view_text;
            activity_view.SelectionStart = activity_view.Text.Length;
            activity_view.ScrollToCaret();
        }

        private void maximize_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                activity_view_text_setter(">>> Window - Normal State @");
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                activity_view_text_setter(">>> Window - Maximized State @");
            }
        }

        private void minimize_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            activity_view_text_setter(">>> Window - Minimized State @");
        }

        private void add_student_Click(object sender, EventArgs e)
        {
            Dashboard view = new Dashboard(librarian_id,"", swstateD);
            this.Hide();
            view.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
        

        private void teacher_user_Click(object sender, EventArgs e)
        {
            teacher_panel_01.Show();
            teacher_panel_02.Show();
            student_panel_01.Hide();
            student_panel_02.Hide();
            nun_ac_panel_01.Hide();
            nun_ac_panel_02.Hide();
            empty_panel.Hide();
            activity_view_text_setter(">>> Add User -> Teacher @");
        }

        private void student_user_Click(object sender, EventArgs e)
        {
            teacher_panel_01.Hide();
            teacher_panel_02.Hide();
            student_panel_01.Show();
            student_panel_02.Show();
            nun_ac_panel_01.Hide();
            nun_ac_panel_02.Hide();
            empty_panel.Hide();
            activity_view_text_setter(">>> Add User -> Student @");
        }

        private void non_ac_user_Click(object sender, EventArgs e)
        {
            teacher_panel_01.Hide();
            teacher_panel_02.Hide();
            student_panel_01.Hide();
            student_panel_02.Hide();
            nun_ac_panel_01.Show();
            nun_ac_panel_02.Show();
            empty_panel.Hide();
            activity_view_text_setter(">>> Add User -> Non Academic User @");
        }

        private void add_non_ac_btn_Click(object sender, EventArgs e)
        {
            string stringQuery = "INSERT INTO library_user (user_registration_number, user_fname, user_lname, user_grade, user_address, contact_number, user_type, user_state, user_web_id, user_registed_date)VALUES(@reg_nu, @f_name, @l_name, @grade, @address, @number, @type, @state, @web_id, @reg_date)";
            
            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
            {
                string find_same_data_q = "SELECT * FROM library_user WHERE user_registration_number = @user_reg_num";
                SQLiteCommand cmd_new = new SQLiteCommand(find_same_data_q, conn);
                cmd_new.Parameters.AddWithValue("@user_reg_num", non_ac_reg_number.Text);
                SQLiteDataAdapter da_new = new SQLiteDataAdapter(cmd_new);
                DataTable dt_new = new DataTable();
                da_new.Fill(dt_new);
                if (dt_new.Rows.Count == 0)
                {
                    SqliteCmd.Parameters.AddWithValue("@reg_nu", non_ac_reg_number.Text); // INTEGER
                    if (!non_ac_f_name.Text.Equals("") && !non_ac_l_name.Text.Equals("") && !non_ac_address.Text.Equals("") && !non_ac_contact.Text.Equals(""))
                    {
                        SqliteCmd.Parameters.AddWithValue("@f_name", non_ac_f_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@l_name", non_ac_l_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@address", non_ac_address.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@number", non_ac_contact.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@grade", "None"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@type", "Non Academic"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@state", "Active"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@web_id", 0);  // INTEGER
                        SqliteCmd.Parameters.AddWithValue("@reg_date", DateTime.Now.ToLongDateString()); //TEXT
                        try
                        {
                            SqliteCmd.ExecuteNonQuery();
                            non_ac_f_name.Text = "";
                            non_ac_l_name.Text = "";
                            non_ac_reg_number.Text = "";
                            non_ac_address.Text = "";
                            non_ac_contact.Text = "";
                            activity_view_text_setter(">>> -------------------------------------------------------------------------------- @>>> Success -> The User Added To The System @>>> Name :" + non_ac_f_name.Text+ " " +non_ac_l_name.Text+ "@>>> User Type : Non Academic @>>> Registed Date : " + DateTime.Now.ToLongDateString()+ "@>>> -------------------------------------------------------------------------------- @");
                            
                        }
                        catch (Exception ex)
                        {
                            activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                            MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                        MessageBox.Show("Please Fill All The Sections !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                    MessageBox.Show("Registration Number Should Be Unique!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void add_student_btn_Click(object sender, EventArgs e)
        {
            string stringQuery = "INSERT INTO library_user (user_registration_number, user_fname, user_lname, user_grade, user_address, contact_number, user_type, user_state, user_web_id, user_registed_date)VALUES(@reg_nu, @f_name, @l_name, @grade, @address, @number, @type, @state, @web_id, @reg_date)";

            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
            {
                string find_same_data_q = "SELECT * FROM library_user WHERE user_registration_number = @user_reg_num";
                SQLiteCommand cmd_new = new SQLiteCommand(find_same_data_q, conn);
                cmd_new.Parameters.AddWithValue("@user_reg_num", student_reg_number.Text);
                SQLiteDataAdapter da_new = new SQLiteDataAdapter(cmd_new);
                DataTable dt_new = new DataTable();
                da_new.Fill(dt_new);
                if (dt_new.Rows.Count == 0)
                {
                    SqliteCmd.Parameters.AddWithValue("@reg_nu", student_reg_number.Text); // INTEGER
                    if (!student_f_name.Text.Equals("") && !student_l_name.Text.Equals("") && !student_address.Text.Equals("") && !student_contact.Text.Equals("") && !student_grade.Text.Equals(""))
                    {
                        SqliteCmd.Parameters.AddWithValue("@f_name", student_f_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@l_name", student_l_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@address", student_address.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@number", student_contact.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@grade", student_grade.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@type", "Student"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@state", "Active"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@web_id", 0);  // INTEGER
                        SqliteCmd.Parameters.AddWithValue("@reg_date", DateTime.Now.ToLongDateString()); //TEXT
                        try
                        {
                            SqliteCmd.ExecuteNonQuery();
                            student_reg_number.Text = "";
                            student_f_name.Text = "";
                            student_l_name.Text = "";
                            student_address.Text = "";
                            student_contact.Text = "";
                            student_grade.Text = "";
                            activity_view_text_setter(">>> -------------------------------------------------------------------------------- @>>> Success -> The User Added To The System @>>> Name :" + student_f_name.Text + " " + student_l_name.Text + "@>>> User Type : Student @>>> Registed Date : " + DateTime.Now.ToLongDateString() + "@>>> -------------------------------------------------------------------------------- @");

                        }
                        catch (Exception ex)
                        {
                            activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                            MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                        MessageBox.Show("Please Fill All The Sections !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                    MessageBox.Show("Registration Number Should Be Unique!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void add_teacher_btn_Click(object sender, EventArgs e)
        {
            string stringQuery = "INSERT INTO library_user (user_registration_number, user_fname, user_lname, user_grade, user_address, contact_number, user_type, user_state, user_web_id, user_registed_date)VALUES(@reg_nu, @f_name, @l_name, @grade, @address, @number, @type, @state, @web_id, @reg_date)";

            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
            {
                string find_same_data_q = "SELECT * FROM library_user WHERE user_registration_number = @user_reg_num";
                SQLiteCommand cmd_new = new SQLiteCommand(find_same_data_q, conn);
                cmd_new.Parameters.AddWithValue("@user_reg_num", teacher_reg_number.Text);
                SQLiteDataAdapter da_new = new SQLiteDataAdapter(cmd_new);
                DataTable dt_new = new DataTable();
                da_new.Fill(dt_new);
                if (dt_new.Rows.Count == 0)
                {
                    SqliteCmd.Parameters.AddWithValue("@reg_nu", teacher_reg_number.Text); // INTEGER
                    if (!teacher_f_name.Text.Equals("") && !teacher_l_name.Text.Equals("") && !teacher_address.Text.Equals("") && !teacher_contact.Text.Equals("") && !teacher_grade.Text.Equals(""))
                    {
                        SqliteCmd.Parameters.AddWithValue("@f_name", teacher_f_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@l_name", teacher_l_name.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@address", teacher_address.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@number", teacher_contact.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@grade", teacher_grade.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@type", "Teacher"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@state", "Active"); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@web_id", 0);  // INTEGER
                        SqliteCmd.Parameters.AddWithValue("@reg_date", DateTime.Now.ToLongDateString()); //TEXT
                        try
                        {
                            SqliteCmd.ExecuteNonQuery();
                            teacher_reg_number.Text = "";
                            teacher_f_name.Text = "";
                            teacher_l_name.Text = "";
                            teacher_address.Text = "";
                            teacher_contact.Text = "";
                            teacher_grade.Text = "";
                            activity_view_text_setter(">>> -------------------------------------------------------------------------------- @>>> Success -> The User Added To The System @>>> Name :" + teacher_f_name.Text + " " + teacher_l_name.Text + "@>>> User Type : Teacher @>>> Registed Date : " + DateTime.Now.ToLongDateString() + "@>>> -------------------------------------------------------------------------------- @");

                        }
                        catch (Exception ex)
                        {
                            activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                            MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                        MessageBox.Show("Please Fill All The Sections !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    activity_view_text_setter(">>> Error -> Add User Syntax Error @");
                    MessageBox.Show("Registration Number Should Be Unique!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }



        //add view close btn
        private void add_user_close_Click(object sender, EventArgs e)
        {
            student_user_count.Text = static_counter("Student");
            teacher_user_count.Text = static_counter("Teacher");
            active_user_count.Text = static_counter_all("Active");
            removed_user_count.Text = static_counter_all("DeActive");
            non_academic_user_count.Text = static_counter("Non Academic");
            welcome_panel.Show();
            add_user_main_view_panel.Hide();

            chart1.Series.Clear();
            string[] names = { "Student", "Teacher", "Non Academic" };
            string[] values = { static_counter("Student"), static_counter("Teacher"), static_counter("Non Academic") };
            fillChart(names, values, "Users Of the System");

            activity_view_text_setter(">>> Add User Window Closed @");
        }
        //add view open btn
        private void add_user_main_btn_Click(object sender, EventArgs e)
        {
            add_user_main_view_panel.Show();
            edit_user_main_view_panel.Hide();
            welcome_panel.Hide();
            activity_view_text_setter(">>> Add User Window Opened @");
        }
        //edit view open btn
        private void edit_user_main_btn_Click(object sender, EventArgs e)
        {
            edit_user_main_view_panel.Show();
            add_user_main_view_panel.Hide();
            welcome_panel.Hide();
            activity_view_text_setter(">>> Edit User Window Opened @");
        }
        //edit view close btn
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            student_user_count.Text = static_counter("Student");
            teacher_user_count.Text = static_counter("Teacher");
            active_user_count.Text = static_counter_all("Active");
            removed_user_count.Text = static_counter_all("DeActive");
            non_academic_user_count.Text = static_counter("Non Academic");
            welcome_panel.Show();
            edit_user_main_view_panel.Hide();

            chart1.Series.Clear();
            string[] names = { "Student", "Teacher", "Non Academic" };
            string[] values = { static_counter("Student"), static_counter("Teacher"), static_counter("Non Academic") };
            fillChart(names, values, "Users Of the System");
            activity_view_text_setter(">>> Edit User Window Closed @");
        }

        private void search_key_word_Enter(object sender, EventArgs e)
        {
            if (search_key_word.Text.Equals("Search here ...."))
            {
                search_key_word.Text = "";
            }
        }

        private void search_key_word_Leave(object sender, EventArgs e)
        {
            if (search_key_word.Text.Equals(""))
            {
                search_key_word.Text = "Search here ....";
            }
        }

        private void search_key_word_OnValueChanged(object sender, EventArgs e)
        {
            if (!search_key_word.Text.Equals(""))
            {
                try
                {
                    //Testing Area --------------- Start
                    string sql_query = "SELECT user_registration_number,user_fname,user_lname,user_grade,contact_number,user_type,user_registed_date FROM library_user" +
                        " WHERE user_registration_number LIKE @keyword " +
                        "OR user_fname LIKE @keyword " +
                        "OR user_lname LIKE @keyword " +
                        "OR user_grade LIKE @keyword " +
                        "OR contact_number LIKE @keyword";
                    SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                    cmd_fill.Parameters.AddWithValue("@keyword", "%" + search_key_word.Text + "%");
                    SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                    DataTable dtable = new DataTable();
                    dadapter.Fill(dtable);
                    search_result_view.DataSource = dtable;

                }
                catch(Exception ex)
                {

                }
            }
            else if(search_key_word.Equals("Search here ...."))
            {
                reload_search_data();
            }
            else if(search_key_word.Equals(""))
            {
                reload_search_data();
            }
        }

        private void search_result_view_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void search_result_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (search_result_view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    search_result_view.CurrentRow.Selected = true;
                    user_edit_function(search_result_view.Rows[e.RowIndex].Cells["user_registration_number"].FormattedValue.ToString());
                    search_panel_1.Hide();
                    user_edit_panel_01.Show();
                    user_edit_panel_02.Show();
                    activity_view_text_setter(">>> Opened Window -> Edit User @");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void reload_search_result_Click(object sender, EventArgs e)
        {
            reload_search_data();
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            search_panel_1.Show();
            user_edit_panel_01.Hide();
            user_edit_panel_02.Hide();
            reload_search_data();
            activity_view_text_setter(">>> Opened Window -> Search User @");
        }

        public void reload_search_data()
        {
            string sql_query = "SELECT user_registration_number,user_fname,user_lname,user_grade,contact_number,user_type,user_registed_date FROM library_user";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            search_result_view.DataSource = dtable;
            activity_view_text_setter(">>> Reloaded -> Search Results @");
        }

        public void user_edit_function(string user_registration_number)
        {
            string sql_query = "SELECT * FROM library_user WHERE user_registration_number = @urn";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@urn", user_registration_number);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);

            get_user_id = dtable.Rows[0]["user_id"].ToString();

            edit_user_reg_number.Text = dtable.Rows[0]["user_registration_number"].ToString();
            get_user_registration_number = dtable.Rows[0]["user_registration_number"].ToString();

            edit_user_f_name.Text = dtable.Rows[0]["user_fname"].ToString();
            get_user_fname = dtable.Rows[0]["user_fname"].ToString();

            edit_user_l_name.Text = dtable.Rows[0]["user_lname"].ToString();
            get_user_lname = dtable.Rows[0]["user_lname"].ToString();

            edit_user_grade.Text = dtable.Rows[0]["user_grade"].ToString();
            get_user_grade = dtable.Rows[0]["user_grade"].ToString();

            edit_user_contact.Text = dtable.Rows[0]["contact_number"].ToString();
            get_contact_number = dtable.Rows[0]["contact_number"].ToString();

            edit_user_address.Text = dtable.Rows[0]["user_address"].ToString();
            get_user_address = dtable.Rows[0]["user_address"].ToString();

            edit_user_web_id.Text = dtable.Rows[0]["user_web_id"].ToString();
            get_user_web_id = dtable.Rows[0]["user_web_id"].ToString();

            if (dtable.Rows[0]["user_state"].ToString().Equals("Active"))
            {
                recover_user_btn.Hide();
                delete_user_btn.Show();
                edit_user_reg_number.Enabled = true;
                edit_user_f_name.Enabled = true;
                edit_user_l_name.Enabled = true;
                edit_user_grade.Enabled = true;
                edit_user_contact.Enabled = true;
                edit_user_address.Enabled = true;
                edit_user_web_id.Enabled = true;
                check_stu.Enabled = true;
                check_tea.Enabled = true;
                check_non.Enabled = true;
                edit_user_btn.Enabled = true;
            }
            if (dtable.Rows[0]["user_state"].ToString().Equals("DeActive"))
            {
                recover_user_btn.Show();
                delete_user_btn.Hide();
                edit_user_reg_number.Enabled = false;
                edit_user_f_name.Enabled = false;
                edit_user_l_name.Enabled = false;
                edit_user_grade.Enabled = false;
                edit_user_contact.Enabled = false;
                edit_user_address.Enabled = false;
                edit_user_web_id.Enabled = false;
                check_stu.Enabled = false;
                check_tea.Enabled = false;
                check_non.Enabled = false;
                edit_user_btn.Enabled = false;
            }
            check_box_function(dtable.Rows[0]["user_type"].ToString());
            get_user_type = dtable.Rows[0]["user_type"].ToString();
            
            if (get_user_type.Equals("Non Academic"))
            {
                edit_user_btn.Font = new Font(edit_user_btn.Font.FontFamily, 8);
                delete_user_btn.Font = new Font(edit_user_btn.Font.FontFamily, 8);
                recover_user_btn.Font = new Font(edit_user_btn.Font.FontFamily, 8);
            }
            edit_user_btn.Text = "Update " + get_user_type;
            delete_user_btn.Text = "Remove " + get_user_type;
            recover_user_btn.Text = "Recover " + get_user_type;
        }

        public void check_box_function(string state_check)
        {
            if (state_check.Equals("Teacher"))
            {
                check_tea.Checked = true;
                check_stu.Checked = false;
                check_non.Checked = false;
                activity_view_text_setter(">>> User -> Teacher @");
            }

            if (state_check.Equals("Non Academic"))
            {
                check_tea.Checked = false;
                check_stu.Checked = false;
                check_non.Checked = true;
                activity_view_text_setter(">>> User -> Non Academic @");
            }

            if (state_check.Equals("Student"))
            {
                check_tea.Checked = false;
                check_stu.Checked = true;
                check_non.Checked = false;
                activity_view_text_setter(">>> User -> Student @");
            }
        }

        private void edit_user_btn_Click(object sender, EventArgs e)
        {
            if (!edit_user_reg_number.Text.Equals(get_user_registration_number))
            {
                string updatecQuery = "UPDATE library_user SET user_registration_number = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_reg_number.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    try
                    {
                        updatec.ExecuteNonQuery();
                    }catch(Exception ex)
                    {
                        MessageBox.Show("Registration Number Should Be Unique!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    activity_view_text_setter(">>> Updated User -> user_registration_number @");
                }
            }
            if (!edit_user_f_name.Text.Equals(get_user_fname))
            {
                string updatecQuery = "UPDATE library_user SET user_fname = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_f_name.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_fname @");
                }
            }
            if (!edit_user_l_name.Text.Equals(get_user_lname))
            {
                string updatecQuery = "UPDATE library_user SET user_lname = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_l_name.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_lname @");
                }
            }
            if (!edit_user_grade.Text.Equals(get_user_grade))
            {
                string updatecQuery = "UPDATE library_user SET user_grade = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_grade.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_grade @");
                }
            }
            if (!edit_user_contact.Text.Equals(get_contact_number))
            {
                string updatecQuery = "UPDATE library_user SET contact_number = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_contact.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> contact_number @");
                }
            }
            if (!edit_user_address.Text.Equals(get_user_address))
            {
                string updatecQuery = "UPDATE library_user SET user_address = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_address.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_address @");
                }
            }
            if (!edit_user_web_id.Text.Equals(get_user_web_id))
            {
                string updatecQuery = "UPDATE library_user SET user_web_id = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", edit_user_web_id.Text);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_web_id @");
                }
            }

            string changed_check_box_value="Error";

            if (check_tea.Checked)
            {
                changed_check_box_value = "Teacher";
            }
            if (check_stu.Checked)
            {
                changed_check_box_value = "Student";
            }
            if (check_non.Checked)
            {
                changed_check_box_value = "Non Academic";
            }

            if (!changed_check_box_value.Equals(get_user_type))
            {
                string updatecQuery = "UPDATE library_user SET user_type = @updateData WHERE user_id = @userId";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", changed_check_box_value);
                    updatec.Parameters.AddWithValue("@userId", get_user_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> user_type @");
                }
            }

            search_panel_1.Show();
            user_edit_panel_01.Hide();
            user_edit_panel_02.Hide();
            reload_search_data();
        }

        private void recover_user_btn_Click(object sender, EventArgs e)
        {
            string recoverQuery = "UPDATE library_user SET user_state = 'Active' WHERE user_id = @userId";
            using (SQLiteCommand recover = new SQLiteCommand(recoverQuery, conn))
            {
                recover.Parameters.AddWithValue("@userId", get_user_id);
                recover.ExecuteNonQuery();
                activity_view_text_setter(">>> Updated User -> Recovered @");
            }
            search_panel_1.Show();
            user_edit_panel_01.Hide();
            user_edit_panel_02.Hide();
            reload_search_data();
        }

        private void delete_user_btn_Click(object sender, EventArgs e)
        {
            string qr = "SELECT * FROM issue_book WHERE student_registration_number=@u_num AND transaction_state='Issued'";
            SQLiteCommand cmdr = new SQLiteCommand(qr, conn);
            cmdr.Parameters.AddWithValue("@u_num", get_user_registration_number);
            SQLiteDataAdapter dar = new SQLiteDataAdapter(cmdr);
            DataTable dtr = new DataTable();
            dar.Fill(dtr);
            if (dtr.Rows.Count == 0)
            {
                string deleteQuery = "UPDATE library_user SET user_state = 'DeActive' WHERE user_id = @userId";
                using (SQLiteCommand delete = new SQLiteCommand(deleteQuery, conn))
                {
                    delete.Parameters.AddWithValue("@userId", get_user_id);
                    delete.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated User -> Deleted @");
                }
                search_panel_1.Show();
                user_edit_panel_01.Hide();
                user_edit_panel_02.Hide();
                reload_search_data();
            }
            else
            {
                MessageBox.Show("The User has Not Returned Books!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void check_stu_Click(object sender, EventArgs e)
        {
            check_box_function("Student");
        }

        private void check_non_Click(object sender, EventArgs e)
        {
            check_box_function("Non Academic");
        }

        private void check_tea_Click(object sender, EventArgs e)
        {
            check_box_function("Teacher");
        }

        private void manage_books_Click(object sender, EventArgs e)
        {
            manage_books_view view = new manage_books_view(librarian_id, swstateD);
            this.Hide();
            view.ShowDialog();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            settings_view view = new settings_view();
            view.ShowDialog();
        }

        // info
        private void btn_info_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=about");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // doc
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=doc");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // privacy
        private void btn_privacy_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=privacy");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // system
        private void btn_web_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=system");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            RegistrationFormView view = new RegistrationFormView();
            view.ShowDialog();
        }

        private void manage_reports_Click(object sender, EventArgs e)
        {
            report_generating_view view = new report_generating_view();
            view.ShowDialog();
        }

        private void resource_statistics_Click(object sender, EventArgs e)
        {
            ResourceStatics view = new ResourceStatics();
            view.ShowDialog();
        }
        
        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            DailyReport view = new DailyReport();
            view.ShowDialog();
        }
        

        private void timer2_Tick(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            label5.Text = data.getInstitiuteName();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            timer3.Start();
        }
    }
}
