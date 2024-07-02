using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class ApplicationSettings : Form
    {
        SQLiteConnection conn;
        private bool mouseDown;
        private Point lastLocation;

        public ApplicationSettings()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
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

        private void ormail_Click(object sender, EventArgs e)
        {
            String mailBody = "";
            String receiver = "elitegroupofficialmail@gmail.com";
            String Bcc = "";
            String Cc = "shashinthanishan00@gmail.com";
            String Subject = "Customer Support | LMS";
            System.Diagnostics.Process.Start("mailto: " + receiver + " ? cc = " + Cc + " & bcc = " + Bcc + " & subject = " + Subject + " & body = " + mailBody);
        }

        private void auemail_Click(object sender, EventArgs e)
        {
            String mailBody = "";
            String receiver = "shashinthanishan00@gmail.com";
            String Bcc = "";
            String Cc = "elitegroupofficialmail@gmail.com";
            String Subject = "Developer Support | LMS";
            System.Diagnostics.Process.Start("mailto: " + receiver + " ? cc = " + Cc + " & bcc = " + Bcc + " & subject = " + Subject + " & body = " + mailBody);
        }

        private void orweb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://elitesolution.us/");
        }

        private void auweb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://nishans.me/");
        }

        private void savaData_Click(object sender, EventArgs e)
        {
            string updatecQuery = "UPDATE system_data SET system_name = @sysName, system_or_contact = @sysContact, system_or_email = @sysEmail WHERE system_id = 1";
            using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
            {
                updatec.Parameters.AddWithValue("@sysName", school.Text);
                updatec.Parameters.AddWithValue("@sysContact", contact.Text);
                updatec.Parameters.AddWithValue("@sysEmail", email.Text);
                updatec.ExecuteNonQuery();
                new ActivityLogDataFetch("Application Settings", "Configured Application Data");
                MessageBox.Show("Done!");
            }
        }

        private void ApplicationSettings_Load(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            school.Text = data.getInstitiuteName();
            contact.Text = data.getInstitiuteContact();
            email.Text = data.getInstitiuteEmail();
        }
        
    }
}
