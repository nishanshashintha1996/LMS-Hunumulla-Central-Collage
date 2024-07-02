using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class WebView : Form
    {
        public string x = "true";
        public string url = "";
        ChromiumWebBrowser browser;
        public WebView(string getUrl)
        {
            this.url = getUrl;
            InitializeComponent();
        }

        private void Documentation_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.WindowState = FormWindowState.Maximized;
            CefSettings settings = new CefSettings();
            try
            {
                Cef.Initialize(settings);
            }catch(Exception ex)
            {

            }
            browser = new ChromiumWebBrowser(this.url);
            this.web.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.Load(this.url);
            browser.LoadingStateChanged += ChromeView_NavStateChanged;
        }
        
        private void ChromeView_NavStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            
            if (!e.IsLoading)
            {
                this.x = "false";
            }
            else
            {
                this.x = "true";
            }
        }


        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.x == "true")
            {
                loadingWeb.Show();
            }
            else
            {
                loadingWeb.Hide();
            }
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (browser.CanGoBack)
                browser.Back();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (browser.CanGoForward)
                browser.Forward();
        }
    }
}
