using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class bot_configuration : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public string path = Directory.GetCurrentDirectory() + @"\Data\_sys-cache.txt";
        public bot_configuration()
        {
            InitializeComponent();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void bot_configuration_Load(object sender, EventArgs e)
        {
            List<string> lines = File.ReadLines(path).ToList();
            foreach (string line in lines)
            {
                if (!line.Equals(""))
                {
                    active.Checked = true;
                    //Store the message in the cache:
                    GlobalCachingProvider.Instance.AddItem("botName", "Alson");
                }
                else
                {
                    deactive.Checked = true;
                    //Store the message in the cache:
                    GlobalCachingProvider.Instance.AddItem("botName", "");
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (active.Checked)
            {
                List<string> list = new List<string>();
                list.Add("Alson");
                File.WriteAllLines(path, list);

                List<string> lines = File.ReadLines(path).ToList();
                foreach (string line in lines)
                {
                    if (!line.Equals(""))
                    {
                        active.Checked = true;
                        //Store the message in the cache:
                        GlobalCachingProvider.Instance.AddItem("botName", "Alson");
                        new ActivityLogDataFetch("Bot Settings", "Bot Activated");
                    }
                    else
                    {
                        deactive.Checked = true;
                        //Store the message in the cache:
                        GlobalCachingProvider.Instance.AddItem("botName", "");
                        new ActivityLogDataFetch("Bot Settings", "Bot Activated");
                    }
                }
            }

            if (deactive.Checked)
            {
                List<string> list = new List<string>();
                list.Add("");
                File.WriteAllLines(path, list);

                List<string> lines = File.ReadLines(path).ToList();
                foreach (string line in lines)
                {
                    if (!line.Equals(""))
                    {
                        active.Checked = true;
                        //Store the message in the cache:
                        GlobalCachingProvider.Instance.AddItem("botName", "Alson");
                        new ActivityLogDataFetch("Bot Settings", "Bot Deactivated");
                    }
                    else
                    {
                        deactive.Checked = true;
                        //Store the message in the cache:
                        GlobalCachingProvider.Instance.AddItem("botName", "");
                        new ActivityLogDataFetch("Bot Settings", "Bot Deactivated");
                    }
                }
            }
            this.Close();
        }
    }
}
