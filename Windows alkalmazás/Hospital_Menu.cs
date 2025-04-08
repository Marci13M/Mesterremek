using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CareCompass
{
    public partial class Hospital_Menu : Form
    {
        public Hospital_Menu()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Korhaz_Menu_Load(object sender, EventArgs e)
        {
            if (ThemeManager.CurrentTheme == "Dark")
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
        }

        //Form megnyitás panelban (középre igazítva)
        private void FormMegnyitasPanelben<T>() where T : Form, new()
        {
            Main mainForm = this.ParentForm as Main;
            if (mainForm != null)
            {
                Panel pnFo = mainForm.GetPn_Fo();

                T formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;

                ThemeManager.ApplyTheme(formInstance);
                CenterControlInPanel(pnFo, formInstance);

                pnFo.Controls.Clear();
                pnFo.Controls.Add(formInstance);
                formInstance.BringToFront();
                formInstance.Show();
            }
        }

        //Középre igazítás
        private void CenterControlInPanel(Panel panel, Control control)
        {
            if (panel != null && control != null)
            {
                int x = (panel.Width - control.Width) / 2;  // Horizontális középre igazítás
                int y = (10); // Vertikális középre igazítás (ha szükséges)
                control.Location = new Point(x, y);
            }
        }

        //Theme beállítása (stílus választó)
        private void ccToggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (ccToggleButton1.Checked)
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
        }

        //Kijelentkezés gomb (click event)
        private void Btn_Kijelentkezes_Click(object sender, EventArgs e)
        {
            // Ellenőrizzük, hogy a szülő form Main-e és aloldalként nyílt-e meg
            if (this.ParentForm is Main mainForm && mainForm.IsSubPage)
            {
                mainForm.Close();
            }
            else
            {
                // Teljes újraindítás a "-restart" paraméterrel
                System.Diagnostics.Process.Start(Application.ExecutablePath, "-restart");
                Application.Exit();
            }
        }

        //Profil megnyitása
        private void Btn_Profil_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Profil>();
        }

        //Szolgáltatások megnyitása
        private void Btn_Módosítás_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Hospital_Services>();
        }
        
        //Statisztika megnyitása
        private void Btn_Statisztika_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Hospital_Statistics>();
        }

        //Alkalmazottak megnyitása
        private void Btn_Orvos_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Hospital_Workers>();
        }
    }
}
