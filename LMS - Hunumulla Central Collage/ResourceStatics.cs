using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using DGVPrinterHelper;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class ResourceStatics : Form
    {
        private SQLiteConnection conn;
        ChartDataFetcher gen = new ChartDataFetcher();
        private bool mouseDown;
        private Point lastLocation;
        public ResourceStatics()
        {
            InitializeComponent();
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void ResourceStatics_Load(object sender, EventArgs e)
        {
            userCountGrid.Hide();
            string[] namesUsers = { "Student", "Teacher", "Non Academic" };
            string[] valuesUsers = { gen.UserCounter("Student"), gen.UserCounter("Teacher"), gen.UserCounter("Non Academic") };
            fillChart("UserCount", namesUsers, valuesUsers, "", userCount, SeriesChartType.Pie);

            bookCountGrid.Hide();
            string[] namesBooks = { "Available Books", "Removed Books"};
            string[] valuesBooks = { gen.BookCounter("Active"), gen.BookCounter("Deleted") };
            fillChart("BookCount", namesBooks, valuesBooks, "", bookChart, SeriesChartType.Pie);

            fillChart("Transaction Data", gen.getIssuedReportData(true, dateTimePicker1.Value), gen.getIssuedReportData(false, dateTimePicker1.Value), "", transData, SeriesChartType.Bar);
            label3.Text = "Week Transaction Summary";
            setChartItems(dropdown);

            fillChart("Student Count", gen.getStudentCountByGrade(false), gen.getStudentCountByGrade(true), "", chart1, SeriesChartType.Pie);

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

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        



        private void btn_info_Click(object sender, EventArgs e)
        {
            userCountGrid.Show();
            string[] names = { "Student", "Teacher", "Non Academic" };
            string[] values = { gen.UserCounter("Student"), gen.UserCounter("Teacher"), gen.UserCounter("Non Academic") };

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(names[0]);
            dt.Columns.Add(names[1]);
            dt.Columns.Add(names[2]);
            DataRow _ravi = dt.NewRow();
            _ravi[names[0]] = values[0];
            _ravi[names[1]] = values[1];
            _ravi[names[2]] = values[2];
            dt.Rows.Add(_ravi);

            userCountGrid.DataSource = dt;
            closeGrid.Show();
            btn_info.Hide();
        }

        private void fillChart(string lable, string[] names, string[] values, string title, Chart a, SeriesChartType x)
        {
            a.Titles.Clear();
            a.Series.Clear();
            a.Titles.Add(title);
            a.Series.Add(lable);
            a.Series[0].ChartType = x;
            if (names.Length.ToString().Equals(values.Length.ToString()))
            {
                for (int i = 0; i < names.Length; i++)
                {
                    a.Series[lable].Points.AddXY(names[i], values[i]);
                }
            }
            foreach (DataPoint p in a.Series[0].Points)
            {
                p.Label = "#PERCENT\n#VALX";
            }
        }

        private void closeGrid_Click(object sender, EventArgs e)
        {
            closeGrid.Hide();
            userCountGrid.Hide();
            btn_info.Show();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            bookClose.Hide();
            bookCountGrid.Hide();
            bookInfo.Show();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            bookCountGrid.Show();
            string[] namesBooks = { "Available Books", "Removed Books" };
            string[] valuesBooks = { gen.BookCounter("Active"), gen.BookCounter("Deleted") };

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(namesBooks[0]);
            dt.Columns.Add(namesBooks[1]);

            DataRow _ravi = dt.NewRow();
            _ravi[namesBooks[0]] = valuesBooks[0];
            _ravi[namesBooks[1]] = valuesBooks[1];
            dt.Rows.Add(_ravi);

            bookCountGrid.DataSource = dt;
            bookClose.Show();
            bookInfo.Hide();
        }

        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            bookChart.Printing.PrintPreview();
        }

        private void bunifuImageButton2_Click_1(object sender, EventArgs e)
        {
            userCount.Printing.PrintPreview();
        }
        
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            transData.Printing.PrintPreview();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            chart1.Printing.PrintPreview();
        }
        

        private void month_CheckedChanged_1(object sender, EventArgs e)
        {
            fillChart("Transaction Data", gen.getIssuedMonthReportDays(dateTimePicker1.Value), gen.getIssuedMonthReportData(dateTimePicker1.Value), "", transData, SeriesChartType.Bar);
            label3.Text = "Month Transaction Summary";
        }

        private void week_CheckedChanged(object sender, EventArgs e)
        {
            fillChart("Transaction Data", gen.getIssuedReportData(true, dateTimePicker1.Value), gen.getIssuedReportData(false, dateTimePicker1.Value), "", transData, SeriesChartType.Bar);
            label3.Text = "Week Transaction Summary";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (month.Checked)
            {
                fillChart("Transaction Data", gen.getIssuedMonthReportDays(dateTimePicker1.Value), gen.getIssuedMonthReportData(dateTimePicker1.Value), "", transData, SeriesChartType.Bar);
                label3.Text = "Month Transaction Summary";
            }

            if (week.Checked)
            {
                fillChart("Transaction Data", gen.getIssuedReportData(true, dateTimePicker1.Value), gen.getIssuedReportData(false, dateTimePicker1.Value), "", transData, SeriesChartType.Bar);
                label3.Text = "Week Transaction Summary";
            }

        }

        private void dropdown_onItemSelected(object sender, EventArgs e)
        {
            setChartType(transData,dropdown);
        }

        private void setChartItems(Bunifu.Framework.UI.BunifuDropdown x)
        {
            x.AddItem("Area Chart");
            x.AddItem("Bar Chart");
            x.AddItem("Box Plot Chart");
            x.AddItem("Bubble Chart");
            x.AddItem("Candlestick Chart");
            x.AddItem("Column Chart");
            x.AddItem("Doughnut Chart");
            x.AddItem("Error Bar Chart");
            x.AddItem("Fast Line Chart");
            x.AddItem("Fast point Chart");
            x.AddItem("Funnel Chart");
            x.AddItem("Kagi Chart");
            x.AddItem("Line Chart");
            x.AddItem("Pie Chart");
            x.AddItem("Point Chart");
            x.AddItem("Point & Figure Chart");
            x.AddItem("Polar Chart");
            x.AddItem("Pyramid Chart");
            x.AddItem("Radar Chart");
            x.AddItem("Range Chart");
            x.AddItem("Range Bar Chart");
            x.AddItem("Range Column Chart");
            x.AddItem("Renko Chart");
            x.AddItem("Spline Chart");
            x.AddItem("Spline Area Chart");
            x.AddItem("Spline Range Chart");
            x.AddItem("Stacked Area Chart");
            x.AddItem("Stacked Area 100 Chart");
            x.AddItem("Stacked Bar Chart");
            x.AddItem("Stacked Bar 100 Chart");
            x.AddItem("Stacked Column Chart");
            x.AddItem("Stacked Column 100 Chart");
            x.AddItem("Step Line Chart");
            x.AddItem("Stock Chart");
            x.AddItem("Three Line Break Chart");
            x.selectedIndex = 0;
        }

        private void setChartType(Chart c, Bunifu.Framework.UI.BunifuDropdown x)
        {
            if (x.selectedIndex == 0)
            {
                c.Series[0].ChartType = SeriesChartType.Area;
            }
            if (x.selectedIndex == 1)
            {
                c.Series[0].ChartType = SeriesChartType.Bar;
            }
            if (x.selectedIndex == 2)
            {
                c.Series[0].ChartType = SeriesChartType.BoxPlot;
            }
            if (x.selectedIndex == 3)
            {
                c.Series[0].ChartType = SeriesChartType.Bubble;
            }
            if (x.selectedIndex == 4)
            {
                c.Series[0].ChartType = SeriesChartType.Candlestick;
            }
            if (x.selectedIndex == 5)
            {
                c.Series[0].ChartType = SeriesChartType.Column;
            }
            if (x.selectedIndex == 6)
            {
                c.Series[0].ChartType = SeriesChartType.Doughnut;
            }
            if (x.selectedIndex == 7)
            {
                c.Series[0].ChartType = SeriesChartType.ErrorBar;
            }
            if (x.selectedIndex == 8)
            {
                c.Series[0].ChartType = SeriesChartType.FastLine;
            }
            if (x.selectedIndex == 9)
            {
                c.Series[0].ChartType = SeriesChartType.FastPoint;
            }
            if (x.selectedIndex == 10)
            {
                c.Series[0].ChartType = SeriesChartType.Funnel;
            }
            if (x.selectedIndex == 11)
            {
                c.Series[0].ChartType = SeriesChartType.Kagi;
            }
            if (x.selectedIndex == 12)
            {
                c.Series[0].ChartType = SeriesChartType.Line;
            }
            if (x.selectedIndex == 13)
            {
                c.Series[0].ChartType = SeriesChartType.Pie;
            }
            if (x.selectedIndex == 14)
            {
                c.Series[0].ChartType = SeriesChartType.Point;
            }
            if (x.selectedIndex == 15)
            {
                c.Series[0].ChartType = SeriesChartType.PointAndFigure;
            }
            if (x.selectedIndex == 16)
            {
                c.Series[0].ChartType = SeriesChartType.Polar;
            }
            if (x.selectedIndex == 17)
            {
                c.Series[0].ChartType = SeriesChartType.Pyramid;
            }
            if (x.selectedIndex == 18)
            {
                c.Series[0].ChartType = SeriesChartType.Radar;
            }
            if (x.selectedIndex == 19)
            {
                c.Series[0].ChartType = SeriesChartType.Range;
            }
            if (x.selectedIndex == 20)
            {
                c.Series[0].ChartType = SeriesChartType.RangeBar;
            }
            if (x.selectedIndex == 21)
            {
                c.Series[0].ChartType = SeriesChartType.RangeColumn;
            }
            if (x.selectedIndex == 22)
            {
                c.Series[0].ChartType = SeriesChartType.Renko;
            }
            if (x.selectedIndex == 23)
            {
                c.Series[0].ChartType = SeriesChartType.Spline;
            }
            if (x.selectedIndex == 24)
            {
                c.Series[0].ChartType = SeriesChartType.SplineArea;
            }
            if (x.selectedIndex == 25)
            {
                c.Series[0].ChartType = SeriesChartType.SplineRange;
            }
            if (x.selectedIndex == 26)
            {
                c.Series[0].ChartType = SeriesChartType.StackedArea;
            }
            if (x.selectedIndex == 27)
            {
                c.Series[0].ChartType = SeriesChartType.StackedArea100;
            }
            if (x.selectedIndex == 28)
            {
                c.Series[0].ChartType = SeriesChartType.StackedBar;
            }
            if (x.selectedIndex == 29)
            {
                c.Series[0].ChartType = SeriesChartType.StackedBar100;
            }
            if (x.selectedIndex == 30)
            {
                c.Series[0].ChartType = SeriesChartType.StackedColumn;
            }
            if (x.selectedIndex == 31)
            {
                c.Series[0].ChartType = SeriesChartType.StackedColumn100;
            }
            if (x.selectedIndex == 32)
            {
                c.Series[0].ChartType = SeriesChartType.StepLine;
            }
            if (x.selectedIndex == 33)
            {
                c.Series[0].ChartType = SeriesChartType.Stock;
            }
            if (x.selectedIndex == 34)
            {
                c.Series[0].ChartType = SeriesChartType.ThreeLineBreak;
            }
        }
    }
}
