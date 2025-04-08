namespace CareCompass
{
    partial class Hospital_Services
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
            this.dGV_UjSzolgaltatas = new System.Windows.Forms.DataGridView();
            this.Nev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Leiras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idotartam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aktiv = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.modositas = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tLP_UjSzolgaltatas = new System.Windows.Forms.TableLayoutPanel();
            this.Pn_UjSzolgaltatasValaszto = new System.Windows.Forms.Panel();
            this.Pn_IdoAr = new System.Windows.Forms.Panel();
            this.nUP_SzolgaltatasIdo = new System.Windows.Forms.NumericUpDown();
            this.Lb_Perc = new System.Windows.Forms.Label();
            this.Lb_Ft = new System.Windows.Forms.Label();
            this.Lb_SzolgaltatasAr = new System.Windows.Forms.Label();
            this.Lb_SzolgaltatasIdo = new System.Windows.Forms.Label();
            this.nUP_SzolgaltatasAr = new System.Windows.Forms.NumericUpDown();
            this.Pn_UjSzolgaltatas = new System.Windows.Forms.Panel();
            this.CB_UjSzolgaltatas = new System.Windows.Forms.ComboBox();
            this.Lb_UjSzolgaltatas = new System.Windows.Forms.Label();
            this.Pn_SzolgaltatasLeiras = new System.Windows.Forms.Panel();
            this.Lb_SzolgaltatasLeiras = new System.Windows.Forms.Label();
            this.Tb_SzolgaltatasLeiras = new System.Windows.Forms.TextBox();
            this.Btn_SzolgaltatasHozzaadas = new CareCompass.Customs.CCButton();
            this.Lb_KorhazSzolgaltatasok = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_UjSzolgaltatas)).BeginInit();
            this.tLP_UjSzolgaltatas.SuspendLayout();
            this.Pn_UjSzolgaltatasValaszto.SuspendLayout();
            this.Pn_IdoAr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_SzolgaltatasIdo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_SzolgaltatasAr)).BeginInit();
            this.Pn_UjSzolgaltatas.SuspendLayout();
            this.Pn_SzolgaltatasLeiras.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGV_UjSzolgaltatas
            // 
            this.dGV_UjSzolgaltatas.AllowUserToAddRows = false;
            this.dGV_UjSzolgaltatas.AllowUserToDeleteRows = false;
            this.dGV_UjSzolgaltatas.AllowUserToResizeRows = false;
            this.dGV_UjSzolgaltatas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_UjSzolgaltatas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_UjSzolgaltatas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV_UjSzolgaltatas.ColumnHeadersHeight = 29;
            this.dGV_UjSzolgaltatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dGV_UjSzolgaltatas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nev,
            this.Leiras,
            this.idotartam,
            this.ar,
            this.aktiv,
            this.modositas});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_UjSzolgaltatas.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGV_UjSzolgaltatas.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dGV_UjSzolgaltatas.Location = new System.Drawing.Point(0, 0);
            this.dGV_UjSzolgaltatas.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_UjSzolgaltatas.Name = "dGV_UjSzolgaltatas";
            this.dGV_UjSzolgaltatas.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_UjSzolgaltatas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGV_UjSzolgaltatas.RowHeadersVisible = false;
            this.dGV_UjSzolgaltatas.RowHeadersWidth = 51;
            this.dGV_UjSzolgaltatas.Size = new System.Drawing.Size(789, 247);
            this.dGV_UjSzolgaltatas.TabIndex = 0;
            this.dGV_UjSzolgaltatas.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGV_UjSzolgaltatas_DataBindingComplete);
            // 
            // Nev
            // 
            this.Nev.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Nev.HeaderText = "Név";
            this.Nev.MinimumWidth = 52;
            this.Nev.Name = "Nev";
            this.Nev.ReadOnly = true;
            this.Nev.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Nev.Width = 52;
            // 
            // Leiras
            // 
            this.Leiras.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Leiras.HeaderText = "Leírás";
            this.Leiras.MinimumWidth = 455;
            this.Leiras.Name = "Leiras";
            this.Leiras.ReadOnly = true;
            this.Leiras.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // idotartam
            // 
            this.idotartam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.idotartam.HeaderText = "Időtartam";
            this.idotartam.MinimumWidth = 6;
            this.idotartam.Name = "idotartam";
            this.idotartam.ReadOnly = true;
            this.idotartam.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.idotartam.Width = 86;
            // 
            // ar
            // 
            this.ar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ar.HeaderText = "Ár";
            this.ar.MinimumWidth = 6;
            this.ar.Name = "ar";
            this.ar.ReadOnly = true;
            this.ar.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ar.Width = 43;
            // 
            // aktiv
            // 
            this.aktiv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.aktiv.HeaderText = "Aktív";
            this.aktiv.MinimumWidth = 6;
            this.aktiv.Name = "aktiv";
            this.aktiv.ReadOnly = true;
            this.aktiv.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.aktiv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.aktiv.Width = 57;
            // 
            // modositas
            // 
            this.modositas.HeaderText = "Módosítás";
            this.modositas.MinimumWidth = 80;
            this.modositas.Name = "modositas";
            this.modositas.ReadOnly = true;
            this.modositas.Width = 80;
            // 
            // tLP_UjSzolgaltatas
            // 
            this.tLP_UjSzolgaltatas.ColumnCount = 2;
            this.tLP_UjSzolgaltatas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 315F));
            this.tLP_UjSzolgaltatas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_UjSzolgaltatas.Controls.Add(this.Pn_UjSzolgaltatasValaszto, 0, 0);
            this.tLP_UjSzolgaltatas.Controls.Add(this.Pn_SzolgaltatasLeiras, 1, 0);
            this.tLP_UjSzolgaltatas.Location = new System.Drawing.Point(0, 247);
            this.tLP_UjSzolgaltatas.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_UjSzolgaltatas.MinimumSize = new System.Drawing.Size(777, 0);
            this.tLP_UjSzolgaltatas.Name = "tLP_UjSzolgaltatas";
            this.tLP_UjSzolgaltatas.RowCount = 1;
            this.tLP_UjSzolgaltatas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_UjSzolgaltatas.Size = new System.Drawing.Size(789, 236);
            this.tLP_UjSzolgaltatas.TabIndex = 1;
            // 
            // Pn_UjSzolgaltatasValaszto
            // 
            this.Pn_UjSzolgaltatasValaszto.Controls.Add(this.Pn_IdoAr);
            this.Pn_UjSzolgaltatasValaszto.Controls.Add(this.Pn_UjSzolgaltatas);
            this.Pn_UjSzolgaltatasValaszto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_UjSzolgaltatasValaszto.Location = new System.Drawing.Point(0, 0);
            this.Pn_UjSzolgaltatasValaszto.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_UjSzolgaltatasValaszto.Name = "Pn_UjSzolgaltatasValaszto";
            this.Pn_UjSzolgaltatasValaszto.Size = new System.Drawing.Size(315, 236);
            this.Pn_UjSzolgaltatasValaszto.TabIndex = 0;
            // 
            // Pn_IdoAr
            // 
            this.Pn_IdoAr.Controls.Add(this.nUP_SzolgaltatasIdo);
            this.Pn_IdoAr.Controls.Add(this.Lb_Perc);
            this.Pn_IdoAr.Controls.Add(this.Lb_Ft);
            this.Pn_IdoAr.Controls.Add(this.Lb_SzolgaltatasAr);
            this.Pn_IdoAr.Controls.Add(this.Lb_SzolgaltatasIdo);
            this.Pn_IdoAr.Controls.Add(this.nUP_SzolgaltatasAr);
            this.Pn_IdoAr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_IdoAr.Location = new System.Drawing.Point(0, 100);
            this.Pn_IdoAr.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_IdoAr.Name = "Pn_IdoAr";
            this.Pn_IdoAr.Size = new System.Drawing.Size(315, 136);
            this.Pn_IdoAr.TabIndex = 3;
            // 
            // nUP_SzolgaltatasIdo
            // 
            this.nUP_SzolgaltatasIdo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nUP_SzolgaltatasIdo.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUP_SzolgaltatasIdo.Location = new System.Drawing.Point(10, 50);
            this.nUP_SzolgaltatasIdo.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nUP_SzolgaltatasIdo.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUP_SzolgaltatasIdo.Name = "nUP_SzolgaltatasIdo";
            this.nUP_SzolgaltatasIdo.Size = new System.Drawing.Size(88, 23);
            this.nUP_SzolgaltatasIdo.TabIndex = 7;
            this.nUP_SzolgaltatasIdo.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUP_SzolgaltatasIdo.ValueChanged += new System.EventHandler(this.nUP_SzolgaltatasIdo_ValueChanged);
            this.nUP_SzolgaltatasIdo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nUP_SzolgaltatasIdo_KeyPress);
            // 
            // Lb_Perc
            // 
            this.Lb_Perc.AutoSize = true;
            this.Lb_Perc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Perc.ForeColor = System.Drawing.Color.White;
            this.Lb_Perc.Location = new System.Drawing.Point(105, 54);
            this.Lb_Perc.Name = "Lb_Perc";
            this.Lb_Perc.Size = new System.Drawing.Size(37, 19);
            this.Lb_Perc.TabIndex = 4;
            this.Lb_Perc.Text = "Perc";
            // 
            // Lb_Ft
            // 
            this.Lb_Ft.AutoSize = true;
            this.Lb_Ft.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_Ft.ForeColor = System.Drawing.Color.White;
            this.Lb_Ft.Location = new System.Drawing.Point(283, 54);
            this.Lb_Ft.Name = "Lb_Ft";
            this.Lb_Ft.Size = new System.Drawing.Size(21, 19);
            this.Lb_Ft.TabIndex = 2;
            this.Lb_Ft.Text = "Ft";
            // 
            // Lb_SzolgaltatasAr
            // 
            this.Lb_SzolgaltatasAr.AutoSize = true;
            this.Lb_SzolgaltatasAr.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_SzolgaltatasAr.ForeColor = System.Drawing.Color.White;
            this.Lb_SzolgaltatasAr.Location = new System.Drawing.Point(167, 28);
            this.Lb_SzolgaltatasAr.Name = "Lb_SzolgaltatasAr";
            this.Lb_SzolgaltatasAr.Size = new System.Drawing.Size(116, 19);
            this.Lb_SzolgaltatasAr.TabIndex = 5;
            this.Lb_SzolgaltatasAr.Text = "Szolgáltatás ára";
            // 
            // Lb_SzolgaltatasIdo
            // 
            this.Lb_SzolgaltatasIdo.AutoSize = true;
            this.Lb_SzolgaltatasIdo.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_SzolgaltatasIdo.ForeColor = System.Drawing.Color.White;
            this.Lb_SzolgaltatasIdo.Location = new System.Drawing.Point(7, 28);
            this.Lb_SzolgaltatasIdo.Name = "Lb_SzolgaltatasIdo";
            this.Lb_SzolgaltatasIdo.Size = new System.Drawing.Size(78, 19);
            this.Lb_SzolgaltatasIdo.TabIndex = 3;
            this.Lb_SzolgaltatasIdo.Text = "Időtartam";
            // 
            // nUP_SzolgaltatasAr
            // 
            this.nUP_SzolgaltatasAr.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nUP_SzolgaltatasAr.Location = new System.Drawing.Point(170, 50);
            this.nUP_SzolgaltatasAr.Maximum = new decimal(new int[] {
            9000000,
            0,
            0,
            0});
            this.nUP_SzolgaltatasAr.Name = "nUP_SzolgaltatasAr";
            this.nUP_SzolgaltatasAr.Size = new System.Drawing.Size(107, 23);
            this.nUP_SzolgaltatasAr.TabIndex = 1;
            // 
            // Pn_UjSzolgaltatas
            // 
            this.Pn_UjSzolgaltatas.Controls.Add(this.CB_UjSzolgaltatas);
            this.Pn_UjSzolgaltatas.Controls.Add(this.Lb_UjSzolgaltatas);
            this.Pn_UjSzolgaltatas.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pn_UjSzolgaltatas.Location = new System.Drawing.Point(0, 0);
            this.Pn_UjSzolgaltatas.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_UjSzolgaltatas.Name = "Pn_UjSzolgaltatas";
            this.Pn_UjSzolgaltatas.Size = new System.Drawing.Size(315, 100);
            this.Pn_UjSzolgaltatas.TabIndex = 2;
            // 
            // CB_UjSzolgaltatas
            // 
            this.CB_UjSzolgaltatas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_UjSzolgaltatas.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CB_UjSzolgaltatas.FormattingEnabled = true;
            this.CB_UjSzolgaltatas.Location = new System.Drawing.Point(10, 50);
            this.CB_UjSzolgaltatas.Name = "CB_UjSzolgaltatas";
            this.CB_UjSzolgaltatas.Size = new System.Drawing.Size(267, 23);
            this.CB_UjSzolgaltatas.TabIndex = 1;
            this.CB_UjSzolgaltatas.SelectedIndexChanged += new System.EventHandler(this.CB_UjSzolgaltatas_SelectedIndexChanged);
            // 
            // Lb_UjSzolgaltatas
            // 
            this.Lb_UjSzolgaltatas.AutoSize = true;
            this.Lb_UjSzolgaltatas.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_UjSzolgaltatas.ForeColor = System.Drawing.Color.White;
            this.Lb_UjSzolgaltatas.Location = new System.Drawing.Point(10, 10);
            this.Lb_UjSzolgaltatas.Name = "Lb_UjSzolgaltatas";
            this.Lb_UjSzolgaltatas.Size = new System.Drawing.Size(219, 23);
            this.Lb_UjSzolgaltatas.TabIndex = 0;
            this.Lb_UjSzolgaltatas.Text = "Új szolgáltatás hozzáadása";
            // 
            // Pn_SzolgaltatasLeiras
            // 
            this.Pn_SzolgaltatasLeiras.Controls.Add(this.Lb_SzolgaltatasLeiras);
            this.Pn_SzolgaltatasLeiras.Controls.Add(this.Tb_SzolgaltatasLeiras);
            this.Pn_SzolgaltatasLeiras.Controls.Add(this.Btn_SzolgaltatasHozzaadas);
            this.Pn_SzolgaltatasLeiras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_SzolgaltatasLeiras.Location = new System.Drawing.Point(315, 0);
            this.Pn_SzolgaltatasLeiras.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_SzolgaltatasLeiras.Name = "Pn_SzolgaltatasLeiras";
            this.Pn_SzolgaltatasLeiras.Size = new System.Drawing.Size(474, 236);
            this.Pn_SzolgaltatasLeiras.TabIndex = 3;
            // 
            // Lb_SzolgaltatasLeiras
            // 
            this.Lb_SzolgaltatasLeiras.AutoSize = true;
            this.Lb_SzolgaltatasLeiras.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_SzolgaltatasLeiras.ForeColor = System.Drawing.Color.White;
            this.Lb_SzolgaltatasLeiras.Location = new System.Drawing.Point(7, 24);
            this.Lb_SzolgaltatasLeiras.Name = "Lb_SzolgaltatasLeiras";
            this.Lb_SzolgaltatasLeiras.Size = new System.Drawing.Size(138, 19);
            this.Lb_SzolgaltatasLeiras.TabIndex = 5;
            this.Lb_SzolgaltatasLeiras.Text = "Szolgáltatás leírása";
            // 
            // Tb_SzolgaltatasLeiras
            // 
            this.Tb_SzolgaltatasLeiras.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_SzolgaltatasLeiras.Location = new System.Drawing.Point(10, 50);
            this.Tb_SzolgaltatasLeiras.Margin = new System.Windows.Forms.Padding(0);
            this.Tb_SzolgaltatasLeiras.Multiline = true;
            this.Tb_SzolgaltatasLeiras.Name = "Tb_SzolgaltatasLeiras";
            this.Tb_SzolgaltatasLeiras.Size = new System.Drawing.Size(425, 123);
            this.Tb_SzolgaltatasLeiras.TabIndex = 4;
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
            this.Btn_SzolgaltatasHozzaadas.Location = new System.Drawing.Point(285, 180);
            this.Btn_SzolgaltatasHozzaadas.Name = "Btn_SzolgaltatasHozzaadas";
            this.Btn_SzolgaltatasHozzaadas.Size = new System.Drawing.Size(150, 40);
            this.Btn_SzolgaltatasHozzaadas.TabIndex = 3;
            this.Btn_SzolgaltatasHozzaadas.Text = "Hozzáadás";
            this.Btn_SzolgaltatasHozzaadas.TextColor = System.Drawing.Color.Black;
            this.Btn_SzolgaltatasHozzaadas.UseVisualStyleBackColor = false;
            this.Btn_SzolgaltatasHozzaadas.Click += new System.EventHandler(this.Btn_SzolgaltatasHozzaadas_Click);
            // 
            // Lb_KorhazSzolgaltatasok
            // 
            this.Lb_KorhazSzolgaltatasok.AutoSize = true;
            this.Lb_KorhazSzolgaltatasok.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazSzolgaltatasok.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazSzolgaltatasok.Location = new System.Drawing.Point(20, 10);
            this.Lb_KorhazSzolgaltatasok.Name = "Lb_KorhazSzolgaltatasok";
            this.Lb_KorhazSzolgaltatasok.Size = new System.Drawing.Size(134, 26);
            this.Lb_KorhazSzolgaltatasok.TabIndex = 2;
            this.Lb_KorhazSzolgaltatasok.Text = "Szolgáltatások";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dGV_UjSzolgaltatas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tLP_UjSzolgaltatas, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(25, 48);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(789, 483);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Hospital_Services
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(840, 557);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Lb_KorhazSzolgaltatasok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(840, 557);
            this.Name = "Hospital_Services";
            this.Text = "Korhaz_Services";
            this.Load += new System.EventHandler(this.Korhaz_Services_Load);
            this.SizeChanged += new System.EventHandler(this.Korhaz_Services_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_UjSzolgaltatas)).EndInit();
            this.tLP_UjSzolgaltatas.ResumeLayout(false);
            this.Pn_UjSzolgaltatasValaszto.ResumeLayout(false);
            this.Pn_IdoAr.ResumeLayout(false);
            this.Pn_IdoAr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_SzolgaltatasIdo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_SzolgaltatasAr)).EndInit();
            this.Pn_UjSzolgaltatas.ResumeLayout(false);
            this.Pn_UjSzolgaltatas.PerformLayout();
            this.Pn_SzolgaltatasLeiras.ResumeLayout(false);
            this.Pn_SzolgaltatasLeiras.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_UjSzolgaltatas;
        private System.Windows.Forms.TableLayoutPanel tLP_UjSzolgaltatas;
        private System.Windows.Forms.Label Lb_UjSzolgaltatas;
        private System.Windows.Forms.ComboBox CB_UjSzolgaltatas;
        private Customs.CCButton Btn_SzolgaltatasHozzaadas;
        private System.Windows.Forms.TextBox Tb_SzolgaltatasLeiras;
        private System.Windows.Forms.Label Lb_SzolgaltatasLeiras;
        private System.Windows.Forms.Label Lb_KorhazSzolgaltatasok;
        private System.Windows.Forms.Panel Pn_SzolgaltatasLeiras;
        private System.Windows.Forms.Panel Pn_UjSzolgaltatasValaszto;
        private System.Windows.Forms.Label Lb_SzolgaltatasAr;
        private System.Windows.Forms.NumericUpDown nUP_SzolgaltatasAr;
        private System.Windows.Forms.Label Lb_Ft;
        private System.Windows.Forms.Label Lb_Perc;
        private System.Windows.Forms.Label Lb_SzolgaltatasIdo;
        private System.Windows.Forms.Panel Pn_IdoAr;
        private System.Windows.Forms.Panel Pn_UjSzolgaltatas;
        private System.Windows.Forms.NumericUpDown nUP_SzolgaltatasIdo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nev;
        private System.Windows.Forms.DataGridViewTextBoxColumn Leiras;
        private System.Windows.Forms.DataGridViewTextBoxColumn idotartam;
        private System.Windows.Forms.DataGridViewTextBoxColumn ar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn aktiv;
        private System.Windows.Forms.DataGridViewButtonColumn modositas;
    }
}