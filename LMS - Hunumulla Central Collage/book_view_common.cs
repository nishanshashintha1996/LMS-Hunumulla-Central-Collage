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
    public partial class book_view_common : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public string book_reg_id = "";
        SQLiteConnection conn;
        public book_view_common(string b_id)
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            InitializeComponent();
            book_reg_id = b_id;
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void book_view_common_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM library_books WHERE registration_number = @value";
            SQLiteCommand cmd_fill = new SQLiteCommand(query, conn);
            cmd_fill.Parameters.AddWithValue("@value", book_reg_id);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count == 1)
            {
                reg_num.Text = dtable.Rows[0]["registration_number"].ToString();
                reg_num.Visible = true;
                b_name.Text = dtable.Rows[0]["book_name"].ToString();
                b_name.Visible = true;
                author.Text = dtable.Rows[0]["author"].ToString();
                author.Visible = true;
                pub_date.Text = dtable.Rows[0]["publication_date"].ToString();
                pub_date.Visible = true;
                pages.Text = dtable.Rows[0]["pages"].ToString();
                pages.Visible = true;
                price.Text = dtable.Rows[0]["price"].ToString();
                price.Visible = true;
                source.Text = dtable.Rows[0]["source"].ToString();
                source.Visible = true;
                category.Text = dtable.Rows[0]["category"].ToString();
                category.Visible = true;
                status.Text = dtable.Rows[0]["status"].ToString();
                status.Visible = true;

                if (status.Text.Equals("Active"))
                {
                    status.BackColor = Color.Green;
                }
                else
                {
                    status.BackColor = Color.Red;
                }
            }
        }

        // Window Movement 

        private void book_view_common_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void book_view_common_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void book_view_common_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
    }
}
