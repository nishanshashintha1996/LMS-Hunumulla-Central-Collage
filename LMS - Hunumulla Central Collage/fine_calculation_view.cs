using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class fine_calculation_view : Form
    {
        SQLiteConnection conn;
        public string activity_view_text = "";
        public float final_calculated_value = 0;
        public float entered_amount = 0;
        private bool mouseDown;
        private Point lastLocation;
        public string userRegistrationNumber = "";
        public string fineBalance = "0";
        public fine_calculation_view(string id)
        {
            InitializeComponent();
            userRegistrationNumber = id;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void fine_calculation_view_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM user_fines WHERE user_registration_number = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
            cmd_fill.Parameters.AddWithValue("@value", userRegistrationNumber);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count == 1)
            {
                fine_bal.Text = dtable.Rows[0]["amount"].ToString()+".00 LKR";
                fineBalance = dtable.Rows[0]["amount"].ToString();
                fine_bal.Visible = true;
                paying_amount.Visible = true;
            }
        }

        // Window Movement 

        private void add_edit_book_close_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fine_calculation_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void fine_calculation_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void fine_calculation_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    entered_amount = float.Parse(paying_amount.Text);
                    final_calculated_value = float.Parse(fineBalance) - entered_amount;
                    if (final_calculated_value >= 0)
                    {
                        calculation_panel.Text = "";
                        activity_view_text_setter("Calculating..@");
                        activity_view_text_setter("Available Amount = " + fineBalance + ".00 LKR@");
                        activity_view_text_setter("Paying Amount = " + entered_amount.ToString() + ".00 LKR@");
                        activity_view_text_setter("Remaining Amount = " + final_calculated_value.ToString() + ".00 LKR@");
                        activity_view_text = "";
                        paying_amount.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Invalid High Amount!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        paying_amount.Enabled = true;
                        checkBox1.Checked = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid Format Error!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    paying_amount.Enabled = true;
                    checkBox1.Checked = false;
                }
            }
            else
            {
                calculation_panel.Text = "Clear Calculations !";
                paying_amount.Enabled = true;
            }
        }

        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@", System.Environment.NewLine);
            activity_view_text += receiveText;
            calculation_panel.Text = activity_view_text;
            calculation_panel.SelectionStart = calculation_panel.Text.Length;
            calculation_panel.ScrollToCaret();
        }

        private void btn_pay_now_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (confirm_payment.Checked)
                {
                    DialogResult dialog = MessageBox.Show("Confirm Transaction?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (dialog == DialogResult.Yes)
                    {
                        bool x = false , y = false;
                        string updatecQuery = "UPDATE user_fines SET amount = @amount WHERE user_registration_number = @u_num";
                        using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                        {
                            updatec.Parameters.AddWithValue("@amount", final_calculated_value.ToString());
                            updatec.Parameters.AddWithValue("@u_num", userRegistrationNumber);
                            try
                            {
                                updatec.ExecuteNonQuery();
                                x = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        string stringQuery = "INSERT INTO school_fine_account (student_reg_number, amount, trans_status, trans_date, s_f_trans_state)" +
                                                                "VALUES(@reg_nu, @amount, @tr_status, @tr_date, 'New')";
                        using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                        {
                            SqliteCmd.Parameters.AddWithValue("@reg_nu", userRegistrationNumber); //TEXT
                            SqliteCmd.Parameters.AddWithValue("@amount", entered_amount.ToString()); //TEXT
                            SqliteCmd.Parameters.AddWithValue("@tr_status", "Paid"); //TEXT
                            SqliteCmd.Parameters.AddWithValue("@tr_date", DateTime.Now.ToLongDateString()); //TEXT
                            try
                            {
                                new ActivityLogDataFetch("Fine Calculations", "Account Updated Value : " + entered_amount.ToString() + " LKR User : " + userRegistrationNumber);
                                SqliteCmd.ExecuteNonQuery();
                                y = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if(x && y)
                        {
                            MessageBox.Show("Payment Completed !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Confirm Payment!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid Calculated Amount!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            user_fine_report view = new user_fine_report(userRegistrationNumber);
            view.ShowDialog();
        }
    }
}