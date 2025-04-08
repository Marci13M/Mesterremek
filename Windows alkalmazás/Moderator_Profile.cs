using CareCompass.Customs;
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
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Moderator_Profile : Form
    {
        public Moderator_Profile()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Moderator_Profile_Load(object sender, EventArgs e)
        {
            hide();
            this.Refresh();
            Application.DoEvents();
            Tb_Email.Texts = GlobalData.Email;
        }

        //Új jelszóhoz szükséges labelek, textboxok és gomb megjelenítése
        private void show()
        {
            Lb_RegiJelszo.Visible = true;
            Tb_RegiJelszo.Visible = true;
            Tb_UjJelszo.Visible = true;
            Tb_UjJelszoMegerosites.Visible = true;
            Btn_JelszoModositasa.Visible = true;
            Lb_Uj_jelszo.Visible=true;
            Lb_JelszoMegerositese.Visible=true;

            Tb_RegiJelszo.Texts = "";
            Tb_UjJelszo.Texts = "";
            Tb_UjJelszoMegerosites.Texts = "";
            // Form frissítése
            this.Invalidate();
            this.Update();
            this.Refresh();

            // Kényszerített újrarajzolás
            Application.DoEvents();
        }

        //Új jelszóhoz szükséges labelek, textboxok és gomb eltüntetése
        private void hide()
        {
            Lb_RegiJelszo.Visible=false;
            Tb_RegiJelszo.Visible = false;
            Lb_JelszoMegerositese.Visible = false;
            Lb_Uj_jelszo.Visible=false;
            Tb_UjJelszo.Visible= false;
            Tb_UjJelszoMegerosites.Visible= false;
            Btn_JelszoModositasa.Visible= false;
            this.Refresh();
        }

        //Jelszó módosítása (click event)
        private async void Btn_AlkalmazottHozzaadas_Click(object sender, EventArgs e)
        {
            CCTextBox[] cCTextBoxes = { Tb_RegiJelszo, Tb_UjJelszo, Tb_UjJelszoMegerosites};
            if (cCTextBoxes.All(tb => !string.IsNullOrWhiteSpace(tb.Texts)))
            {
                if (Tb_UjJelszo.Texts == Tb_UjJelszoMegerosites.Texts)
                {
                    string password = Tb_UjJelszo.Texts.ToString();
                    if (ValidatePassword(password))
                    {
                        int user_id = GlobalData.UserId;
                        string old_password = Tb_RegiJelszo.Texts;
                        string new_password=Tb_UjJelszo.Texts;
                        string new_password_again = Tb_UjJelszoMegerosites.Texts;
                        await UpdatePassword(user_id, old_password, new_password, new_password_again);
                        hide();
                    }

                }
                else
                {
                    MessageBox.Show("A két jelszó nem egyezik meg");
                    Tb_UjJelszoMegerosites.Texts = "";
                    //Ide jön majd 
                    MessageBox.Show("Siker");
                }
            }
            else
            {
                MessageBox.Show("Nincs kitöltve az összes mező");
            }
            
        }

        //Jelszó módosítása
        private async Task UpdatePassword(int user_id, string old_password, string new_password, string new_password_again)
        {
            var data = new { user_id, old_password, new_password, new_password_again };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/users/UpdatePassword", data);
        }

        //Jelszó ellenőrzése (megfelelő legyen biztonsági szempontból)
        private bool ValidatePassword(string password)
        {
            // Ellenőrzés, hogy a jelszó hossza legalább 8 karakter
            if (password.Length < 8)
            {
                MessageBox.Show("A jelszónak legalább 8 karakter hosszúnak kell lennie!");
                return false;
            }
            // Ellenőrzés, hogy tartalmaz-e nagybetűt
            else if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell nagybetűt!");
                return false;
            }
            // Ellenőrzés, hogy tartalmaz-e kisbetűt
            else if (!Regex.IsMatch(password, @"[a-z]"))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell kisbetűt!");
                return false;
            }
            // Ellenőrzés, hogy tartalmaz-e számot
            else if (!Regex.IsMatch(password, @"[0-9]"))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell számot!");
                return false;
            }
            // Ellenőrzés, hogy tartalmaz-e különleges karaktert
            else if (!Regex.IsMatch(password, @"[^A-Za-z0-9]"))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell különleges karaktert!");
                return false;
            }

            // Ha minden feltétel teljesül
            return true;
        }

        //Új jelszó label kattintás (event)
        private void Lb_UjJelszo_Click(object sender, EventArgs e)
        {
            show();
        }
    }
}
