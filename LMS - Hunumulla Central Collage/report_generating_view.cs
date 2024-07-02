using DGVPrinterHelper;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class report_generating_view : Form
    {
        ReportViewTableGenerator gen = new ReportViewTableGenerator();
        SQLiteConnection conn;
        private bool mouseDown;
        private Point lastLocation;
        public report_generating_view()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void report_generating_view_Load(object sender, EventArgs e)
        {
            category.Visible = false;
            category_view.Visible = false;
        }

        
        

        private string GetSubtitle(string firstText, string secoundText, string thirdText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("".PadRight(88, '_'));
            sb.AppendLine();
            sb.AppendLine(string.Format("{0,-10} | {1,-10} | {2,5}", firstText, secoundText, thirdText));
            sb.AppendLine("".PadRight(88, '_'));
            sb.AppendLine();
            return sb.ToString();
        }

        private void ToCsV(DataGridView dGV, string filename)
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        private void printingFunction(string title, string subtitleContent01, string subtitleContent02, string subtitleContent03, string doc)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = title;
            printer.SubTitle = GetSubtitle(subtitleContent01, subtitleContent02, subtitleContent03);
            printer.DocName = doc;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Hunumulla Central Collage LMS               " + DateTime.Now;
            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView1);

        }

        // Print Button Clicked -------------------------------------------------------------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            if (activeBooks.Checked)
            {
                new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Available Books");
                printingFunction("System Available Books Report of The Library", "Time : " +
                    date_time.Value.ToLongTimeString(), "Date : " +
                    date_time.Value.ToLongDateString(), "Available Books Report", "Available Books Report");
            }

            if (deletedBooks.Checked)
            {
                new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Removed Books");
                printingFunction("System Removed Books Report of The Library", "Time : " +
                    date_time.Value.ToLongTimeString(), "Date : " +
                    date_time.Value.ToLongDateString(), "Removed Books Report", "Removed Books Report");
            }

            if (activeUsers.Checked)
            {
                new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Available Users");
                printingFunction("System Available Users Report of The Library", "Time : " +
                    date_time.Value.ToLongTimeString(), "Date : " +
                    date_time.Value.ToLongDateString(), "Available Users Report", "Available Users Report");
            }

            if (removedUsers.Checked)
            {
                new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Removed Users");
                printingFunction("System Removed Users Report of The Library", "Time : " +
                    date_time.Value.ToLongTimeString(), "Date : " +
                    date_time.Value.ToLongDateString(), "Removed Users Report", "Removed Users Report");
            }

            if (daily_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Daily Book Issued Report");
                    printingFunction("Daily Book Transaction Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Book Issued Report", "Daily Book Transaction Report");
                }

                if (ReceivedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Daily Book Received Report");
                    printingFunction("Daily Book Transaction Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Book Received Report", "Daily Book Transaction Report");
                }
                if (AddedUserReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added User Report / Daily");
                    printingFunction("Daily Added User Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Newly Added User Report", "Daily Added User Report");
                }

                if (AddedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Book Report / Daily");
                    printingFunction("Daily Added Book Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Newly Added Book Report", "Daily Added Book Report");
                }

                if (AddedCategoryReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Category Report / Daily");
                    printingFunction("Daily Added Category Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Newly Added Category Report", "Daily Added Category Report");
                }

                if (FineAccountReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Fine Account Transactions Report / Daily");
                    printingFunction("Daily Fine Account Transactions Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Fine Account Transactions Report", "Daily Fine Account Transactions Report");
                }

                if (LibrarianLog.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Librarian Activity Log / Daily");
                    printingFunction("Daily Librarian Activity Log Report of The Library", "Time : " +
                        date_time.Value.ToLongTimeString(), "Date : " +
                        date_time.Value.ToLongDateString(), "Librarian Activity Log", "Daily Librarian Activity Log Report");
                }
            }

            if (week_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Weekly Book Issued Report");
                    printingFunction("Weekly Book Transaction Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Book Issued Report", "Weekly Book Transaction Report");
                }

                if (ReceivedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Weekly Book Received Report");
                    printingFunction("Weekly Book Transaction Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Book Received Report", "Weekly Book Transaction Report");
                }

                if (AddedUserReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added User Report / Weekly");
                    printingFunction("Weekly Added User Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Newly Added User Report", "Weekly Added User Report");
                }

                if (AddedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Book Report / Weekly");
                    printingFunction("Weekly Added Book Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Newly Added Book Report", "Weekly Added Book Report");
                }

                if (AddedCategoryReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Category Report / Weekly");
                    printingFunction("Weekly Added Category Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Newly Added Category Report", "Weekly Added Category Report");
                }

                if (FineAccountReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Fine Account Transactions Report / Weekly");
                    printingFunction("Weekly Fine Account Transactions Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Fine Account Transactions Report ", "Weekly Fine Account Transactions Report");
                }

                if (LibrarianLog.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Librarian Activity Log Report / Weekly");
                    printingFunction("Weekly Librarian Activity Log Report of The Library", "Week Start Date : " +
                        DateTimeCalculatoin.getWeekStartDate(date_time.Value).ToLongDateString(), "Week End Date : " +
                        DateTimeCalculatoin.getWeekEndDate(date_time.Value).ToLongDateString(), "Librarian Activity Log Report ", "Librarian Activity Log Report");
                }
                
            }

            if (month_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Monthly Book Issued Report");
                    printingFunction("Monthly Book Transaction Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Book Issued Report", "Monthly Book Transaction Report");
                }

                if (ReceivedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Monthly Book Received Report");
                    printingFunction("Monthly Book Transaction Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Book Received Report", "Monthly Book Transaction Report");
                }

                if (AddedUserReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added User Report / Monthly");
                    printingFunction("Monthly Added User Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Newly Added User Report", "Monthly Added User Report");
                }

                if (AddedBookReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Book Report / Monthly");
                    printingFunction("Monthly Added Book Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Newly Added Book Report", "Monthly Added Book Report");
                }

                if (AddedCategoryReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Newly Added Category Report / Monthly");
                    printingFunction("Monthly Added Category Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Newly Added Category Report", "Monthly Added Category Report");
                }

                if (FineAccountReport.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Fine Account Transactions Report / Monthly");
                    printingFunction("Monthly Fine Account Transactions Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Fine Account Transactions Report", "Monthly Fine Account Transactions Report");
                }

                if (LibrarianLog.Checked)
                {
                    new ActivityLogDataFetch("Report Generating", "Successfully Generated Report : Librarian Activity Log Report / Monthly");
                    printingFunction("Monthly Librarian Activity Log Report of The Library", "Month Start Date : " +
                        DateTimeCalculatoin.getMonthStart(date_time.Value).ToLongDateString(), "Month End Date : " +
                        DateTimeCalculatoin.getMonthEnd(date_time.Value).ToLongDateString(), "Librarian Activity Log Report", "Monthly Librarian Activity Log Report");
                }
            }
        }





        //Window Movemet

        private void issue_book_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void issue_book_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void issue_book_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        

        // Report Buttons

        private void IssueBookReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportIssuedCommon(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportIssuedCommon(Convert.ToDateTime(date_time.Text),"week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (IssueBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportIssuedCommon(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void ReceivedBookReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (ReceivedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportReceivedCommon(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (ReceivedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportReceivedCommon(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (ReceivedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.bookTransactionReportReceivedCommon(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void AddedBookReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (AddedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedBooksReport(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (AddedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedBooksReport(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (AddedBookReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedBooksReport(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void AddedCategoryReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (AddedCategoryReport.Checked)
                {
                    dataGridView1.DataSource = gen.categoryReport(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (AddedCategoryReport.Checked)
                {
                    dataGridView1.DataSource = gen.categoryReport(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (AddedCategoryReport.Checked)
                {
                    dataGridView1.DataSource = gen.categoryReport(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void FineAccountReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (FineAccountReport.Checked)
                {
                    dataGridView1.DataSource = gen.schoolFineAccountReport(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (FineAccountReport.Checked)
                {
                    dataGridView1.DataSource = gen.schoolFineAccountReport(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (FineAccountReport.Checked)
                {
                    dataGridView1.DataSource = gen.schoolFineAccountReport(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void AddedUserReport_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (AddedUserReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedUserReport(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (AddedUserReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedUserReport(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (AddedUserReport.Checked)
                {
                    dataGridView1.DataSource = gen.addedUserReport(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }

        private void LibrarianLog_CheckedChanged(object sender, EventArgs e)
        {
            ClearLeft();
            if (daily_report.Checked)
            {
                if (LibrarianLog.Checked)
                {
                    dataGridView1.DataSource = gen.LibrarianLog(Convert.ToDateTime(date_time.Text), "day");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (week_report.Checked)
            {
                if (LibrarianLog.Checked)
                {
                    dataGridView1.DataSource = gen.LibrarianLog(Convert.ToDateTime(date_time.Text), "week");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }

            if (month_report.Checked)
            {
                if (LibrarianLog.Checked)
                {
                    dataGridView1.DataSource = gen.LibrarianLog(Convert.ToDateTime(date_time.Text), "month");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
        }


        // Function buttons
        private void maximize_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void minimize_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            daily_report.Checked = false;
            month_report.Checked = false;
            week_report.Checked = false;

            IssueBookReport.Checked = false;
            ReceivedBookReport.Checked = false;
            AddedBookReport.Checked = false;
            AddedUserReport.Checked = false;
            AddedCategoryReport.Checked = false;
            FineAccountReport.Checked = false;
            LibrarianLog.Checked = false;
        }

        private void daily_report_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            IssueBookReport.Checked = false;
            ReceivedBookReport.Checked = false;
            AddedBookReport.Checked = false;
            AddedUserReport.Checked = false;
            AddedCategoryReport.Checked = false;
            FineAccountReport.Checked = false;
            LibrarianLog.Checked = false;
        }

        private void week_report_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            IssueBookReport.Checked = false;
            ReceivedBookReport.Checked = false;
            AddedBookReport.Checked = false;
            AddedUserReport.Checked = false;
            AddedCategoryReport.Checked = false;
            FineAccountReport.Checked = false;
            LibrarianLog.Checked = false;
        }

        private void month_report_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            IssueBookReport.Checked = false;
            ReceivedBookReport.Checked = false;
            AddedBookReport.Checked = false;
            AddedUserReport.Checked = false;
            AddedCategoryReport.Checked = false;
            FineAccountReport.Checked = false;
            LibrarianLog.Checked = false;
        }

        private void date_time_ValueChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = gen.AvailableBooks();
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = gen.NotAvailableBooks();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = gen.AvUsers();
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.DataSource = gen.NotAvUsers();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                category.Visible = true;
            }
            else
            {
                Clear();
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void ClearLeft()
        {
            activeBooks.Checked = false;
            deletedBooks.Checked = false;
            activeUsers.Checked = false;
            removedUsers.Checked = false;
            radioButton9.Checked = false;
        }

        private void category_OnValueChanged(object sender, EventArgs e)
        {
            category_view.Visible = true;

            if (!category.Equals(""))
            {
                string q = "SELECT cat_name FROM book_category WHERE cat_name LIKE @category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", "%" + category.Text + "%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                category_view.DataSource = dt;
            }
            else
            {
                string q = "SELECT cat_name FROM book_category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                category_view.DataSource = dt;
            }
        }


        private void category_Leave(object sender, EventArgs e)
        {
            string q = "SELECT cat_name FROM book_category";
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@category", category.Text);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                category_view.Hide();
            }
        }

        private void category_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            category.Text = category_view.Rows[e.RowIndex].Cells["cat_name"].FormattedValue.ToString();
            category_view.Hide();
            dataGridView1.DataSource = gen.bookReportByCategory(category.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "export.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ToCsV(dataGridView1, sfd.FileName);
            }
        }
    }
}
