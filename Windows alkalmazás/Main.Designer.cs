namespace CareCompass
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Pn_Menu = new System.Windows.Forms.Panel();
            this.Pn_Fo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Pn_Menu
            // 
            this.Pn_Menu.BackColor = System.Drawing.Color.SteelBlue;
            this.Pn_Menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pn_Menu.Location = new System.Drawing.Point(0, 0);
            this.Pn_Menu.MinimumSize = new System.Drawing.Size(220, 557);
            this.Pn_Menu.Name = "Pn_Menu";
            this.Pn_Menu.Size = new System.Drawing.Size(176, 557);
            this.Pn_Menu.TabIndex = 0;
            // 
            // Pn_Fo
            // 
            this.Pn_Fo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pn_Fo.Location = new System.Drawing.Point(141, 0);
            this.Pn_Fo.MinimumSize = new System.Drawing.Size(840, 557);
            this.Pn_Fo.Name = "Pn_Fo";
            this.Pn_Fo.Size = new System.Drawing.Size(919, 557);
            this.Pn_Fo.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1325, 696);
            this.Controls.Add(this.Pn_Fo);
            this.Controls.Add(this.Pn_Menu);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1080, 600);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CareCompass";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Pn_Fo;
        public System.Windows.Forms.Panel Pn_Menu;
    }
}