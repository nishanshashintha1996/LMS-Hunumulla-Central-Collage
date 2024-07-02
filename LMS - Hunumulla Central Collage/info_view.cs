using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class info_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public info_view()
        {
            InitializeComponent();
        }

        private void _close_Click(object sender, EventArgs e)
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

        private void info_view_Load(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            label5.Text = data.getInstitiuteName();
        }
    }
}
