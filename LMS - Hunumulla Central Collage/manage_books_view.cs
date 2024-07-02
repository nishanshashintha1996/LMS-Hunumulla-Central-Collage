using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class manage_books_view : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public string get_book_id = "", get_registration_number = "", get_book_name = "", get_author = "", get_publication_date = "",
            get_pages = "", get_price = "", get_source = "", get_date = "", get_category = "", get_comments = "", get_status = "";
        public string librarian_id = "";
        public string librarian_name = "";
        public string librarian_email = "";
        public string librarian_username = "";
        public string last_login = "";
        public string swstateD = "";
        public string librarian_password = "";
        public string state = "";
        public string activity_view_text = "";
        SQLiteConnection conn;

        //Initializing method
        public manage_books_view(string id, string swstate)
        {
            swstateD = swstate;
            librarian_id = id;
            string q = "SELECT * FROM librarian_user";
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
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
            InitializeComponent();
        }

        //Loading window method
        private void manage_books_view_Load(object sender, EventArgs e)
        {
            setChartItems(dropdown);
            new ActivityLogDataFetch("Manage Books", "Open Manage Book Panel");
            timer2.Start();
            timer3.Start();
            loading_view view = new loading_view();
            view.ShowDialog();
            welcome_panel.Show();
            category_view.Hide();
            add_edit_book_main_view_panel.Hide();
            this.WindowState = FormWindowState.Maximized;
            timer1.Start();
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            date.Text = DateTime.Now.ToLongDateString();
            user_name_lable.Text = librarian_name;
            activity_view_text_setter("Hunumulla Central Collage LMS.@Version 1.3.0 @");


            all_tr.Text = static_book_counter("Returned");
            av_books.Text = static_counter_all("Active");
            re_books.Text = static_counter_all("Deleted");
            is_books.Text = static_book_counter("Issued");
            av_cat.Text = static_category_counter();

            string[] names = { "Active", "Deleted", "Issued" , "Returned" };
            string[] values = { static_counter_all("Active"), static_counter_all("Deleted"), static_book_counter("Issued"), static_book_counter("Returned")};
            fillChart(names, values, "Books Of the System");

        }

        private string static_book_counter(string val)
        {
            string sql_query = "SELECT * FROM issue_book WHERE transaction_state = @value";
            SQLiteCommand cmd = new SQLiteCommand(sql_query, conn);
            cmd.Parameters.AddWithValue("@value", val);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows.Count.ToString();
        }

        private string static_category_counter()
        {
            string sql_query = "SELECT * FROM book_category";
            SQLiteCommand cmd = new SQLiteCommand(sql_query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows.Count.ToString();
        }

        //btn user view
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            manage_users_view view = new manage_users_view(librarian_id, swstateD);
            this.Hide();
            view.ShowDialog();
        }

        //btn dashboard
        private void add_student_Click(object sender, EventArgs e)
        {
            Dashboard view = new Dashboard(librarian_id,"", swstateD);
            this.Hide();
            view.ShowDialog();
        }

        //btn maximize
        private void maximize_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                activity_view_text_setter(">>> Window - Normal State @");
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                activity_view_text_setter(">>> Window - Maximized State @");
            }
        }

        //btn minimize
        private void minimize_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            activity_view_text_setter(">>> Window - Minimized State @");
        }

        //console text setter 
        public void activity_view_text_setter(String receiveText)
        {
            receiveText = receiveText.Replace("@", System.Environment.NewLine);
            activity_view_text += receiveText;
            activity_view.Text = activity_view_text;
            activity_view.SelectionStart = activity_view.Text.Length;
            activity_view.ScrollToCaret();
        }

        public string random_book_registration_number_generator()
        {
            bool run_condition = true;
            string book_reg_number_text = "";
            while (run_condition)
            {
                Random rnd = new Random();
                int number = rnd.Next(1000, 10000);
                book_reg_number_text = "HC" + number.ToString() + "C" + (number - 999).ToString();
                string sql_query = "SELECT * FROM library_books WHERE registration_number = @value";
                SQLiteCommand cmd = new SQLiteCommand(sql_query, conn);
                cmd.Parameters.AddWithValue("@value", book_reg_number_text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    run_condition = false;
                }
            }
            return book_reg_number_text;
        }

        private void fillChart(string[] names, string[] values, string title)
        {
            //chart title
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart1.Titles.Add(title);
            chart1.Series.Add("UserCount");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            chart1.Series[0]["PieLabelStyle"] = "Disabled";
            if (names.Length.ToString().Equals(values.Length.ToString()))
            {
                for (int i = 0; i < names.Length; i++)
                {
                    chart1.Series["UserCount"].Points.AddXY(names[i], values[i]);
                }
            }
        }

        private void add_book_main_btn_Click(object sender, EventArgs e)
        {
            book_reg_number.Enabled = false;
            book_name.Text = "";
            author.Text = "";
            pub_date.Text = DateTime.Now.ToLongDateString();
            pages_count.Text = "0";
            price.Text = "";
            source.Text = "";
            category.Text = "";
            comment.Text = "";
            string book_registration_number_temp = random_book_registration_number_generator();
            if (!book_registration_number_temp.Equals(""))
            {
                btn_barcode_view.Hide();
                btn_delete.Hide();
                btn_save.Hide();
                btn_recover.Hide();
                clear_data.Show();
                add_book.Show();
                welcome_panel.Hide();
                window_name.Text = "Add New Book";
                search_panel1.Hide();
                add_edit_book_main_view_panel.Show();
                activity_view_text_setter(">>> Add Book Window Opened @");
                book_reg_number.Text = book_registration_number_temp;
            }
        }

        

        private void edit_book_main_btn_Click(object sender, EventArgs e)
        {
            clear_data.Hide();
            add_book.Hide();
            welcome_panel.Hide();
            reload_search_data();
            search_panel1.Show();
            book_reg_number.Enabled = true;
            window_name.Text = "Edit Book";
            add_edit_book_main_view_panel.Show();
            activity_view_text_setter(">>> Edit Book Window Opened @");
        }

        

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            string[] names = { "Active", "Deleted", "Issued", "Returned" };
            string[] values = { static_counter_all("Active"), static_counter_all("Deleted"), static_book_counter("Issued"), static_book_counter("Returned") };
            fillChart(names, values, "Books Of the System");

            welcome_panel.Show();
            av_cat.Text = static_category_counter();
            is_books.Text = static_book_counter("Issued");
            av_books.Text = static_counter_all("Active");
            re_books.Text = static_counter_all("Deleted");
            all_tr.Text = static_book_counter("Returned");
            add_edit_book_main_view_panel.Hide();
            activity_view_text_setter(">>> Window Closed @");
        }

        

        private void add_book_Click(object sender, EventArgs e)
        {
            
            if (!book_reg_number.Text.Equals("") && !book_name.Text.Equals("") && !author.Text.Equals("") && !pub_date.Text.Equals("") && !pages_count.Text.Equals("")
                && !price.Text.Equals("") && !source.Text.Equals(""))
            {
                string q = "SELECT cat_name FROM book_category WHERE cat_name = @category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int page_int = Int32.Parse(pages_count.Text);
                    if (page_int > 0)
                    {
                        float float_price = float.Parse(price.Text);
                        if (float_price > 0)
                        {

                            string stringQuery = "INSERT INTO library_books (registration_number, book_name, author, publication_date, pages, price, source, date, category, comments, status)" +
                                "VALUES(@reg_nu, @b_name, @author, @pub_date, @page, @price, @source, @date, @cat, @com, 'Active')";
                            using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                            {
                                SqliteCmd.Parameters.AddWithValue("@reg_nu", book_reg_number.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@b_name", book_name.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@author", author.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@pub_date", pub_date.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@page", pages_count.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@price", price.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@source", source.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@date", DateTime.Now.ToLongDateString()); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@cat", category.Text); //TEXT
                                SqliteCmd.Parameters.AddWithValue("@com", comment.Text); //TEXT

                                try
                                {
                                    SqliteCmd.ExecuteNonQuery();
                                    activity_view_text_setter(">>> -------------------------------------------------------------------------------- @>>> Success -> The Book Added To The System @>>> Name :" + book_name.Text + "@>>> Book Registration Number : " + book_reg_number.Text + " @>>> Registed Date : " + DateTime.Now.ToLongDateString() + "@>>> -------------------------------------------------------------------------------- @");
                                    barcode_printer_view view = new barcode_printer_view(book_reg_number.Text, book_name.Text, author.Text, pages_count.Text, price.Text, librarian_id);
                                    book_reg_number.Text = random_book_registration_number_generator();
                                    book_name.Text = "";
                                    author.Text = "";
                                    pub_date.Text = DateTime.Now.ToLongDateString();
                                    pages_count.Text = "0";
                                    price.Text = "";
                                    source.Text = "";
                                    category.Text = "";
                                    comment.Text = "";
                                    view.ShowDialog();
                                }
                                catch (Exception ex)
                                {
                                    activity_view_text_setter(">>> Error -> Add Book Syntax Error @");
                                    MessageBox.Show("There is an error. Please contact developer !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Price Should be Greater than 0.00 LKR", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Page Count Should be Greater than 0 pages", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Unknown Category !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pleasr fill all required fields", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void clear_data_Click(object sender, EventArgs e)
        {
            book_name.Text = "";
            author.Text = "";
            pub_date.Text = DateTime.Now.ToLongDateString();
            pages_count.Text = "0";
            price.Text = "";
            source.Text = "";
            category.Text = "";
            comment.Text = "";
        }

        private void settings_Click(object sender, EventArgs e)
        {
            settings_view view = new settings_view();
            view.ShowDialog();
        }
        
        private void search_box_Enter(object sender, EventArgs e)
        {
            if (search_box.Text.Equals("Search Book...."))
            {
                reload_search_data();
                search_box.Text = "";
            }
        }

        private void search_box_Leave(object sender, EventArgs e)
        {
            if (search_box.Text.Equals(""))
            {
                search_box.Text = "Search Book....";
                reload_search_data();
            }
        }

        

        private void search_box_OnValueChanged(object sender, EventArgs e)
        {
            if (!search_box.Equals("") && !dropdown.selectedValue.Equals(""))
            {
                try
                {
                    //Testing Area --------------- Start
                    string sql_query = "SELECT registration_number,book_name,author,pages,price,category FROM library_books"+
                        " WHERE (registration_number LIKE @keyword "+
                        "OR book_name LIKE @keyword "+
                        "OR author LIKE @keyword) "+
                        "AND category LIKE @cat";
                    SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                    cmd_fill.Parameters.AddWithValue("@keyword", "%" + search_box.Text + "%");
                    cmd_fill.Parameters.AddWithValue("@cat", "%" + dropdown.selectedValue + "%");
                    SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                    DataTable dtable = new DataTable();
                    dadapter.Fill(dtable);
                    grid_search_result.DataSource = dtable;
                }
                catch (Exception ex)
                {

                }
            }
        }

        

        private void reload_search_result_Click(object sender, EventArgs e)
        {
            reload_search_data();
        }

        private void manage_reports_Click(object sender, EventArgs e)
        {
            report_generating_view view = new report_generating_view();
            view.ShowDialog();
        }

        private void resource_statistics_Click(object sender, EventArgs e)
        {
            ResourceStatics view = new ResourceStatics();
            view.ShowDialog();
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
        

        private void timer2_Tick(object sender, EventArgs e)
        {
            SoftwareUserData data = new SoftwareUserData();
            label5.Text = data.getInstitiuteName();
        }

        private void category_OnValueChanged(object sender, EventArgs e)
        {
            if (!category.Equals(""))
            {
                string q = "SELECT cat_name FROM book_category WHERE cat_name LIKE @category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", "%" + category.Text + "%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                category_view.DataSource = dt;
            }
            else
            {
                string q = "SELECT cat_name FROM book_category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                category_view.DataSource = dt;
            }
            
        }

        private void btn_help_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            time.Text = "Time :" + DateTime.Now.ToLongTimeString();
            timer3.Start();
        }

        public void reload_search_data()
        {
            if (!dropdown.selectedValue.Equals(""))
            {
                string sql_query = "SELECT registration_number,book_name,author,pages,price,category FROM library_books WHERE category LIKE @cat";
                SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
                cmd_fill.Parameters.AddWithValue("@cat", "%" + dropdown.selectedValue + "%");
                SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
                DataTable dtable = new DataTable();
                dadapter.Fill(dtable);
                grid_search_result.DataSource = dtable;
                activity_view_text_setter(">>> Reloaded -> Search Results @");
            }
        }

        // Cell Click Activity

        private void grid_search_result_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid_search_result.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    grid_search_result.CurrentRow.Selected = true;
                    
                    //book_edit_function

                    book_edit_function(grid_search_result.Rows[e.RowIndex].Cells["registration_number"].FormattedValue.ToString());
                    search_panel1.Hide();
                    activity_view_text_setter(">>> Opened Window -> Edit Book Details @");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        public void book_edit_function(string book_registration_number)
        {
            string sql_query = "SELECT * FROM library_books WHERE registration_number = @urn";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@urn", book_registration_number);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);

            get_book_id = dtable.Rows[0]["book_id"].ToString();

            book_reg_number.Text = dtable.Rows[0]["registration_number"].ToString();
            get_registration_number = dtable.Rows[0]["registration_number"].ToString();
            book_reg_number.Enabled = false;

            book_name.Text = dtable.Rows[0]["book_name"].ToString();
            get_book_name = dtable.Rows[0]["book_name"].ToString();

            author.Text = dtable.Rows[0]["author"].ToString();
            get_author = dtable.Rows[0]["author"].ToString();

            pub_date.Text = dtable.Rows[0]["publication_date"].ToString();
            get_publication_date = dtable.Rows[0]["publication_date"].ToString();

            pages_count.Text = dtable.Rows[0]["pages"].ToString();
            get_pages = dtable.Rows[0]["pages"].ToString();

            price.Text = dtable.Rows[0]["price"].ToString();
            get_price = dtable.Rows[0]["price"].ToString();

            source.Text = dtable.Rows[0]["source"].ToString();
            get_source = dtable.Rows[0]["source"].ToString();

            category.Text = dtable.Rows[0]["category"].ToString();
            get_category = dtable.Rows[0]["category"].ToString();

            comment.Text = dtable.Rows[0]["comments"].ToString();
            get_comments = dtable.Rows[0]["comments"].ToString();

            if (dtable.Rows[0]["status"].ToString().Equals("Active"))
            {
                book_name.Enabled = true;
                author.Enabled = true;
                pub_date.Enabled = true;
                pages_count.Enabled = true;
                price.Enabled = true;
                source.Enabled = true;
                category.Enabled = true;
                comment.Enabled = true;

                btn_barcode_view.Show();
                btn_barcode_view.Enabled = true;
                btn_delete.Show();
                btn_save.Show();
                btn_save.Enabled = true;
                btn_recover.Hide();
            }
            if (dtable.Rows[0]["status"].ToString().Equals("Deleted"))
            {
                book_reg_number.Enabled = false;
                book_name.Enabled = false;
                author.Enabled = false;
                pub_date.Enabled = false;
                pages_count.Enabled = false;
                price.Enabled = false;
                source.Enabled = false;
                category.Enabled = false;
                comment.Enabled = false;

                btn_barcode_view.Show();
                btn_barcode_view.Enabled = false;
                btn_delete.Hide();
                btn_save.Enabled = false;
                btn_recover.Show();
            }

        }

        //=============================================================================
        //btn processes


        private void btn_save_Click_1(object sender, EventArgs e)
        {
            if (!book_name.Text.Equals(get_book_name))
            {
                string updatecQuery = "UPDATE library_books SET book_name = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", book_name.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> book_name @");
                    get_book_name = book_name.Text;
                }
            }

            if (!author.Text.Equals(get_author))
            {
                string updatecQuery = "UPDATE library_books SET author = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", author.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> author @");
                    get_author = author.Text;
                }
            }

            if (!pub_date.Text.Equals(get_publication_date))
            {
                string updatecQuery = "UPDATE library_books SET publication_date = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", pub_date.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> publication_date @");
                    get_publication_date = pub_date.Text;
                }
            }

            if (!pages_count.Text.Equals(get_pages))
            {
                string updatecQuery = "UPDATE library_books SET pages = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", pages_count.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> pages @");
                    get_pages = pages_count.Text;
                }
            }

            if (!price.Text.Equals(get_price))
            {
                string updatecQuery = "UPDATE library_books SET price = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", price.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> price @");
                    get_price = price.Text;
                }
            }

            if (!source.Text.Equals(get_source))
            {
                string updatecQuery = "UPDATE library_books SET source = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", source.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> source @");
                    get_source = source.Text;
                }
            }

            if (!category.Text.Equals(get_category))
            {


                string q = "SELECT cat_name FROM book_category WHERE cat_name = @category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string updatecQuery = "UPDATE library_books SET category = @updateData WHERE book_id = @book_Id";
                    using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                    {
                        updatec.Parameters.AddWithValue("@updateData", category.Text);
                        updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                        updatec.ExecuteNonQuery();
                        activity_view_text_setter(">>> Updated Book -> category @");
                        get_category = category.Text;
                    }
                }
                else
                {
                    MessageBox.Show("Unknown Category !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (!comment.Text.Equals(get_comments))
            {
                string updatecQuery = "UPDATE library_books SET comments = @updateData WHERE book_id = @book_Id";
                using (SQLiteCommand updatec = new SQLiteCommand(updatecQuery, conn))
                {
                    updatec.Parameters.AddWithValue("@updateData", comment.Text);
                    updatec.Parameters.AddWithValue("@book_Id", get_book_id);
                    updatec.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> comments @");
                    get_comments = comment.Text;
                }
            }
        }

        private void btn_recover_Click_1(object sender, EventArgs e)
        {
            string recoverQuery = "UPDATE library_books SET status = 'Active' WHERE book_id = @bookId";
            using (SQLiteCommand recover = new SQLiteCommand(recoverQuery, conn))
            {
                recover.Parameters.AddWithValue("@bookId", get_book_id);
                recover.ExecuteNonQuery();
                activity_view_text_setter(">>> Updated Book -> Recovered @");
                clear_data.Hide();
                add_book.Hide();
                reload_search_data();
                search_panel1.Show();
                book_reg_number.Enabled = true;
                window_name.Text = "Edit Book";
                add_edit_book_main_view_panel.Show();
                activity_view_text_setter(">>> Edit Book Window Opened @");
            }
        }

        private void dropdown_onItemSelected(object sender, EventArgs e)
        {
            reload_search_data();
        }

        private void setChartItems(Bunifu.Framework.UI.BunifuDropdown x)
        {
            x.AddItem("");
            string q = "SELECT cat_name FROM book_category";
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@category", category.Text);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int xx = 0; xx < dt.Rows.Count; xx++)
                {
                    x.AddItem(dt.Rows[xx]["cat_name"].ToString());
                }
            }
            x.selectedIndex = 0;
        }

        private void btn_barcode_view_Click_1(object sender, EventArgs e)
        {
            barcode_printer_view view = new barcode_printer_view(get_registration_number, get_book_name, get_author, get_pages, get_price, librarian_id);
            view.ShowDialog();
        }

        private void btn_delete_Click_1(object sender, EventArgs e)
        {
            string qr = "SELECT * FROM issue_book WHERE book_registration_number=@b_num AND transaction_state='Issued'";
            SQLiteCommand cmdr = new SQLiteCommand(qr, conn);
            cmdr.Parameters.AddWithValue("@b_num", get_registration_number);
            SQLiteDataAdapter dar = new SQLiteDataAdapter(cmdr);
            DataTable dtr = new DataTable();
            dar.Fill(dtr);
            if (dtr.Rows.Count == 0)
            {
                string recoverQuery = "UPDATE library_books SET status = 'Deleted' WHERE book_id = @bookId";
                using (SQLiteCommand recover = new SQLiteCommand(recoverQuery, conn))
                {
                    recover.Parameters.AddWithValue("@bookId", get_book_id);
                    recover.ExecuteNonQuery();
                    activity_view_text_setter(">>> Updated Book -> Deleted @");
                    clear_data.Hide();
                    add_book.Hide();
                    reload_search_data();
                    search_panel1.Show();
                    book_reg_number.Enabled = true;
                    window_name.Text = "Edit Book";
                    add_edit_book_main_view_panel.Show();
                    activity_view_text_setter(">>> Edit Book Window Opened @");
                }
            }
            else
            {
                MessageBox.Show("This book is not available in the Library !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void category_Enter(object sender, EventArgs e)
        {
            string sql_query = "SELECT cat_name FROM book_category";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            category_view.DataSource = dtable;
            category_view.Show();
        }

        private void category_Leave(object sender, EventArgs e)
        {
            string q = "SELECT cat_name FROM book_category";
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@category", category.Text);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                category_view.Hide();
            }
        }

        private void category_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            category.Text = category_view.Rows[e.RowIndex].Cells["cat_name"].FormattedValue.ToString();
            category_view.Hide();
        }

        public string static_counter_all(string value)
        {
            string sql_query = "SELECT * FROM library_books WHERE status = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@value", value);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            //search_result_view.DataSource = dtable;
            return dtable.Rows.Count.ToString();
        }

    }
}
