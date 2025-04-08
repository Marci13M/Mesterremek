using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using System.IO;
using CareCompass.Customs;
using System.Security.Cryptography;

namespace CareCompass
{
    public partial class Profil : Form
    {
        public Profil()
        {
            InitializeComponent();
        }
        public Button jelenlegiProfilGomb;
        
        //Form betöltése (formok betöltése role-tól függöen)
        private void Ceg_Profil_Load(object sender, EventArgs e)
        {
            Debug.WriteLine(GlobalData.RoleIdentifier);

            Main parentMain = this.ParentForm as Main;
            if (parentMain != null && parentMain.IsSubPage)
            {

                if (GlobalData.RoleIdentifier == "company")
                {
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Company_Profil>();

                    Btn_CegProfil.Text = $"{GlobalData.Institution_name} profilja";
                    Btn_CegModerator.Text = $"";
                    GombokBeállítása(Btn_CegProfil);
                    Btn_CegModerator.Enabled = false;
                }
                else if (GlobalData.RoleIdentifier == "hospital")
                {
                    Btn_CegProfil.Text = $"{GlobalData.Institution_name} profilja";
                    Btn_CegModerator.Text = $"";
                    Btn_CegModerator.Enabled = false;
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Hospital_Profil>();
                }
                else if (GlobalData.RoleIdentifier == "admin")
                {
                    FormMegnyitasPanelben<Admin_Users>();
                    GombokBeállítása(Btn_CegProfil);
                    Btn_CegProfil.Text = $"Felhasználók létrehozása és törlése";
                    Btn_CegModerator.Text = $"{GlobalData.UserName} profilja";
                }
                else if (GlobalData.RoleIdentifier == "doctor")
                {
                    Btn_CegProfil.Text = $"{GlobalData.UserName} profilja";
                    Btn_CegModerator.Text = $"";
                    Btn_CegModerator.Enabled = false;
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Doctor_Profil>();
                }
            }
            else
            {
                if (GlobalData.RoleIdentifier == "company")
                {
                    Btn_CegProfil.Text = $"{GlobalData.Institution_name} profilja";
                    Btn_CegModerator.Text = $"{GlobalData.UserName} módosítások";
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Company_Profil>();
                }
                else if (GlobalData.RoleIdentifier == "hospital")
                {
                    Btn_CegProfil.Text = $"{GlobalData.Institution_name} profilja";
                    Btn_CegModerator.Text = $"{GlobalData.UserName} módosítások";
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Hospital_Profil>();
                }
                else if (GlobalData.RoleIdentifier == "admin")
                {
                    FormMegnyitasPanelben<Admin_Users>();
                    GombokBeállítása(Btn_CegProfil);
                    Btn_CegProfil.Text = $"Felhasználók létrehozása és törlése";
                    Btn_CegModerator.Text = $"{GlobalData.UserName} profilja";
                }
                else if (GlobalData.RoleIdentifier == "doctor")
                {
                    Btn_CegProfil.Text = $"{GlobalData.UserName} profilja";
                    Btn_CegModerator.Text = $"{GlobalData.UserName} módosítások";
                    GombokBeállítása(Btn_CegProfil);
                    FormMegnyitasPanelbenKozepreIgazitva<Doctor_Profil>();
                }
            }
            

        }
       
        //Form megnyitása
        private void FormMegnyitasPanelben<T>() where T : Form, new()
        {
            Main mainForm = this.ParentForm as Main;
            if (mainForm != null)
            {
                T formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;

                ThemeManager.ApplyTheme(formInstance);

                Pn_Profil.Controls.Clear();
                Pn_Profil.Controls.Add(formInstance);

                formInstance.BringToFront();
                formInstance.Show();
            }
        }

        //Form megnyitása középre igazítva
        private void FormMegnyitasPanelbenKozepreIgazitva<T>() where T : Form, new()
        {
            Main mainForm = this.ParentForm as Main;
            if (mainForm != null)
            {
                T formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;

                ThemeManager.ApplyTheme(formInstance);

                Pn_Profil.Controls.Clear();
                Pn_Profil.Controls.Add(formInstance);

                // **Középre igazítás**
                CenterControlInPanel(Pn_Profil, formInstance);
                Pn_Profil.SizeChanged += (s, ev) => CenterControlInPanel(Pn_Profil, formInstance);

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
                // Először biztosítjuk, hogy a control mérete nem nagyobb, mint a panel
                control.Width = Math.Min(panel.Width, control.PreferredSize.Width);
                control.Height = Math.Min(panel.Height, control.PreferredSize.Height);

                // Középre igazítás a panelen belül
                int x = (panel.Width - control.Width) / 2;  // Vízszintesen középre
                int y = (0); // Függőlegesen középre

                // Új pozíció beállítása
                control.Location = new Point(x, y);
            }
        }

