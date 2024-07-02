using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SQLite;
using System.Windows.Forms;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class ConfirmWithPassword : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        SQLiteConnection conn;

        public ConfirmWithPassword()
        {
            InitializeComponent();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (password.isPassword)
            {
                password.isPassword = false;
            }
            else
            {
                password.isPassword = true;
            }
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

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shiftDown_Click(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            string q = "SELECT * FROM librarian_user WHERE librarian_id = @libID";
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@libID", data.getLoggedUserId());
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                if (password.Text.Equals(dt.Rows[0]["password"].ToString()))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Wrong Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void password_MouseClick(object sender, MouseEventArgs e)
        {
            if (password.Text.Equals("Password"))
            {
                password.Text = "";
            }
        }

        private void password_MouseLeave(object sender, EventArgs e)
        {
            if (password.Text.Equals(""))
            {
                password.Text = "Password";
            }
        }
    }
}
