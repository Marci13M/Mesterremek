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
    public partial class Admin_Menu : Form
    {
        public Admin_Menu()
        {
            InitializeComponent();
        }
        public Button jelenlegiMenü;

        //Menü betöltése
        private void Admin_Menu_Load(object sender, EventArgs e)
        {
            //Theme beállítása
            if (ThemeManager.CurrentTheme == "Dark")
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
        }

        //Form meghívása a pnfo panelba
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

                pnFo.Controls.Clear();
                pnFo.Controls.Add(formInstance);
                formInstance.BringToFront();
                formInstance.Show();
            }
        }

        //Form meghívása a pnfo panelba középre igazítva
        private void FormMegnyitasPanelbenKozepreIgazitva<T>() where T : Form, new()
        {
            Main mainForm = this.ParentForm as Main;
            if (mainForm != null)
            {
                Panel pnFo = mainForm.GetPn_Fo();
                T formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;

                ThemeManager.ApplyTheme(formInstance);

                pnFo.Controls.Clear();
                pnFo.Controls.Add(formInstance);

                // **Középre igazítás**
                CenterControlInPanel(pnFo, formInstance);
                pnFo.SizeChanged += (s, ev) => CenterControlInPanel(pnFo, formInstance);

                formInstance.BringToFront();
                formInstance.Show();
                formInstance.Refresh();
            }
        }

        //Középre igazítás
        private void CenterControlInPanel(Panel panel, Control control)
        {
            if (panel != null && control != null)
            {
                int x = (panel.Width - control.Width) / 2;  // Horizontális középre igazítás
                int y = (0); // Vertikális középre igazítás (ha szükséges)
                control.Location = new Point(x, y);
            }
        }

        //Kijelentkezés gomb kattintás
        private void Btn_Kijelentkezes_Click(object sender, EventArgs e)
        {
            // Teljes újraindítás a "-restart" paraméterrel
            System.Diagnostics.Process.Start(Application.ExecutablePath, "-restart");
            Application.Exit();
        }
       
        //Kórház oldal
        private void Btn_Korhaz_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Admin_Hospital>();
        }

        //Cég oldal
        private void Btn_Ceg_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Admin_Company>();
        }

        //Theme megváltoztatás (event)
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

        //Statisztika oldal
        private void Btn_Statisztika_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Admin_Statistics>();
        }

        //Szolgáltatások oldal
        private void Btn_Szolgaltatasok_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Admin_Services>();
        }

        //Felhasználók oldal
        private void Btn_Felhasznalok_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Admin_Web_Users>();
        }

        //Egyéb adatok oldal
        private void Btn_Add_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelbenKozepreIgazitva<Admin_Add>();
        }

        private void Btn_Felhasznalok_Click_1(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Profil>();
        }
    }
}
