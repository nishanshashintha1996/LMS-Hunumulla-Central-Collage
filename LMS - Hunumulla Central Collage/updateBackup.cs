using System;
using System.Net;
using System.Data.SQLite;
using System.Data;
using Newtonsoft.Json;

namespace LMS___Hunumulla_Central_Collage
{
    class updateBackup
    {
        public static string val = "";
        public static SQLiteConnection conn;
        public static string updateAllData()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            return libraryBooks() + "@" + updateDataFromCloud();

            /*return bookCategoryUpdate() + "@" + issueBookUpdate() + "@" + libraryBooks() + "@" + libraryUsers()
                + "@" + schoolFineAccount() + "@" + userFineAccount() + "@" + updateDataFromCloud();*/
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string bookCategoryUpdate()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM book_category";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-book-category.php?" +
                                "cid=" + dt_data_fetch.Rows[xx]["cat_id"].ToString() +
                                "&cname=" + Base64Encode(dt_data_fetch.Rows[xx]["cat_name"].ToString()) +
                                "&cstatus=" + dt_data_fetch.Rows[xx]["cat_status"].ToString() +
                                "&date=" + dt_data_fetch.Rows[xx]["date"].ToString()
                                );
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }

        public static string issueBookUpdate()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM issue_book";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-issue-book.php?" +
                                "ib_id=" + dt_data_fetch.Rows[xx]["ib_id"].ToString() +
                                "&book_registration_number=" + dt_data_fetch.Rows[xx]["book_registration_number"].ToString() +
                                "&student_registration_number=" + dt_data_fetch.Rows[xx]["student_registration_number"].ToString() +
                                "&issed_date=" + dt_data_fetch.Rows[xx]["issed_date"].ToString() +
                                "&returned_date=" + dt_data_fetch.Rows[xx]["returned_date"].ToString() +
                                "&transaction_state=" + dt_data_fetch.Rows[xx]["transaction_state"].ToString() +
                                "&issue_damage=" + dt_data_fetch.Rows[xx]["issue_damage"].ToString() +
                                "&return_damage=" + dt_data_fetch.Rows[xx]["return_damage"].ToString() +
                                "&book_fine_amount=" + dt_data_fetch.Rows[xx]["book_fine_amount"].ToString() +
                                "&book_fine_status=" + dt_data_fetch.Rows[xx]["book_fine_status"].ToString()
                                );
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }


        public static string libraryBooks()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM library_books";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
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
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }


        public static string libraryUsers()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM library_user";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-library-user.php?" +
                                "user_id=" + dt_data_fetch.Rows[xx]["user_id"].ToString() +
                                "&user_registration_number=" + dt_data_fetch.Rows[xx]["user_registration_number"].ToString() +
                                "&user_fname=" + Base64Encode(dt_data_fetch.Rows[xx]["user_fname"].ToString()) +
                                "&user_lname=" + Base64Encode(dt_data_fetch.Rows[xx]["user_lname"].ToString()) +
                                "&user_grade=" + dt_data_fetch.Rows[xx]["user_grade"].ToString() +
                                "&user_address=" + Base64Encode(dt_data_fetch.Rows[xx]["user_address"].ToString()) +
                                "&contact_number=" + dt_data_fetch.Rows[xx]["contact_number"].ToString() +
                                "&user_type=" + dt_data_fetch.Rows[xx]["user_type"].ToString() +
                                "&user_state=" + dt_data_fetch.Rows[xx]["user_state"].ToString() +
                                "&user_registed_date=" + dt_data_fetch.Rows[xx]["user_registed_date"].ToString()
                                );
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }


        public static string schoolFineAccount()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM school_fine_account";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-scool-fine-account.php?" +
                                "s_f_id=" + dt_data_fetch.Rows[xx]["s_f_id"].ToString() +
                                "&student_reg_number=" + dt_data_fetch.Rows[xx]["student_reg_number"].ToString() +
                                "&amount=" + dt_data_fetch.Rows[xx]["amount"].ToString() +
                                "&trans_status=" + dt_data_fetch.Rows[xx]["trans_status"].ToString() +
                                "&trans_date=" + dt_data_fetch.Rows[xx]["trans_date"].ToString() +
                                "&s_f_trans_state=" + dt_data_fetch.Rows[xx]["s_f_trans_state"].ToString()
                                );
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }

        public static string userFineAccount()
        {
            string responce = "null";
            try
            {
                string data_fetch = "SELECT * FROM user_fines";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = client.DownloadString("https://api.elitesolution.us/LMS/api-user-fines.php?" +
                                "fines_id=" + dt_data_fetch.Rows[xx]["fines_id"].ToString() +
                                "&user_registration_number=" + dt_data_fetch.Rows[xx]["user_registration_number"].ToString() +
                                "&amount=" + dt_data_fetch.Rows[xx]["amount"].ToString() +
                                "&transaction_status=" + dt_data_fetch.Rows[xx]["transaction_status"].ToString() +
                                "&transaction_date=" + dt_data_fetch.Rows[xx]["transaction_date"].ToString()
                                );
                            //MessageBox.Show(src);
                            responce = src;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responce = ex.ToString();
            }
            return responce;
        }



        public static string updateTablesFromCloud()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
                string src = "";
                using (WebClient client = new WebClient())
                {
                    src = client.DownloadString("https://api.elitesolution.us/LMS/api-librarian-user.php");
                }

                string[] words = src.Split('$');
                string[] query = new string[7];
                int count = 0;
                foreach (var word in words)
                {
                    if (!word.Equals("/"))
                    {
                        query[count] = word.ToString();
                        count++;
                    }
                    else
                    {
                        val += upadetDb(query);
                        count = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                val += ">>> " + ex.ToString() + "@";
            }
            return val;
        }

        public static string updateDataFromCloud()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
                string src = "";
                using (WebClient client = new WebClient())
                {
                    src = client.DownloadString("https://api.elitesolution.us/LMS/api-library-user-web-id.php");
                }

                string[] words = src.Split('$');
                string[] query = new string[2];
                int count = 0;
                foreach (var word in words)
                {
                    if (!word.Equals("/"))
                    {
                        query[count] = word.ToString();
                        count++;
                    }
                    else
                    {
                        val += upadetDbData(query);
                        count = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                val += ">>> " + ex.ToString() + "@";
            }
            return val;
        }


        public static string upadetDbData(string[] arr)
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            try
            {
                string sql_query = "SELECT * FROM library_user WHERE user_registration_number = @id";
                SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                cmd_fill.Parameters.AddWithValue("@id", arr[0]);
                SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);
                if (dtable.Rows.Count == 1)
                {
                    string updatecQuery = "UPDATE library_user SET user_web_id = @a WHERE user_registration_number = @id";
                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                    {
                        updatec.Parameters.AddWithValue("@a", arr[1]);
                        updatec.Parameters.AddWithValue("@id", arr[0]);
                        updatec.ExecuteNonQuery();
                        val += ">>> Web Id Update Success !" + "@";
                    }
                }
            }
            catch (Exception ex)
            {
                val += ">>> " + ex.ToString() + "@";
            }
            return val;
        }




        public static string upadetDb(string[] arr)
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            try
            {
                string sql_query = "SELECT * FROM librarian_user WHERE librarian_id = @id";
                SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                cmd_fill.Parameters.AddWithValue("@id", arr[0]);
                SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);
                if (dtable.Rows.Count == 1)
                {
                    string updatecQuery = "UPDATE librarian_user SET librarian_id = @a, librarian_name = @b, librarian_email=@c, librarian_username=@d, password = @f, state = @g WHERE librarian_id = @id";
                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                    {
                        updatec.Parameters.AddWithValue("@a", arr[0]);
                        updatec.Parameters.AddWithValue("@b", arr[1]);
                        updatec.Parameters.AddWithValue("@c", arr[2]);
                        updatec.Parameters.AddWithValue("@d", arr[3]);
                        updatec.Parameters.AddWithValue("@e", arr[4]);
                        updatec.Parameters.AddWithValue("@f", arr[5]);
                        updatec.Parameters.AddWithValue("@g", arr[6]);
                        updatec.Parameters.AddWithValue("@id", arr[0]);
                        updatec.ExecuteNonQuery();
                        val += ">>> Credentials Update Success !" + "@";
                    }
                }
                else if(dtable.Rows.Count == 0)
                {
                    string stringQuery = "INSERT INTO librarian_user (librarian_id, librarian_name, librarian_email, librarian_username, last_login, password, state)" +
                               "VALUES(@a, @b, @c, @d, @e, @f, @g)";
                    using (SQLiteCommand updatec = new SQLiteCommand(stringQuery, conn))
                    {
                        updatec.Parameters.AddWithValue("@a", arr[0]);
                        updatec.Parameters.AddWithValue("@b", arr[1]);
                        updatec.Parameters.AddWithValue("@c", arr[2]);
                        updatec.Parameters.AddWithValue("@d", arr[3]);
                        updatec.Parameters.AddWithValue("@e", arr[4]);
                        updatec.Parameters.AddWithValue("@f", arr[5]);
                        updatec.Parameters.AddWithValue("@g", arr[6]);
                        updatec.ExecuteNonQuery();
                        val = ">>> Credentials Created Success !" + "@";
                    }
                }
            }catch (Exception ex)
            {
                val += ">>> "+ex.ToString() + "@";
            }
            return val;
        }
    }
}
