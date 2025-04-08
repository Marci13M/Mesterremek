namespace CareCompass
{
    partial class Company_Statistics
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
            this.Pn_CegStatisztikaMenu = new System.Windows.Forms.Panel();
            this.CB_Valaszto = new System.Windows.Forms.ComboBox();
            this.Btn_CegValaszt = new CareCompass.Customs.CCButton();
            this.Btn_OrvosValaszt = new CareCompass.Customs.CCButton();
            this.Btn_KorhazValaszt = new CareCompass.Customs.CCButton();
            this.Pn_CegPnFo = new System.Windows.Forms.Panel();
            this.Pn_CegStatisztikaMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pn_CegStatisztikaMenu
            // 
            this.Pn_CegStatisztikaMenu.Controls.Add(this.CB_Valaszto);
            this.Pn_CegStatisztikaMenu.Controls.Add(this.Btn_CegValaszt);
            this.Pn_CegStatisztikaMenu.Controls.Add(this.Btn_OrvosValaszt);
            this.Pn_CegStatisztikaMenu.Controls.Add(this.Btn_KorhazValaszt);
            this.Pn_CegStatisztikaMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pn_CegStatisztikaMenu.Location = new System.Drawing.Point(0, 0);
            this.Pn_CegStatisztikaMenu.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_CegStatisztikaMenu.Name = "Pn_CegStatisztikaMenu";
            this.Pn_CegStatisztikaMenu.Size = new System.Drawing.Size(1120, 55);
            this.Pn_CegStatisztikaMenu.TabIndex = 6;
            // 
            // CB_Valaszto
            // 
            this.CB_Valaszto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Valaszto.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CB_Valaszto.FormattingEnabled = true;
            this.CB_Valaszto.Location = new System.Drawing.Point(452, 16);
            this.CB_Valaszto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CB_Valaszto.Name = "CB_Valaszto";
            this.CB_Valaszto.Size = new System.Drawing.Size(239, 26);
            this.CB_Valaszto.TabIndex = 3;
            this.CB_Valaszto.SelectedIndexChanged += new System.EventHandler(this.CB_Valaszto_SelectedIndexChanged);
            // 
            // Btn_CegValaszt
            // 
            this.Btn_CegValaszt.BackColor = System.Drawing.Color.Transparent;
            this.Btn_CegValaszt.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_CegValaszt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_CegValaszt.BorderRadius = 5;
            this.Btn_CegValaszt.BorderSize = 0;
            this.Btn_CegValaszt.FlatAppearance.BorderSize = 0;
            this.Btn_CegValaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_CegValaszt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_CegValaszt.ForeColor = System.Drawing.Color.White;
            this.Btn_CegValaszt.Location = new System.Drawing.Point(12, 6);
            this.Btn_CegValaszt.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_CegValaszt.Name = "Btn_CegValaszt";
            this.Btn_CegValaszt.Size = new System.Drawing.Size(133, 43);
            this.Btn_CegValaszt.TabIndex = 2;
            this.Btn_CegValaszt.Text = "Cég";
            this.Btn_CegValaszt.TextColor = System.Drawing.Color.White;
            this.Btn_CegValaszt.UseVisualStyleBackColor = false;
            this.Btn_CegValaszt.Click += new System.EventHandler(this.Btn_CegValaszt_Click);
            // 
            // Btn_OrvosValaszt
            // 
            this.Btn_OrvosValaszt.BackColor = System.Drawing.Color.Transparent;
            this.Btn_OrvosValaszt.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_OrvosValaszt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_OrvosValaszt.BorderRadius = 5;
            this.Btn_OrvosValaszt.BorderSize = 0;
            this.Btn_OrvosValaszt.FlatAppearance.BorderSize = 0;
            this.Btn_OrvosValaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_OrvosValaszt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_OrvosValaszt.ForeColor = System.Drawing.Color.White;
            this.Btn_OrvosValaszt.Location = new System.Drawing.Point(305, 6);
            this.Btn_OrvosValaszt.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_OrvosValaszt.Name = "Btn_OrvosValaszt";
            this.Btn_OrvosValaszt.Size = new System.Drawing.Size(133, 43);
            this.Btn_OrvosValaszt.TabIndex = 1;
            this.Btn_OrvosValaszt.Text = "Orvos";
            this.Btn_OrvosValaszt.TextColor = System.Drawing.Color.White;
            this.Btn_OrvosValaszt.UseVisualStyleBackColor = false;
            this.Btn_OrvosValaszt.Click += new System.EventHandler(this.Btn_OrvosValaszt_Click);
            // 
            // Btn_KorhazValaszt
            // 
            this.Btn_KorhazValaszt.BackColor = System.Drawing.Color.Transparent;
            this.Btn_KorhazValaszt.BackgroundColor = System.Drawing.Color.Transparent;
            this.Btn_KorhazValaszt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_KorhazValaszt.BorderRadius = 5;
            this.Btn_KorhazValaszt.BorderSize = 0;
            this.Btn_KorhazValaszt.FlatAppearance.BorderSize = 0;
            this.Btn_KorhazValaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_KorhazValaszt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_KorhazValaszt.ForeColor = System.Drawing.Color.White;
            this.Btn_KorhazValaszt.Location = new System.Drawing.Point(159, 6);
            this.Btn_KorhazValaszt.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_KorhazValaszt.Name = "Btn_KorhazValaszt";
            this.Btn_KorhazValaszt.Size = new System.Drawing.Size(133, 43);
            this.Btn_KorhazValaszt.TabIndex = 0;
            this.Btn_KorhazValaszt.Text = "Kórház";
            this.Btn_KorhazValaszt.TextColor = System.Drawing.Color.White;
            this.Btn_KorhazValaszt.UseVisualStyleBackColor = false;
            this.Btn_KorhazValaszt.Click += new System.EventHandler(this.Btn_KorhazValaszt_Click);
            // 
            // Pn_CegPnFo
            // 
            this.Pn_CegPnFo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_CegPnFo.Location = new System.Drawing.Point(0, 55);
            this.Pn_CegPnFo.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_CegPnFo.Name = "Pn_CegPnFo";
            this.Pn_CegPnFo.Size = new System.Drawing.Size(1120, 631);
            this.Pn_CegPnFo.TabIndex = 7;
            // 
            // Company_Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.Pn_CegPnFo);
            this.Controls.Add(this.Pn_CegStatisztikaMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Company_Statistics";
            this.Text = "Ceg_Statistics";
            this.Load += new System.EventHandler(this.Ceg_Statistics_Load);
            this.Pn_CegStatisztikaMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Pn_CegStatisztikaMenu;
        private System.Windows.Forms.ComboBox CB_Valaszto;
        private Customs.CCButton Btn_CegValaszt;
        private Customs.CCButton Btn_OrvosValaszt;
        private Customs.CCButton Btn_KorhazValaszt;
        private System.Windows.Forms.Panel Pn_CegPnFo;
    }
}