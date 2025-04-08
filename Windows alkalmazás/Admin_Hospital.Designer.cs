namespace CareCompass
{
    partial class Admin_Hospital
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dGV_Hospitals = new System.Windows.Forms.DataGridView();
            this.nev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aktiv = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.felulet = new System.Windows.Forms.DataGridViewButtonColumn();
            this.torles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Pn_UjKorhaz = new System.Windows.Forms.Panel();
            this.cB_Ceg = new System.Windows.Forms.ComboBox();
            this.Lb_Ceg = new System.Windows.Forms.Label();
            this.Lb_KorhazHazszam = new System.Windows.Forms.Label();
            this.Lb_KorhazUtca = new System.Windows.Forms.Label();
            this.Lb_KorhazTelepules = new System.Windows.Forms.Label();
            this.Lb_KorhazIranyitoszam = new System.Windows.Forms.Label();
            this.Btn_KorhazHozzaadas = new CareCompass.Customs.CCButton();
            this.Tb_KorhazEmail = new System.Windows.Forms.TextBox();
            this.Tb_Hazszam = new System.Windows.Forms.TextBox();
            this.Tb_Utca = new System.Windows.Forms.TextBox();
            this.Tb_Telepules = new System.Windows.Forms.TextBox();
            this.Tb_Iranyitoszam = new System.Windows.Forms.TextBox();
            this.Lb_KorhazEmail = new System.Windows.Forms.Label();
            this.Lb_KorhazCim = new System.Windows.Forms.Label();
            this.Lb_KorhazNev = new System.Windows.Forms.Label();
            this.Tb_Korhaznev = new System.Windows.Forms.TextBox();
            this.Lb_KorhazHozzaadas = new System.Windows.Forms.Label();
            this.Pn_AdminKorhaz = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Hospitals)).BeginInit();
            this.Pn_UjKorhaz.SuspendLayout();
            this.Pn_AdminKorhaz.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 373F));
            this.tableLayoutPanel1.Controls.Add(this.dGV_Hospitals, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Pn_UjKorhaz, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1067, 599);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dGV_Hospitals
            // 
            this.dGV_Hospitals.AllowUserToAddRows = false;
            this.dGV_Hospitals.AllowUserToDeleteRows = false;
            this.dGV_Hospitals.AllowUserToResizeRows = false;
            this.dGV_Hospitals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_Hospitals.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Hospitals.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dGV_Hospitals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Hospitals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nev,
            this.email,
            this.Aktiv,
            this.felulet,
            this.torles});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_Hospitals.DefaultCellStyle = dataGridViewCellStyle5;
            this.dGV_Hospitals.Location = new System.Drawing.Point(0, 0);
            this.dGV_Hospitals.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_Hospitals.Name = "dGV_Hospitals";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Hospitals.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dGV_Hospitals.RowHeadersVisible = false;
            this.dGV_Hospitals.RowHeadersWidth = 51;
            this.dGV_Hospitals.Size = new System.Drawing.Size(641, 599);
            this.dGV_Hospitals.TabIndex = 1;
            this.dGV_Hospitals.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGV_Hospitals_DataBindingComplete);
            // 
            // nev
            // 
            this.nev.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.nev.HeaderText = "Név";
            this.nev.MinimumWidth = 6;
            this.nev.Name = "nev";
            this.nev.ReadOnly = true;
            this.nev.Width = 62;
            // 
            // email
            // 
            this.email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.email.HeaderText = "Email";
            this.email.MinimumWidth = 100;
            this.email.Name = "email";
            this.email.ReadOnly = true;
            // 
            // Aktiv
            // 
            this.Aktiv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Aktiv.FillWeight = 50F;
            this.Aktiv.HeaderText = "Aktív";
            this.Aktiv.MinimumWidth = 6;
            this.Aktiv.Name = "Aktiv";
            this.Aktiv.Width = 45;
            // 
            // felulet
            // 
            this.felulet.HeaderText = "Felület";
            this.felulet.MinimumWidth = 80;
            this.felulet.Name = "felulet";
            this.felulet.ReadOnly = true;
            this.felulet.Width = 80;
            // 
            // torles
            // 
            this.torles.HeaderText = "Törlés";
            this.torles.MinimumWidth = 80;
            this.torles.Name = "torles";
            this.torles.ReadOnly = true;
            this.torles.Width = 80;
            // 
            // Pn_UjKorhaz
            // 
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazHozzaadas);
            this.Pn_UjKorhaz.Controls.Add(this.cB_Ceg);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_Ceg);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazHazszam);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazUtca);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazTelepules);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazIranyitoszam);
            this.Pn_UjKorhaz.Controls.Add(this.Btn_KorhazHozzaadas);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_KorhazEmail);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_Hazszam);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_Utca);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_Telepules);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_Iranyitoszam);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazEmail);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazCim);
            this.Pn_UjKorhaz.Controls.Add(this.Lb_KorhazNev);
            this.Pn_UjKorhaz.Controls.Add(this.Tb_Korhaznev);
            this.Pn_UjKorhaz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_UjKorhaz.Location = new System.Drawing.Point(694, 0);
            this.Pn_UjKorhaz.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_UjKorhaz.Name = "Pn_UjKorhaz";
            this.Pn_UjKorhaz.Size = new System.Drawing.Size(373, 599);
            this.Pn_UjKorhaz.TabIndex = 0;
            // 
            // cB_Ceg
            // 
            this.cB_Ceg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_Ceg.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cB_Ceg.FormattingEnabled = true;
            this.cB_Ceg.Location = new System.Drawing.Point(19, 459);
            this.cB_Ceg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cB_Ceg.Name = "cB_Ceg";
            this.cB_Ceg.Size = new System.Drawing.Size(332, 27);
            this.cB_Ceg.TabIndex = 22;
            // 
            // Lb_Ceg
            // 
            this.Lb_Ceg.AutoSize = true;
            this.Lb_Ceg.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Ceg.ForeColor = System.Drawing.Color.White;
            this.Lb_Ceg.Location = new System.Drawing.Point(15, 433);
            this.Lb_Ceg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_Ceg.Name = "Lb_Ceg";
            this.Lb_Ceg.Size = new System.Drawing.Size(126, 24);
            this.Lb_Ceg.TabIndex = 21;
            this.Lb_Ceg.Text = "Felügyelő cég:";
            // 
            // Lb_KorhazHazszam
            // 
            this.Lb_KorhazHazszam.AutoSize = true;
            this.Lb_KorhazHazszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazHazszam.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazHazszam.Location = new System.Drawing.Point(13, 324);
            this.Lb_KorhazHazszam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazHazszam.Name = "Lb_KorhazHazszam";
            this.Lb_KorhazHazszam.Size = new System.Drawing.Size(72, 21);
            this.Lb_KorhazHazszam.TabIndex = 20;
            this.Lb_KorhazHazszam.Text = "Házszám";
            // 
            // Lb_KorhazUtca
            // 
            this.Lb_KorhazUtca.AutoSize = true;
            this.Lb_KorhazUtca.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazUtca.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazUtca.Location = new System.Drawing.Point(15, 279);
            this.Lb_KorhazUtca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazUtca.Name = "Lb_KorhazUtca";
            this.Lb_KorhazUtca.Size = new System.Drawing.Size(42, 21);
            this.Lb_KorhazUtca.TabIndex = 19;
            this.Lb_KorhazUtca.Text = "Utca";
            // 
            // Lb_KorhazTelepules
            // 
            this.Lb_KorhazTelepules.AutoSize = true;
            this.Lb_KorhazTelepules.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazTelepules.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazTelepules.Location = new System.Drawing.Point(13, 233);
            this.Lb_KorhazTelepules.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazTelepules.Name = "Lb_KorhazTelepules";
            this.Lb_KorhazTelepules.Size = new System.Drawing.Size(73, 21);
            this.Lb_KorhazTelepules.TabIndex = 18;
            this.Lb_KorhazTelepules.Text = "Település";
            // 
            // Lb_KorhazIranyitoszam
            // 
            this.Lb_KorhazIranyitoszam.AutoSize = true;
            this.Lb_KorhazIranyitoszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazIranyitoszam.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazIranyitoszam.Location = new System.Drawing.Point(15, 190);
            this.Lb_KorhazIranyitoszam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazIranyitoszam.Name = "Lb_KorhazIranyitoszam";
            this.Lb_KorhazIranyitoszam.Size = new System.Drawing.Size(100, 21);
            this.Lb_KorhazIranyitoszam.TabIndex = 17;
            this.Lb_KorhazIranyitoszam.Text = "Irányítószám";
            // 
            // Btn_KorhazHozzaadas
            // 
            this.Btn_KorhazHozzaadas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_KorhazHozzaadas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_KorhazHozzaadas.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_KorhazHozzaadas.BorderRadius = 5;
            this.Btn_KorhazHozzaadas.BorderSize = 0;
            this.Btn_KorhazHozzaadas.FlatAppearance.BorderSize = 0;
            this.Btn_KorhazHozzaadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_KorhazHozzaadas.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_KorhazHozzaadas.ForeColor = System.Drawing.Color.Black;
            this.Btn_KorhazHozzaadas.Location = new System.Drawing.Point(151, 512);
            this.Btn_KorhazHozzaadas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_KorhazHozzaadas.Name = "Btn_KorhazHozzaadas";
            this.Btn_KorhazHozzaadas.Size = new System.Drawing.Size(200, 49);
            this.Btn_KorhazHozzaadas.TabIndex = 16;
            this.Btn_KorhazHozzaadas.Text = "Hozzáadás";
            this.Btn_KorhazHozzaadas.TextColor = System.Drawing.Color.Black;
            this.Btn_KorhazHozzaadas.UseVisualStyleBackColor = false;
            this.Btn_KorhazHozzaadas.Click += new System.EventHandler(this.Btn_KorhazHozzaadas_Click);
            // 
            // Tb_KorhazEmail
            // 
            this.Tb_KorhazEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_KorhazEmail.Location = new System.Drawing.Point(19, 393);
            this.Tb_KorhazEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_KorhazEmail.Name = "Tb_KorhazEmail";
            this.Tb_KorhazEmail.Size = new System.Drawing.Size(332, 27);
            this.Tb_KorhazEmail.TabIndex = 15;
            // 
            // Tb_Hazszam
            // 
            this.Tb_Hazszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Hazszam.Location = new System.Drawing.Point(139, 324);
            this.Tb_Hazszam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Hazszam.Name = "Tb_Hazszam";
            this.Tb_Hazszam.Size = new System.Drawing.Size(212, 27);
            this.Tb_Hazszam.TabIndex = 14;
            // 
            // Tb_Utca
            // 
            this.Tb_Utca.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Utca.Location = new System.Drawing.Point(139, 279);
            this.Tb_Utca.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Utca.Name = "Tb_Utca";
            this.Tb_Utca.Size = new System.Drawing.Size(212, 27);
            this.Tb_Utca.TabIndex = 13;
            // 
            // Tb_Telepules
            // 
            this.Tb_Telepules.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Telepules.Location = new System.Drawing.Point(139, 233);
            this.Tb_Telepules.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Telepules.Name = "Tb_Telepules";
            this.Tb_Telepules.Size = new System.Drawing.Size(212, 27);
            this.Tb_Telepules.TabIndex = 12;
            // 
            // Tb_Iranyitoszam
            // 
            this.Tb_Iranyitoszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Iranyitoszam.Location = new System.Drawing.Point(139, 190);
            this.Tb_Iranyitoszam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Iranyitoszam.MaxLength = 4;
            this.Tb_Iranyitoszam.Name = "Tb_Iranyitoszam";
            this.Tb_Iranyitoszam.Size = new System.Drawing.Size(212, 27);
            this.Tb_Iranyitoszam.TabIndex = 11;
            this.Tb_Iranyitoszam.TextChanged += new System.EventHandler(this.Tb_Iranyitoszam_TextChanged);
            // 
            // Lb_KorhazEmail
            // 
            this.Lb_KorhazEmail.AutoSize = true;
            this.Lb_KorhazEmail.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazEmail.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazEmail.Location = new System.Drawing.Point(13, 365);
            this.Lb_KorhazEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazEmail.Name = "Lb_KorhazEmail";
            this.Lb_KorhazEmail.Size = new System.Drawing.Size(167, 24);
            this.Lb_KorhazEmail.TabIndex = 10;
            this.Lb_KorhazEmail.Text = "Kórház Email címe:";
            // 
            // Lb_KorhazCim
            // 
            this.Lb_KorhazCim.AutoSize = true;
            this.Lb_KorhazCim.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazCim.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazCim.Location = new System.Drawing.Point(15, 142);
            this.Lb_KorhazCim.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazCim.Name = "Lb_KorhazCim";
            this.Lb_KorhazCim.Size = new System.Drawing.Size(116, 24);
            this.Lb_KorhazCim.TabIndex = 9;
            this.Lb_KorhazCim.Text = "Kórház címe:";
            // 
            // Lb_KorhazNev
            // 
            this.Lb_KorhazNev.AutoSize = true;
            this.Lb_KorhazNev.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazNev.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazNev.Location = new System.Drawing.Point(15, 68);
            this.Lb_KorhazNev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazNev.Name = "Lb_KorhazNev";
            this.Lb_KorhazNev.Size = new System.Drawing.Size(117, 24);
            this.Lb_KorhazNev.TabIndex = 8;
            this.Lb_KorhazNev.Text = "Kórház neve:";
            // 
            // Tb_Korhaznev
            // 
            this.Tb_Korhaznev.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_Korhaznev.Location = new System.Drawing.Point(19, 96);
            this.Tb_Korhaznev.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_Korhaznev.Name = "Tb_Korhaznev";
            this.Tb_Korhaznev.Size = new System.Drawing.Size(332, 27);
            this.Tb_Korhaznev.TabIndex = 7;
            // 
            // Lb_KorhazHozzaadas
            // 
            this.Lb_KorhazHozzaadas.AutoSize = true;
            this.Lb_KorhazHozzaadas.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazHozzaadas.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazHozzaadas.Location = new System.Drawing.Point(14, 11);
            this.Lb_KorhazHozzaadas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazHozzaadas.Name = "Lb_KorhazHozzaadas";
            this.Lb_KorhazHozzaadas.Size = new System.Drawing.Size(230, 29);
            this.Lb_KorhazHozzaadas.TabIndex = 6;
            this.Lb_KorhazHozzaadas.Text = "Új kórház hozzáadása";
            // 
            // Pn_AdminKorhaz
            // 
            this.Pn_AdminKorhaz.Controls.Add(this.tableLayoutPanel1);
            this.Pn_AdminKorhaz.Location = new System.Drawing.Point(31, 66);
            this.Pn_AdminKorhaz.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Pn_AdminKorhaz.Name = "Pn_AdminKorhaz";
            this.Pn_AdminKorhaz.Size = new System.Drawing.Size(1067, 599);
            this.Pn_AdminKorhaz.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "Kórházak";
            // 
            // Admin_Hospital
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Pn_AdminKorhaz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Admin_Hospital";
            this.Text = "Admin_Hospital";
            this.Load += new System.EventHandler(this.Admin_Hospital_Load);
            this.SizeChanged += new System.EventHandler(this.Admin_Hospital_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Hospitals)).EndInit();
            this.Pn_UjKorhaz.ResumeLayout(false);
            this.Pn_UjKorhaz.PerformLayout();
            this.Pn_AdminKorhaz.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Pn_UjKorhaz;
        private System.Windows.Forms.Label Lb_KorhazHazszam;
        private System.Windows.Forms.Label Lb_KorhazUtca;
        private System.Windows.Forms.Label Lb_KorhazTelepules;
        private System.Windows.Forms.Label Lb_KorhazIranyitoszam;
        private Customs.CCButton Btn_KorhazHozzaadas;
        private System.Windows.Forms.TextBox Tb_KorhazEmail;
        private System.Windows.Forms.TextBox Tb_Hazszam;
        private System.Windows.Forms.TextBox Tb_Utca;
        private System.Windows.Forms.TextBox Tb_Telepules;
        private System.Windows.Forms.TextBox Tb_Iranyitoszam;
        private System.Windows.Forms.Label Lb_KorhazEmail;
        private System.Windows.Forms.Label Lb_KorhazCim;
        private System.Windows.Forms.Label Lb_KorhazNev;
        private System.Windows.Forms.TextBox Tb_Korhaznev;
        private System.Windows.Forms.Label Lb_KorhazHozzaadas;
        private System.Windows.Forms.DataGridView dGV_Hospitals;
        private System.Windows.Forms.Panel Pn_AdminKorhaz;
        private System.Windows.Forms.ComboBox cB_Ceg;
        private System.Windows.Forms.Label Lb_Ceg;
        private System.Windows.Forms.DataGridViewTextBoxColumn nev;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Aktiv;
        private System.Windows.Forms.DataGridViewButtonColumn felulet;
        private System.Windows.Forms.DataGridViewButtonColumn torles;
        private System.Windows.Forms.Label label1;
    }
}