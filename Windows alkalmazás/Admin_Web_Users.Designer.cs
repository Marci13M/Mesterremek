namespace CareCompass
{
    partial class Admin_Web_Users
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
            this.Pn_MainPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Felahasznalo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Emial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Szuldatum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Modositas = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Torles = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Lb_AdminWebUser = new System.Windows.Forms.Label();
            this.Pn_MainPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Pn_MainPanel
            // 
            this.Pn_MainPanel.Controls.Add(this.tableLayoutPanel1);
            this.Pn_MainPanel.Location = new System.Drawing.Point(24, 63);
            this.Pn_MainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.Pn_MainPanel.Name = "Pn_MainPanel";
            this.Pn_MainPanel.Size = new System.Drawing.Size(1067, 599);
            this.Pn_MainPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1067, 599);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Felahasznalo,
            this.Emial,
            this.Szuldatum,
            this.Modositas,
            this.Torles});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1067, 599);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // Felahasznalo
            // 
            this.Felahasznalo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Felahasznalo.HeaderText = "Felhasználónév";
            this.Felahasznalo.MinimumWidth = 100;
            this.Felahasznalo.Name = "Felahasznalo";
            this.Felahasznalo.ReadOnly = true;
            this.Felahasznalo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Felahasznalo.Width = 131;
            // 
            // Emial
            // 
            this.Emial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Emial.HeaderText = "Email";
            this.Emial.MinimumWidth = 200;
            this.Emial.Name = "Emial";
            this.Emial.ReadOnly = true;
            // 
            // Szuldatum
            // 
            this.Szuldatum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Szuldatum.HeaderText = "Születésnap";
            this.Szuldatum.MinimumWidth = 100;
            this.Szuldatum.Name = "Szuldatum";
            this.Szuldatum.ReadOnly = true;
            this.Szuldatum.Width = 110;
            // 
            // Modositas
            // 
            this.Modositas.HeaderText = "Módosítás";
            this.Modositas.MinimumWidth = 80;
            this.Modositas.Name = "Modositas";
            this.Modositas.ReadOnly = true;
            this.Modositas.Width = 80;
            // 
            // Torles
            // 
            this.Torles.HeaderText = "Törlés";
            this.Torles.MinimumWidth = 80;
            this.Torles.Name = "Torles";
            this.Torles.ReadOnly = true;
            this.Torles.Width = 80;
            // 
            // Lb_AdminWebUser
            // 
            this.Lb_AdminWebUser.AutoSize = true;
            this.Lb_AdminWebUser.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lb_AdminWebUser.ForeColor = System.Drawing.Color.White;
            this.Lb_AdminWebUser.Location = new System.Drawing.Point(20, 10);
            this.Lb_AdminWebUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_AdminWebUser.Name = "Lb_AdminWebUser";
            this.Lb_AdminWebUser.Size = new System.Drawing.Size(446, 33);
            this.Lb_AdminWebUser.TabIndex = 9;
            this.Lb_AdminWebUser.Text = "Web felhasználó  módosítás és törlése";
            // 
            // Admin_Web_Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 686);
            this.Controls.Add(this.Lb_AdminWebUser);
            this.Controls.Add(this.Pn_MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Admin_Web_Users";
            this.Text = "Admin_Web_Users";
            this.Load += new System.EventHandler(this.Admin_Web_Users_Load);
            this.SizeChanged += new System.EventHandler(this.Admin_Web_Users_SizeChanged);
            this.Pn_MainPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Pn_MainPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label Lb_AdminWebUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn Felahasznalo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Emial;
        private System.Windows.Forms.DataGridViewTextBoxColumn Szuldatum;
        private System.Windows.Forms.DataGridViewButtonColumn Modositas;
        private System.Windows.Forms.DataGridViewButtonColumn Torles;
    }
}