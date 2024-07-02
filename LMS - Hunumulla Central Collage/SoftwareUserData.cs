using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    class SoftwareUserData
    {
        private SQLiteConnection conn;
        private string softwareStatus;
        private string institiuteName;
        private string institiuteEmail;
        private string institiuteContact;
        private string lastActivityLogUser;
        private string lastActivityLogCategory;
        private string lastActivityLogStatus;
        private string lastActivityLogDate;
        private string responce;
        private static string loggedUser;
        private static string loggedUserId;

        public SoftwareUserData()
        {
            //Constructor
            fetchActivityLogData();
            fetchData();
        }

        //Setters
        public void setLoggedUser(string name)
        {
            loggedUser = name;
        }

        //Setters
        public void setSoftwareStatus(string status)
        {
            softwareStatus = status;
        }

        public string getSoftwareStatus()
        {
            return softwareStatus;
        }

        public void setLoggedUserId(string id)
        {
            loggedUserId = id;
        }

        // Getters
        public string getLoggedUser()
        {
            return loggedUser;
        }

        public string getLoggedUserId()
        {
            return loggedUserId;
        }

        public string getInstitiuteName()
        {
            return institiuteName;
        }

        public string getInstitiuteContact()
        {
            return institiuteContact;
        }

        public string getInstitiuteEmail()
        {
            return institiuteEmail;
        }

        public string getLastActivityLogUser()
        {
            return lastActivityLogUser;
        }

        public string getLastActivityLogCategory()
        {
            return lastActivityLogCategory;
        }

        public string getLastActivityLogStatus()
        {
            return lastActivityLogStatus;
        }

        public string getLastActivityLogDate()
        {
            return lastActivityLogDate;
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

        private void fetchActivityLogData()
        {
            databaseConnection();
            try
            {
                string data_fetch = "SELECT * FROM system_data";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        institiuteName = dt_data_fetch.Rows[xx]["system_name"].ToString();
                        institiuteContact = dt_data_fetch.Rows[xx]["system_or_contact"].ToString();
                        institiuteEmail = dt_data_fetch.Rows[xx]["system_or_email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
        }

        private void fetchData()
        {
            databaseConnection();
            try
            {
                string data_fetch = "SELECT * FROM configuration_activity_log ORDER BY activity_id DESC LIMIT 1;";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        lastActivityLogUser = dt_data_fetch.Rows[xx]["active_user"].ToString();
                        lastActivityLogCategory = dt_data_fetch.Rows[xx]["activity_category"].ToString();
                        lastActivityLogStatus = dt_data_fetch.Rows[xx]["activity_status"].ToString();
                        lastActivityLogDate = dt_data_fetch.Rows[xx]["activity_date_time"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
        }


    }
}
