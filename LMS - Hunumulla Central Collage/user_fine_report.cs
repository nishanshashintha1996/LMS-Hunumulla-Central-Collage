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
    public partial class user_fine_report : Form
    {
        SQLiteConnection conn;
        public string userRegistrationNumber = "";
        private bool mouseDown;
        private Point lastLocation;
        public user_fine_report(string id)
        {
            InitializeComponent();
            userRegistrationNumber = id;
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
        }

        private void user_fine_report_Load(object sender, EventArgs e)
        {

            name_setter();
            grid_setter();

        }

        public void name_setter()
        {
            string sql_query = "SELECT * FROM library_user WHERE user_registration_number = @reg_num";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@reg_num", userRegistrationNumber);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                user_name.Text = dtable.Rows[0]["user_fname"].ToString() + dtable.Rows[0]["user_lname"].ToString();
                user_name.Visible = true;
            }
        }

        public void grid_setter()
        {
            string sql_query = "SELECT book_registration_number,issed_date,returned_date,transaction_state,book_fine_amount,book_fine_status  FROM issue_book WHERE student_registration_number = @reg_num";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@reg_num", userRegistrationNumber);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                data_view.DataSource = dtable;
            }
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fine_calculation_view_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void fine_calculation_view_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void fine_calculation_view_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void data_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (data_view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    data_view.CurrentRow.Selected = true;
                    book_view_common view = new book_view_common(data_view.Rows[e.RowIndex].Cells["book_registration_number"].FormattedValue.ToString());
                    view.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Cell to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
