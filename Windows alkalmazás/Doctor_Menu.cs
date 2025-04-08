using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static CareCompass.Bejelentkezés;
using System.Net.Http;
using Newtonsoft.Json;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Doctor_Menu : Form
    {
        public Doctor_Menu()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Doctor_Menu_Load(object sender, EventArgs e)
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

                // Bring the new form to the front and show it
                formInstance.BringToFront();
                formInstance.Show();
            }
        }

        //Naptár oldal meghívása (click event)
        private async void Btn_Naptar_Click(object sender, EventArgs e)
        {
            Main parentMain = this.ParentForm as Main;
            if (parentMain != null && parentMain.IsSubPage)
            {
                MessageBox.Show("Erről az oldalról a kórház felülete nem elérhető!",
                                "Hozzáférés korlátozva",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return; // Kilépünk a metódusból
            }
            else
            {
                int user_id = GlobalData.UserId;
                string response = await OpenCalendar(user_id);

                if (!string.IsNullOrEmpty(response))
                {
                    await Task.Delay(1500);
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = response,
                        UseShellExecute = true // Windows 10+ szükséges beállítás
                    });
                }
                else
                {
                    MessageBox.Show("Nem sikerült lekérni a naptár linket.");
                }
            }
        }

        //Naptár lekérdezése 
        private async Task<string> OpenCalendar(int user_id)
        {
            var data = new { user_id };

            // A kérést elküldjük, de most várunk a válaszra manuálisan.
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);

                string jsonData = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/v1/api.php/calendar/OpenCalendar")
                {
                    Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"HIBA: {errorMessage}");
                    return null;
                }

                string jsonResult = await response.Content.ReadAsStringAsync(); // Itt tároljuk el a JSON választ
                Debug.WriteLine($"Kapott válasz: {jsonResult}");

                try
                {
                    var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResult);
                    if (responseObject != null && responseObject.ContainsKey("success") && Convert.ToBoolean(responseObject["success"]) && responseObject.ContainsKey("data"))
                    {
                        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseObject["data"].ToString());
                        if (dataDict.ContainsKey("link"))
                        {
                            return dataDict["link"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt a válasz feldolgozásakor: {ex.Message}");
                }
            }

            return null;
        }

        //Kijelentkezés
        private void Btn_Kijelentkezes_Click(object sender, EventArgs e)
        {
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

        //Doktor profil megnyitása
        private void Btn_Profil_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Profil>();
        }

        //Statisztika almenü megjelenítése
        private void Btn_Statisztika_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Statisztikai adatok megnyitása
        private void Btn_StatisztikaAdatok_Click(object sender, EventArgs e)
        {
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Theme váltás
        private void ccToggleButton1_CheckedChanged_1(object sender, EventArgs e)
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
    }
}
