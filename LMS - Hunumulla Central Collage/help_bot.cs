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
    public partial class help_bot : Form
    {
        public string _ava = "";
        public string _botName;
        public string _control;
        public string[] lines;
        public int conter = 0;
        private double _opacityControl = 0.1;
        private bool mouseDown;
        private Point lastLocation;
        public help_bot(string text, string _con ,string ava)
        {
            _ava = ava;
            _control = _con;
            lines = SplitString(text, 100);
            this.BackColor = Color.DimGray;
            this.TransparencyKey = Color.DimGray;
            InitializeComponent();
        }

        private void help_bot_Load(object sender, EventArgs e)
        {
            if (_ava.Equals(""))
            {
                this.Close();
            }
            else
            {
                _botName = _ava;
                PlaceLowerRight();
                if (_control.Equals(""))
                {
                    if (_botRun.RandomNumber() == 2)
                    {
                        textLable.Text = "He he... I will help you. Dont Worry.";
                        timer2.Start();
                    }
                    else if (_botRun.RandomNumber() == 3)
                    {
                        textLable.Text = "I am back. I am here to help you. Let's do";
                        timer2.Start();
                    }
                    else if (_botRun.RandomNumber() == 4)
                    {
                        textLable.Text = "Hi there. I think you need a help. Try this.";
                        timer2.Start();
                    }
                    else if (_botRun.RandomNumber() == 5)
                    {
                        textLable.Text = "Never give up. This system is very easy to use. I will help you. ";
                        timer2.Start();
                    }
                }
                else
                {
                    textLable.Text = "Hello " + _control + ", I am " + _botName + ". Wormly Welcome to C.W.W.Kannangara Collage Library Management System";
                    timer2.Start();
                }
            }
        }

        public static string[] SplitString(string input, int lineLen)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = input.Split(' ');
            string line = string.Empty;
            string sp = string.Empty;
            foreach (string w in words)
            {
                string word = w;
                while (word != string.Empty)
                {
                    if (line == string.Empty)
                    {
                        while (word.Length >= lineLen)
                        {
                            sb.Append(word.Substring(0, lineLen) + "~");
                            word = word.Substring(lineLen);
                        }
                        if (word != string.Empty)
                            line = word;
                        word = string.Empty;
                        sp = " ";
                    }
                    else if (line.Length + word.Length <= lineLen)
                    {
                        line += sp + word;
                        sp = " ";
                        word = string.Empty;
                    }
                    else
                    {
                        sb.Append(line + "~");
                        line = string.Empty;
                        sp = string.Empty;
                    }
                }
            }
            if (line != string.Empty)
                sb.Append(line);
            return sb.ToString().Split('~');
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Start();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.Opacity > 0.0)
            {
                this.Opacity -= _opacityControl;
            }
            else
            {
                timer1.Stop();
                this.Close();
            }
        }

        private void PlaceLowerRight()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (conter != lines.Count())
            {
               conter++;
               textLable.Text = lines[conter-1];
            }
            else
            {
                timer1.Start();
            }
        }
    }
}
