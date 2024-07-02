using System;
using System.Net;
using System.Data;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    class ChartDataFetcher
    {
        private SQLiteConnection conn;
        public ChartDataFetcher()
        {
            // Constructor
        }

        private void databaseConnection()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        public string BookCounter(string value)
        {
            databaseConnection();

            string sql_query = "SELECT * FROM library_books WHERE status = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            return dtable.Rows.Count.ToString();
        }

        public string UserCounter(string value)
        {
            databaseConnection();

            string sql_query = "SELECT * FROM library_user WHERE user_type = @value AND user_state = 'Active'";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            return dtable.Rows.Count.ToString();
        }

        public string bookIssuedCount(string value)
        {
            databaseConnection();
            string sql_query = "SELECT * FROM issue_book WHERE returned_date = @value AND transaction_state = 'Returned'";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            return dtable.Rows.Count.ToString();
        }

        public string[] getIssuedReportData(bool controller, DateTime date)
        {
            string[] values = {"Monday","Tuesday","Wednesday", "Thursday", "Friday"};
            if (!controller)
            {
                int counter = 0;
                for (int x = 0; x < 7; x++)
                {
                    if (DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).DayOfWeek.ToString().Equals("Sunday") || DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).DayOfWeek.ToString().Equals("Saturday"))
                    {

                    }
                    else
                    {
                        values[counter] = bookIssuedCount(DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                        counter++;
                    }
                }
            }
            return values;
        }

        public string[] getIssuedMonthReportDays(DateTime date)
        {
            string[] values = new string[DateTime.DaysInMonth(date.Year, date.Month)];
            int counter = 0;
            string temArr = "";
            for (int x = 1; x < DateTime.DaysInMonth(date.Year, date.Month) + 1;)
            {
                values[counter] = x.ToString();
                DateTime value = new DateTime(date.Year, date.Month, Int32.Parse(values[counter]));
                if (value.DayOfWeek.ToString().Equals("Sunday") || value.DayOfWeek.ToString().Equals("Saturday"))
                {

                }
                else
                {
                    temArr += x.ToString() + ":";
                }
                counter++;
                x++;
            }
            temArr = temArr.TrimEnd(':');
            string[] words = temArr.Split(':');
            return words;
        }

        public int UserCounterByGrade(string value)
        {
            databaseConnection();
            string sql_query = "SELECT * FROM library_user WHERE user_grade = @value AND user_type = 'Student' AND user_state = 'Active'";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            return dtable.Rows.Count;
        }

        public string[] getStudentCountByGrade(bool controller)
        {
            string[] grade = { "Grade One", "Grade Two", "Grade Three", "Grade Four", "Grade Five", "Grade Six", "Grade Seven", "Grade Eight", "Grade Nine", "Grade Ten", "Grade Eleven", "Grade Twelve", "Grade Thirteen" };
            if (controller)
            {
                int counter = 1;
                for(int x=0; x<grade.Length; x++)
                {
                    grade[x] = UserCounterByGrade(counter.ToString()).ToString();
                    counter++;
                }
            }
            else
            {
                int counter = 1;
                for (int x = 0; x < grade.Length; x++)
                {
                    if (UserCounterByGrade(counter.ToString()) == 0)
                    {
                        grade[x] = "";
                    }
                    counter++;
                }
            }
            return grade;
        }

        public string[] getIssuedMonthReportData(DateTime date)
        {
            string[] values = new string[DateTime.DaysInMonth(date.Year, date.Month)];
            int counter = 0;
            string temArr = "";
            for (int x = 1; x < DateTime.DaysInMonth(date.Year, date.Month) + 1;)
            {
                values[counter] = x.ToString();
                DateTime value = new DateTime(date.Year, date.Month, Int32.Parse(values[counter]));
                if (value.DayOfWeek.ToString().Equals("Sunday") || value.DayOfWeek.ToString().Equals("Saturday"))
                {

                }
                else
                {
                    temArr += x.ToString() + ":";
                }
                counter++;
                x++;
            }
            temArr = temArr.TrimEnd(':');
            string[] words = temArr.Split(':');
            int counterTem = 0;
            string[] temArray = new string[words.Length];
            foreach (var word in words)
            {
                DateTime value = new DateTime(date.Year, date.Month, Int32.Parse(word));
                temArray[counterTem] = bookIssuedCount(value.ToLongDateString());
                counterTem++;
            }
            return temArray;
        }

    }
}
