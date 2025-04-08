namespace CareCompass
{
    partial class Company_Workers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Pn_CegAlkalmazottak = new System.Windows.Forms.Panel();
            this.tLP_CegAlkalmazottak = new System.Windows.Forms.TableLayoutPanel();
            this.tLP_Moderatorok = new System.Windows.Forms.TableLayoutPanel();
            this.dGV_Moderatorok = new System.Windows.Forms.DataGridView();
            this.MNev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MIntezmeny = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MTorles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Pn_Orvosok = new System.Windows.Forms.Panel();
            this.Lb_Orvosok = new System.Windows.Forms.Label();
            this.dGV_Orvosok = new System.Windows.Forms.DataGridView();
            this.nev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.torles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Pn_Moderatorok = new System.Windows.Forms.Panel();
            this.Lb_Moderatorok = new System.Windows.Forms.Label();
            this.Pn_UjAlkalmazott = new System.Windows.Forms.Panel();
            this.CB_AlkalmazottKorhaz = new System.Windows.Forms.ComboBox();
            this.Lb_AlkalmazottKorhaz = new System.Windows.Forms.Label();
            this.CB_AlkalmazottMunkaterulet = new System.Windows.Forms.ComboBox();
            this.Btn_AlkalmazottHozzaadas = new CareCompass.Customs.CCButton();
            this.Tb_AlkalmazottEmail = new System.Windows.Forms.TextBox();
            this.Lb_KorhazEmail = new System.Windows.Forms.Label();
            this.Lb_OrvosNev = new System.Windows.Forms.Label();
            this.Lb_AlkalmazottHozzaadas = new System.Windows.Forms.Label();
            this.Lb_CegAlkalmazottak = new System.Windows.Forms.Label();
            this.Pn_CegAlkalmazottak.SuspendLayout();
            this.tLP_CegAlkalmazottak.SuspendLayout();
            this.tLP_Moderatorok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Moderatorok)).BeginInit();
            this.Pn_Orvosok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orvosok)).BeginInit();
            this.Pn_Moderatorok.SuspendLayout();
            this.Pn_UjAlkalmazott.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pn_CegAlkalmazottak
            // 
            this.Pn_CegAlkalmazottak.Controls.Add(this.tLP_CegAlkalmazottak);
            this.Pn_CegAlkalmazottak.Location = new System.Drawing.Point(27, 44);
            this.Pn_CegAlkalmazottak.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_CegAlkalmazottak.Name = "Pn_CegAlkalmazottak";
            this.Pn_CegAlkalmazottak.Size = new System.Drawing.Size(1067, 599);
            this.Pn_CegAlkalmazottak.TabIndex = 1;
            // 
            // tLP_CegAlkalmazottak
            // 
            this.tLP_CegAlkalmazottak.ColumnCount = 3;
            this.tLP_CegAlkalmazottak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_CegAlkalmazottak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tLP_CegAlkalmazottak.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tLP_CegAlkalmazottak.Controls.Add(this.tLP_Moderatorok, 0, 0);
            this.tLP_CegAlkalmazottak.Controls.Add(this.Pn_UjAlkalmazott, 2, 0);
            this.tLP_CegAlkalmazottak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_CegAlkalmazottak.Location = new System.Drawing.Point(0, 0);
            this.tLP_CegAlkalmazottak.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_CegAlkalmazottak.Name = "tLP_CegAlkalmazottak";
            this.tLP_CegAlkalmazottak.RowCount = 1;
            this.tLP_CegAlkalmazottak.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_CegAlkalmazottak.Size = new System.Drawing.Size(1067, 599);
            this.tLP_CegAlkalmazottak.TabIndex = 0;
            // 
            // tLP_Moderatorok
            // 
            this.tLP_Moderatorok.ColumnCount = 1;
            this.tLP_Moderatorok.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_Moderatorok.Controls.Add(this.dGV_Moderatorok, 0, 3);
            this.tLP_Moderatorok.Controls.Add(this.Pn_Orvosok, 0, 0);
            this.tLP_Moderatorok.Controls.Add(this.dGV_Orvosok, 0, 1);
            this.tLP_Moderatorok.Controls.Add(this.Pn_Moderatorok, 0, 2);
            this.tLP_Moderatorok.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_Moderatorok.Location = new System.Drawing.Point(0, 0);
            this.tLP_Moderatorok.Margin = new System.Windows.Forms.Padding(0);
            this.tLP_Moderatorok.Name = "tLP_Moderatorok";
            this.tLP_Moderatorok.RowCount = 4;
            this.tLP_Moderatorok.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tLP_Moderatorok.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Moderatorok.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tLP_Moderatorok.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Moderatorok.Size = new System.Drawing.Size(707, 599);
            this.tLP_Moderatorok.TabIndex = 1;
            // 
            // dGV_Moderatorok
            // 
            this.dGV_Moderatorok.AllowUserToAddRows = false;
            this.dGV_Moderatorok.AllowUserToDeleteRows = false;
            this.dGV_Moderatorok.AllowUserToResizeRows = false;
            this.dGV_Moderatorok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_Moderatorok.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Moderatorok.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV_Moderatorok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Moderatorok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MNev,
            this.MEmail,
            this.MRole,
            this.MIntezmeny,
            this.MTorles});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_Moderatorok.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGV_Moderatorok.Location = new System.Drawing.Point(0, 338);
            this.dGV_Moderatorok.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_Moderatorok.Name = "dGV_Moderatorok";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Moderatorok.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGV_Moderatorok.RowHeadersVisible = false;
            this.dGV_Moderatorok.RowHeadersWidth = 51;
            this.dGV_Moderatorok.RowTemplate.Height = 24;
            this.dGV_Moderatorok.Size = new System.Drawing.Size(707, 260);
            this.dGV_Moderatorok.TabIndex = 1;
            this.dGV_Moderatorok.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGV_Moderatorok_DataBindingComplete);
            // 
            // MNev
            // 
            this.MNev.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MNev.HeaderText = "Név";
            this.MNev.MinimumWidth = 6;
            this.MNev.Name = "MNev";
            this.MNev.Width = 62;
            // 
            // MEmail
            // 
            this.MEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MEmail.HeaderText = "Email";
            this.MEmail.MinimumWidth = 200;
            this.MEmail.Name = "MEmail";
            this.MEmail.ReadOnly = true;
            // 
            // MRole
            // 
            this.MRole.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MRole.HeaderText = "Hatáskör";
            this.MRole.MinimumWidth = 6;
            this.MRole.Name = "MRole";
            this.MRole.ReadOnly = true;
            this.MRole.Width = 90;
            // 
            // MIntezmeny
            // 
            this.MIntezmeny.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MIntezmeny.HeaderText = "Intézmény";
            this.MIntezmeny.MinimumWidth = 6;
            this.MIntezmeny.Name = "MIntezmeny";
            this.MIntezmeny.ReadOnly = true;
            this.MIntezmeny.Width = 103;
            // 
            // MTorles
            // 
            this.MTorles.HeaderText = "Törlés";
            this.MTorles.MinimumWidth = 80;
            this.MTorles.Name = "MTorles";
            this.MTorles.ReadOnly = true;
            this.MTorles.Width = 80;
            // 
            // Pn_Orvosok
            // 
            this.Pn_Orvosok.Controls.Add(this.Lb_Orvosok);
            this.Pn_Orvosok.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_Orvosok.Location = new System.Drawing.Point(0, 0);
            this.Pn_Orvosok.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_Orvosok.Name = "Pn_Orvosok";
            this.Pn_Orvosok.Size = new System.Drawing.Size(707, 39);
            this.Pn_Orvosok.TabIndex = 2;
            // 
            // Lb_Orvosok
            // 
            this.Lb_Orvosok.AutoSize = true;
            this.Lb_Orvosok.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.Lb_Orvosok.ForeColor = System.Drawing.Color.White;
            this.Lb_Orvosok.Location = new System.Drawing.Point(-5, 5);
            this.Lb_Orvosok.Name = "Lb_Orvosok";
            this.Lb_Orvosok.Size = new System.Drawing.Size(97, 29);
            this.Lb_Orvosok.TabIndex = 2;
            this.Lb_Orvosok.Text = "Orvosok";
            // 
            // dGV_Orvosok
            // 
            this.dGV_Orvosok.AllowUserToAddRows = false;
            this.dGV_Orvosok.AllowUserToDeleteRows = false;
            this.dGV_Orvosok.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dGV_Orvosok.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Orvosok.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dGV_Orvosok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Orvosok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nev,
            this.email,
            this.torles});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_Orvosok.DefaultCellStyle = dataGridViewCellStyle5;
            this.dGV_Orvosok.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Orvosok.Location = new System.Drawing.Point(0, 39);
            this.dGV_Orvosok.Margin = new System.Windows.Forms.Padding(0);
            this.dGV_Orvosok.Name = "dGV_Orvosok";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_Orvosok.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dGV_Orvosok.RowHeadersVisible = false;
            this.dGV_Orvosok.RowHeadersWidth = 51;
            this.dGV_Orvosok.Size = new System.Drawing.Size(707, 260);
            this.dGV_Orvosok.TabIndex = 1;
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
            this.email.MinimumWidth = 6;
            this.email.Name = "email";
            this.email.ReadOnly = true;
            // 
            // torles
            // 
            this.torles.HeaderText = "Törlés";
            this.torles.MinimumWidth = 6;
            this.torles.Name = "torles";
            this.torles.ReadOnly = true;
            this.torles.Width = 80;
            // 
            // Pn_Moderatorok
            // 
            this.Pn_Moderatorok.Controls.Add(this.Lb_Moderatorok);
            this.Pn_Moderatorok.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_Moderatorok.Location = new System.Drawing.Point(0, 299);
            this.Pn_Moderatorok.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_Moderatorok.Name = "Pn_Moderatorok";
            this.Pn_Moderatorok.Size = new System.Drawing.Size(707, 39);
            this.Pn_Moderatorok.TabIndex = 0;
            // 
            // Lb_Moderatorok
            // 
            this.Lb_Moderatorok.AutoSize = true;
            this.Lb_Moderatorok.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.Lb_Moderatorok.ForeColor = System.Drawing.Color.White;
            this.Lb_Moderatorok.Location = new System.Drawing.Point(-5, 5);
            this.Lb_Moderatorok.Name = "Lb_Moderatorok";
            this.Lb_Moderatorok.Size = new System.Drawing.Size(147, 29);
            this.Lb_Moderatorok.TabIndex = 3;
            this.Lb_Moderatorok.Text = "Moderátorok";
            // 
            // Pn_UjAlkalmazott
            // 
            this.Pn_UjAlkalmazott.Controls.Add(this.CB_AlkalmazottKorhaz);
            this.Pn_UjAlkalmazott.Controls.Add(this.Lb_AlkalmazottKorhaz);
            this.Pn_UjAlkalmazott.Controls.Add(this.CB_AlkalmazottMunkaterulet);
            this.Pn_UjAlkalmazott.Controls.Add(this.Btn_AlkalmazottHozzaadas);
            this.Pn_UjAlkalmazott.Controls.Add(this.Tb_AlkalmazottEmail);
            this.Pn_UjAlkalmazott.Controls.Add(this.Lb_KorhazEmail);
            this.Pn_UjAlkalmazott.Controls.Add(this.Lb_OrvosNev);
            this.Pn_UjAlkalmazott.Controls.Add(this.Lb_AlkalmazottHozzaadas);
            this.Pn_UjAlkalmazott.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_UjAlkalmazott.Location = new System.Drawing.Point(747, 0);
            this.Pn_UjAlkalmazott.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_UjAlkalmazott.MinimumSize = new System.Drawing.Size(300, 599);
            this.Pn_UjAlkalmazott.Name = "Pn_UjAlkalmazott";
            this.Pn_UjAlkalmazott.Size = new System.Drawing.Size(320, 599);
            this.Pn_UjAlkalmazott.TabIndex = 0;
            // 
            // CB_AlkalmazottKorhaz
            // 
            this.CB_AlkalmazottKorhaz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_AlkalmazottKorhaz.FormattingEnabled = true;
            this.CB_AlkalmazottKorhaz.Location = new System.Drawing.Point(17, 231);
            this.CB_AlkalmazottKorhaz.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CB_AlkalmazottKorhaz.Name = "CB_AlkalmazottKorhaz";
            this.CB_AlkalmazottKorhaz.Size = new System.Drawing.Size(287, 24);
            this.CB_AlkalmazottKorhaz.TabIndex = 19;
            // 
            // Lb_AlkalmazottKorhaz
            // 
            this.Lb_AlkalmazottKorhaz.AutoSize = true;
            this.Lb_AlkalmazottKorhaz.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AlkalmazottKorhaz.ForeColor = System.Drawing.Color.White;
            this.Lb_AlkalmazottKorhaz.Location = new System.Drawing.Point(16, 206);
            this.Lb_AlkalmazottKorhaz.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_AlkalmazottKorhaz.Name = "Lb_AlkalmazottKorhaz";
            this.Lb_AlkalmazottKorhaz.Size = new System.Drawing.Size(72, 24);
            this.Lb_AlkalmazottKorhaz.TabIndex = 18;
            this.Lb_AlkalmazottKorhaz.Text = "Kórház:";
            // 
            // CB_AlkalmazottMunkaterulet
            // 
            this.CB_AlkalmazottMunkaterulet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_AlkalmazottMunkaterulet.FormattingEnabled = true;
            this.CB_AlkalmazottMunkaterulet.Location = new System.Drawing.Point(17, 100);
            this.CB_AlkalmazottMunkaterulet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CB_AlkalmazottMunkaterulet.Name = "CB_AlkalmazottMunkaterulet";
            this.CB_AlkalmazottMunkaterulet.Size = new System.Drawing.Size(287, 24);
            this.CB_AlkalmazottMunkaterulet.TabIndex = 17;
            this.CB_AlkalmazottMunkaterulet.SelectedIndexChanged += new System.EventHandler(this.CB_AlkalmazottMunkaterulet_SelectedIndexChanged);
            // 
            // Btn_AlkalmazottHozzaadas
            // 
            this.Btn_AlkalmazottHozzaadas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_AlkalmazottHozzaadas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(204)))), ((int)(((byte)(188)))));
            this.Btn_AlkalmazottHozzaadas.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Btn_AlkalmazottHozzaadas.BorderRadius = 5;
            this.Btn_AlkalmazottHozzaadas.BorderSize = 0;
            this.Btn_AlkalmazottHozzaadas.FlatAppearance.BorderSize = 0;
            this.Btn_AlkalmazottHozzaadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_AlkalmazottHozzaadas.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Btn_AlkalmazottHozzaadas.ForeColor = System.Drawing.Color.Black;
            this.Btn_AlkalmazottHozzaadas.Location = new System.Drawing.Point(105, 281);
            this.Btn_AlkalmazottHozzaadas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_AlkalmazottHozzaadas.Name = "Btn_AlkalmazottHozzaadas";
            this.Btn_AlkalmazottHozzaadas.Size = new System.Drawing.Size(200, 49);
            this.Btn_AlkalmazottHozzaadas.TabIndex = 16;
            this.Btn_AlkalmazottHozzaadas.Text = "Hozzáadás";
            this.Btn_AlkalmazottHozzaadas.TextColor = System.Drawing.Color.Black;
            this.Btn_AlkalmazottHozzaadas.UseVisualStyleBackColor = false;
            this.Btn_AlkalmazottHozzaadas.Click += new System.EventHandler(this.Btn_AlkalmazottHozzaadas_Click);
            // 
            // Tb_AlkalmazottEmail
            // 
            this.Tb_AlkalmazottEmail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tb_AlkalmazottEmail.Location = new System.Drawing.Point(17, 166);
            this.Tb_AlkalmazottEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tb_AlkalmazottEmail.Name = "Tb_AlkalmazottEmail";
            this.Tb_AlkalmazottEmail.Size = new System.Drawing.Size(287, 27);
            this.Tb_AlkalmazottEmail.TabIndex = 15;
            // 
            // Lb_KorhazEmail
            // 
            this.Lb_KorhazEmail.AutoSize = true;
            this.Lb_KorhazEmail.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_KorhazEmail.ForeColor = System.Drawing.Color.White;
            this.Lb_KorhazEmail.Location = new System.Drawing.Point(16, 138);
            this.Lb_KorhazEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_KorhazEmail.Name = "Lb_KorhazEmail";
            this.Lb_KorhazEmail.Size = new System.Drawing.Size(209, 24);
            this.Lb_KorhazEmail.TabIndex = 10;
            this.Lb_KorhazEmail.Text = "Alkalmazott email címe:";
            // 
            // Lb_OrvosNev
            // 
            this.Lb_OrvosNev.AutoSize = true;
            this.Lb_OrvosNev.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_OrvosNev.ForeColor = System.Drawing.Color.White;
            this.Lb_OrvosNev.Location = new System.Drawing.Point(16, 74);
            this.Lb_OrvosNev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_OrvosNev.Name = "Lb_OrvosNev";
            this.Lb_OrvosNev.Size = new System.Drawing.Size(243, 24);
            this.Lb_OrvosNev.TabIndex = 8;
            this.Lb_OrvosNev.Text = "Alkalmazott munkaterülete:";
            // 
            // Lb_AlkalmazottHozzaadas
            // 
            this.Lb_AlkalmazottHozzaadas.AutoSize = true;
            this.Lb_AlkalmazottHozzaadas.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AlkalmazottHozzaadas.ForeColor = System.Drawing.Color.White;
            this.Lb_AlkalmazottHozzaadas.Location = new System.Drawing.Point(13, 12);
            this.Lb_AlkalmazottHozzaadas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_AlkalmazottHozzaadas.Name = "Lb_AlkalmazottHozzaadas";
            this.Lb_AlkalmazottHozzaadas.Size = new System.Drawing.Size(281, 29);
            this.Lb_AlkalmazottHozzaadas.TabIndex = 6;
            this.Lb_AlkalmazottHozzaadas.Text = "Új alkalmazott hozzáadása";
            // 
            // Lb_CegAlkalmazottak
            // 
            this.Lb_CegAlkalmazottak.AutoSize = true;
            this.Lb_CegAlkalmazottak.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_CegAlkalmazottak.ForeColor = System.Drawing.Color.White;
            this.Lb_CegAlkalmazottak.Location = new System.Drawing.Point(20, 10);
            this.Lb_CegAlkalmazottak.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_CegAlkalmazottak.Name = "Lb_CegAlkalmazottak";
            this.Lb_CegAlkalmazottak.Size = new System.Drawing.Size(176, 33);
            this.Lb_CegAlkalmazottak.TabIndex = 6;
            this.Lb_CegAlkalmazottak.Text = "Alkalmazottak";
            // 
            // Company_Workers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.Lb_CegAlkalmazottak);
            this.Controls.Add(this.Pn_CegAlkalmazottak);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Company_Workers";
            this.Text = "Ceg_Orvos";
            this.Load += new System.EventHandler(this.Ceg_Orvos_Load);
            this.SizeChanged += new System.EventHandler(this.Ceg_Orvos_SizeChanged);
            this.Pn_CegAlkalmazottak.ResumeLayout(false);
            this.tLP_CegAlkalmazottak.ResumeLayout(false);
            this.tLP_Moderatorok.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Moderatorok)).EndInit();
            this.Pn_Orvosok.ResumeLayout(false);
            this.Pn_Orvosok.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orvosok)).EndInit();
            this.Pn_Moderatorok.ResumeLayout(false);
            this.Pn_Moderatorok.PerformLayout();
            this.Pn_UjAlkalmazott.ResumeLayout(false);
            this.Pn_UjAlkalmazott.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Pn_CegAlkalmazottak;
        private System.Windows.Forms.TableLayoutPanel tLP_CegAlkalmazottak;
        private System.Windows.Forms.DataGridView dGV_Orvosok;
        private System.Windows.Forms.Panel Pn_UjAlkalmazott;
        private Customs.CCButton Btn_AlkalmazottHozzaadas;
        private System.Windows.Forms.TextBox Tb_AlkalmazottEmail;
        private System.Windows.Forms.Label Lb_KorhazEmail;
        private System.Windows.Forms.Label Lb_OrvosNev;
        private System.Windows.Forms.Label Lb_AlkalmazottHozzaadas;
        private System.Windows.Forms.Label Lb_CegAlkalmazottak;
        private System.Windows.Forms.Panel Pn_Orvosok;
        private System.Windows.Forms.Label Lb_Orvosok;
        private System.Windows.Forms.TableLayoutPanel tLP_Moderatorok;
        private System.Windows.Forms.Panel Pn_Moderatorok;
        private System.Windows.Forms.Label Lb_Moderatorok;
        private System.Windows.Forms.DataGridView dGV_Moderatorok;
        private System.Windows.Forms.ComboBox CB_AlkalmazottKorhaz;
        private System.Windows.Forms.Label Lb_AlkalmazottKorhaz;
        private System.Windows.Forms.ComboBox CB_AlkalmazottMunkaterulet;
        private System.Windows.Forms.DataGridViewTextBoxColumn nev;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewButtonColumn torles;
        private System.Windows.Forms.DataGridViewTextBoxColumn MNev;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn MRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn MIntezmeny;
        private System.Windows.Forms.DataGridViewButtonColumn MTorles;
    }
}