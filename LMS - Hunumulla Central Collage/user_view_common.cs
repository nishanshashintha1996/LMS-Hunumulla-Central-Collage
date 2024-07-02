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
    public partial class user_view_common : Form
    {
        SQLiteConnection conn;
        private bool mouseDown;
        private Point lastLocation;
        private string user_reg_num;
        public user_view_common(string u_id)
        {
            user_reg_num = u_id;
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        // Loading Function
        private void user_view_common_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM library_user WHERE user_registration_number = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
            cmd_fill.Parameters.AddWithValue("@value", user_reg_num);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count == 1)
            {
                if (dtable.Rows[0]["user_type"].ToString().Equals("Non Academic"))
                {
                    pic_non.Visible = true;
                }
                else if (dtable.Rows[0]["user_type"].ToString().Equals("Student"))
                {
                    pic_stu.Visible = true;
                }
                else if (dtable.Rows[0]["user_type"].ToString().Equals("Teacher"))
                {
                    pic_tea.Visible = true;
                }

                reg_num.Text = dtable.Rows[0]["user_registration_number"].ToString();
                reg_num.Visible = true;
                user_name.Text = dtable.Rows[0]["user_fname"].ToString() + " " + dtable.Rows[0]["user_lname"].ToString();
                user_name.Visible = true;
                user_grade.Text = dtable.Rows[0]["user_grade"].ToString();
                user_grade.Visible = true;
                user_contact.Text = dtable.Rows[0]["contact_number"].ToString();
                user_contact.Visible = true;
                user_address.Text = dtable.Rows[0]["user_address"].ToString();
                user_address.Visible = true;
                user_web.Text = dtable.Rows[0]["user_web_id"].ToString();
                user_web.Visible = true;
                user_reg_date.Text = dtable.Rows[0]["user_registed_date"].ToString();
                user_reg_date.Visible = true;
                user_type.Text = dtable.Rows[0]["user_type"].ToString();
                user_type.Visible = true;
                user_state.Text = dtable.Rows[0]["user_state"].ToString();
                user_state.Visible = true;
                if (user_state.Text.Equals("Active"))
                {
                    user_state.BackColor = Color.Green;
                }
                else
                {
                    user_state.BackColor = Color.Red;
                }

                string query_fines = "SELECT amount FROM user_fines WHERE user_registration_number = @value";
                SQLiteCommand cmd_fill_fines = new SQLiteCommand(query_fines, conn);
                cmd_fill_fines.Parameters.AddWithValue("@value", user_reg_num);
                SQLiteDataAdapter dadapter_fines = new SQLiteDataAdapter(cmd_fill_fines);
                DataTable dtable_fines = new DataTable();
                dadapter_fines.Fill(dtable_fines);
                if (dtable_fines.Rows.Count == 1)
                {
                    available_fines.Text = dtable_fines.Rows[0]["amount"].ToString()+".00 LKR";
                    available_fines.Visible = true;
                    btn_pay_fines.Enabled = true;
                }
                else
                {
                    available_fines.Text = "No Fine Balance";
                    available_fines.Visible = true;
                }
            }
        }

        
        // Window Movement 

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

        private void user_view_common_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_pay_fines_Click(object sender, EventArgs e)
        {
            fine_calculation_view view = new fine_calculation_view(user_reg_num);
            view.ShowDialog();
            this.Close();
        }
    }
}
