using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class category_setteings_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        SQLiteConnection conn;
        public string cat_id;
        public category_setteings_view()
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            InitializeComponent();
        }

        private void category_setteings_view_Load(object sender, EventArgs e)
        {
            add_new_panel.Hide();
            edit_panel.Hide();
            category_view.Hide();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_new_Click(object sender, EventArgs e)
        {
            add_new_panel.Show();
            edit_panel.Hide();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!category_text.Text.Equals(""))
            {
                string q = "SELECT cat_name FROM book_category WHERE cat_name = @category";
                SQLiteCommand cmd = new SQLiteCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category_text.Text);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    string stringQuery = "INSERT INTO book_category (cat_name, cat_status, date)" +
                                "VALUES(@cat_name,'Active', @date)";
                    using (SQLiteCommand SqliteCmd = new SQLiteCommand(stringQuery, conn))
                    {
                        SqliteCmd.Parameters.AddWithValue("@cat_name", category_text.Text); //TEXT
                        SqliteCmd.Parameters.AddWithValue("@date", DateTime.Now.ToLongDateString()); //TEXT
                        try
                        {
                            SqliteCmd.ExecuteNonQuery();
                            string receiveText = "Category Added @@Category Name :" + category_text.Text;
                            receiveText = receiveText.Replace("@", System.Environment.NewLine);
                            category_status.Text = receiveText;
                            new ActivityLogDataFetch("Category Settings", "New Category Added : " + receiveText);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Existing Category Name !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Category Name is Empty !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            add_new_panel.Hide();
            edit_panel.Show();
        }

        private void existing_cat_name_Enter(object sender, EventArgs e)
        {
            string sql_query = "SELECT cat_id,cat_name FROM book_category";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            category_view.DataSource = dtable;
            category_view.Show();
        }

        private void existing_cat_name_Leave(object sender, EventArgs e)
        {

        }

        private void category_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            existing_cat_name.Text = category_view.Rows[e.RowIndex].Cells["cat_name"].FormattedValue.ToString();
            cat_id = category_view.Rows[e.RowIndex].Cells["cat_id"].FormattedValue.ToString();
            category_view.Hide();
        }

        private void edit_save_btn_Click(object sender, EventArgs e)
        {
            string q = "SELECT cat_name FROM book_category WHERE cat_name = @category";
            SQLiteCommand cmd = new SQLiteCommand(q, conn);
            cmd.Parameters.AddWithValue("@category", existing_cat_name.Text);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                if (!new_cat_name.Text.Equals(""))
                {
                    if (!existing_cat_name.Text.Equals(new_cat_name.Text))
                    {
                        string recoverQuery = "UPDATE book_category SET cat_name = @value WHERE cat_id = @cat_id";
                        using (SQLiteCommand recover = new SQLiteCommand(recoverQuery, conn))
                        {
                            recover.Parameters.AddWithValue("@cat_id", cat_id);
                            recover.Parameters.AddWithValue("@value", new_cat_name.Text);
                            recover.ExecuteNonQuery();
                            string receiveText = "Category Updated @@Category Name :" + new_cat_name.Text;
                            receiveText = receiveText.Replace("@", System.Environment.NewLine);
                            edit_status.Text = receiveText;
                            new ActivityLogDataFetch("Category Settings", "Category Updated : " + receiveText);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Same Name !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Field !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Wrong Name !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void category_setteings_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void category_setteings_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void category_setteings_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
    }
}
