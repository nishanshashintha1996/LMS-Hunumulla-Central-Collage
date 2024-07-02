using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Threading;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class uploaderView : Form
    {
        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        public static SQLiteConnection conn;
        private DataParameter _inputParameter;
        SQLiteDataAdapter da_data_fetch;
        SQLiteCommand cmd_data_fetch;
        DataTable dt_data_fetch;

        public uploaderView()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            InitializeComponent();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private void uploaderView_Load(object sender, EventArgs e)
        {
            try
            {
                string data_fetch = "SELECT * FROM library_books";
                cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (!backgroundWorker1.IsBusy)
            {
                _inputParameter.Delay = 100;
                _inputParameter.Process = dt_data_fetch.Rows.Count;
                backgroundWorker1.RunWorkerAsync(_inputParameter);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;
            try
            {
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-library-books.php?" +
                                "book_id=" + dt_data_fetch.Rows[xx]["book_id"].ToString() +
                                "&registration_number=" + dt_data_fetch.Rows[xx]["registration_number"].ToString() +
                                "&book_name=" + Base64Encode(dt_data_fetch.Rows[xx]["book_name"].ToString()) +
                                "&author=" + Base64Encode(dt_data_fetch.Rows[xx]["author"].ToString()) +
                                "&publication_date=" + dt_data_fetch.Rows[xx]["publication_date"].ToString() +
                                "&pages=" + dt_data_fetch.Rows[xx]["pages"].ToString() +
                                "&price=" + dt_data_fetch.Rows[xx]["price"].ToString() +
                                "&source=" + Base64Encode(dt_data_fetch.Rows[xx]["source"].ToString()) +
                                "&date=" + dt_data_fetch.Rows[xx]["date"].ToString() +
                                "&category=" + Base64Encode(dt_data_fetch.Rows[xx]["category"].ToString()) +
                                "&comments=" + Base64Encode(dt_data_fetch.Rows[xx]["comments"].ToString()) +
                                "&status=" + dt_data_fetch.Rows[xx]["status"].ToString()
                                );
                        }
                        if (!backgroundWorker1.CancellationPending)
                        {
                            backgroundWorker1.ReportProgress(index++ * 100 / process, string.Format("Data Base Update Processing {0}%", xx));
                            Thread.Sleep(delay);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                backgroundWorker1.CancelAsync();
                MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = string.Format("Data Base Update Processing {0}%", e.ProgressPercentage);
            progressBar1.Update();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Update Process Completed !");
            this.Close();
        }

        private void close_window_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            MessageBox.Show("Update Process Terminated !");
            this.Close();
        }

        private void minimize_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
