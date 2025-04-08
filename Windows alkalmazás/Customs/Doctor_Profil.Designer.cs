namespace CareCompass.Customs
{
    partial class Doctor_Profil
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
            this.ccCircularPictureBox1 = new CareCompass.Customs.CCCircularPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ccCircularPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ccCircularPictureBox1
            // 
            this.ccCircularPictureBox1.BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat;
            this.ccCircularPictureBox1.BorderColor = System.Drawing.Color.RoyalBlue;
            this.ccCircularPictureBox1.BorderColor2 = System.Drawing.Color.HotPink;
            this.ccCircularPictureBox1.BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.ccCircularPictureBox1.BorderSize = 3;
            this.ccCircularPictureBox1.GradientAngle = 50F;
            this.ccCircularPictureBox1.Image = global::CareCompass.Properties.Resources.profile;
            this.ccCircularPictureBox1.Location = new System.Drawing.Point(12, 12);
            this.ccCircularPictureBox1.Name = "ccCircularPictureBox1";
            this.ccCircularPictureBox1.Size = new System.Drawing.Size(100, 100);
            this.ccCircularPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ccCircularPictureBox1.TabIndex = 0;
            this.ccCircularPictureBox1.TabStop = false;
            // 
            // Doctor_Profil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(840, 557);
            this.Controls.Add(this.ccCircularPictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Doctor_Profil";
            this.Text = "Doctor_Profil";
            ((System.ComponentModel.ISupportInitialize)(this.ccCircularPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCCircularPictureBox ccCircularPictureBox1;
    }
}