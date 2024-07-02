using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Net;
using System.Data.SQLite;
using Tulpep.NotificationWindow;
using System.Runtime.InteropServices;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class Dashboard : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public string activity_view_text = "";
        public string librarian_id = "";
        public string librarian_name = "";
        public string librarian_email = "";
        public string librarian_username = "";
        public string last_login = "";
        public string librarian_password = "";
        public string state = "";
        public string swstateD = "";
        public int conter = 0;
        public int runner = 1;
        SQLiteConnection conn;
        private static int DataCount = 17;
        private static bool TooTipState = false;
        private static Point[] PointData = new Point[DataCount];
        private static string[] Message = new string[DataCount];
        private static string[] PointerPosition = new string[DataCount];
        GoogleCredential credential = null;
        string bucketName;
        StorageClient storageClient;
        public Dashboard(string id, string passw, string swstate)
        {
            swstateD = swstate;
            InitializeComponent();
            try
            {
                if (swstate != "")
                {
                    if (swstate == "Trial")
                    {
                        pActive.Text = swstate;
                        pActive.BackColor = Color.Red;
                    }
                    else
                    {
                        pActive.Text = swstate;
                        pActive.BackColor = Color.Green;
                    }
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
                conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
                conn.Open();
                librarian_id = id;
                string q_check = "SELECT * FROM librarian_user WHERE librarian_id = @l_id_check";
                SQLiteCommand cmd_check = new SQLiteCommand(q_check, conn);
                cmd_check.Parameters.AddWithValue("@l_id_check", librarian_id); //TEXT
                SQLiteDataAdapter da_check = new SQLiteDataAdapter(cmd_check);
                DataTable dt_check = new DataTable();
                da_check.Fill(dt_check);
                if (dt_check.Rows.Count == 0)
                {
                    string stringQuery = "INSERT INTO librarian_user (librarian_id, librarian_name, librarian_email, librarian_username, last_login, password, state)VALUES(@l_id, @l_name, @l_email, @l_username, @l_login, @l_pass, @l_state)";
                    using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                    {
                        using (WebClient client = new WebClient())
                        {
                            string src = "";
                            try
                            {
                                src = client.DownloadString("https://api.elitesolution.us/auth/api/auth_data.php?id=" + librarian_id);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            string[] data = src.Split('@');
                            SqliteCmd.Parameters.AddWithValue("@l_id", librarian_id);
                            SqliteCmd.Parameters.AddWithValue("@l_name", Base64Decode(data[0]));
                            SqliteCmd.Parameters.AddWithValue("@l_email", Base64Decode(data[1]));
                            SqliteCmd.Parameters.AddWithValue("@l_username", Base64Decode(data[2]));
                            SqliteCmd.Parameters.AddWithValue("@l_login", "");
                            SqliteCmd.Parameters.AddWithValue("@l_pass", passw);
                            SqliteCmd.Parameters.AddWithValue("@l_state", "Active");
                        }
                        try
                        {
                            SqliteCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

                string updatequery = "UPDATE librarian_user SET last_login=@l_login WHERE librarian_id =@lib_id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatequery, conn))
                {
                    updatec.Parameters.AddWithValue("@lib_id", librarian_id);
                    updatec.Parameters.AddWithValue("@l_login", DateTime.Now.ToLongTimeString());
                    updatec.ExecuteNonQuery();
                }
                string q = "SELECT * FROM librarian_user";
                librarian_id = id;
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("librarian_id=" + librarian_id);
                    foreach (DataRow row in dr)
                    {
                        librarian_name = row["librarian_name"].ToString();
                        librarian_email = row["librarian_email"].ToString();
                        librarian_username = row["librarian_username"].ToString();
                        last_login = row["last_login"].ToString();
                        librarian_password = row["password"].ToString();
                        state = row["state"].ToString();
                    }
                }

                using (var jsonStream = new FileStream("laptrinhvb-c96be-e4ea5c001fd4.json", FileMode.Open,
                        FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
                bucketName = "lms-hunumulla-1616871574788.appspot.com";
                storageClient = StorageClient.Create(credential);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // ToolTip Initializer
            //lt / rt / lb / rb
            PointData[0] = new Point(issue_book.Location.X + 30, issue_book.Location.Y + 30);
            Message[0] = "Issue Book " + Environment.NewLine + "මෙම බොත්තම හරහා පුස්තකාලයට අයත් පොතක් බැහැර දීම සිදුු කළ හැකි වේ. මෙම බොත්තම click කර ලැබෙන කවුළුව සදහා barcode කියවනයේ ආදානය ලබා දිය යුතුය.";
            PointerPosition[0] = "lt";

            PointData[1] = new Point(bunifuTileButton1.Location.X + 30, bunifuTileButton1.Location.Y + 30);
            Message[1] = "Receive Book " + Environment.NewLine + "මෙම බොත්තම හරහා පුස්තකාලයෙන් බැහැර දුන් පොතක් ලබාගැනීම සිදු කරයි. මේ සදහා barcode කියවනයේ ආදානය ලබාදිය යුතුය.";
            PointerPosition[1] = "lt";

            PointData[2] = new Point(add_student.Location.X + 30, add_student.Location.Y + 30);
            Message[2] = "Manage Users" + Environment.NewLine + "මෙම බොත්තම හරහා පුස්තකාලය පරිහරණය කරන්නාවූ පරිශීලකයන් කළමනාකරණයට අදාළ කවුළුව විවෘත කරගත හැක. එහිදී නව පරිශීලකයන් ඇතුළත් කිරීම, පවතින පරිශීලක දත්ත වෙනස් කිරීම සිදුකළ හැකි වේ.";
            PointerPosition[2] = "lt";

            PointData[3] = new Point(manage_books.Location.X + 30, manage_books.Location.Y + 30);
            Message[3] = "Manage Books" + Environment.NewLine + "මෙම බොත්තම හරහා පුස්තකාලයේ පොත් කළමනාකරණයට ආදළ කවුළුව විවෘත කර ගත හැක. එහිදී නව පොත් පද්ධතියට ඇතුළත් කිරීම, පවතින පොත් විස්තර වෙනස් කිරීම, පවතින පොත් පද්ධතියෙන් ඉවත් කිරීම ආදී පහසුකම් සලසනු ලබයි.";
            PointerPosition[3] = "lt";

            PointData[4] = new Point(manage_reports.Location.X + 30, manage_reports.Location.Y + 30);
            Message[4] = "Manage Reports" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධතියේ පවතින දත්ත වාර්තා ලෙස මෘද්‍රණය කිරීමට හෝ .pdf ගොනු ලෙස සුරැකීමට හැකියාව ලැබේ. මෙහිදි ,"
                                                                     + Environment.NewLine
                                                                     + Environment.NewLine
                                                                     + "->බැහැර දුන් පොත් වාර්තාව"
                                                                     + Environment.NewLine
                                                                     + "->බැහැර දී නැවත ලබාදුන් පොත් වාර්තාව"
                                                                     + Environment.NewLine
                                                                     + "->නව පරිශීලක ඇතුළත් වීම් වාර්තාව"
                                                                     + Environment.NewLine
                                                                     + "->නව පොත් ඇතුළත් කිරීම් වාර්තාව"
                                                                     + Environment.NewLine
                                                                     + "->නව වර්ගීකරණ ඇතුළත් කිරීම් වාර්තාව"
                                                                     + Environment.NewLine
                                                                     + Environment.NewLine
                                                                     + "ආදී වාර්තා විශාල ප්‍රමාණයක් අදාළ දිනය, සතිය, මාසය වශයෙන් ලබාගත හැකි වේ.";
            PointerPosition[4] = "lt";

            PointData[5] = new Point(resource_statistics.Location.X + 30, resource_statistics.Location.Y + 30);
            Message[5] = "Resource Statictics" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධතියේ පවතින දත්ත සටහන් ආකාරයට ලබාගැනීමට හැකියාව ලැබේ. මෙමගින් විශ්ලේෂණාත්මක  නිගමන වලට එළැබීමට හැකියාව අදාළ පුස්තකාලාධිපතිට ලැබේ.";
            PointerPosition[5] = "lt";

            PointData[6] = new Point(bunifuImageButton2.Location.X + 30, bunifuImageButton2.Location.Y - 30);
            Message[6] = "Documentation" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධතිය පරිහරණයට අදාළ මාර්ගෝපදේශණය ලබාගත හැකි වේ. මේ සදහා අන්තර්ජාල සබඳතාවය අත්‍යවශ්‍ය කරුණක් වේ.";
            PointerPosition[6] = "lb";

            PointData[7] = new Point(settings.Location.X + 30, settings.Location.Y - 30);
            Message[7] = "Settings" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධතියේ සැකසුම් කවුළුව විවෘත කරගත හැකි වේ. එම කවුළුව මගින් පද්ධතියේ වර්ගීකරණ සැකසුම් සැකසීම, පුස්තකාලාධිපති හෝ පද්ධති පරිශීලකයාගේ පද්ධති විස්තර යාවත්කාලීන කිරීම හෝ වෙනස් කිරීම් වලට අදාළ ඉල්ලීම් සිදුකළ හැකි කවුළුව විවෘත කර ගැනීම, පද්ධතිය පරිහරණය පහසුව සදහා උදව් ලබාදෙන කෘතීම උපදේශන සේවය ක්‍රියාත්මක කිරීම හෝ ක්‍රියාවිරහිත කිරීමට අදාළ කවුළුව විවෘත කරගැනීම ආදී සේවා ලබාගත හැකි වේ.";
            PointerPosition[7] = "lb";

            PointData[8] = new Point(bunifuImageButton3.Location.X + 30, bunifuImageButton3.Location.Y - 30);
            Message[8] = "Registration Form" + Environment.NewLine + "මෙම බොත්තම click කිරීම මගින් පද්ධතියට අදාළ පරිශීලකයන් ලියාපදිංචියට අවශ්‍ය පෝරමය සකස් කර ගැනීමට හැකියාව ලැබේ. මෙහිදී ලැබෙන කවුළුව මගින් සාමන්‍ය වදන් සැකසුම් මෘදුකාංගයක් ආකාරයෙන් සිදුකර ගැනීමට හැකි වේ.";
            PointerPosition[8] = "lb";

            PointData[9] = new Point(bunifuImageButton4.Location.X + 30, bunifuImageButton4.Location.Y - 30);
            Message[9] = "Daily Report Generator" + Environment.NewLine + "මෙම බොත්තම හරහා ලැබෙන කවුළුව මගින් පුස්තකාලාධිපතිගේ දෛනික ක්‍රියාවලියේ වාර්තාවක් ලබාගැනීමට හැකියාව ලැබේ.";
            PointerPosition[9] = "lb";

            PointData[10] = new Point(btn_help.Location.X + 30, btn_help.Location.Y - 30);
            Message[10] = "Tool Tip" + Environment.NewLine + "මෙම බොත්තම හරහා විවෘත වී ඇති කවුළුවට අදාළව ක්ෂණික මාර්ගෝපදේශණයක් ලබාගත හැක.";
            PointerPosition[10] = "lb";

            PointData[11] = new Point(btn_web.Location.X + 30, btn_web.Location.Y - 30);
            Message[11] = "Digital Library Web Site" + Environment.NewLine + "මෙම බොත්තම හරහා පාසලට අදාළ ඩිජිටල් පුස්තකාල වෙබ් පිටුවට පිවිසීමට හැකියාව ලැබේ.";
            PointerPosition[11] = "lb";

            PointData[12] = new Point(btn_privacy.Location.X + 30, btn_privacy.Location.Y - 30);
            Message[12] = "Privacy & Policy Document" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධති පරිශීලකයා හා පද්ධති නිෂ්පාදකයා වන EliteSolution ආයතනය අතර පවතින ගිවිසුම් විස්තර ලබාගත හැකි කවුළුව විවෘත කරගත හැකි වේ.";
            PointerPosition[12] = "lb";

            PointData[13] = new Point(btn_info.Location.X + 30, btn_info.Location.Y - 30);
            Message[13] = "About the Sottware" + Environment.NewLine + "මෙම බොත්තම හරහා පද්ධතිය පිළිබඳ කෙටි විස්තරයක් ලබාගැනීමට හැකි වේ.";
            PointerPosition[13] = "lb";


            PointData[14] = new Point(pictureBox3.Location.X + 30, pictureBox3.Location.Y);
            Message[14] = "Activity Overview" + Environment.NewLine + "මෙම අනු කොටස හරහා පද්ධති තොරතුරු ගවේෂණයට පහසුකම් සලසයි.";
            PointerPosition[14] = "lt";

            PointData[15] = new Point(pictureBox1.Location.X + 30, pictureBox1.Location.Y);
            Message[15] = "Logged User" + Environment.NewLine + "මෙම අනු කොටස හරහා පද්ධති පරිශීලකයාගේ විස්තර පෙන්වීම හා බැහැර කිරීම සිදුකළ හැක.";
            PointerPosition[15] = "rt";

            PointData[16] = new Point(pictureBox2.Location.X + 30, pictureBox2.Location.Y + 30);
            Message[16] = "BackUp data" + Environment.NewLine + "මෙම අනු කොටස හරහා පද්ධතියේ දත්ත වලාකුළු උපස්ථයකට පිටපත් කිරීම සිදුකළ හැකි වේ.";
            PointerPosition[16] = "rt";
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
                SoftwareUserData userDta = new SoftwareUserData();
                userDta.setLoggedUser(librarian_name);
                new ActivityLogDataFetch("Dashboard", "Open Dashboard");
                userDta.setLoggedUserId(librarian_id);
                setLateReturns();
                timer3.Start();
                top_user_timer.Start();
                loading_view view = new loading_view();
                view.ShowDialog();
                this.WindowState = FormWindowState.Maximized;
                logged_user_name.Text = librarian_name;
                logged_last_time.Text = last_login;
                logged_user_state.Text = state;
                timer1.Start();
                time.Text = "Time :" + DateTime.Now.ToLongTimeString();
                date.Text = DateTime.Now.ToLongDateString();
                activity_view_text_setter(userDta.getInstitiuteName() + " LMS.@Version 1.3.0 @");

                if (!File.Exists(Environment.CurrentDirectory + "/Data/" + "fines.txt"))
                {
                    try
                    {
                        File.CreateText(Environment.CurrentDirectory + "/Data/" + "fines.txt");
                        using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Data/" + "fines.txt"))
                        {
                            int y = 0;
                            Int32.TryParse(sr.ReadLine(), out y);
                            if (y == 0)
                            {
                                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/Data/" + "fines.txt"))
                                {
                                    sw.WriteLine("1");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.Information;
                        popup.TitleText = "Error";
                        popup.ContentText = "Internal File Writting Error!";
                        popup.Delay = 5000;
                        popup.Popup();
                    }
                }
                else
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Data/" + "fines.txt"))
                        {
                            int y = 0;
                            Int32.TryParse(sr.ReadLine(), out y);
                            if (y == 0)
                            {
                                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/Data/" + "fines.txt"))
                                {
                                    sw.WriteLine("1");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.Information;
                        popup.TitleText = "Error";
                        popup.ContentText = "Internal File Writting Error!";
                        popup.Delay = 5000;
                        popup.Popup();
                    }
                }
                help_bot help = new help_bot("This system will help to librarian to manage dailty actvities and other stuffs during your duty period. Also this system will help to manage library functions perfectly. So you can activate help bot service using settings option.", librarian_name, _botRun.checkRobot());
                help.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void setFiltersData()
        {
            string responce01 = library_filters.getUserNameByPosition(1).Remove(library_filters.getUserNameByPosition(1).Length - 1, 1);
            firstPlace.Text = responce01.Replace("@", System.Environment.NewLine);

            string responce02 = library_filters.getUserNameByPosition(2).Remove(library_filters.getUserNameByPosition(2).Length - 1, 1);
            secoundPlace.Text = responce02.Replace("@", System.Environment.NewLine);

            string responce03 = library_filters.getUserNameByPosition(3).Remove(library_filters.getUserNameByPosition(3).Length - 1, 1);
            thirdPlace.Text = responce03.Replace("@", System.Environment.NewLine);
        }

        private void maximize_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                closeToolTip();
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

        private void logout_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you want logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dialog == DialogResult.Yes)
            {
                this.Hide();
                Login_UI login = new Login_UI();
                new ActivityLogDataFetch("User Logout", "Sccessfully Logged out from the system");
                login.ShowDialog();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void add_student_Click(object sender, EventArgs e)
        {
            manage_users_view view = new manage_users_view(librarian_id, swstateD);
            this.Hide();
            view.ShowDialog();
        }

        private void manage_books_Click(object sender, EventArgs e)
        {
            manage_books_view view = new manage_books_view(librarian_id, swstateD);
            this.Hide();
            view.ShowDialog();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            settings_view view = new settings_view();
            view.ShowDialog();
        }

        private void issue_book_Click(object sender, EventArgs e)
        {
            help_bot help = new help_bot("First you need to scan book barcode using your barcode reader. If you have not any barcode reader, you can user manual mode. First select manual check box and type barcode number. Then select particular barcode correctly.", "", _botRun.checkRobot());
            help.ShowDialog();
            reading_barcode_view view = new reading_barcode_view(true);
            view.ShowDialog();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            reading_barcode_view view = new reading_barcode_view(false);
            view.ShowDialog();
        }


        // Main Search Functions Start ------------------------------------------------------------------------------------

        private void bookOrUserSearchKeywork_Leave(object sender, EventArgs e)
        {
            if (bookOrUserSearchKeywork.Text.Equals(""))
            {
                bookOrUserSearchKeywork.Text = "Search Here....";
            }
        }

        private void bookOrUserSearchKeywork_Enter(object sender, EventArgs e)
        {
            if (bookOrUserSearchKeywork.Text.Equals("Search Here...."))
            {
                bookOrUserSearchKeywork.Text = "";
            }
        }

        private void bookOrUserSearchKeywork_OnValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radiobtnUser.Checked)
                {
                    if (!bookOrUserSearchKeywork.Text.Equals(""))
                    {
                        try
                        {
                            string sql_query = "SELECT user_registration_number,user_fname,user_lname,user_type FROM library_user " +
                                "WHERE user_registration_number LIKE @keyword " +
                                "OR user_fname LIKE @keyword " +
                                "OR user_lname LIKE @keyword " +
                                "OR user_grade LIKE @keyword " +
                                "OR contact_number LIKE @keyword";
                            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                            cmd_fill.Parameters.AddWithValue("@keyword", "%" + bookOrUserSearchKeywork.Text + "%");
                            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                            DataTable dtable = new DataTable();
                            dadapter.Fill(dtable);
                            if (dtable.Rows.Count > 0)
                            {
                                result_grid.DataSource = dtable;
                                result_grid.Show();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        result_grid.Hide();
                    }
                }

                if (radiobtnBook.Checked)
                {
                    if (!bookOrUserSearchKeywork.Text.Equals(""))
                    {
                        try
                        {
                            string sql_query = "SELECT registration_number,book_name,author,price,category FROM library_books " +
                                "WHERE " +
                                "registration_number LIKE @keyword " +
                                "OR book_name LIKE @keyword " +
                                "OR author LIKE @keyword " +
                                "OR category LIKE @keyword";
                            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                            cmd_fill.Parameters.AddWithValue("@keyword", "%" + bookOrUserSearchKeywork.Text + "%");
                            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                            DataTable dtable = new DataTable();
                            dadapter.Fill(dtable);
                            if (dtable.Rows.Count > 0)
                            {
                                result_grid.DataSource = dtable;
                                result_grid.Show();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        result_grid.Hide();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void result_grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (radiobtnBook.Checked)
            {
                try
                {
                    if (result_grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        result_grid.CurrentRow.Selected = true;
                        book_view_common view = new book_view_common(result_grid.Rows[e.RowIndex].Cells["registration_number"].FormattedValue.ToString());
                        view.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (radiobtnUser.Checked)
            {
                try
                {
                    if (result_grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        result_grid.CurrentRow.Selected = true;
                        user_view_common view = new user_view_common(result_grid.Rows[e.RowIndex].Cells["user_registration_number"].FormattedValue.ToString());
                        view.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@", System.Environment.NewLine);
            activity_view_text += receiveText;
            terminal.Text = activity_view_text;
            terminal.SelectionStart = terminal.Text.Length;
            terminal.ScrollToCaret();
        }

        private void btn_conn_Click(object sender, EventArgs e)
        {
            timeProcessStart();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (conter != 20)
            {
                conter++;
                activity_view_text_setter("#");
            }
            else
            {
                int Desc;
                if (InternetGetConnectedState(out Desc, 0))
                {
                    activity_view_text_setter("@>>> Internet Connection.@>>> Available @");
                    conter = 0;
                    this.timer2.Stop();
                }
                else
                {
                    activity_view_text_setter("@>>> Internet Connection.@>>> Not Available @");
                    conter = 0;
                    this.timer2.Stop();
                }
            }
        }

        private void timeProcessStart()
        {
            activity_view_text_setter("@>>> ");
            this.timer2.Start();
        }

        private void radiobtnUser_CheckedChanged(object sender, EventArgs e)
        {
            result_grid.Hide();
            bookOrUserSearchKeywork.Text = "";
        }

        private void manage_reports_Click(object sender, EventArgs e)
        {
            report_generating_view view = new report_generating_view();
            view.ShowDialog();
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCollection fc = Application.OpenForms;
            bool windowStatus = false;
            foreach (Form frm in fc)
            {
                if (frm.Name == "uploaderView")
                {
                    MessageBox.Show("Backup Process Already Runnig", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    windowStatus = true;
                }
            }

            if (!windowStatus)
            {
                string extend = DateTime.Now.ToString("MM-dd-yyyy hh-mm tt");
                string fileToCopy = "System\\_sysdd.db";
                string destinationDirectory = "System\\Temp\\" + extend + " " + SystemInformation.ComputerName;

                try
                {
                    File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

                    // Client ID : 1051141231421-38a4iric2lr5kmcne2ec6uhlrfiiu5jm.apps.googleusercontent.com
                    // Client Secret : hs53GmcCGwYMI7hr-2IIT91Y

                    Cursor.Current = Cursors.WaitCursor;
                    int Desc;
                    if (InternetGetConnectedState(out Desc, 0))
                    {
                        if (checkBox1.Checked)
                        {
                            activity_view_text_setter(">>> Backup Update Processing...... @");
                            uploaderView view = new uploaderView();
                            view.Show();
                        }
                    }
                    else
                    {
                        activity_view_text_setter(">>> ");
                        this.timer2.Start();
                    }


                    using (var fileStream = new FileStream("System\\Temp\\" + extend + " " + SystemInformation.ComputerName + "_sysdd.db", FileMode.Open,
                            FileAccess.Read, FileShare.Read))
                    {
                        //progressBar1.Maximum = (int)fileStream.Length;

                        var uploadObjectOptions = new UploadObjectOptions
                        {
                            ChunkSize = UploadObjectOptions.MinimumChunkSize
                        };
                        //var progressReporter = new Progress<IUploadProgress>(OnUploadProgress);
                        await storageClient.UploadObjectAsync(bucketName, Path.GetFileName("System\\Temp\\" + extend + " " + SystemInformation.ComputerName + "_sysdd.db"), "application/octet-stream", fileStream, uploadObjectOptions).ConfigureAwait(true);
                        //btn_getFiles_Click(sender, e);
                        if (!checkBox1.Checked)
                        {
                            MessageBox.Show("Update Process Completed !");
                        }
                        activity_view_text_setter(">>> Backup File Uploaded...... @");
                    }

                    File.Delete("System\\Temp\\" + extend + " " + SystemInformation.ComputerName + "_sysdd.db");

                    Cursor.Current = Cursors.Default;
                    new ActivityLogDataFetch("Backup Data", "Updated Backup Data");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Delete("System\\Temp\\" + extend + " " + SystemInformation.ComputerName + "_sysdd.db");
                }
            }
        }



        private void resource_statistics_Click(object sender, EventArgs e)
        {
            ResourceStatics view = new ResourceStatics();
            view.ShowDialog();
        }

        private void top_user_timer_Tick(object sender, EventArgs e)
        {
            setFiltersData();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            RegistrationFormView view = new RegistrationFormView();
            view.ShowDialog();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            DailyReport view = new DailyReport();
            view.ShowDialog();
        }

        // info
        private void btn_info_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=about");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // doc
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=doc");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // privacy
        private void btn_privacy_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=privacy");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }
        // system
        private void btn_web_Click(object sender, EventArgs e)
        {
            int Desc;
            if (InternetGetConnectedState(out Desc, 0))
            {
                WebView web = new WebView("https://api.elitesolution.us/redirectSystemFiles.php?redirect=system");
                web.ShowDialog();
            }
            else
            {
                MessageBox.Show("Internet Connection Not Available");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            label5.Text = data.getInstitiuteName();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void setLateReturns()
        {
            //Grid Name = lateReturnTable
            try
            {
                string data_fetch = "SELECT book_registration_number,student_registration_number,returned_date FROM issue_book WHERE transaction_state = @state";
                SQLiteCommand cmd_data_fetch = new SQLiteCommand(data_fetch, conn);
                cmd_data_fetch.Parameters.AddWithValue("@state", "Issued");
                SQLiteDataAdapter da_data_fetch = new SQLiteDataAdapter(cmd_data_fetch);
                DataTable dt_data_fetch = new DataTable();
                da_data_fetch.Fill(dt_data_fetch);
                if (dt_data_fetch.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("Book_Registration_Number");
                    dt.Columns.Add("Student_Registration_Number");
                    dt.Columns.Add("Returned_Date");
                    dt.Columns.Add("Date_Count");
                    for (int xx = 0; xx < dt_data_fetch.Rows.Count; xx++)
                    {
                        string returned_date = dt_data_fetch.Rows[xx]["returned_date"].ToString();
                        DateTime enteredDate = DateTime.Parse(returned_date);
                        DateTime todayDate = DateTime.Parse(DateTime.Now.ToLongDateString());
                        double x = (enteredDate - todayDate).TotalDays;
                        if (x < 0)
                        {
                            //MessageBox.Show(x.ToString());
                            DataRow dataRow = dt.NewRow();
                            dataRow["Book_Registration_Number"] = dt_data_fetch.Rows[xx]["book_registration_number"].ToString();
                            dataRow["Student_Registration_Number"] = dt_data_fetch.Rows[xx]["student_registration_number"].ToString();
                            dataRow["Returned_Date"] = dt_data_fetch.Rows[xx]["returned_date"].ToString();
                            dataRow["Date_Count"] = (x*-1).ToString() + " Days Late";
                            dt.Rows.Add(dataRow);
                        }
                    }

                    lateReturnTable.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            setLateReturns();
        }

        private void lateReturnTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (lateReturnTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    lateReturnTable.CurrentRow.Selected = true;
                    user_view_common view = new user_view_common(lateReturnTable.Rows[e.RowIndex].Cells["Student_Registration_Number"].FormattedValue.ToString());
                    view.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            runner = 0;
            if (this.WindowState == FormWindowState.Maximized)
            {
                callToolTip();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                callToolTip();
            }
        }

        private void callToolTip()
        {
            //lt / rt / lb / rb
            if (PointerPosition[0].Equals("lt"))
            {
                ltop.Visible = true;
                rtop.Visible = false;
                lbottom.Visible = false;
                rbottom.Visible = false;
            }
            else if (PointerPosition[0].Equals("rt"))
            {
                rtop.Visible = true;
                lbottom.Visible = false;
                rbottom.Visible = false;
                ltop.Visible = false;
            }
            else if (PointerPosition[0].Equals("lb"))
            {
                lbottom.Visible = true;
                rbottom.Visible = false;
                ltop.Visible = false;
                rtop.Visible = false;
            }
            else if (PointerPosition[0].Equals("rb"))
            {
                rbottom.Visible = true;
                lbottom.Visible = false;
                ltop.Visible = false;
                rtop.Visible = false;
            }
            toolTip.Visible = true;
            nxt.Text = "Next";
            toolTip.Location = PointData[0];
            toolTipText.Text = Message[0];
        }

        private void cls_Click(object sender, EventArgs e)
        {
            toolTip.Visible = false;
        }

        private void callNextToolTip()
        {
            try
            {
                if (PointerPosition[runner].Equals("lt"))
                {
                    ltop.Visible = true;
                    rtop.Visible = false;
                    lbottom.Visible = false;
                    rbottom.Visible = false;
                }
                else if (PointerPosition[runner].Equals("rt"))
                {
                    rtop.Visible = true;
                    lbottom.Visible = false;
                    rbottom.Visible = false;
                    ltop.Visible = false;
                }
                else if (PointerPosition[runner].Equals("lb"))
                {
                    lbottom.Visible = true;
                    rbottom.Visible = false;
                    ltop.Visible = false;
                    rtop.Visible = false;
                }
                else if (PointerPosition[runner].Equals("rb"))
                {
                    rbottom.Visible = true;
                    lbottom.Visible = false;
                    ltop.Visible = false;
                    rtop.Visible = false;
                }
                else { }
            }
            catch (Exception ex) { }

            if (PointData.Length == runner)
            {
                runner = 0;
                toolTip.Visible = false;
            }
            else if (PointData.Length - 1 == runner)
            {
                nxt.Text = "Finish";
                toolTip.Location = PointData[runner];
                toolTipText.Text = Message[runner];
                runner++;
            }
            else
            {
                nxt.Text = "Next";
                toolTip.Location = PointData[runner];
                toolTipText.Text = Message[runner];
                runner++;
            }
        }

        private void nxt_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                callNextToolTip();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                callNextToolTip();
            }
        }

        private void closeToolTip()
        {
            toolTip.Visible = false;
        }

        private void toolTipText_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (TooTipState)
            {
                toolTipText.Font = new Font(toolTipText.Font.FontFamily, 10);
                nxt.Visible = true;
                if (runner != 0)
                {
                    toolTip.Location = PointData[runner - 1];
                }
                else
                {
                    toolTip.Location = PointData[runner];
                }
                toolTip.Width = 321;
                toolTip.Height = 158;
                TooTipState = false;
            }
            else
            {
                toolTipText.Font = new Font(toolTipText.Font.FontFamily, 16);
                nxt.Visible = false;
                toolTip.Location = new Point(100, 100);
                toolTip.Width = this.Width - 200;
                toolTip.Height = this.Height - 200;
                TooTipState = true;
            }
        }
    }
}
