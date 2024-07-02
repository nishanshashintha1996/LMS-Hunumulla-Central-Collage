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
    public partial class reading_barcode_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public bool targer_window_state;
        SQLiteConnection conn;
        public reading_barcode_view(bool x)
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            InitializeComponent();
            targer_window_state = x;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                manual_book_reg_number.Enabled = true;
                manual_book_reg_number.Text = "";
            }
            else
            {
                manual_book_reg_number.Enabled = false;
                manual_book_reg_number.Text = "Enter Book Registration Number";
            }
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void manual_book_reg_number_TextChanged(object sender, EventArgs e)
        {
            if (targer_window_state)
            {
                string auto_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_auto_book_reg = new SQLiteCommand(auto_book_reg, conn);
                cmd_auto_book_reg.Parameters.AddWithValue("@value_auto", auto_book_reg_number.Text);
                SQLiteDataAdapter da_auto_book_reg = new SQLiteDataAdapter(cmd_auto_book_reg);
                DataTable dt_auto_book_reg = new DataTable();
                da_auto_book_reg.Fill(dt_auto_book_reg);
                if (dt_auto_book_reg.Rows.Count == 1)
                {
                    issue_book_view view = new issue_book_view(auto_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }

                string maual_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_maual_book_reg = new SQLiteCommand(maual_book_reg, conn);
                cmd_maual_book_reg.Parameters.AddWithValue("@value_auto", manual_book_reg_number.Text);
                SQLiteDataAdapter da_maual_book_reg = new SQLiteDataAdapter(cmd_maual_book_reg);
                DataTable dt_maual_book_reg = new DataTable();
                da_maual_book_reg.Fill(dt_maual_book_reg);
                if (dt_maual_book_reg.Rows.Count == 1)
                {
                    issue_book_view view = new issue_book_view(manual_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                string auto_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_auto_book_reg = new SQLiteCommand(auto_book_reg, conn);
                cmd_auto_book_reg.Parameters.AddWithValue("@value_auto", auto_book_reg_number.Text);
                SQLiteDataAdapter da_auto_book_reg = new SQLiteDataAdapter(cmd_auto_book_reg);
                DataTable dt_auto_book_reg = new DataTable();
                da_auto_book_reg.Fill(dt_auto_book_reg);
                if (dt_auto_book_reg.Rows.Count == 1)
                {
                    receive_book view = new receive_book(auto_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }

                string maual_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_maual_book_reg = new SQLiteCommand(maual_book_reg, conn);
                cmd_maual_book_reg.Parameters.AddWithValue("@value_auto", manual_book_reg_number.Text);
                SQLiteDataAdapter da_maual_book_reg = new SQLiteDataAdapter(cmd_maual_book_reg);
                DataTable dt_maual_book_reg = new DataTable();
                da_maual_book_reg.Fill(dt_maual_book_reg);
                if (dt_maual_book_reg.Rows.Count == 1)
                {
                    receive_book view = new receive_book(manual_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }
            }
        }

        private void reading_barcode_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void reading_barcode_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void reading_barcode_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void reading_barcode_view_Load(object sender, EventArgs e)
        {
            string auto_book_reg = "SELECT registration_number FROM library_books WHERE status = 'Active'";
            SQLiteCommand cmd_auto_book_reg = new SQLiteCommand(auto_book_reg, conn);
            SQLiteDataAdapter da_auto_book_reg = new SQLiteDataAdapter(cmd_auto_book_reg);
            DataTable dt_auto_book_reg = new DataTable();
            da_auto_book_reg.Fill(dt_auto_book_reg);
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            if (dt_auto_book_reg.Rows.Count > 0)
            {
                SQLiteDataReader rdr = cmd_auto_book_reg.ExecuteReader();
                while (rdr.Read())
                {
                    autotext.Add(rdr.GetString(0).ToString());
                }
                manual_book_reg_number.AutoCompleteMode = AutoCompleteMode.Suggest;
                manual_book_reg_number.AutoCompleteSource = AutoCompleteSource.CustomSource;
                manual_book_reg_number.AutoCompleteCustomSource = autotext;
            }
        }

        private void auto_book_reg_number_TextChanged(object sender, EventArgs e)
        {
            if (targer_window_state)
            {
                string auto_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_auto_book_reg = new SQLiteCommand(auto_book_reg, conn);
                cmd_auto_book_reg.Parameters.AddWithValue("@value_auto", auto_book_reg_number.Text);
                SQLiteDataAdapter da_auto_book_reg = new SQLiteDataAdapter(cmd_auto_book_reg);
                DataTable dt_auto_book_reg = new DataTable();
                da_auto_book_reg.Fill(dt_auto_book_reg);
                if (dt_auto_book_reg.Rows.Count == 1)
                {
                    issue_book_view view = new issue_book_view(auto_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }

                string maual_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_maual_book_reg = new SQLiteCommand(maual_book_reg, conn);
                cmd_maual_book_reg.Parameters.AddWithValue("@value_auto", manual_book_reg_number.Text);
                SQLiteDataAdapter da_maual_book_reg = new SQLiteDataAdapter(cmd_maual_book_reg);
                DataTable dt_maual_book_reg = new DataTable();
                da_maual_book_reg.Fill(dt_maual_book_reg);
                if (dt_maual_book_reg.Rows.Count == 1)
                {
                    issue_book_view view = new issue_book_view(manual_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                string auto_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_auto_book_reg = new SQLiteCommand(auto_book_reg, conn);
                cmd_auto_book_reg.Parameters.AddWithValue("@value_auto", auto_book_reg_number.Text);
                SQLiteDataAdapter da_auto_book_reg = new SQLiteDataAdapter(cmd_auto_book_reg);
                DataTable dt_auto_book_reg = new DataTable();
                da_auto_book_reg.Fill(dt_auto_book_reg);
                if (dt_auto_book_reg.Rows.Count == 1)
                {
                    receive_book view = new receive_book(auto_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }

                string maual_book_reg = "SELECT * FROM library_books WHERE registration_number = @value_auto";
                SQLiteCommand cmd_maual_book_reg = new SQLiteCommand(maual_book_reg, conn);
                cmd_maual_book_reg.Parameters.AddWithValue("@value_auto", manual_book_reg_number.Text);
                SQLiteDataAdapter da_maual_book_reg = new SQLiteDataAdapter(cmd_maual_book_reg);
                DataTable dt_maual_book_reg = new DataTable();
                da_maual_book_reg.Fill(dt_maual_book_reg);
                if (dt_maual_book_reg.Rows.Count == 1)
                {
                    receive_book view = new receive_book(manual_book_reg_number.Text);
                    view.ShowDialog();
                    this.Close();
                }
            }
        }
    }
}
