using System;
using System.Data;
using System.Data.SQLite;
using System.Net;

namespace LMS___Hunumulla_Central_Collage
{
    class ReportViewTableGenerator
    {
        private SQLiteConnection conn;

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

        public DataTable NotAvUsers()
        {
            databaseConnection();
            string data_fetch = "SELECT user_registration_number AS පරිශීලක_ලියාපදිංචි_අංකය,user_fname AS පළමු_නම,user_lname AS දෙවන_නම,user_grade AS ශ්‍රේණිය,user_address AS ලිපිනය,contact_number AS දුරකථන_අංකය,user_type AS තත්වය FROM library_user WHERE user_state = @statusTemp ORDER BY user_id";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@statusTemp", "DeActive");
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            return dt_data_fetch;
        }

        public DataTable AvUsers()
        {
            databaseConnection();
            string data_fetch = "SELECT user_registration_number AS පරිශීලක_ලියාපදිංචි_අංකය,user_fname AS පළමු_නම,user_lname AS දෙවන_නම,user_grade AS ශ්‍රේණිය,user_address AS ලිපිනය,contact_number AS දුරකථන_අංකය,user_type AS තත්වය FROM library_user WHERE user_state = @statusTemp ORDER BY user_id";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@statusTemp", "Active");
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            return dt_data_fetch;
        }

        public DataTable NotAvailableBooks()
        {
            databaseConnection();
            string data_fetch = "SELECT registration_number AS ⁣පොත්_අංකය,book_name AS පොතේ_නම,author AS කතෘ,publication_date AS ප්‍රකාශිත_දිනය,pages AS පිටු_ගණන,price AS මිල,source AS මූලාශ්‍රය,category AS ප්‍රවර්ගය,comments AS වෙනත් FROM library_books WHERE status = @statusTemp ORDER BY book_id";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@statusTemp", "Deleted");
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            return dt_data_fetch;
        }

        public DataTable AvailableBooks()
        {
            databaseConnection();
            string data_fetch = "SELECT registration_number AS ⁣පොත්_අංකය,book_name AS පොතේ_නම,author AS කතෘ,publication_date AS ප්‍රකාශිත_දිනය,pages AS පිටු_ගණන,price AS මිල,source AS මූලාශ්‍රය,category AS ප්‍රවර්ගය,comments AS වෙනත් FROM library_books WHERE status = @statusTemp ORDER BY book_id";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@statusTemp", "Active");
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            return dt_data_fetch;
        }

