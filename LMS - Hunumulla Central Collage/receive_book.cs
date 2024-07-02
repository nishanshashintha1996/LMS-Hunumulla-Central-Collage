using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class receive_book : Form
    {
        public string pre_fines = "0";
        private bool mouseDown;
        private Point lastLocation;
        SQLiteConnection conn;
        public string receiverdBook_id;
        public receive_book(string book_id)
        {
            InitializeComponent();
            receiverdBook_id = book_id;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void receive_book_Load(object sender, EventArgs e)
        {
            string data_fetch_search = "SELECT * FROM issue_book WHERE book_registration_number = @value AND transaction_state = 'Issued'";
            SQLiteCommand cmd_data_fetch_search = new SQLiteCommand(data_fetch_search, conn);
            cmd_data_fetch_search.Parameters.AddWithValue("@value", receiverdBook_id);
            SQLiteDataAdapter da_data_fetch_search = new SQLiteDataAdapter(cmd_data_fetch_search);
            DataTable dt_data_fetch_search = new DataTable();
            da_data_fetch_search.Fill(dt_data_fetch_search);
            if (dt_data_fetch_search.Rows.Count == 0)
            {
                book_status_now.Text = "Book Available in the Library";
                book_status_now_panel.BackColor = Color.Red;
                book_data_setter();
                damage_check.Enabled = false;
                accept_book.Enabled = false;
            }
            else
            {
                string user_id = "";
                book_status_now.Text = "Book Finded";
                book_status_now_panel.BackColor = Color.Green;
                book_data_setter();
                DataRow[] dr_data_fetch = dt_data_fetch_search.Select("");
                try
                {
                    foreach (DataRow row in dr_data_fetch)
                    {
                        user_id = dr_data_fetch[0]["student_registration_number"].ToString();
                        user_data_setter(user_id);

                        string issed_date = dr_data_fetch[0]["issed_date"].ToString();
                        string returned_date = dr_data_fetch[0]["returned_date"].ToString();

                        DateTime enteredDate = DateTime.Parse(returned_date);
                        DateTime todayDate = DateTime.Parse(DateTime.Now.ToLongDateString());
                        double x = (enteredDate - todayDate).TotalDays;
                        if (x < 0)
                        {
                            pay_selection.Enabled = true;
                            user_status_now.Text = "Late Returning (" + (x*-1).ToString() + " Days Lated)";
                            user_status_now.BackColor = Color.Red;
                            user_status_now.ForeColor = Color.White;
                            user_status_now_panel.BackColor = Color.Red;
                            try
                            {
                                Directory.CreateDirectory("Data");
                                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Data/" + "_sys-data.txt"))
                                {
                                    int y = 0;
                                    Int32.TryParse(sr.ReadLine(), out y);
                                    fines_calculations.Text = ((x * -1) * y).ToString();
                                    pre_fines = fines_calculations.Text;
                                    //MessageBox.Show(pre_fines);
                                }
                            }catch (Exception ex)
                            {
                                fines_calculations.Text = ((x * -1) * 1).ToString() ;
                                pre_fines = fines_calculations.Text;
                                //MessageBox.Show(pre_fines);
                            }
                        }
                        else if (x == 0)
                        {
                            user_status_now.Text = "Today is the Return Day";
                            user_status_now.BackColor = Color.White;
                            user_status_now.ForeColor = Color.Black;
                            user_status_now_panel.BackColor = Color.Green;
                        }
                        else
                        {
                            user_status_now.Text = (x).ToString() + " Days Remaining";
                            user_status_now.BackColor = Color.White;
                            user_status_now.ForeColor = Color.Black;
                            user_status_now_panel.BackColor = Color.Green;
                        }
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void user_data_setter(string user_id)
        {
            string data_fetch = "SELECT * FROM library_user WHERE user_registration_number = @value";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@value", user_id);
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            if (dt_data_fetch.Rows.Count == 1)
            {
                DataRow[] dr_data_fetch = dt_data_fetch.Select("");
                foreach (DataRow row in dr_data_fetch)
                {
                    user_reg_number.Text = dr_data_fetch[0]["user_registration_number"].ToString();
                    user_name.Text = dr_data_fetch[0]["user_fname"].ToString() +" "+ dr_data_fetch[0]["user_lname"].ToString();
                    user_type.Text = dr_data_fetch[0]["user_type"].ToString();
                    grade.Text = dr_data_fetch[0]["user_grade"].ToString();
                    contact_number.Text = dr_data_fetch[0]["contact_number"].ToString();
                }
            }
        }

        public void book_data_setter()
        {
            string data_fetch = "SELECT * FROM library_books WHERE registration_number = @value";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@value", receiverdBook_id);
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

        private void receive_book_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void receive_book_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void receive_book_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void pay_selection_CheckedChanged(object sender, EventArgs e)
        {
            if (pay_selection.Checked)
            {
                pay_selection.Text = "Paid Fines";
            }
            else
            {
                pay_selection.Text = "Add to Account";
            }
        }

        private void damage_check_CheckedChanged(object sender, EventArgs e)
        {
            if (damage_check.Checked)
            {
                damage_query.Enabled = false;
            }
            else
            {
                damage_query.Enabled = true;
            }
        }
        
        private void damage_query_Leave(object sender, EventArgs e)
        {
            if (damage_query.Text.Equals(""))
            {
                damage_query.Text = "Damage Details";
            }
        }

        private void damage_query_Enter(object sender, EventArgs e)
        {
            if (damage_query.Text.Equals("Damage Details"))
            {
                damage_query.Text = "";
            }
        }

        private void accept_book_Click(object sender, EventArgs e)
        {
            if (confirm_transaction.Checked)
            {
                DialogResult dialog = MessageBox.Show("Confirm Transaction?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (dialog == DialogResult.Yes)
                {
                    string updatecQuery = "UPDATE issue_book SET transaction_state = 'Returned', return_damage = @r_damage, book_fine_amount=@b_amount, book_fine_status=@b_f_state, returned_date = @ret_date WHERE book_registration_number = @b_num AND student_registration_number = @u_num AND transaction_state = 'Issued'";
                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                    {
                        updatec.Parameters.AddWithValue("@u_num", user_reg_number.Text);
                        updatec.Parameters.AddWithValue("@b_num", registration_number.Text);
                        updatec.Parameters.AddWithValue("@b_amount", fines_calculations.Text);
                        updatec.Parameters.AddWithValue("@ret_date", DateTime.Now.ToLongDateString()); 
                        if (pay_selection.Checked)
                        {
                            updatec.Parameters.AddWithValue("@b_f_state", "Paid");
                        }
                        else
                        {
                            updatec.Parameters.AddWithValue("@b_f_state", "Added to Account");
                        }
                        if (damage_check.Checked)
                        {
                            updatec.Parameters.AddWithValue("@r_damage", "Damage Free");
                            updatec.ExecuteNonQuery();
                            if (lost_book.Checked)
                            {
                                updateBookStatus();
                            }
                            if (!pay_selection.Checked)
                            {
                                updateFinesFunstion();
                            }
                            new ActivityLogDataFetch("Book Receive", "Successfully Received Book / User ID : " + user_reg_number.Text + " Book ID : " + registration_number.Text);
                            DialogResult successMessage = MessageBox.Show("Successfully Returned Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            if (damage_query.Text.Equals(""))
                            {
                                updatec.Parameters.AddWithValue("@r_damage", "Undefined"); //TEXT
                            }
                            else if (damage_query.Text.Equals("Damage Details"))
                            {
                                updatec.Parameters.AddWithValue("@r_damage", "Undefined"); //TEXT
                            }
                            else
                            {
                                try
                                {
                                    updatec.Parameters.AddWithValue("@r_damage", damage_query.Text);
                                    updatec.ExecuteNonQuery();

                                    if (!pay_selection.Checked)
                                    {
                                        updateFinesFunstion();
                                    }
                                    if (lost_book.Checked)
                                    {
                                        updateBookStatus();
                                    }
                                    DialogResult successMessage = MessageBox.Show("Successfully Returned Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Please Contact Developer", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Confirm Your Transaction Process !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        public void updateBookStatus()
        {
            string stringQuery = "UPDATE library_books SET status = @status WHERE registration_number = @reg_nu";
            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
            {
                SqliteCmd.Parameters.AddWithValue("@reg_nu", receiverdBook_id); //TEXT
                SqliteCmd.Parameters.AddWithValue("@status", "Deleted"); //TEXT
                try
                {
                    SqliteCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void updateFinesFunstion()
        {

            string q = "SELECT * FROM user_fines WHERE user_registration_number = @us_reg";
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@us_reg", user_reg_number.Text);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                float am1 = float.Parse(dt.Rows[0]["amount"].ToString());
                float am2 = float.Parse(fines_calculations.Text);
                float tot = am1 + am2;
                string stringQuery = "UPDATE user_fines SET amount = @amount, transaction_status = @tr_status, transaction_date = @tr_date WHERE user_registration_number = @reg_nu";
                using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                {
                    SqliteCmd.Parameters.AddWithValue("@reg_nu", user_reg_number.Text); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@amount", tot.ToString()); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@tr_status", "Added"); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@tr_date", DateTime.Now.ToLongDateString()); //TEXT
                    try
                    {
                        SqliteCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                string stringQuery = "INSERT INTO user_fines (user_registration_number, amount, transaction_status, transaction_date)" +
                                                            "VALUES(@reg_nu, @amount, @tr_status, @tr_date)";
                using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                {
                    SqliteCmd.Parameters.AddWithValue("@reg_nu", user_reg_number.Text); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@amount", fines_calculations.Text); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@tr_status", "Added"); //TEXT
                    SqliteCmd.Parameters.AddWithValue("@tr_date", DateTime.Now.ToLongDateString()); //TEXT
                    try
                    {
                        SqliteCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void lost_book_CheckedChanged(object sender, EventArgs e)
        {
            if (lost_book.Checked)
            {
                try
                {
                    fines_calculations.Text = (float.Parse(fines_calculations.Text) + (float.Parse(price.Text) * 2) + ((float.Parse(price.Text) * 2) * 25 / 100)).ToString();
                    if (Int32.Parse(fines_calculations.Text) > 0)
                    {
                        pay_selection.Enabled = true;
                    }
                    else
                    {
                        pay_selection.Enabled = false;
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(pre_fines);
                }
            }
            else
            {
                fines_calculations.Text = pre_fines;
                if (Int32.Parse(fines_calculations.Text) > 0)
                {
                    pay_selection.Enabled = true;
                }
                else
                {
                    pay_selection.Enabled = false;
                }
            }
        }
    }
}
