namespace CareCompass
{
    partial class Profil
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_CegProfil = new CareCompass.Customs.CCButton();
            this.Btn_CegModerator = new CareCompass.Customs.CCButton();
            this.Pn_Profil = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Btn_CegProfil, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Btn_CegModerator, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(840, 37);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Btn_CegProfil
            // 
            this.Btn_CegProfil.AllowDrop = true;
            this.Btn_CegProfil.BackColor = System.Drawing.Color.Transparent;
            this.Btn_CegProfil.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_CegProfil.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_CegProfil.BorderRadius = 0;
            this.Btn_CegProfil.BorderSize = 0;
            this.Btn_CegProfil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_CegProfil.FlatAppearance.BorderSize = 0;
            this.Btn_CegProfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_CegProfil.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_CegProfil.ForeColor = System.Drawing.Color.Black;
            this.Btn_CegProfil.Location = new System.Drawing.Point(420, 0);
            this.Btn_CegProfil.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_CegProfil.Name = "Btn_CegProfil";
            this.Btn_CegProfil.Size = new System.Drawing.Size(420, 37);
            this.Btn_CegProfil.TabIndex = 0;
            this.Btn_CegProfil.Text = "Cép profil";
            this.Btn_CegProfil.TextColor = System.Drawing.Color.Black;
            this.Btn_CegProfil.UseVisualStyleBackColor = false;
            this.Btn_CegProfil.Click += new System.EventHandler(this.ccButton1_Click);
            this.Btn_CegProfil.MouseEnter += new System.EventHandler(this.Btn_CegProfil_MouseEnter);
            this.Btn_CegProfil.MouseLeave += new System.EventHandler(this.Btn_CegProfil_MouseLeave);
            // 
            // Btn_CegModerator
            // 
            this.Btn_CegModerator.BackColor = System.Drawing.Color.Transparent;
            this.Btn_CegModerator.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_CegModerator.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_CegModerator.BorderRadius = 0;
            this.Btn_CegModerator.BorderSize = 0;
            this.Btn_CegModerator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_CegModerator.FlatAppearance.BorderSize = 0;
            this.Btn_CegModerator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_CegModerator.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_CegModerator.ForeColor = System.Drawing.Color.Black;
            this.Btn_CegModerator.Location = new System.Drawing.Point(0, 0);
            this.Btn_CegModerator.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_CegModerator.Name = "Btn_CegModerator";
            this.Btn_CegModerator.Size = new System.Drawing.Size(420, 37);
            this.Btn_CegModerator.TabIndex = 1;
            this.Btn_CegModerator.Text = "Moderátor profil";
            this.Btn_CegModerator.TextColor = System.Drawing.Color.Black;
            this.Btn_CegModerator.UseVisualStyleBackColor = false;
            this.Btn_CegModerator.Click += new System.EventHandler(this.Btn_CegModerator_Click);
            this.Btn_CegModerator.MouseEnter += new System.EventHandler(this.Btn_CegModerator_MouseEnter);
            this.Btn_CegModerator.MouseLeave += new System.EventHandler(this.Btn_CegModerator_MouseLeave);
            // 
            // Pn_Profil
            // 
            this.Pn_Profil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_Profil.Location = new System.Drawing.Point(0, 37);
            this.Pn_Profil.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_Profil.Name = "Pn_Profil";
            this.Pn_Profil.Size = new System.Drawing.Size(840, 520);
            this.Pn_Profil.TabIndex = 1;
            // 
            // Profil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(840, 557);
            this.Controls.Add(this.Pn_Profil);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(840, 557);
            this.Name = "Profil";
            this.Text = "Ceg_Profil";
            this.Load += new System.EventHandler(this.Ceg_Profil_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Customs.CCButton Btn_CegProfil;
        private Customs.CCButton Btn_CegModerator;
        private System.Windows.Forms.Panel Pn_Profil;
    }
}