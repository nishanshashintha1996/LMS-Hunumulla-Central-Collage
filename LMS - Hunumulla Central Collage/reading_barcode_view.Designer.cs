namespace LMS___Hunumulla_Central_Collage
{
    partial class reading_barcode_view
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reading_barcode_view));
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.add_edit_book_close = new Bunifu.Framework.UI.BunifuImageButton();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.auto_book_reg_number = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.manual_book_reg_number = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.add_edit_book_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 22);
            this.label1.TabIndex = 56;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox1.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(15, 234);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 26);
            this.checkBox1.TabIndex = 57;
            this.checkBox1.Text = "Manual Enter";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // add_edit_book_close
            // 
            this.add_edit_book_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add_edit_book_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.add_edit_book_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.add_edit_book_close.Image = global::LMS___Hunumulla_Central_Collage.Properties.Resources.close_small1;
            this.add_edit_book_close.ImageActive = null;
            this.add_edit_book_close.Location = new System.Drawing.Point(482, 12);
            this.add_edit_book_close.Name = "add_edit_book_close";
            this.add_edit_book_close.Size = new System.Drawing.Size(32, 31);
            this.add_edit_book_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.add_edit_book_close.TabIndex = 50;
            this.add_edit_book_close.TabStop = false;
            this.add_edit_book_close.Zoom = 10;
            this.add_edit_book_close.Click += new System.EventHandler(this.add_edit_book_close_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(59, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(274, 22);
            this.label4.TabIndex = 51;
            this.label4.Text = "Waiting for Barcode Reading";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::LMS___Hunumulla_Central_Collage.Properties.Resources.option;
            this.pictureBox4.Location = new System.Drawing.Point(14, 21);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(39, 22);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 52;
            this.pictureBox4.TabStop = false;
            // 
            // auto_book_reg_number
            // 
            this.auto_book_reg_number.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.auto_book_reg_number.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.auto_book_reg_number.Font = new System.Drawing.Font("Century Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.auto_book_reg_number.Location = new System.Drawing.Point(0, 26);
            this.auto_book_reg_number.Multiline = true;
            this.auto_book_reg_number.Name = "auto_book_reg_number";
            this.auto_book_reg_number.Size = new System.Drawing.Size(500, 72);
            this.auto_book_reg_number.TabIndex = 54;
            this.auto_book_reg_number.Text = "Waiting For Reading";
            this.auto_book_reg_number.UseWaitCursor = true;
            this.auto_book_reg_number.TextChanged += new System.EventHandler(this.auto_book_reg_number_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.auto_book_reg_number);
            this.panel1.Location = new System.Drawing.Point(13, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(501, 123);
            this.panel1.TabIndex = 55;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Century Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(500, 72);
            this.textBox1.TabIndex = 55;
            this.textBox1.Text = "Waiting For Reading";
            this.textBox1.UseWaitCursor = true;
            // 
            // manual_book_reg_number
            // 
            this.manual_book_reg_number.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.manual_book_reg_number.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.manual_book_reg_number.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.manual_book_reg_number.Enabled = false;
            this.manual_book_reg_number.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.manual_book_reg_number.Location = new System.Drawing.Point(15, 266);
            this.manual_book_reg_number.Name = "manual_book_reg_number";
            this.manual_book_reg_number.Size = new System.Drawing.Size(494, 31);
            this.manual_book_reg_number.TabIndex = 58;
            this.manual_book_reg_number.Text = "Enter Book Registration Number";
            this.manual_book_reg_number.TextChanged += new System.EventHandler(this.manual_book_reg_number_TextChanged);
            // 
            // reading_barcode_view
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(38)))), ((int)(((byte)(105)))));
            this.ClientSize = new System.Drawing.Size(526, 319);
            this.Controls.Add(this.manual_book_reg_number);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.add_edit_book_close);
            this.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "reading_barcode_view";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "reading_barcode_view";
            this.Load += new System.EventHandler(this.reading_barcode_view_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.reading_barcode_view_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.reading_barcode_view_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.reading_barcode_view_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.add_edit_book_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private Bunifu.Framework.UI.BunifuImageButton add_edit_book_close;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.TextBox auto_book_reg_number;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox manual_book_reg_number;
        private System.Windows.Forms.TextBox textBox1;
    }
}