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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static CareCompass.Bejelentkezés;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Admin_Add : Form
    {
        public Admin_Add()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Language = new List<dynamic>();
        private List<dynamic> Education = new List<dynamic>();

        //Form betöltés
        private void Admin_Add_Load(object sender, EventArgs e)
        {
            //FőPanel méretének változtatása
            Pn_MainAdd.Location = new Point(20, 50);
            Pn_MainAdd.Size = new Size(this.Width - 2 * 20, this.Height - 70);
            GetLanguage();
            GetEducation();
            dGV_Education.CellContentClick += DataGridView_CellContentClick;
            dGV_Language.CellContentClick += DataGridView_CellContentClick;
        }

        //Form méretének változtatása
        private void Admin_Add_SizeChanged(object sender, EventArgs e)
        {
            //FőPanel méretének változtatása
            Pn_MainAdd.Location = new Point(20, 50);
            Pn_MainAdd.Size = new Size(this.Width - 2 * 20, this.Height - 70);
        }

        //DataGridView-ek feltöltése
        #region DataGridView-ek feltöltése
        //Nyelvek feltöltése
        private async void GetLanguage()
        {
            dGV_Language.Rows.Clear();
            var data =await FetchData("/api/v1/api.php/data/general/Languages");
            if (data == null || data.languages == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Language = data.languages.ToObject<List<dynamic>>();
            foreach (var language in Language)
            {
                int rowIndex = dGV_Language.Rows.Add(language.name.ToString());
                dGV_Language.Rows[rowIndex].Cells[dGV_Language.Columns.Count - 1].Value = "Törlés";
            }
            
        }
        //Oktatások feltöltése
        private async void GetEducation()
        {
            dGV_Education.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/data/general/Educations");
            if (data == null || data.educations == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Education = data.educations.ToObject<List<dynamic>>();
            foreach (var education in Education)
            {
                int rowIndex = dGV_Education.Rows.Add(education.name.ToString());
                dGV_Education.Rows[rowIndex].Cells[dGV_Education.Columns.Count - 1].Value = "Törlés";
            }
        }
        #endregion

        //DataGridView-ek kattintás eseményei
        private async void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ellenőrizzük, hogy nem fejlécet vagy érvénytelen cellát kattintottak
            {
                var dgv = sender as DataGridView;
                if (dgv == null) return;

                // Az oszlop neve, ahol a kattintás történt
                string columnName = dgv.Columns[e.ColumnIndex].Name;

                // Oszlopnév alapján külön logikák
                switch (columnName)
                {
                    case "LTorles":
                        var language = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {language} nyelvet?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int language_id = Convert.ToInt32(Language[e.RowIndex].id);
                            // Call the separate function for deleting the language
                            await DeleteLanguage(language_id);

                            MessageBox.Show("Nyelv sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetLanguage();
                        }
                        break;
                    case "ETorles":
                        var education = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteEducation = MessageBox.Show($"Biztosan törlöd a(z) {education} oktatást?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteEducation == DialogResult.Yes)
                        {
                            int education_id = Convert.ToInt32(Education[e.RowIndex].id); // Get language_id

                            // Call the separate function for deleting the language
                            await DeleteEducation(education_id);

                            MessageBox.Show("Oktatás sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetEducation();
                        }
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Nyelv törlése
        private async Task DeleteLanguage(int language_id)
        {
            var data = new { language_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/data/general/DeleteLanguage", data);
        }
        //Végzettség törlése
        private async Task DeleteEducation(int education_id)
        {
            var data = new { education_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/data/general/DeleteEducation", data);
        }


        //Nyelv hozzáadása esemény (gomb kattintás)
        private async void Btn_NyelvHozzaadas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_Language.Text.ToString()))
            {
                string language=Tb_Language.Text;
                await AddLanguage(language);
                GetLanguage();
                Tb_Language.Clear();
            }
            else
            {
                MessageBox.Show("Kérem töltse ki a mezőt!");
            }
        }
        //Nyelv hozzáadása
        private async Task AddLanguage(string language)
        {
            var data = new { language };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/data/general/AddLanguage", data);
        }

        //Oktatás hozzáadása esemény (gomb kattintás)
        private async void Btn_OktatasHozzaadas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_Oktatas.Text.ToString()))
            {
                string education=Tb_Oktatas.Text;
                await AddEducation(education);
                GetEducation();
                Tb_Oktatas.Clear();
            }
            else
            {
                MessageBox.Show("Kérem töltse ki a mezőt!");
            }
        }
        //Oktatás hozzáadása
        private async Task AddEducation(string education)
        {
            var data = new { education };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/data/general/AddEducation", data);
        }
    }
}
