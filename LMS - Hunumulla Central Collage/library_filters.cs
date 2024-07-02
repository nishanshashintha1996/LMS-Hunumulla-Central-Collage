using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace LMS___Hunumulla_Central_Collage
{
    class library_filters
    {
        public static SQLiteConnection conn;
        public static void requestCredentials()
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        public static string getUserNameByPosition(int position)
        {
            requestCredentials();
            string responce = "";
            try
            {
                int count = 0;
                string query = "SELECT * FROM issed_book_count";
                SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
                SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);
                int[] arr = new int[dtable.Rows.Count];
                if (dtable.Rows.Count > 0)
                {
                    for (int x = 0; x < dtable.Rows.Count; x++)
                    {
                        if (Int32.Parse(dtable.Rows[count]["issued_library_book_count"].ToString()) != 0)
                        {
                            arr[count] = Int32.Parse(dtable.Rows[count]["issued_library_book_count"].ToString());
                            count++;
                        }
                    }
                }
                Array.Sort(arr);
                Array.Reverse(arr);
                int[] _finalArr = arr.Distinct().ToArray();
                if (position == 1)
                {
                    string _query = "SELECT * FROM issed_book_count WHERE issued_library_book_count = @val";
                    SQLiteCommand _cmd_fill = new SQLiteCommand(_query, conn);
                    _cmd_fill.Parameters.AddWithValue("@val", _finalArr[0].ToString());
                    SQLiteDataAdapter _dadapter = new SQLiteDataAdapter(_cmd_fill);
                    DataTable _dtable = new DataTable();
                    _dadapter.Fill(_dtable);
                    if (_dtable.Rows.Count > 0)
                    {
                        for(int x = 0; x < _dtable.Rows.Count; x++)
                        {
                            responce += _dataGetterSetter.getUserNameByUserRegistrationNumber(_dtable.Rows[x]["user_registration_number"].ToString()) + "  ( " + _finalArr[0].ToString() + " ) " + '@';
                        }
                    }
                }
                else if (position == 2)
                {
                    string _query = "SELECT * FROM issed_book_count WHERE issued_library_book_count = @val";
                    SQLiteCommand _cmd_fill = new SQLiteCommand(_query, conn);
                    _cmd_fill.Parameters.AddWithValue("@val", _finalArr[1].ToString());
                    SQLiteDataAdapter _dadapter = new SQLiteDataAdapter(_cmd_fill);
                    DataTable _dtable = new DataTable();
                    _dadapter.Fill(_dtable);
                    if (_dtable.Rows.Count > 0)
                    {
                        for (int x = 0; x < _dtable.Rows.Count; x++)
                        {
                            responce += _dataGetterSetter.getUserNameByUserRegistrationNumber(_dtable.Rows[x]["user_registration_number"].ToString()) + "  ( " + _finalArr[1].ToString() + " ) " + '@';
                        }
                    }
                }
                else if (position == 3)
                {
                    string _query = "SELECT * FROM issed_book_count WHERE issued_library_book_count = @val";
                    SQLiteCommand _cmd_fill = new SQLiteCommand(_query, conn);
                    _cmd_fill.Parameters.AddWithValue("@val", _finalArr[2].ToString());
                    SQLiteDataAdapter _dadapter = new SQLiteDataAdapter(_cmd_fill);
                    DataTable _dtable = new DataTable();
                    _dadapter.Fill(_dtable);
                    if (_dtable.Rows.Count > 0)
                    {
                        for (int x = 0; x < _dtable.Rows.Count; x++)
                        {
                            responce += _dataGetterSetter.getUserNameByUserRegistrationNumber(_dtable.Rows[x]["user_registration_number"].ToString()) + "  ( " + _finalArr[2].ToString() + " ) " + '@';
                        }
                    }
                }
                else
                {
                    responce = "position not found@";
                }
            }
            catch (Exception ex)
            {
                responce = "position not found@";
            }
            return responce;
        }

        

    }
}
