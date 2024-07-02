namespace LMS___Hunumulla_Central_Collage
{
    partial class bot_configuration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bot_configuration));
            this.active = new System.Windows.Forms.RadioButton();
            this.deactive = new System.Windows.Forms.RadioButton();
            this.btn_save = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.activity_panel = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.add_edit_book_close = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.add_edit_book_close)).BeginInit();
            this.SuspendLayout();
            // 
            // active
            // 
            this.active.AutoSize = true;
            this.active.Cursor = System.Windows.Forms.Cursors.Hand;
            this.active.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.active.ForeColor = System.Drawing.Color.White;
            this.active.Location = new System.Drawing.Point(45, 106);
            this.active.Name = "active";
            this.active.Size = new System.Drawing.Size(92, 22);
            this.active.TabIndex = 48;
            this.active.TabStop = true;
            this.active.Text = "Active Bot";
            this.active.UseVisualStyleBackColor = true;
            // 
            // deactive
            // 
            this.deactive.AutoSize = true;
            this.deactive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deactive.ForeColor = System.Drawing.Color.White;
            this.deactive.Location = new System.Drawing.Point(182, 106);
            this.deactive.Name = "deactive";
            this.deactive.Size = new System.Drawing.Size(110, 22);
            this.deactive.TabIndex = 49;
            this.deactive.TabStop = true;
            this.deactive.Text = "Deactive Bot";
            this.deactive.UseVisualStyleBackColor = true;
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.btn_save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_save.Location = new System.Drawing.Point(402, 372);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(126, 31);
            this.btn_save.TabIndex = 50;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(60, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 22);
            this.label4.TabIndex = 58;
            this.label4.Text = "Book Details View";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(28, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 21);
            this.label1.TabIndex = 60;
            this.label1.Text = "Bot Activation";
            // 
            // activity_panel
            // 
            this.activity_panel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activity_panel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.activity_panel.Location = new System.Drawing.Point(125, 244);
            this.activity_panel.Name = "activity_panel";
            this.activity_panel.Size = new System.Drawing.Size(295, 95);
            this.activity_panel.TabIndex = 63;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::LMS___Hunumulla_Central_Collage.Properties.Resources.option;
            this.pictureBox4.Location = new System.Drawing.Point(15, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(39, 22);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 59;
            this.pictureBox4.TabStop = false;
            // 
            // add_edit_book_close
            // 
            this.add_edit_book_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add_edit_book_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(153)))));
            this.add_edit_book_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.add_edit_book_close.Image = global::LMS___Hunumulla_Central_Collage.Properties.Resources.close_small1;
            this.add_edit_book_close.ImageActive = null;
            this.add_edit_book_close.Location = new System.Drawing.Point(496, 12);
            this.add_edit_book_close.Name = "add_edit_book_close";
            this.add_edit_book_close.Size = new System.Drawing.Size(32, 31);
            this.add_edit_book_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.add_edit_book_close.TabIndex = 47;
            this.add_edit_book_close.TabStop = false;
            this.add_edit_book_close.Zoom = 10;
            this.add_edit_book_close.Click += new System.EventHandler(this.add_edit_book_close_Click);
            // 
            // bot_configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(38)))), ((int)(((byte)(105)))));
            this.ClientSize = new System.Drawing.Size(540, 415);
            this.Controls.Add(this.activity_panel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.deactive);
            this.Controls.Add(this.active);
            this.Controls.Add(this.add_edit_book_close);
            this.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "bot_configuration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bot_configuration";
            this.Load += new System.EventHandler(this.bot_configuration_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.user_view_common_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.user_view_common_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.user_view_common_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.add_edit_book_close)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuImageButton add_edit_book_close;
        private System.Windows.Forms.RadioButton active;
        private System.Windows.Forms.RadioButton deactive;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label activity_panel;
    }
}