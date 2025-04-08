using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
    public partial class Hospital_Workers : Form
    {
        public Hospital_Workers()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Doctors = new List<dynamic>();
        private List<dynamic> CompanyDoctors = new List<dynamic>();
        private List<dynamic> Moderators = new List<dynamic>();

        //Form betöltése
        private void Korhaz_Doctor_Load(object sender, EventArgs e)
        {
            MeretValtas();
            GetModerators();
            GetDoctors();
            GetCompanyDoctors();
            dGV_OrvosAdatok.CellContentClick += DataGridView_CellContentClick;
            dGV_Moderatorok.CellContentClick += DataGridView_CellContentClick;
            dGV_OrvosAdatok.DataBindingComplete += dGV_OrvosAdatok_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezés
        private void dGV_OrvosAdatok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_OrvosAdatok);
        }

        //Form méretének változása (event)
        private void Korhaz_Doctor_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_OrvosAdatok.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_OrvosAdatok);
            }
        }

        //Méret váltás
        private void MeretValtas()
        {
            //Főcim helyzetének változtatása
            Lb_KorhazAlkalmazottak.Location = new Point(20, 10);

            //FőPanel méretének változtatása
            Pn_KorhazAlkalmazottak.Location = new Point(20, 50);
            Pn_KorhazAlkalmazottak.Size = new Size(this.Width - 2 * 20, this.Height - 70);

            //Jobb oldali panel középre igazítása
            Pn_UjKorhaz.Location = new Point((Pn_Jobb.Width - Pn_UjKorhaz.Width) / 2 + 10, 0);
            Pn_UjKorhaz.Size = new Size(280, Pn_Jobb.Height);

            Btn_OrvosHozzaadas.Location = new Point(CB_OrvosNev.Location.X + (CB_OrvosNev.Width - Btn_OrvosHozzaadas.Width), CB_OrvosNev.Location.Y + 40);
        }

        //Orvosok lekérdezése
        private async void GetDoctors()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_OrvosAdatok.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/hospitals/GetDoctors?hospital_id={GlobalData.HospitalId}");
            if (data == null || data.hospital_doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Doctors = data.hospital_doctors.ToObject<List<dynamic>>();
            foreach (var doctor in Doctors)
            {
                // Profilkép átalakítása
                Image profileImage = null;
                var doctorImageBase64 = doctor.image.ToString();
                if (!string.IsNullOrEmpty(doctorImageBase64))
                {
                    if (doctorImageBase64.Contains(","))
                    {
                        // Eltávolítjuk az előtagot, ha van
                        doctorImageBase64 = doctorImageBase64.Split(',')[1];
                    }
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(doctorImageBase64);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            profileImage = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Hiba történt a kép átalakításakor: {ex.Message}");
                    }
                }
                else
                {
                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    // Kép teljes elérési útja a projekt mappájában
                    Debug.WriteLine("Nincs kép megadva, helyettesítő kép használata.");
                    string replacementImagePath = Path.Combine(projectDirectory, "Images", "Replacement", "doctor_placeholder.png");

                    if (File.Exists(replacementImagePath))
                    {
                        profileImage = Image.FromFile(replacementImagePath);
                    }
                    else
                    {
                        Debug.WriteLine("A helyettesítő kép nem található: " + replacementImagePath);
                        profileImage = null;
                    }
                }
                int rowIndex = dGV_OrvosAdatok.Rows.Add(profileImage, doctor.name.ToString(), doctor.email.ToString());
                dGV_OrvosAdatok.Rows[rowIndex].Cells[dGV_OrvosAdatok.Columns.Count - 1].Value = "Törlés";
            }
            AdjustDataGridViewHeight(dGV_OrvosAdatok);
        }

        //Moderátorok lekérdezése
        private async void GetModerators()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_Moderatorok.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/hospitals/GetModerators?hospital_id={GlobalData.HospitalId}");
            if (data == null || data.hospital_moderators == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Moderators = data.hospital_moderators.ToObject<List<dynamic>>();
            foreach (var moderator in Moderators)
            {
                int rowIndex = dGV_Moderatorok.Rows.Add(moderator.user_name.ToString(), moderator.user_email.ToString());
            }
            
        }

        //Cég orvosainak lekérdezése (akik nincsenek a kórházhoz rendelve még)
        private async void GetCompanyDoctors()
        {
            CB_OrvosNev.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/hospital/doctors?hospital_id={GlobalData.HospitalId}&company_id={GlobalData.CompanyId}");
            if (data == null || data.doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            CompanyDoctors = data.doctors.ToObject<List<dynamic>>();
            foreach (var doctor in CompanyDoctors)
            {

                CB_OrvosNev.Items.Add(doctor.name);

            }
            #endregion
            
        }


        //DataGridView kattintások
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
                    case "torles":
                        // Orvos törlése
                        var doctor = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                        DialogResult confirmDeleteEducation = MessageBox.Show($"Biztosan törlöd a(z) {doctor} orvost?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteEducation == DialogResult.Yes)
                        {
                            int doctor_id = Doctors[e.RowIndex].doctor_id; // Az ID lekérése a listából
                            int hospital_id = GlobalData.HospitalId;
                            await DeleteDoctor(doctor_id, hospital_id);
                            GetDoctors();
                            GetCompanyDoctors();
                        }
                        break;

                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }
        
        //Orvos törlése
        private async Task DeleteDoctor(int doctor_id, int hospital_id)
        {
            var data = new { doctor_id, hospital_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/RemoveDoctor", data);
        }

        //Orvos hozzáadása a kórházhoz (click event)
        private async void Btn_OrvosHozzaadas_Click(object sender, EventArgs e)
        {
            if (CB_OrvosNev.SelectedIndex != -1)
            {
                string selectedDoctorName = CB_OrvosNev.SelectedItem.ToString();

                foreach (var doctor in CompanyDoctors)
                {
                    if (doctor.name.ToString() == selectedDoctorName)
                    {
                        int doctor_id = Convert.ToInt32(doctor.id);
                        int hospital_id = GlobalData.HospitalId;

                        // Api meghívása
                        await AddDoctorToCompany(doctor_id, hospital_id);
                        GetDoctors();
                        GetCompanyDoctors();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérlek válassz oktatást a listából!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Orvos kórházhoz adása
        private async Task AddDoctorToCompany(int doctor_id, int hospital_id)
        {
            var data = new { doctor_id, hospital_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/AddDoctor", data);
        }
    }
}
