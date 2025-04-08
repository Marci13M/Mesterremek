using CareCompass.Customs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;
using System.Net;
using System.Globalization;
using System.Threading;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Bejelentkezés : Form
    {
        //Bejelentkezési adatok
        public static class GlobalData
        {
            public static string RoleIdentifier { get; set; }
            public static string UserName { get; set; }
            public static string Email { get; set; }
            public static int UserId { get; set; }
            public static int HospitalId { get; set; }
            public static int CompanyId { get; set; }
            public static string Institution_name { get; set; }
        }

        //Token adatai
        public static class Token
        {
            public static string Access_token { get; set; }
            public static DateTime ExpiresAt { get; set; }
            public static int timezone_type { get; set; }
            public static string timezone { get; set; }
        }

        public Bejelentkezés()
        {
            InitializeComponent();
        }
        //Main form megynitása
        private void megnyitas()
        {
            Main mainForm = new Main();
            mainForm.role_identifier = GlobalData.RoleIdentifier;
            mainForm.Text = "Care Compass - " + GlobalData.UserName; //Care Compass + a felhasználó neve

            // Megnyitjuk a Main formot
            mainForm.Show();

            // Elrejtjük a Form1-et
            this.Hide();

            // Amikor a Main form bezárul Form1 bezárul
            mainForm.FormClosed += (s, args) => this.Close();
        }

        //Bejelentkezés gomb
        private void Btn_Bejelentkezes_Click(object sender, EventArgs e)
        {
            Bejelentkezes();
        }

        //Bejelentkezés
        private void Bejelentkezes()
        {
            using (var client = new HttpClient())
            {
                string apiUrl = $"{BaseUrl}/api/v1/api.php/auth/login";
                var loginRequest = new LoginRequest()
                {
                    email = Tb_Felhasznalonev.Texts,
                    password = Tb_Jelszo.Texts
                };

                var jsonRequest = JsonConvert.SerializeObject(loginRequest);
                Debug.WriteLine("JSON Request: " + jsonRequest);

                try
                {
                    HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // POST kérés küldése
                    var result = client.PostAsync(apiUrl, content).Result.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(result);

                    // JSON válasz feldolgozása
                    dynamic responses = JArray.Parse($"[{result}]");
                    dynamic response = responses[0];

                    bool succes = response.success;

                    // Ha sikeres válasz érkezik
                    if (succes)
                    {
                        //Bejelentkezési adatok
                        GlobalData.UserName = response.data.user_name;
                        GlobalData.RoleIdentifier = response.data.role_identifier;
                        GlobalData.UserId = response.data.user_id;
                        GlobalData.Email = response.data.user_email;
                        GlobalData.Institution_name = response.data.institution_name;

                        if (GlobalData.RoleIdentifier == "hospital")
                        {
                            GlobalData.HospitalId = response.data.hospital_id;
                            GlobalData.CompanyId = response.data.company_id;
                        }
                        else if (GlobalData.RoleIdentifier == "company")
                        {
                            GlobalData.CompanyId = response.data.company_id;
                        }

                        //Token adatok
                        Token.Access_token = response.data.token.access_token;
                        Token.ExpiresAt = response.data.token.expiresAt.date;
                        Token.timezone_type = response.data.token.expiresAt.timezone_type;
                        Token.timezone = response.data.token.expiresAt.timezone;


                        Debug.WriteLine($"Role identifier: {GlobalData.RoleIdentifier}");
                        Debug.WriteLine($"User name: {GlobalData.UserName}");
                        Debug.WriteLine($"user Id: {GlobalData.UserId}");
                        Debug.WriteLine($"token: {Token.Access_token}");
                        megnyitas();
                    }
                    else
                    {
                        // Ha sikertelen válasz érkezik
                        Debug.WriteLine($"Error Code: {response.error.code}");
                        Debug.WriteLine($"Error Message: {response.error.message}");
                        MessageBox.Show($"{response.error.message}", "Hibás bejelentkezés", MessageBoxButtons.OK);
                        Tb_Jelszo.Texts = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}");
                    Tb_Jelszo.Texts = "";
                }
            }
        }

        //Bejelentkezési adatok
        private class LoginRequest
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        // Elfelejtett jelszó
        private void Lb_ElfelejtettJelszo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"{BaseUrl}/elfelejtett-jelszo");
        }

        #region Bejelentkezés gomb hover
        private void Btn_Bejelentkezes_MouseEnter(object sender, EventArgs e)
        {

            Btn_Bejelentkezes.BackColor = Color.FromArgb(63, 204, 220);
        }
        private void Btn_Bejelentkezes_MouseLeave(object sender, EventArgs e)
        {
            Btn_Bejelentkezes.BackColor = Color.FromArgb(63, 204, 188);
        }
        #endregion
        
        //Datagridview magasságagának állítása
        public static void AdjustDataGridViewHeight(DataGridView datagridview)
        {
            // Először számítsuk ki, mennyi hely áll rendelkezésre a szülő konténerben
            int availableHeight = datagridview.Parent.Height;
            int totalRowsHeight = datagridview.ColumnHeadersHeight;

            foreach (DataGridViewRow row in datagridview.Rows)
            {
                if (row.Visible)
                {
                    totalRowsHeight += row.Height;
                }
            }

            // Ellenőrizzük, hogy szükség van-e vízszintes görgetősávra
            int totalWidth = 0;
            foreach (DataGridViewColumn col in datagridview.Columns)
            {
                if (col.Visible)
                {
                    totalWidth += col.Width;
                }
            }

            bool needsHorizontalScrollBar = totalWidth > datagridview.Width;
            bool needsVerticalScrollBar = totalRowsHeight > availableHeight;

            if (needsHorizontalScrollBar)
            {
                totalRowsHeight += SystemInformation.HorizontalScrollBarHeight;
            }

            if (needsVerticalScrollBar)
            {
                datagridview.Height = availableHeight;
            }
            else
            {
                datagridview.Height = totalRowsHeight;
            }

            // Mindkét görgetősáv engedélyezése
            datagridview.ScrollBars = ScrollBars.Both;
        }
    }
}
