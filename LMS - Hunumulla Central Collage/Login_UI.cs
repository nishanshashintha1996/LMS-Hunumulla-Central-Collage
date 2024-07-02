using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Net;
using System.Runtime.InteropServices;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class Login_UI : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        private string activeStateMsg = "";
        private bool mouseDown;
        private Point lastLocation;
        public Login_UI()
        {
            InitializeComponent();
            timer1.Start();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
        }

        private void txtUserEnter(object sender, EventArgs e)
        {
            if (username.Text.Equals(@"User Email / Username"))
            {
                username.Text = "";
            }
        }

        private void txtUserLeave(object sender, EventArgs e)
        {
            if (username.Text.Equals(""))
            {
                username.Text = @"User Email / Username";
            }
        }

        private void txtPasswordEnter(object sender, EventArgs e)
        {
            if (password.Text.Equals("Password"))
            {
                password.isPassword = true;
                password.Text = "";
            }
        }

        private void txtPasswordLeave(object sender, EventArgs e)
        {
            if (password.Text.Equals(""))
            {
                password.Text = "Password";
                password.isPassword = false;
            }
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "Loading...";
            if (username.Text.Trim() != "" && password.Text.Trim() != "" && username.Text.Trim() != @"User Email" && password.Text.Trim() != "Password")
            {

                int Desc;
                if (InternetGetConnectedState(out Desc, 0))
                {
                    using (WebClient client = new WebClient())
                    {
                        string src = "";
                        try
                        {
                            src = client.DownloadString("https://api.elitesolution.us/auth/api/auth.php?"
                            + "user=" + Base64Encode(username.Text)
                            + "&pass=" + Base64Encode(password.Text));
                        }catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        int n;
                        bool isNumeric = int.TryParse(src, out n);
                        if (isNumeric)
                        {
                            this.Hide();
                            Dashboard dashboard = new Dashboard(src, password.Text, pActive.Text);
                            PopupNotifier popup = new PopupNotifier();
                            popup.Image = Properties.Resources.Information;
                            popup.TitleText = "LMS - Hunumulla Central Collage";
                            popup.ContentText = "The user successfully loged to the LMS system.";
                            popup.Delay = 5000;
                            popup.Popup();
                            new ActivityLogDataFetch("Login", "Successfully Legged In to the System");
                            button1.Enabled = true;
                            button1.Text = "LOGIN";
                            dashboard.ShowDialog();
                        }
                        else
                        {
                            button1.Enabled = true;
                            button1.Text = "LOGIN";
                            MessageBox.Show(src, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    button1.Enabled = true;
                    button1.Text = "LOGIN";
                    pActive.Text = "Offline Mode";
                    pActive.Visible = true; ;
                    DialogResult dialogResult = MessageBox.Show("Do you want to login as offline mode", "Alert", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        checkDisconnectedLogin(username.Text, password.Text);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Please try again");
                    }
                }
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "LOGIN";
                MessageBox.Show("Please Enter Your Username and Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkDisconnectedLogin(string username, string password)
        {
            try
            {
                string q = "SELECT * FROM librarian_user WHERE librarian_username = @user OR librarian_email = @user";
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@user", username);
                //cmd.Parameters.AddWithValue("@pass", password.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (password.Equals(dt.Rows[0]["password"].ToString()))
                    {
                        string librarian_id = "";
                        DataRow[] dr = dt.Select("");
                        foreach (DataRow row in dr)
                        {
                            librarian_id = row["librarian_id"].ToString();
                        }
                        this.Hide();
                        Dashboard dashboard = new Dashboard(librarian_id, password, "Offline Mode");
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.Information;
                        popup.TitleText = "LMS - Hunumulla Central Collage";
                        popup.ContentText = "The user successfully loged to the LMS system.";
                        popup.Delay = 5000;
                        popup.Popup();
                        new ActivityLogDataFetch("Login", "Successfully Legged In to the System");
                        dashboard.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Password is Wrong", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Email or Username is Wrong", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void close_window_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you want exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk); 
            if (dialog == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void fogot_password_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://lms.fosl.us/password-reset");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
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
        

        private void password_OnValueChanged(object sender, EventArgs e)
        {

        }

        // doc
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=doc");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        
        // system
        private void btn_web_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=system");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                internetStatus.Text = "";
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        string src = client.DownloadString("https://api.elitesolution.us/auth/api/activate.php?device="+ Base64Encode(SystemInformation.ComputerName));
                        
                        if(activeStateMsg != src)
                        {
                            if (src == "Hdbdasbk")
                            {
                                timer1.Stop();
                                DialogResult dialogResult = MessageBox.Show("Product Not Activated. Do you want to upgrade it?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    button1.Enabled = false;
                                    fogot_password.Enabled = false;
                                    btn_web.Enabled = false;
                                    bunifuImageButton2.Enabled = false;
                                    activeStateMsg = src;
                                    this.Close();
                                    MessageBox.Show("link to web");
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    button1.Enabled = false;
                                    fogot_password.Enabled = false;
                                    btn_web.Enabled = false;
                                    bunifuImageButton2.Enabled = false;
                                    activeStateMsg = src;
                                    this.Close();
                                }
                            }
                            else
                            {
                                if(src== "Trial")
                                {
                                    pActive.BackColor = Color.Red;
                                    tryPro.Visible = true;
                                }
                                else
                                {
                                    pActive.BackColor = Color.Green;
                                    tryPro.Visible = false;
                                }
                                pActive.Visible = true;
                                pActive.Text = src;
                                button1.Enabled = true;
                                fogot_password.Enabled = true;
                                btn_web.Enabled = true;
                                bunifuImageButton2.Enabled = true;
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        timer1.Stop();
                    }
                }
            }
            else
            {
                if(internetStatus.Text == "")
                {
                    internetStatus.Text = "Internet Connection Not Available";
                    pActive.Text = "Offline Mode";
                    pActive.Visible = true;
                }
            }
        }

        private void Login_UI_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
