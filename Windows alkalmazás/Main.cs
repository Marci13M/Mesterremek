using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CareCompass
{
    public partial class Main : Form
    {
        //Aloldal változó
        public bool IsSubPage { get; set; } = false; // Alapértelmezett érték
        
        public Main()
        {
            InitializeComponent();
        }

        //Jogosultás változó
        public string role_identifier { get; set; }

        // Theme beállítása
        public void SetTheme(string theme)
        {
            ThemeManager.SetTheme(theme);
            ThemeManager.ApplyTheme(this);
        }

        //Panel publikussá tétele (Menu elérés miatt)
        public Panel GetPn_Fo()
        {
            return Pn_Fo;
        }

        //Form betöltése (megfelelő menü form meghívása )
        private void Main_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("bent vagyunk");
            Debug.WriteLine(role_identifier);
            ThemeManager.ApplyTheme(this);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            if ( role_identifier== "doctor")
            {
                Doctor_Menu dm = new Doctor_Menu();
                dm.TopLevel = false;
                Pn_Menu.Controls.Add(dm);
                dm.BringToFront();
                dm.Show();
                this.Resize += new EventHandler(Main_Resize);
            }
            else if(role_identifier== "hospital")
            {
                Hospital_Menu km = new Hospital_Menu();
                km.TopLevel = false;
                Pn_Menu.Controls.Add(km);
                km.BringToFront();
                km.Show();
                this.Resize += new EventHandler(Main_Resize);
            }
            else if (role_identifier == "company")
            {
                Company_Menu cm = new Company_Menu();
                cm.TopLevel = false;
                Pn_Menu.Controls.Add(cm);
                cm.BringToFront();
                cm.Show();
                this.Resize += new EventHandler(Main_Resize);
            }
            else if (role_identifier == "admin")
            {
                Admin_Menu am = new Admin_Menu();
                am.TopLevel = false;
                Pn_Menu.Controls.Add(am);
                am.BringToFront();
                am.Show();
                this.Resize += new EventHandler(Main_Resize);
            }
            Debug.WriteLine($"{IsSubPage}ez az állapot2");
        }

        //Form átméretezése (event)
        private void Main_Resize(object sender, EventArgs e)
        {
            foreach (Control control in Pn_Menu.Controls)
            {
                if (control is Doctor_Menu || control is Hospital_Menu || control is Company_Menu || control is Admin_Menu)
                {
                    control.Width = Pn_Menu.Width;
                    control.Height = Pn_Menu.Height;
                }
            }
        }
    }
}
