using System;
using System.Data.SQLite;
using System.Data;

namespace LMS___Hunumulla_Central_Collage
{
    class _dataGetterSetter
    {
        public static SQLiteConnection conn;
        public static void requestCredentials()
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        public static string getUserNameByUserRegistrationNumber(string regNumber)
        {
            requestCredentials();
            string responce = "";
            try
            {
                string _query = "SELECT user_fname,user_lname FROM library_user WHERE user_registration_number = @val";
                SQLiteCommand _cmd_fill = new SQLiteCommand(_query, conn);
                _cmd_fill.Parameters.AddWithValue("@val", regNumber);
                SQLiteDataAdapter _dadapter = new SQLiteDataAdapter(_cmd_fill);
                DataTable _dtable = new DataTable();
                _dadapter.Fill(_dtable);
                if (_dtable.Rows.Count > 0)
                {
                    responce = _dtable.Rows[0]["user_fname"].ToString() +" "+ _dtable.Rows[0]["user_lname"].ToString();
                }
                else
                {
                    responce = "no data found!";
                }
                return responce;
            }catch (Exception ex)
            {
                return responce = "no data found!";
            }
        }
        
    }
}
