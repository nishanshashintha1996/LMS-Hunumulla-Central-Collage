using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class issue_book_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public string received_book_reg_id;
        public string selected_user_id = "";
        public bool book_state;
        public int user_book_count = 0;
        public string userType = "";
        SQLiteConnection conn;
        public issue_book_view(string book_reg_id)
        {
            received_book_reg_id = book_reg_id;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            InitializeComponent();
        }

        private void issue_book_view_Load(object sender, EventArgs e)
        {
            result_panel.Hide();
            result_grid.Hide();
            string data_fetch_search = "SELECT * FROM issue_book WHERE book_registration_number = @value AND transaction_state = 'Issued'";
            SQLiteCommand cmd_data_fetch_search = new SQLiteCommand(data_fetch_search, conn);
            cmd_data_fetch_search.Parameters.AddWithValue("@value", received_book_reg_id);
            SQLiteDataAdapter da_data_fetch_search = new SQLiteDataAdapter(cmd_data_fetch_search);
            DataTable dt_data_fetch_search = new DataTable();
            da_data_fetch_search.Fill(dt_data_fetch_search);
            if (dt_data_fetch_search.Rows.Count == 0)
            {
                book_status_now.Text = "Book Available";
                book_status_now_panel.BackColor = Color.Green;
                book_data_setter();
                issue_book.Enabled = true;
                confirm_transaction.Enabled = true;
                damage_state.Enabled = true;
            }
            else
            {
                book_status_now.Text = "Book Not Available";
                book_status_now_panel.BackColor = Color.Red;
                book_data_setter();
                issue_book.Enabled = false;
                confirm_transaction.Enabled = false;
                damage_state.Enabled = false;
            }
            
        }

        public void book_data_setter()
        {
            string data_fetch = "SELECT * FROM library_books WHERE registration_number = @value";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@value", received_book_reg_id);
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            if (dt_data_fetch.Rows.Count == 1)
            {
                DataRow[] dr_data_fetch = dt_data_fetch.Select("");
                foreach (DataRow row in dr_data_fetch)
                {
                    registration_number.Text = dr_data_fetch[0]["registration_number"].ToString();
                    book_name.Text = dr_data_fetch[0]["book_name"].ToString();
                    author.Text = dr_data_fetch[0]["author"].ToString();
                    publication_date.Text = dr_data_fetch[0]["publication_date"].ToString();
                    pages.Text = dr_data_fetch[0]["pages"].ToString();
                    price.Text = dr_data_fetch[0]["price"].ToString();
                    source.Text = dr_data_fetch[0]["source"].ToString();
                    category.Text = dr_data_fetch[0]["category"].ToString();
                }

            }
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void user_search_keyword_Enter(object sender, EventArgs e)
        {
            if(user_search_keyword.Text.Equals("Search Here...."))
            {
                user_search_keyword.Text = "";
            }
        }

        private void user_search_keyword_Leave(object sender, EventArgs e)
        {
            if (user_search_keyword.Text.Equals(""))
            {
                user_search_keyword.Text = "Search Here....";
            }
        }

        private void user_search_keyword_OnValueChanged(object sender, EventArgs e)
        {
            //panel_select_user.Hide();
            if (!user_search_keyword.Text.Equals(""))
            {
                result_panel.Show();
                result_grid.Show();
                try
                {
                    //Testing Area --------------- Start
                    string sql_query = "SELECT user_registration_number,user_fname,user_lname,user_type FROM library_user " +
                        "WHERE user_state = 'Active' AND user_registration_number LIKE @keyword " +
                        "OR user_fname LIKE @keyword " +
                        "OR user_lname LIKE @keyword " +
                        "OR user_grade LIKE @keyword " +
                        "OR contact_number LIKE @keyword";
                    SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                    cmd_fill.Parameters.AddWithValue("@keyword", "%" + user_search_keyword.Text + "%");
                    SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                    DataTable dtable = new DataTable();
                    dadapter.Fill(dtable);
                    result_grid.DataSource = dtable;
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                result_panel.Hide();
                result_grid.Hide();
            }
        }

        private void result_grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (result_grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    result_grid.CurrentRow.Selected = true;
                    user_attach_function(result_grid.Rows[e.RowIndex].Cells["user_registration_number"].FormattedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void user_attach_function(string id)
        {
            string query = "SELECT * FROM library_user WHERE user_registration_number = @value AND user_state = 'Active'";
            SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
            cmd_fill.Parameters.AddWithValue("@value", id);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count == 1)
            {
                fine_bal.Text = "0";
                fine_bal_lable.Visible = true;
                user_reg_num.Text = "User Reg. Number : " + dtable.Rows[0]["user_registration_number"].ToString();
                selected_user_id = dtable.Rows[0]["user_registration_number"].ToString();
                label14.Text = "User Full Name : " + dtable.Rows[0]["user_fname"].ToString() + " " + dtable.Rows[0]["user_lname"].ToString();
                grade.Text = "User Grade : " + dtable.Rows[0]["user_grade"].ToString(); ;
                contact_number.Text = "User Contact Number : " + dtable.Rows[0]["contact_number"].ToString(); ;
                user_type.Text = "User Type : " + dtable.Rows[0]["user_type"].ToString();
                userType = dtable.Rows[0]["user_type"].ToString();
                result_panel.Hide();
                result_grid.Hide();
                panel_select_user.Hide();
                DateTime returnDate;
                if (dtable.Rows[0]["user_type"].ToString().Equals("Teacher"))
                {
                    returnDate = DateTime.Today.AddDays(14);
                }
                else
                {
                    returnDate = DateTime.Today.AddDays(7);
                }
                returnerd_date.Text = returnDate.ToLongDateString().ToString();
                string q_user_book_count = "SELECT * FROM issue_book WHERE student_registration_number = @value AND transaction_state <> 'Returned'";
                SQLiteCommand cmd_fill_user_book_count = new SQLiteCommand(q_user_book_count, conn);
                cmd_fill_user_book_count.Parameters.AddWithValue("@value", selected_user_id);
                SQLiteDataAdapter dadapter_user_book_count = new SQLiteDataAdapter(cmd_fill_user_book_count);
                DataTable dtable_user_book_count = new DataTable();
                dadapter_user_book_count.Fill(dtable_user_book_count);
                user_book_count = dtable_user_book_count.Rows.Count;
                remaining_book_count.Text = dtable_user_book_count.Rows.Count.ToString();

                string q = "SELECT * FROM user_fines WHERE user_registration_number = @us_reg";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@us_reg", selected_user_id);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    fine_bal.Text = dt.Rows[0]["amount"].ToString();
                }

            }
            else
            {
                MessageBox.Show("Deleted User Account Selected !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void issue_book_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Confirm Transaction?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dialog == DialogResult.Yes)
            {
                string stringQuery = "INSERT INTO issue_book (book_registration_number, student_registration_number, issed_date, returned_date, transaction_state, issue_damage, return_damage)" +
                               "VALUES(@b_reg, @u_reg, @i_date, @r_date, 'Issued', @i_damage, 'Not Returned')";
                using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                {
                    if (!selected_user_id.Equals(""))
                    {
                        SqliteCmd.Parameters.AddWithValue("@b_reg", registration_number.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@u_reg", selected_user_id); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@i_date", DateTime.Now.ToLongDateString()); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@r_date", returnerd_date.Text); //TEXT
                        if (damage_state.Checked)
                        {
                            SqliteCmd.Parameters.AddWithValue("@i_damage", "Damage Free"); //TEXT
                        }
                        else
                        {
                            if (damage_issue_state.Text == "")
                            {
                                SqliteCmd.Parameters.AddWithValue("@i_damage", "Undefined"); //TEXT
                            }
                            else if (damage_issue_state.Text == "Damage Details")
                            {
                                SqliteCmd.Parameters.AddWithValue("@i_damage", "Undefined"); //TEXT
                            }
                            else
                            {
                                SqliteCmd.Parameters.AddWithValue("@i_damage", damage_issue_state.Text); //TEXT
                            }
                        }

                        if (userType.Equals("Teacher"))
                        {
                            if (user_book_count < 2)
                            {
                                if (confirm_transaction.Checked)
                                {
                                    try
                                    {
                                        updateBookCount(selected_user_id);
                                        new ActivityLogDataFetch("Book Issued", "Successfully Issued Book / User ID : " + selected_user_id + " Book ID : " + registration_number.Text);
                                        SqliteCmd.ExecuteNonQuery();
                                        DialogResult successMessage = MessageBox.Show("Successfully Issued Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Confirm Your Transaction Process !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("User Exceeded Maximum Book Count!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            if (user_book_count < 1)
                            {
                                if (confirm_transaction.Checked)
                                {
                                    try
                                    {
                                        updateBookCount(selected_user_id);
                                        new ActivityLogDataFetch("Book Issued", "Successfully Issued Book / User ID : " + selected_user_id + " Book ID : " + registration_number.Text);
                                        SqliteCmd.ExecuteNonQuery();
                                        DialogResult successMessage = MessageBox.Show("Successfully Issued Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Confirm Your Transaction Process !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("User Exceeded Maximum Book Count!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is an error. Please Select an User !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
        }

        public void updateBookCount(string _userReg)
        {
            string query = "SELECT * FROM issed_book_count WHERE user_registration_number = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
            cmd_fill.Parameters.AddWithValue("@value", _userReg);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count == 1)
            {
                string stringQuery = "UPDATE issed_book_count SET issued_library_book_count = @updateData WHERE user_registration_number = @userId";
                using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                {
                    SqliteCmd.Parameters.AddWithValue("@userId", _userReg); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@updateData", Int32.Parse(dtable.Rows[0]["issued_library_book_count"].ToString())+1); //TEXT
                    SqliteCmd.ExecuteNonQuery();
                }
            }
            else if (dtable.Rows.Count == 0)
            {
                string stringQuery = "INSERT INTO issed_book_count (user_registration_number, issued_library_book_count)" +
                               "VALUES(@u_reg,1)";
                using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                {
                    SqliteCmd.Parameters.AddWithValue("@u_reg", _userReg); //TEXT
                    SqliteCmd.ExecuteNonQuery();
                }
            }
        }

        private void issue_book_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void issue_book_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void issue_book_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void damage_state_CheckedChanged(object sender, EventArgs e)
        {
            if (damage_state.Checked)
            {
                damage_issue_state.Enabled = false;
            }
            else
            {
                damage_issue_state.Enabled = true;
            }
        }

        private void damage_issue_state_Leave(object sender, EventArgs e)
        {
            if(damage_issue_state.Text == "")
            {
                damage_issue_state.Text = "Damage Details";
            }
        }

        private void damage_issue_state_Enter(object sender, EventArgs e)
        {
            if (damage_issue_state.Text == "Damage Details")
            {
                damage_issue_state.Text = "";
            }
        }
    }
}