        // Librarian Activity Log
        public DataTable LibrarianLog(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            string[] coloumnNames = { "අනු අංකය", "ක්‍රියාවලි විස්තරය", "වේලාව", "වෙනත් ඇමුණුම්", "ක්‍රියාව සිදුකළ පරිශීලකයා", "දිනය"};
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT log_process,log_time,comment,active_user,log_date FROM daily_activity_log WHERE log_date = @value";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row[coloumnNames[0]] = count;
                        row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["log_process"].ToString();
                        row[coloumnNames[2]] = dt_data_fetch.Rows[xx]["log_time"].ToString();
                        row[coloumnNames[3]] = dt_data_fetch.Rows[xx]["comment"].ToString();
                        row[coloumnNames[4]] = dt_data_fetch.Rows[xx]["active_user"].ToString();
                        row[coloumnNames[5]] = dt_data_fetch.Rows[xx]["log_date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }


        //Added User Report - Added User
        public DataTable addedUserReport(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            string[] coloumnNames = { "අනු අංකය", "පරිශීලක අංකය", "පරිශීලකයාගේ නම", "ශ්‍රේණිය", "ලිපිනය", "දුරකතන අංකය", "පරිශීලක වර්ගය", "දිනය" };
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM library_user WHERE user_registed_date = @value AND user_state = 'Active'";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row[coloumnNames[0]] = count;
                        row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["user_registration_number"].ToString();
                        row[coloumnNames[2]] = dt_data_fetch.Rows[xx]["user_fname"].ToString() + " " + dt_data_fetch.Rows[xx]["user_lname"].ToString();
                        row[coloumnNames[3]] = dt_data_fetch.Rows[xx]["user_grade"].ToString();
                        row[coloumnNames[4]] = dt_data_fetch.Rows[xx]["user_address"].ToString();
                        row[coloumnNames[5]] = dt_data_fetch.Rows[xx]["contact_number"].ToString();
                        row[coloumnNames[6]] = dt_data_fetch.Rows[xx]["user_type"].ToString();
                        row[coloumnNames[7]] = dt_data_fetch.Rows[xx]["user_registed_date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        //Added Category Report - Added Category
        public DataTable schoolFineAccountReport(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            string[] coloumnNames = { "අනු අංකය", "පරිශීලක අංකය", "පරිශීලකයාගේ නම", "මුදල", "ගනුදෙනු තත්ත්වය", "දිනය" };
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM school_fine_account WHERE trans_date = @value";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row[coloumnNames[0]] = count;
                        row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["student_reg_number"].ToString();
                        row[coloumnNames[2]] = commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_reg_number"].ToString(), "user_fname") + " " + commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_reg_number"].ToString(), "user_lname");
                        row[coloumnNames[3]] = "රුපියල් " + dt_data_fetch.Rows[xx]["amount"].ToString() + ".00";
                        row[coloumnNames[4]] = dt_data_fetch.Rows[xx]["trans_status"].ToString();
                        row[coloumnNames[5]] = dt_data_fetch.Rows[xx]["trans_date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }


        //Generate Book Transaction Report - Issued Common
        public DataTable bookTransactionReportIssuedCommon(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            dt.Columns.Add("අනු අංකය");
            dt.Columns.Add("පරිශීලක අංකය");
            dt.Columns.Add("පරිශීලකයාගේ නම");
            dt.Columns.Add("පොත් අංකය");
            dt.Columns.Add("පොතේ නම");
            dt.Columns.Add("නිකුත් හානි");
            dt.Columns.Add("දිනය");
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM issue_book WHERE issed_date = @value";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row["අනු අංකය"] = count;
                        row["පරිශීලක අංකය"] = dt_data_fetch.Rows[xx]["student_registration_number"].ToString();
                        row["පරිශීලකයාගේ නම"] = commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_registration_number"].ToString(), "user_fname") + " " + commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_registration_number"].ToString(), "user_lname");
                        row["පොත් අංකය"] = dt_data_fetch.Rows[xx]["book_registration_number"].ToString();
                        row["පොතේ නම"] = commonFilterData("library_books", "registration_number", dt_data_fetch.Rows[xx]["book_registration_number"].ToString(), "book_name");
                        row["නිකුත් හානි"] = dt_data_fetch.Rows[xx]["issue_damage"].ToString();
                        row["දිනය"] = dt_data_fetch.Rows[xx]["issed_date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
                //MessageBox.Show(DateTimeCalculatoin.getWeekStartDate(DateTime.Now).AddDays(x).ToLongDateString());
            }

            return dt;
        }

        //Generate Book Transaction Report - Received Common
        public DataTable bookTransactionReportReceivedCommon(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            dt.Columns.Add("අනු අංකය");
            dt.Columns.Add("පරිශීලක අංකය");
            dt.Columns.Add("පරිශීලකයාගේ නම");
            dt.Columns.Add("පොත් අංකය");
            dt.Columns.Add("පොතේ නම");
            dt.Columns.Add("ලැබීම් හානි");
            dt.Columns.Add("දඩ මුදල්");
            dt.Columns.Add("දිනය");
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM issue_book WHERE returned_date = @value";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row["අනු අංකය"] = count;
                        row["පරිශීලක අංකය"] = dt_data_fetch.Rows[xx]["student_registration_number"].ToString();
                        row["පරිශීලකයාගේ නම"] = commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_registration_number"].ToString(), "user_fname") + " " + commonFilterData("library_user", "user_registration_number", dt_data_fetch.Rows[xx]["student_registration_number"].ToString(), "user_lname");
                        row["පොත් අංකය"] = dt_data_fetch.Rows[xx]["book_registration_number"].ToString();
                        row["පොතේ නම"] = commonFilterData("library_books", "registration_number", dt_data_fetch.Rows[xx]["book_registration_number"].ToString(), "book_name");
                        row["ලැබීම් හානි"] = dt_data_fetch.Rows[xx]["return_damage"].ToString();
                        row["දඩ මුදල්"] = dt_data_fetch.Rows[xx]["book_fine_amount"].ToString();
                        row["දිනය"] = dt_data_fetch.Rows[xx]["returned_date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
                //MessageBox.Show(DateTimeCalculatoin.getWeekStartDate(DateTime.Now).AddDays(x).ToLongDateString());
            }

            return dt;
        }


        public DataTable bookReportByCategory(string cat)
        {
            databaseConnection();
            DataTable dt = new DataTable();

            string[] coloumnNames = { "අනු අංකය", "පොත් අංකය", "පොතේ නම", "කර්තෘ", "ප්‍රකාශිත දිනය", "පිටු ගණන", "මිල", "මූලාශ්‍රය", "වර්ගය", "දිනය" };
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }

            int count = 0;

            string data_fetch = "SELECT * FROM library_books WHERE category = @value";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@value", cat);
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            if (dt_data_fetch.Rows.Count > 0)
            {
                for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                {
                    count++;
                    DataRow row = dt.NewRow();
                    row[coloumnNames[0]] = count;
                    row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["registration_number"].ToString();
                    row[coloumnNames[2]] = dt_data_fetch.Rows[xx]["book_name"].ToString();
                    row[coloumnNames[3]] = dt_data_fetch.Rows[xx]["author"].ToString();
                    row[coloumnNames[4]] = dt_data_fetch.Rows[xx]["publication_date"].ToString();
                    row[coloumnNames[5]] = dt_data_fetch.Rows[xx]["pages"].ToString();
                    row[coloumnNames[6]] = "රුපියල් " + dt_data_fetch.Rows[xx]["price"].ToString() + ".00";
                    row[coloumnNames[7]] = dt_data_fetch.Rows[xx]["source"].ToString();
                    row[coloumnNames[8]] = dt_data_fetch.Rows[xx]["category"].ToString();
                    row[coloumnNames[9]] = dt_data_fetch.Rows[xx]["date"].ToString();
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }


        //Added Books Report - Added Books
        public DataTable addedBooksReport(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            string[] coloumnNames = { "අනු අංකය", "පොත් අංකය", "පොතේ නම", "කර්තෘ", "ප්‍රකාශිත දිනය", "පිටු ගණන", "මිල", "මූලාශ්‍රය", "වර්ගය", "දිනය" };
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }

            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM library_books WHERE date = @value AND status = 'Active'";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row[coloumnNames[0]] = count;
                        row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["registration_number"].ToString();
                        row[coloumnNames[2]] = dt_data_fetch.Rows[xx]["book_name"].ToString();
                        row[coloumnNames[3]] = dt_data_fetch.Rows[xx]["author"].ToString();
                        row[coloumnNames[4]] = dt_data_fetch.Rows[xx]["publication_date"].ToString();
                        row[coloumnNames[5]] = dt_data_fetch.Rows[xx]["pages"].ToString();
                        row[coloumnNames[6]] = "රුපියල් " + dt_data_fetch.Rows[xx]["price"].ToString() + ".00";
                        row[coloumnNames[7]] = dt_data_fetch.Rows[xx]["source"].ToString();
                        row[coloumnNames[8]] = dt_data_fetch.Rows[xx]["category"].ToString();
                        row[coloumnNames[9]] = dt_data_fetch.Rows[xx]["date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        //Added Category Report - Added Category
        public DataTable categoryReport(DateTime date, string type)
        {
            databaseConnection();
            DataTable dt = new DataTable();
            string[] coloumnNames = { "අනු අංකය", "ප්‍රවර්ග නාමය", "දිනය" };
            for (int y = 0; y < coloumnNames.Length; y++)
            {
                dt.Columns.Add(coloumnNames[y]);
            }
            int typeVal = 0, count = 0;
            if (type.Equals("day"))
            {
                typeVal = 1;
            }
            else if (type.Equals("week"))
            {
                typeVal = 7;
            }
            else if (type.Equals("month"))
            {
                typeVal = DateTime.DaysInMonth(date.Year, date.Month);
            }
            for (int x = 0; x < typeVal; x++)
            {
                string data_fetch = "SELECT * FROM book_category WHERE date = @value";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                if (type.Equals("day"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", date.ToLongDateString());
                }
                else if (type.Equals("week"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getWeekStartDate(date).AddDays(x).ToLongDateString());
                }
                else if (type.Equals("month"))
                {
                    cmd_data_fetch.Parameters.AddWithValue("@value", DateTimeCalculatoin.getMonthStart(date).AddDays(x).ToLongDateString());
                }
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        count++;
                        DataRow row = dt.NewRow();
                        row[coloumnNames[0]] = count;
                        row[coloumnNames[1]] = dt_data_fetch.Rows[xx]["cat_name"].ToString();
                        row[coloumnNames[2]] = dt_data_fetch.Rows[xx]["date"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }


        private string commonFilterData(string _table, string _key, string _value, string _filter)
        {
            string return_value = "";
            string data_fetch = "SELECT " + _filter + " FROM " + _table + " WHERE " + _key + " = @value";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@value", _value);
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            if (dt_data_fetch.Rows.Count > 0)
            {
                return_value = dt_data_fetch.Rows[0][_filter].ToString();
            }
            return return_value;
        }
    }
}
