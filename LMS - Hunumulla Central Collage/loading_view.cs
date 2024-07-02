using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class loading_view : Form
    {
        int counter = 0;
        public loading_view()
        {
            InitializeComponent();
        }

        private void loading_view_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter += 10;
            if (counter >= 50)
            {
                this.Close();
            }
        }
    }
}
