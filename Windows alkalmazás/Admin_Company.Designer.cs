namespace CareCompass
{
    partial class Admin_Company
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
            this.tLP_CegResztracio = new System.Windows.Forms.TableLayoutPanel();
            this.dGV_AdminCegek = new System.Windows.Forms.DataGridView();
            this.Nev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aktiv = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CFelulet = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Torles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Pn_CegHozzaadasa = new System.Windows.Forms.Panel();
            this.Lb_CegHozzaadasCim = new System.Windows.Forms.Label();
            this.Tb_Hazszam = new System.Windows.Forms.TextBox();
            this.Tb_Iranyitoszam = new System.Windows.Forms.TextBox();
            this.Lb_CegHazszam = new System.Windows.Forms.Label();
            this.Lb_CegUtca = new System.Windows.Forms.Label();
            this.Lb_CegIranyitoszam = new System.Windows.Forms.Label();
            this.Lb_CegEmail = new System.Windows.Forms.Label();
            this.Lb_CegIgazgato = new System.Windows.Forms.Label();
            this.Lb_CegAdoszam = new System.Windows.Forms.Label();
            this.Lb_CegCegjegyzekSzam = new System.Windows.Forms.Label();
            this.Lb_CegNeve = new System.Windows.Forms.Label();
            this.Lb_CegTelepules = new System.Windows.Forms.Label();
            this.Btn_CegHozzaadas = new CareCompass.Customs.CCButton();
            this.Tb_CegUtca = new System.Windows.Forms.TextBox();
            this.Tb_CegTelepules = new System.Windows.Forms.TextBox();
            this.Tb_CegEmail = new System.Windows.Forms.TextBox();
            this.Tb_CegIgazgatoNeve = new System.Windows.Forms.TextBox();
            this.Tb_CegAdoszam = new System.Windows.Forms.TextBox();
            this.Tb_CegCegjegyzekSzam = new System.Windows.Forms.TextBox();
            this.Tb_CegNev = new System.Windows.Forms.TextBox();
            this.Lb_AdminCeg = new System.Windows.Forms.Label();
            this.Pn_CegMain = new System.Windows.Forms.Panel();
            this.tLP_CegResztracio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AdminCegek)).BeginInit();
            this.Pn_CegHozzaadasa.SuspendLayout();
            this.Pn_CegMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tLP_CegResztracio
            // 
            this.tLP_CegResztracio.ColumnCount = 3;
            this.tLP_CegResztracio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_CegResztracio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tLP_CegResztracio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tLP_CegResztracio.Controls.Add(this.dGV_AdminCegek, 0, 0);
            this.tLP_CegResztracio.Controls.Add(this.Pn_CegHozzaadasa, 2, 0);
            this.tLP_CegResztracio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_CegResztracio.Location = new System.Drawing.Point(0, 0);
            this.tLP_CegResztracio.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_CegResztracio.Name = "tLP_CegResztracio";
            this.tLP_CegResztracio.RowCount = 1;
            this.tLP_CegResztracio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_CegResztracio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 487F));
            this.tLP_CegResztracio.Size = new System.Drawing.Size(800, 487);
            this.tLP_CegResztracio.TabIndex = 0;
            // 
            // dGV_AdminCegek
            // 
            this.dGV_AdminCegek.AllowUserToAddRows = false;
            this.dGV_AdminCegek.AllowUserToDeleteRows = false;
            this.dGV_AdminCegek.AllowUserToResizeRows = false;
            this.dGV_AdminCegek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_AdminCegek.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_AdminCegek.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV_AdminCegek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_AdminCegek.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nev,
            this.Email,
            this.Cim,
            this.Aktiv,
            this.CFelulet,
            this.Torles});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_AdminCegek.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGV_AdminCegek.Location = new System.Drawing.Point(0, 0);
            this.dGV_AdminCegek.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_AdminCegek.Name = "dGV_AdminCegek";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_AdminCegek.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGV_AdminCegek.RowHeadersVisible = false;
            this.dGV_AdminCegek.RowHeadersWidth = 30;
            this.dGV_AdminCegek.RowTemplate.Height = 24;
            this.dGV_AdminCegek.Size = new System.Drawing.Size(530, 487);
            this.dGV_AdminCegek.TabIndex = 0;
            this.dGV_AdminCegek.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGV_AdminCegek_DataBindingComplete);
            // 
            // Nev
            // 
            this.Nev.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Nev.HeaderText = "Név";
            this.Nev.MinimumWidth = 6;
            this.Nev.Name = "Nev";
            this.Nev.ReadOnly = true;
            this.Nev.Width = 52;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 100;
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // Cim
            // 
            this.Cim.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Cim.HeaderText = "Cím";
            this.Cim.MinimumWidth = 6;
            this.Cim.Name = "Cim";
            this.Cim.ReadOnly = true;
            this.Cim.Width = 51;
            // 
            // Aktiv
            // 
            this.Aktiv.HeaderText = "Aktív";
            this.Aktiv.MinimumWidth = 55;
            this.Aktiv.Name = "Aktiv";
            this.Aktiv.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Aktiv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Aktiv.Width = 68;
            // 
            // CFelulet
            // 
            this.CFelulet.HeaderText = "Felület";
            this.CFelulet.MinimumWidth = 80;
            this.CFelulet.Name = "CFelulet";
            this.CFelulet.Width = 80;
            // 
            // Torles
            // 
            this.Torles.FillWeight = 80F;
            this.Torles.HeaderText = "Törlés";
            this.Torles.MinimumWidth = 80;
            this.Torles.Name = "Torles";
            this.Torles.Width = 80;
            // 
            // Pn_CegHozzaadasa
            // 
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegHozzaadasCim);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_Hazszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_Iranyitoszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegHazszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegUtca);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegIranyitoszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegEmail);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegIgazgato);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegAdoszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegCegjegyzekSzam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegNeve);
            this.Pn_CegHozzaadasa.Controls.Add(this.Lb_CegTelepules);
            this.Pn_CegHozzaadasa.Controls.Add(this.Btn_CegHozzaadas);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegUtca);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegTelepules);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegEmail);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegIgazgatoNeve);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegAdoszam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegCegjegyzekSzam);
            this.Pn_CegHozzaadasa.Controls.Add(this.Tb_CegNev);
            this.Pn_CegHozzaadasa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_CegHozzaadasa.Location = new System.Drawing.Point(560, 0);
            this.Pn_CegHozzaadasa.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_CegHozzaadasa.Name = "Pn_CegHozzaadasa";
            this.Pn_CegHozzaadasa.Size = new System.Drawing.Size(240, 487);
            this.Pn_CegHozzaadasa.TabIndex = 2;
            // 
            // Lb_CegHozzaadasCim
            // 
            this.Lb_CegHozzaadasCim.AutoSize = true;
            this.Lb_CegHozzaadasCim.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegHozzaadasCim.ForeColor = System.Drawing.Color.White;
            this.Lb_CegHozzaadasCim.Location = new System.Drawing.Point(18, 10);
            this.Lb_CegHozzaadasCim.Name = "Lb_CegHozzaadasCim";
            this.Lb_CegHozzaadasCim.Size = new System.Drawing.Size(133, 23);
            this.Lb_CegHozzaadasCim.TabIndex = 8;
            this.Lb_CegHozzaadasCim.Text = "Cég hozzáadása";
            // 
            // Tb_Hazszam
            // 
            this.Tb_Hazszam.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_Hazszam.Location = new System.Drawing.Point(22, 453);
            this.Tb_Hazszam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_Hazszam.Name = "Tb_Hazszam";
            this.Tb_Hazszam.Size = new System.Drawing.Size(99, 23);
            this.Tb_Hazszam.TabIndex = 39;
            // 
            // Tb_Iranyitoszam
            // 
            this.Tb_Iranyitoszam.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_Iranyitoszam.Location = new System.Drawing.Point(22, 313);
            this.Tb_Iranyitoszam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_Iranyitoszam.MaxLength = 4;
            this.Tb_Iranyitoszam.Name = "Tb_Iranyitoszam";
            this.Tb_Iranyitoszam.Size = new System.Drawing.Size(99, 23);
            this.Tb_Iranyitoszam.TabIndex = 38;
            this.Tb_Iranyitoszam.TextChanged += new System.EventHandler(this.Tb_Iranyitoszam_TextChanged);
            // 
            // Lb_CegHazszam
            // 
            this.Lb_CegHazszam.AutoSize = true;
            this.Lb_CegHazszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegHazszam.ForeColor = System.Drawing.Color.White;
            this.Lb_CegHazszam.Location = new System.Drawing.Point(20, 435);
            this.Lb_CegHazszam.Name = "Lb_CegHazszam";
            this.Lb_CegHazszam.Size = new System.Drawing.Size(55, 15);
            this.Lb_CegHazszam.TabIndex = 37;
            this.Lb_CegHazszam.Text = "Házszám";
            // 
            // Lb_CegUtca
            // 
            this.Lb_CegUtca.AutoSize = true;
            this.Lb_CegUtca.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegUtca.ForeColor = System.Drawing.Color.White;
            this.Lb_CegUtca.Location = new System.Drawing.Point(21, 390);
            this.Lb_CegUtca.Name = "Lb_CegUtca";
            this.Lb_CegUtca.Size = new System.Drawing.Size(32, 15);
            this.Lb_CegUtca.TabIndex = 36;
            this.Lb_CegUtca.Text = "Utca";
            // 
            // Lb_CegIranyitoszam
            // 
            this.Lb_CegIranyitoszam.AutoSize = true;
            this.Lb_CegIranyitoszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegIranyitoszam.ForeColor = System.Drawing.Color.White;
            this.Lb_CegIranyitoszam.Location = new System.Drawing.Point(20, 296);
            this.Lb_CegIranyitoszam.Name = "Lb_CegIranyitoszam";
            this.Lb_CegIranyitoszam.Size = new System.Drawing.Size(78, 15);
            this.Lb_CegIranyitoszam.TabIndex = 34;
            this.Lb_CegIranyitoszam.Text = "Irányítószám";
            // 
            // Lb_CegEmail
            // 
            this.Lb_CegEmail.AutoSize = true;
            this.Lb_CegEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegEmail.ForeColor = System.Drawing.Color.White;
            this.Lb_CegEmail.Location = new System.Drawing.Point(19, 247);
            this.Lb_CegEmail.Name = "Lb_CegEmail";
            this.Lb_CegEmail.Size = new System.Drawing.Size(41, 15);
            this.Lb_CegEmail.TabIndex = 33;
            this.Lb_CegEmail.Text = "Email:";
            // 
            // Lb_CegIgazgato
            // 
            this.Lb_CegIgazgato.AutoSize = true;
            this.Lb_CegIgazgato.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegIgazgato.ForeColor = System.Drawing.Color.White;
            this.Lb_CegIgazgato.Location = new System.Drawing.Point(19, 198);
            this.Lb_CegIgazgato.Name = "Lb_CegIgazgato";
            this.Lb_CegIgazgato.Size = new System.Drawing.Size(84, 15);
            this.Lb_CegIgazgato.TabIndex = 32;
            this.Lb_CegIgazgato.Text = "Igazgató neve:";
            // 
            // Lb_CegAdoszam
            // 
            this.Lb_CegAdoszam.AutoSize = true;
            this.Lb_CegAdoszam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegAdoszam.ForeColor = System.Drawing.Color.White;
            this.Lb_CegAdoszam.Location = new System.Drawing.Point(20, 148);
            this.Lb_CegAdoszam.Name = "Lb_CegAdoszam";
            this.Lb_CegAdoszam.Size = new System.Drawing.Size(59, 15);
            this.Lb_CegAdoszam.TabIndex = 31;
            this.Lb_CegAdoszam.Text = "Adószám:";
            // 
            // Lb_CegCegjegyzekSzam
            // 
            this.Lb_CegCegjegyzekSzam.AutoSize = true;
            this.Lb_CegCegjegyzekSzam.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegCegjegyzekSzam.ForeColor = System.Drawing.Color.White;
            this.Lb_CegCegjegyzekSzam.Location = new System.Drawing.Point(20, 97);
            this.Lb_CegCegjegyzekSzam.Name = "Lb_CegCegjegyzekSzam";
            this.Lb_CegCegjegyzekSzam.Size = new System.Drawing.Size(99, 15);
            this.Lb_CegCegjegyzekSzam.TabIndex = 30;
            this.Lb_CegCegjegyzekSzam.Text = "Cégjegyzék szám:";
            // 
            // Lb_CegNeve
            // 
            this.Lb_CegNeve.AutoSize = true;
            this.Lb_CegNeve.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegNeve.ForeColor = System.Drawing.Color.White;
            this.Lb_CegNeve.Location = new System.Drawing.Point(19, 45);
            this.Lb_CegNeve.Name = "Lb_CegNeve";
            this.Lb_CegNeve.Size = new System.Drawing.Size(30, 15);
            this.Lb_CegNeve.TabIndex = 29;
            this.Lb_CegNeve.Text = "Név:";
            // 
            // Lb_CegTelepules
            // 
            this.Lb_CegTelepules.AutoSize = true;
            this.Lb_CegTelepules.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegTelepules.ForeColor = System.Drawing.Color.White;
            this.Lb_CegTelepules.Location = new System.Drawing.Point(21, 342);
            this.Lb_CegTelepules.Name = "Lb_CegTelepules";
            this.Lb_CegTelepules.Size = new System.Drawing.Size(58, 15);
            this.Lb_CegTelepules.TabIndex = 28;
            this.Lb_CegTelepules.Text = "Település";
            // 
            // Btn_CegHozzaadas
            // 
            this.Btn_CegHozzaadas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_CegHozzaadas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_CegHozzaadas.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_CegHozzaadas.BorderRadius = 5;
            this.Btn_CegHozzaadas.BorderSize = 0;
            this.Btn_CegHozzaadas.FlatAppearance.BorderSize = 0;
            this.Btn_CegHozzaadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_CegHozzaadas.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_CegHozzaadas.ForeColor = System.Drawing.Color.Black;
            this.Btn_CegHozzaadas.Location = new System.Drawing.Point(130, 436);
            this.Btn_CegHozzaadas.Name = "Btn_CegHozzaadas";
            this.Btn_CegHozzaadas.Size = new System.Drawing.Size(100, 40);
            this.Btn_CegHozzaadas.TabIndex = 27;
            this.Btn_CegHozzaadas.Text = "Hozzáadás";
            this.Btn_CegHozzaadas.TextColor = System.Drawing.Color.Black;
            this.Btn_CegHozzaadas.UseVisualStyleBackColor = false;
            this.Btn_CegHozzaadas.Click += new System.EventHandler(this.Btn_CegHozzaadas_Click);
            // 
            // Tb_CegUtca
            // 
            this.Tb_CegUtca.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegUtca.Location = new System.Drawing.Point(22, 407);
            this.Tb_CegUtca.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegUtca.Name = "Tb_CegUtca";
            this.Tb_CegUtca.Size = new System.Drawing.Size(99, 23);
            this.Tb_CegUtca.TabIndex = 8;
            // 
            // Tb_CegTelepules
            // 
            this.Tb_CegTelepules.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegTelepules.Location = new System.Drawing.Point(22, 359);
            this.Tb_CegTelepules.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegTelepules.Name = "Tb_CegTelepules";
            this.Tb_CegTelepules.Size = new System.Drawing.Size(99, 23);
            this.Tb_CegTelepules.TabIndex = 7;
            // 
            // Tb_CegEmail
            // 
            this.Tb_CegEmail.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegEmail.Location = new System.Drawing.Point(22, 266);
            this.Tb_CegEmail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegEmail.Name = "Tb_CegEmail";
            this.Tb_CegEmail.Size = new System.Drawing.Size(209, 23);
            this.Tb_CegEmail.TabIndex = 5;
            // 
            // Tb_CegIgazgatoNeve
            // 
            this.Tb_CegIgazgatoNeve.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegIgazgatoNeve.Location = new System.Drawing.Point(22, 215);
            this.Tb_CegIgazgatoNeve.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegIgazgatoNeve.MaxLength = 200;
            this.Tb_CegIgazgatoNeve.Name = "Tb_CegIgazgatoNeve";
            this.Tb_CegIgazgatoNeve.Size = new System.Drawing.Size(209, 23);
            this.Tb_CegIgazgatoNeve.TabIndex = 4;
            // 
            // Tb_CegAdoszam
            // 
            this.Tb_CegAdoszam.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegAdoszam.Location = new System.Drawing.Point(22, 165);
            this.Tb_CegAdoszam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegAdoszam.MaxLength = 13;
            this.Tb_CegAdoszam.Name = "Tb_CegAdoszam";
            this.Tb_CegAdoszam.Size = new System.Drawing.Size(209, 23);
            this.Tb_CegAdoszam.TabIndex = 3;
            this.Tb_CegAdoszam.TextChanged += new System.EventHandler(this.Tb_CegAdoszam_TextChanged);
            // 
            // Tb_CegCegjegyzekSzam
            // 
            this.Tb_CegCegjegyzekSzam.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegCegjegyzekSzam.Location = new System.Drawing.Point(22, 114);
            this.Tb_CegCegjegyzekSzam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegCegjegyzekSzam.MaxLength = 12;
            this.Tb_CegCegjegyzekSzam.Name = "Tb_CegCegjegyzekSzam";
            this.Tb_CegCegjegyzekSzam.Size = new System.Drawing.Size(209, 23);
            this.Tb_CegCegjegyzekSzam.TabIndex = 2;
            this.Tb_CegCegjegyzekSzam.TextChanged += new System.EventHandler(this.Tb_CegCegjegyzekSzam_TextChanged);
            // 
            // Tb_CegNev
            // 
            this.Tb_CegNev.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.Tb_CegNev.Location = new System.Drawing.Point(22, 62);
            this.Tb_CegNev.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Tb_CegNev.Name = "Tb_CegNev";
            this.Tb_CegNev.Size = new System.Drawing.Size(209, 23);
            this.Tb_CegNev.TabIndex = 1;
            // 
            // Lb_AdminCeg
            // 
            this.Lb_AdminCeg.AutoSize = true;
            this.Lb_AdminCeg.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AdminCeg.ForeColor = System.Drawing.Color.White;
            this.Lb_AdminCeg.Location = new System.Drawing.Point(15, 8);
            this.Lb_AdminCeg.Name = "Lb_AdminCeg";
            this.Lb_AdminCeg.Size = new System.Drawing.Size(256, 26);
            this.Lb_AdminCeg.TabIndex = 8;
            this.Lb_AdminCeg.Text = "Cégek hozzáadása és törlése";
            // 
            // Pn_CegMain
            // 
            this.Pn_CegMain.Controls.Add(this.tLP_CegResztracio);
            this.Pn_CegMain.Location = new System.Drawing.Point(20, 61);
            this.Pn_CegMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Pn_CegMain.Name = "Pn_CegMain";
            this.Pn_CegMain.Size = new System.Drawing.Size(800, 487);
            this.Pn_CegMain.TabIndex = 9;
            // 
            // Admin_Company
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(840, 557);
            this.Controls.Add(this.Pn_CegMain);
            this.Controls.Add(this.Lb_AdminCeg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Admin_Company";
            this.Text = "Admin_CegRegisztracio";
            this.Load += new System.EventHandler(this.Admin_CegRegisztracio_Load);
            this.SizeChanged += new System.EventHandler(this.Admin_CegRegisztracio_SizeChanged);
            this.tLP_CegResztracio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AdminCegek)).EndInit();
            this.Pn_CegHozzaadasa.ResumeLayout(false);
            this.Pn_CegHozzaadasa.PerformLayout();
            this.Pn_CegMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tLP_CegResztracio;
        private System.Windows.Forms.Label Lb_AdminCeg;
        private System.Windows.Forms.DataGridView dGV_AdminCegek;
        private System.Windows.Forms.Panel Pn_CegHozzaadasa;
        private System.Windows.Forms.TextBox Tb_CegUtca;
        private System.Windows.Forms.TextBox Tb_CegTelepules;
        private System.Windows.Forms.TextBox Tb_CegEmail;
        private System.Windows.Forms.TextBox Tb_CegIgazgatoNeve;
        private System.Windows.Forms.TextBox Tb_CegAdoszam;
        private System.Windows.Forms.TextBox Tb_CegCegjegyzekSzam;
        private System.Windows.Forms.TextBox Tb_CegNev;
        private Customs.CCButton Btn_CegHozzaadas;
        private System.Windows.Forms.Label Lb_CegHazszam;
        private System.Windows.Forms.Label Lb_CegUtca;
        private System.Windows.Forms.Label Lb_CegIranyitoszam;
        private System.Windows.Forms.Label Lb_CegEmail;
        private System.Windows.Forms.Label Lb_CegIgazgato;
        private System.Windows.Forms.Label Lb_CegAdoszam;
        private System.Windows.Forms.Label Lb_CegCegjegyzekSzam;
        private System.Windows.Forms.Label Lb_CegNeve;
        private System.Windows.Forms.Label Lb_CegTelepules;
        private System.Windows.Forms.Panel Pn_CegMain;
        private System.Windows.Forms.TextBox Tb_Hazszam;
        private System.Windows.Forms.TextBox Tb_Iranyitoszam;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nev;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cim;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Aktiv;
        private System.Windows.Forms.DataGridViewButtonColumn CFelulet;
        private System.Windows.Forms.DataGridViewButtonColumn Torles;
        private System.Windows.Forms.Label Lb_CegHozzaadasCim;
    }
}