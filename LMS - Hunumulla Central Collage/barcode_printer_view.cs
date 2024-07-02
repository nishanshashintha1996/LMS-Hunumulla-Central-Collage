using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using System.Data.SQLite;
using System.Drawing.Printing;

namespace LMS___Hunumulla_Central_Collage
{
    public partial class barcode_printer_view : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        Image File;
        SQLiteConnection conn;
        public string book_reg_number,book_name_s, book_author,book_pages,book_price;
        public string librarian_id;
        public barcode_printer_view(string reg_number,string n,string a, string b, string c, string id)
        {
            conn = new SQLiteConnection("Data Source=" + setup_data.getDataBaseLocation() + setup_data.getDataBaseName() + ";Version=3;");
            conn.Open();
            book_reg_number = reg_number;
            book_name_s = n;
            book_author = a;
            book_pages = b;
            book_price = c;
            librarian_id = id;
            InitializeComponent();
        }

        private void barcode_printer_view_Load(object sender, EventArgs e)
        {
            string sql_query = "SELECT * FROM library_books WHERE registration_number = @urn";
            SQLiteCommand cmd_fill = new SQLiteCommand(sql_query, conn);
            cmd_fill.Parameters.AddWithValue("@urn", book_reg_number);
            SQLiteDataAdapter dadapter = new SQLiteDataAdapter(cmd_fill);
            DataTable dtable = new DataTable();
            dadapter.Fill(dtable);

            //,,,

            book_name.Text = "Book Name : " + book_name_s;
            author.Text = "Book Author : " + book_author;
            pages.Text = "Number of Pages : " + book_pages;
            price.Text = "Book Price : " + book_price + "LKR";

            BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
            pic_barcode.Image = writer.Write(book_reg_number);
            File = pic_barcode.Image;

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void save_barcode_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();

            f.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            if (f.ShowDialog() == DialogResult.OK)
            {
                new ActivityLogDataFetch("Saved Barcode As a Image","Book ID : " + book_reg_number);
                File.Save(f.FileName);
            }
        }

        private void print_barcode_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_PrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                new ActivityLogDataFetch("Printed Barcode / Printed Barcode As a pdf", "Book ID : " + book_reg_number);
                doc.Print();
            }
                
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(pic_barcode.Width, pic_barcode.Height);
            pic_barcode.DrawToBitmap(bm, new Rectangle(0, 0, pic_barcode.Width, pic_barcode.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }

        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
