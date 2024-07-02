using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DGVPrinterHelper;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class DailyReport : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public DailyReport()
        {
            InitializeComponent();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            new ActivityLogDataFetch("Daily Activity Log", "Report Generated and Printed / Saved");
            printingFunction("Daily Librarian Activity Log", "Time : " +
                        DateTime.Now.ToLongTimeString(), "Date : " +
                        DateTime.Now.ToLongDateString(), "Activity Log Report", DateTime.Now.ToLongDateString() + " Librarian Activity Log Report");
        }

        private void DailyReport_Load(object sender, EventArgs e)
        {
            ActivityLogDataFetch a = new ActivityLogDataFetch();
            dataGridView1.DataSource = a.GetDayLog();
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

    }
}
