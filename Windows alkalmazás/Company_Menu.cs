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
    public partial class Company_Menu : Form
    {
        public Company_Menu()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Ceg_Menu_Load(object sender, EventArgs e)
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

        //Form megnyitása metódus
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
        
        //Theme váltás (event)
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
         
        //Kijelentkezés (click event)
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
        
        //Cég profil megjelenítese
        private void Btn_Profil_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Profil>();
        }

        //Kórház form meghívása
        private void Btn_Korhaz_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Company_Hospitals>();
        }

        //Szolgáltatások form meghívása
        private void Btn_Szolgaltatasok_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Company_Services>();
        }

        //Statisztika meghívása
        private void Btn_Statisztika_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Company_Statistics>();
        }

        //Alkalmazottak form megnyitása
        private void Btn_Alkalmazottak_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Company_Workers>();
        }
    }
}
