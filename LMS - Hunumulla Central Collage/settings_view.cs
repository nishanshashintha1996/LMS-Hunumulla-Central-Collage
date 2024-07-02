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
    public partial class settings_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public settings_view()
        {
            InitializeComponent();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_student_Click(object sender, EventArgs e)
        {
            category_setteings_view view = new category_setteings_view();
            view.ShowDialog();
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

        private void configure_login_Click(object sender, EventArgs e)
        {
            configure_librarian_data view = new configure_librarian_data();
            view.ShowDialog();
        }

        private void bot_settings_Click(object sender, EventArgs e)
        {
            bot_configuration bot = new bot_configuration();
            bot.ShowDialog();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            ApplicationSettings view = new ApplicationSettings();
            view.ShowDialog();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            DataConfiguration view = new DataConfiguration();
            view.ShowDialog();
        }
    }
}