        #region Gombok beállítása
        //Aktív gombok beállítása (lényegében melyik menüben/időszakban vagyunk)
        public void GombokBeállítása(object button)
        {
            var btn = (Button)button;

            // Alap háttérszín visszaállítása az előző gombra
            if (jelenlegiProfilGomb != null)
            {
                jelenlegiProfilGomb.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.Transparent : Color.Transparent;
                jelenlegiProfilGomb.ForeColor = ThemeManager.CurrentTheme == "Light" ? Color.Black : Color.White;
            }
            else
            {
                btn.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                btn.ForeColor = ThemeManager.CurrentTheme == "Light" ? Color.Black : Color.White;
            }
            // Frissítjük az aktuális gomb referenciát
            jelenlegiProfilGomb = btn;
        }

        //Gombok átszínezése ha az egér rámegy
        private void GombHoverOn(object button)
        {
            var btn = (CCButton)button;

            // Ha ez az aktuális gomb, csak enyhe változás legyen
            if (btn == jelenlegiProfilGomb)
            {   
                btn.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
            }
            else
            {
                btn.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
            }
            btn.ForeColor = ThemeManager.CurrentTheme == "Light" ? Color.Black : Color.White;
        }

        //Gombok átszínezése ha az egér lejön a gombról
        private void GombHoverOff(object button)
        {
            var btn = (CCButton)button;

            // Ha ez az aktuális gomb, állítsuk vissza az aktív színt
            if (btn == jelenlegiProfilGomb)
            {
                btn.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                btn.ForeColor = ThemeManager.CurrentTheme == "Light" ? Color.Black : Color.White;
            }
            else
            {
                btn.BackColor = ThemeManager.CurrentTheme == "Light" ? Color.Transparent : Color.Transparent;
                btn.ForeColor = ThemeManager.CurrentTheme == "Light" ? Color.Black : Color.White;
            }
        }

        //Profil gomb (click eventje) felhasználótól függően nyílik meg különboző form
        private void ccButton1_Click(object sender, EventArgs e)
        {
            if (GlobalData.RoleIdentifier == "company")
            {
                FormMegnyitasPanelbenKozepreIgazitva<Company_Profil>();
            }
            else if (GlobalData.RoleIdentifier == "hospital")
            {
                //FormMegnyitasPanelben<Korhaz_Profil>();
                FormMegnyitasPanelbenKozepreIgazitva<Hospital_Profil>();
            }
            else if (GlobalData.RoleIdentifier == "admin")
            {
                FormMegnyitasPanelben<Admin_Users>();
            }
            else if (GlobalData.RoleIdentifier == "doctor")
            {
                FormMegnyitasPanelbenKozepreIgazitva<Doctor_Profil>();
            }
            GombokBeállítása(sender);
        }

        //Jelszó módosító felület megnyitása (click event)
        private void Btn_CegModerator_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            if (mainForm.IsSubPage == true)
            {
                MessageBox.Show("Nincs hozzáférése");
            }
            else
            {
                FormMegnyitasPanelbenKozepreIgazitva<Moderator_Profile>();
                GombokBeállítása(sender);
            }
        }

        //Profil hover on 
        private void Btn_CegProfil_MouseEnter(object sender, EventArgs e)
        {
            GombHoverOn(sender);
        }

        //Profil hover off 
        private void Btn_CegProfil_MouseLeave(object sender, EventArgs e)
        {
            GombHoverOff(sender);
        }

        //Jelszó változtatás hover on 
        private void Btn_CegModerator_MouseEnter(object sender, EventArgs e)
        {
            GombHoverOn(sender);
        }

        //Jelszó változtatás hover off
        private void Btn_CegModerator_MouseLeave(object sender, EventArgs e)
        {
            GombHoverOff(sender);
        }
        #endregion
    }
}
