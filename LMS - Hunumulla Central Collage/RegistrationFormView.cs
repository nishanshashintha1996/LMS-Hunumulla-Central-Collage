using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS___Hunumulla_Central_Collage
{

    public partial class RegistrationFormView : Form
    {
        private bool mouseDown;
        private Point lastLocation;


        public RegistrationFormView()
        {
            InitializeComponent();
            editorRichText.AcceptsTab = true;
            editorRichText.WordWrap = true;
        }
        
        private void add_edit_book_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Text = "";
        }

        

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            if(printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Copy();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Undo();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.Cut();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorRichText.SelectAll();
        }

       

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            editorRichText.SelectionFont = new Font(fontDialog1.Font.FontFamily,fontDialog1.Font.Size, fontDialog1.Font.Style);
        }

        private void highlightTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            editorRichText.SelectionBackColor = colorDialog1.Color;
        }

        private void aboutLMSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tryGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(editorRichText.Text,
                editorRichText.Font,
                Brushes.Black, 
                12, 
                10,
                StringFormat.GenericTypographic);
        }

        private void editorRichText_TextChanged(object sender, EventArgs e)
        {
            if (editorRichText.Text.Length > 0)
            {
                cutToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
            }
            else
            {
                cutToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
            }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            editorRichText.SelectionColor = colorDialog1.Color;
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

    }
}
