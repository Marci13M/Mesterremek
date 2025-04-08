namespace CareCompass
{
    partial class Admin_Services
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Pn_AdminSzolgaltatasok = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Lb_AdminSzolgaltatasHozzaadasaCim = new System.Windows.Forms.Label();
            this.Pn_SzolgaltatasHozzaadas = new System.Windows.Forms.Panel();
            this.Btn_SzolgaltatasHozzaadas = new CareCompass.Customs.CCButton();
            this.Lb_SzolgaltatasLeiras = new System.Windows.Forms.Label();
            this.Lb_CegNeve = new System.Windows.Forms.Label();
            this.Tb_SzolgaltatasLeiras = new System.Windows.Forms.TextBox();
            this.Tb_SzolgaltatasNev = new System.Windows.Forms.TextBox();
            this.dGV_Szolgaltatasok = new System.Windows.Forms.DataGridView();
            this.Nev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Leiras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositas = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Torles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Lb_AdminSzolgaltatas = new System.Windows.Forms.Label();
            this.Pn_AdminSzolgaltatasok.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Pn_SzolgaltatasHozzaadas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Szolgaltatasok)).BeginInit();
            this.SuspendLayout();
            // 
            // Pn_AdminSzolgaltatasok
            // 
            this.Pn_AdminSzolgaltatasok.Controls.Add(this.tableLayoutPanel1);
            this.Pn_AdminSzolgaltatasok.Location = new System.Drawing.Point(27, 66);
            this.Pn_AdminSzolgaltatasok.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_AdminSzolgaltatasok.Name = "Pn_AdminSzolgaltatasok";
            this.Pn_AdminSzolgaltatasok.Size = new System.Drawing.Size(1067, 599);
            this.Pn_AdminSzolgaltatasok.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLayoutPanel1.Controls.Add(this.Pn_SzolgaltatasHozzaadas, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dGV_Szolgaltatasok, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1067, 599);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Lb_AdminSzolgaltatasHozzaadasaCim
            // 
            this.Lb_AdminSzolgaltatasHozzaadasaCim.AutoSize = true;
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AdminSzolgaltatasHozzaadasaCim.ForeColor = System.Drawing.Color.White;
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Location = new System.Drawing.Point(15, 12);
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Name = "Lb_AdminSzolgaltatasHozzaadasaCim";
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Size = new System.Drawing.Size(254, 29);
            this.Lb_AdminSzolgaltatasHozzaadasaCim.TabIndex = 9;
            this.Lb_AdminSzolgaltatasHozzaadasaCim.Text = "Szolgáltatás hozzáadása";
            // 
            // Pn_SzolgaltatasHozzaadas
            // 
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Lb_AdminSzolgaltatasHozzaadasaCim);
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Btn_SzolgaltatasHozzaadas);
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Lb_SzolgaltatasLeiras);
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Lb_CegNeve);
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Tb_SzolgaltatasLeiras);
            this.Pn_SzolgaltatasHozzaadas.Controls.Add(this.Tb_SzolgaltatasNev);
            this.Pn_SzolgaltatasHozzaadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_SzolgaltatasHozzaadas.Location = new System.Drawing.Point(718, 0);
            this.Pn_SzolgaltatasHozzaadas.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_SzolgaltatasHozzaadas.Name = "Pn_SzolgaltatasHozzaadas";
            this.Pn_SzolgaltatasHozzaadas.Size = new System.Drawing.Size(349, 599);
            this.Pn_SzolgaltatasHozzaadas.TabIndex = 2;
            // 
            // Btn_SzolgaltatasHozzaadas
            // 
            this.Btn_SzolgaltatasHozzaadas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_SzolgaltatasHozzaadas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_SzolgaltatasHozzaadas.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_SzolgaltatasHozzaadas.BorderRadius = 5;
            this.Btn_SzolgaltatasHozzaadas.BorderSize = 0;
            this.Btn_SzolgaltatasHozzaadas.FlatAppearance.BorderSize = 0;
            this.Btn_SzolgaltatasHozzaadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SzolgaltatasHozzaadas.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_SzolgaltatasHozzaadas.ForeColor = System.Drawing.Color.Black;
            this.Btn_SzolgaltatasHozzaadas.Location = new System.Drawing.Point(199, 349);
            this.Btn_SzolgaltatasHozzaadas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_SzolgaltatasHozzaadas.Name = "Btn_SzolgaltatasHozzaadas";
            this.Btn_SzolgaltatasHozzaadas.Size = new System.Drawing.Size(131, 50);
            this.Btn_SzolgaltatasHozzaadas.TabIndex = 32;
            this.Btn_SzolgaltatasHozzaadas.Text = "Hozzáadás";
            this.Btn_SzolgaltatasHozzaadas.TextColor = System.Drawing.Color.Black;
            this.Btn_SzolgaltatasHozzaadas.UseVisualStyleBackColor = false;
            this.Btn_SzolgaltatasHozzaadas.Click += new System.EventHandler(this.Btn_SzolgaltatasHozzaadas_Click);
            // 
            // Lb_SzolgaltatasLeiras
            // 
            this.Lb_SzolgaltatasLeiras.AutoSize = true;
            this.Lb_SzolgaltatasLeiras.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_SzolgaltatasLeiras.ForeColor = System.Drawing.Color.White;
            this.Lb_SzolgaltatasLeiras.Location = new System.Drawing.Point(16, 137);
            this.Lb_SzolgaltatasLeiras.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_SzolgaltatasLeiras.Name = "Lb_SzolgaltatasLeiras";
            this.Lb_SzolgaltatasLeiras.Size = new System.Drawing.Size(147, 21);
            this.Lb_SzolgaltatasLeiras.TabIndex = 31;
            this.Lb_SzolgaltatasLeiras.Text = "Szolgáltatás leírása:";
            // 
            // Lb_CegNeve
            // 
            this.Lb_CegNeve.AutoSize = true;
            this.Lb_CegNeve.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegNeve.ForeColor = System.Drawing.Color.White;
            this.Lb_CegNeve.Location = new System.Drawing.Point(16, 66);
            this.Lb_CegNeve.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_CegNeve.Name = "Lb_CegNeve";
            this.Lb_CegNeve.Size = new System.Drawing.Size(42, 21);
            this.Lb_CegNeve.TabIndex = 30;
            this.Lb_CegNeve.Text = "Név:";
            // 
            // Tb_SzolgaltatasLeiras
            // 
            this.Tb_SzolgaltatasLeiras.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_SzolgaltatasLeiras.Location = new System.Drawing.Point(20, 160);
            this.Tb_SzolgaltatasLeiras.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Tb_SzolgaltatasLeiras.Multiline = true;
            this.Tb_SzolgaltatasLeiras.Name = "Tb_SzolgaltatasLeiras";
            this.Tb_SzolgaltatasLeiras.Size = new System.Drawing.Size(310, 174);
            this.Tb_SzolgaltatasLeiras.TabIndex = 1;
            // 
            // Tb_SzolgaltatasNev
            // 
            this.Tb_SzolgaltatasNev.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_SzolgaltatasNev.Location = new System.Drawing.Point(20, 89);
            this.Tb_SzolgaltatasNev.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Tb_SzolgaltatasNev.Name = "Tb_SzolgaltatasNev";
            this.Tb_SzolgaltatasNev.Size = new System.Drawing.Size(310, 27);
            this.Tb_SzolgaltatasNev.TabIndex = 0;
            // 
            // dGV_Szolgaltatasok
            // 
            this.dGV_Szolgaltatasok.AllowUserToAddRows = false;
            this.dGV_Szolgaltatasok.AllowUserToDeleteRows = false;
            this.dGV_Szolgaltatasok.AllowUserToResizeRows = false;
            this.dGV_Szolgaltatasok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_Szolgaltatasok.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Szolgaltatasok.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV_Szolgaltatasok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Szolgaltatasok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nev,
            this.Leiras,
            this.modositas,
            this.Torles});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_Szolgaltatasok.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGV_Szolgaltatasok.Location = new System.Drawing.Point(0, 0);
            this.dGV_Szolgaltatasok.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_Szolgaltatasok.Name = "dGV_Szolgaltatasok";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Szolgaltatasok.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGV_Szolgaltatasok.RowHeadersVisible = false;
            this.dGV_Szolgaltatasok.RowHeadersWidth = 51;
            this.dGV_Szolgaltatasok.RowTemplate.Height = 24;
            this.dGV_Szolgaltatasok.Size = new System.Drawing.Size(678, 599);
            this.dGV_Szolgaltatasok.TabIndex = 3;
            this.dGV_Szolgaltatasok.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGV_Szolgaltatasok_DataBindingComplete);
            // 
            // Nev
            // 
            this.Nev.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Nev.HeaderText = "Név";
            this.Nev.MaxInputLength = 100;
            this.Nev.MinimumWidth = 6;
            this.Nev.Name = "Nev";
            this.Nev.ReadOnly = true;
            this.Nev.Width = 62;
            // 
            // Leiras
            // 
            this.Leiras.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Leiras.FillWeight = 300F;
            this.Leiras.HeaderText = "Leírás";
            this.Leiras.MinimumWidth = 455;
            this.Leiras.Name = "Leiras";
            this.Leiras.ReadOnly = true;
            // 
            // modositas
            // 
            this.modositas.HeaderText = "Módosítás";
            this.modositas.MinimumWidth = 80;
            this.modositas.Name = "modositas";
            this.modositas.Width = 80;
            // 
            // Torles
            // 
            this.Torles.HeaderText = "Törlés";
            this.Torles.MinimumWidth = 80;
            this.Torles.Name = "Torles";
            this.Torles.Width = 80;
            // 
            // Lb_AdminSzolgaltatas
            // 
            this.Lb_AdminSzolgaltatas.AutoSize = true;
            this.Lb_AdminSzolgaltatas.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AdminSzolgaltatas.ForeColor = System.Drawing.Color.White;
            this.Lb_AdminSzolgaltatas.Location = new System.Drawing.Point(20, 10);
            this.Lb_AdminSzolgaltatas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_AdminSzolgaltatas.Name = "Lb_AdminSzolgaltatas";
            this.Lb_AdminSzolgaltatas.Size = new System.Drawing.Size(177, 33);
            this.Lb_AdminSzolgaltatas.TabIndex = 9;
            this.Lb_AdminSzolgaltatas.Text = "Szolgáltatások";
            // 
            // Admin_Services
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.Pn_AdminSzolgaltatasok);
            this.Controls.Add(this.Lb_AdminSzolgaltatas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Admin_Services";
            this.Text = "Admin_Services";
            this.Load += new System.EventHandler(this.Admin_Services_Load);
            this.SizeChanged += new System.EventHandler(this.Admin_Services_SizeChanged);
            this.Pn_AdminSzolgaltatasok.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.Pn_SzolgaltatasHozzaadas.ResumeLayout(false);
            this.Pn_SzolgaltatasHozzaadas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Szolgaltatasok)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Pn_AdminSzolgaltatasok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Pn_SzolgaltatasHozzaadas;
        private System.Windows.Forms.DataGridView dGV_Szolgaltatasok;
        private System.Windows.Forms.Label Lb_AdminSzolgaltatas;
        private System.Windows.Forms.Label Lb_AdminSzolgaltatasHozzaadasaCim;
        private System.Windows.Forms.TextBox Tb_SzolgaltatasLeiras;
        private System.Windows.Forms.TextBox Tb_SzolgaltatasNev;
        private System.Windows.Forms.Label Lb_SzolgaltatasLeiras;
        private System.Windows.Forms.Label Lb_CegNeve;
        private Customs.CCButton Btn_SzolgaltatasHozzaadas;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nev;
        private System.Windows.Forms.DataGridViewTextBoxColumn Leiras;
        private System.Windows.Forms.DataGridViewButtonColumn modositas;
        private System.Windows.Forms.DataGridViewButtonColumn Torles;
    }
}