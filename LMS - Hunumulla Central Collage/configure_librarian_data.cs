using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class configure_librarian_data : Form
    {
        public string activity_view_text = "";
        private bool mouseDown;
        private Point lastLocation;
        public configure_librarian_data()
        {
            InitializeComponent();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void update_cerdentials_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            activity_view_text_setter(updateBackup.updateTablesFromCloud());
            new ActivityLogDataFetch("Librarian Credentials", "Credentials Updated");
            Cursor.Current = Cursors.Default;
        }

        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@", System.Environment.NewLine);
            activity_view_text += receiveText;
            terminal.Text = activity_view_text;
            terminal.SelectionStart = terminal.Text.Length;
            terminal.ScrollToCaret();
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
        private void user_view_common_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void configure_librarian_data_Load(object sender, EventArgs e)
        {
            activity_view_text_setter("Hunumulla Central Collage LMS.@Version 1.0 @");
        }

        private void request_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://google.com");
        }
    }
}
