namespace CareCompass
{
    partial class Bejelentkezés
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bejelentkezés));
            this.PB_CareCompass_Logo = new System.Windows.Forms.PictureBox();
            this.Lb_Felhasznalonev = new System.Windows.Forms.Label();
            this.Lb_jelszo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_Bejelentkezes = new CareCompass.Customs.CCButton();
            this.Tb_Jelszo = new CareCompass.Customs.CCTextBox();
            this.Tb_Felhasznalonev = new CareCompass.Customs.CCTextBox();
            this.Lb_ElfelejtettJelszo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PB_CareCompass_Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_CareCompass_Logo
            // 
            this.PB_CareCompass_Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PB_CareCompass_Logo.Dock = System.Windows.Forms.DockStyle.Top;
            this.PB_CareCompass_Logo.Image = global::CareCompass.Properties.Resources.logó___fehér;
            this.PB_CareCompass_Logo.Location = new System.Drawing.Point(0, 30);
            this.PB_CareCompass_Logo.MinimumSize = new System.Drawing.Size(106, 40);
            this.PB_CareCompass_Logo.Name = "PB_CareCompass_Logo";
            this.PB_CareCompass_Logo.Size = new System.Drawing.Size(484, 120);
            this.PB_CareCompass_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PB_CareCompass_Logo.TabIndex = 0;
            this.PB_CareCompass_Logo.TabStop = false;
            // 
            // Lb_Felhasznalonev
            // 
            this.Lb_Felhasznalonev.AutoSize = true;
            this.Lb_Felhasznalonev.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Felhasznalonev.ForeColor = System.Drawing.Color.White;
            this.Lb_Felhasznalonev.Location = new System.Drawing.Point(121, 237);
            this.Lb_Felhasznalonev.Name = "Lb_Felhasznalonev";
            this.Lb_Felhasznalonev.Size = new System.Drawing.Size(46, 19);
            this.Lb_Felhasznalonev.TabIndex = 2;
            this.Lb_Felhasznalonev.Text = "Email";
            // 
            // Lb_jelszo
            // 
            this.Lb_jelszo.AutoSize = true;
            this.Lb_jelszo.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_jelszo.ForeColor = System.Drawing.Color.White;
            this.Lb_jelszo.Location = new System.Drawing.Point(121, 347);
            this.Lb_jelszo.Name = "Lb_jelszo";
            this.Lb_jelszo.Size = new System.Drawing.Size(47, 19);
            this.Lb_jelszo.TabIndex = 4;
            this.Lb_jelszo.Text = "Jelszó";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 30);
            this.panel1.TabIndex = 6;
            // 
            // Btn_Bejelentkezes
            // 
            this.Btn_Bejelentkezes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Bejelentkezes.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_Bejelentkezes.BorderColor = System.Drawing.Color.White;
            this.Btn_Bejelentkezes.BorderRadius = 10;
            this.Btn_Bejelentkezes.BorderSize = 0;
            this.Btn_Bejelentkezes.FlatAppearance.BorderSize = 0;
            this.Btn_Bejelentkezes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Bejelentkezes.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_Bejelentkezes.ForeColor = System.Drawing.Color.White;
            this.Btn_Bejelentkezes.Location = new System.Drawing.Point(159, 462);
            this.Btn_Bejelentkezes.Name = "Btn_Bejelentkezes";
            this.Btn_Bejelentkezes.Size = new System.Drawing.Size(180, 40);
            this.Btn_Bejelentkezes.TabIndex = 5;
            this.Btn_Bejelentkezes.Text = "Bejelentkezés";
            this.Btn_Bejelentkezes.TextColor = System.Drawing.Color.White;
            this.Btn_Bejelentkezes.UseVisualStyleBackColor = false;
            this.Btn_Bejelentkezes.Click += new System.EventHandler(this.Btn_Bejelentkezes_Click);
            this.Btn_Bejelentkezes.MouseEnter += new System.EventHandler(this.Btn_Bejelentkezes_MouseEnter);
            this.Btn_Bejelentkezes.MouseLeave += new System.EventHandler(this.Btn_Bejelentkezes_MouseLeave);
            // 
            // Tb_Jelszo
            // 
            this.Tb_Jelszo.BackColor = System.Drawing.SystemColors.Window;
            this.Tb_Jelszo.BorderColor = System.Drawing.SystemColors.Window;
            this.Tb_Jelszo.BorderFocusColor = System.Drawing.Color.LawnGreen;
            this.Tb_Jelszo.BorderRadius = 10;
            this.Tb_Jelszo.BorderSize = 2;
            this.Tb_Jelszo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Jelszo.Location = new System.Drawing.Point(125, 370);
            this.Tb_Jelszo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Jelszo.Multiline = false;
            this.Tb_Jelszo.Name = "Tb_Jelszo";
            this.Tb_Jelszo.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Tb_Jelszo.PasswordChar = true;
            this.Tb_Jelszo.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.Tb_Jelszo.PlaceholderText = "";
            this.Tb_Jelszo.Size = new System.Drawing.Size(250, 30);
            this.Tb_Jelszo.TabIndex = 3;
            this.Tb_Jelszo.Texts = "";
            this.Tb_Jelszo.UnderlinedStyle = false;
            // 
            // Tb_Felhasznalonev
            // 
            this.Tb_Felhasznalonev.BackColor = System.Drawing.SystemColors.Window;
            this.Tb_Felhasznalonev.BorderColor = System.Drawing.Color.White;
            this.Tb_Felhasznalonev.BorderFocusColor = System.Drawing.Color.Red;
            this.Tb_Felhasznalonev.BorderRadius = 10;
            this.Tb_Felhasznalonev.BorderSize = 2;
            this.Tb_Felhasznalonev.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Felhasznalonev.Location = new System.Drawing.Point(125, 260);
            this.Tb_Felhasznalonev.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Felhasznalonev.Multiline = false;
            this.Tb_Felhasznalonev.Name = "Tb_Felhasznalonev";
            this.Tb_Felhasznalonev.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Tb_Felhasznalonev.PasswordChar = false;
            this.Tb_Felhasznalonev.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.Tb_Felhasznalonev.PlaceholderText = "";
            this.Tb_Felhasznalonev.Size = new System.Drawing.Size(250, 30);
            this.Tb_Felhasznalonev.TabIndex = 1;
            this.Tb_Felhasznalonev.Texts = "";
            this.Tb_Felhasznalonev.UnderlinedStyle = false;
            // 
            // Lb_ElfelejtettJelszo
            // 
            this.Lb_ElfelejtettJelszo.AutoSize = true;
            this.Lb_ElfelejtettJelszo.Font = new System.Drawing.Font("Calibri", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_ElfelejtettJelszo.ForeColor = System.Drawing.Color.White;
            this.Lb_ElfelejtettJelszo.Location = new System.Drawing.Point(191, 522);
            this.Lb_ElfelejtettJelszo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_ElfelejtettJelszo.Name = "Lb_ElfelejtettJelszo";
            this.Lb_ElfelejtettJelszo.Size = new System.Drawing.Size(116, 19);
            this.Lb_ElfelejtettJelszo.TabIndex = 7;
            this.Lb_ElfelejtettJelszo.Text = "Elfelejtett jelszó";
            this.Lb_ElfelejtettJelszo.Click += new System.EventHandler(this.Lb_ElfelejtettJelszo_Click);
            // 
            // Bejelentkezés
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(484, 581);
            this.Controls.Add(this.Lb_ElfelejtettJelszo);
            this.Controls.Add(this.Btn_Bejelentkezes);
            this.Controls.Add(this.Tb_Jelszo);
            this.Controls.Add(this.Lb_jelszo);
            this.Controls.Add(this.Tb_Felhasznalonev);
            this.Controls.Add(this.Lb_Felhasznalonev);
            this.Controls.Add(this.PB_CareCompass_Logo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Bejelentkezés";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bejelentkezés";
            ((System.ComponentModel.ISupportInitialize)(this.PB_CareCompass_Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_CareCompass_Logo;
        private System.Windows.Forms.Label Lb_Felhasznalonev;
        private System.Windows.Forms.Label Lb_jelszo;
        private Customs.CCTextBox Tb_Jelszo;
        private Customs.CCTextBox Tb_Felhasznalonev;
        private Customs.CCButton Btn_Bejelentkezes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Lb_ElfelejtettJelszo;
    }
}

