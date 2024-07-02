using System;
using System.Net;
using System.Data;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    class ActivityLogDataFetch
    {
        private SQLiteConnection conn;
        public ActivityLogDataFetch() { }

        //Request Template
        //ActivityLogDataFetch logData = new ActivityLogDataFetch("System Login", "Successfully logged to the system");
        public ActivityLogDataFetch(string a, string b) {
            AddNewLog(a, b);
        }

        public void AddNewLog(string process, string comment)
        {
            databaseConnection();

            SoftwareUserData u = new SoftwareUserData();
            u.getLoggedUser();

            string stringQuery = "INSERT INTO daily_activity_log (log_process,log_time,comment,active_user,log_status,log_date)" +
                                "VALUES(@lgpros, @lgtim, @com, @acuser, @lgstat, @lgdat)";
            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
            {

                SqliteCmd.Parameters.AddWithValue("@lgpros", process);
                SqliteCmd.Parameters.AddWithValue("@lgtim", DateTime.Now.ToLongTimeString());
                SqliteCmd.Parameters.AddWithValue("@com", comment);
                SqliteCmd.Parameters.AddWithValue("@acuser", u.getLoggedUser());
                SqliteCmd.Parameters.AddWithValue("@lgstat", 1);
                SqliteCmd.Parameters.AddWithValue("@lgdat", DateTime.Now.ToLongDateString());

                try
                {
                    SqliteCmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {

                }
            }
        }

        public DataTable GetDayLog()
        {
            databaseConnection();
            string data_fetch = "SELECT log_process AS ක්‍රියාවලි_විස්තරය,log_time AS වේලාව,comment AS වෙනත්_ඇමුණුම්,active_user AS ක්‍රියාව_සිදුකළ_පරිශීලකයා,log_date AS දිනය FROM daily_activity_log WHERE log_date = @lgdate ORDER BY log_id";
            SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
            cmd_data_fetch.Parameters.AddWithValue("@lgdate", DateTime.Now.ToLongDateString());
            SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
            DataTable dt_data_fetch = new DataTable();
            da_data_fetch.Fill(dt_data_fetch);
            return dt_data_fetch;
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

    }
}
