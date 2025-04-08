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
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Company_Workers : Form
    {
        public Company_Workers()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Doctors = new List<dynamic>();
        private List<dynamic> Moderators = new List<dynamic>();

        private List<dynamic> Roles = new List<dynamic>();
        private List<dynamic> Hospitals = new List<dynamic>();
        private List<dynamic> NotCompanyDoctors = new List<dynamic>();

        private int RoleID;

        //Form betöltése
        private void Ceg_Orvos_Load(object sender, EventArgs e)
        {
            Lb_AlkalmazottKorhaz.Visible = false;
            CB_AlkalmazottKorhaz.Visible = false;
            Lb_KorhazEmail.Visible = false;
            Tb_AlkalmazottEmail.Visible = false;

            MeretValtas();
            GetDoctors();
            GetModerators();
            GetRoles();
            dGV_Orvosok.CellContentClick += DataGridView_CellContentClick;
            dGV_Moderatorok.CellContentClick += DataGridView_CellContentClick;
        }

        //DataGridView adatainak feltöltés
        private void dGV_Moderatorok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Moderatorok);
        }

        //Form méretének változása (event)
        private void Ceg_Orvos_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_Moderatorok.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Moderatorok);
            }
        }

        //Méret váltás
        private void MeretValtas()
        {
            //Főcim helyzetének változtatása
            Lb_CegAlkalmazottak.Location = new Point(20, 10);

            //FőPanel méretének változtatása
            Pn_CegAlkalmazottak.Location = new Point(20, 50);
            Pn_CegAlkalmazottak.Size = new Size(this.Width - 2 * 20, this.Height - 70);

            Pn_UjAlkalmazott.Size = new Size(300, this.Height);


            Btn_AlkalmazottHozzaadas.Location = new Point(CB_AlkalmazottKorhaz.Location.X + (CB_AlkalmazottKorhaz.Width - Btn_AlkalmazottHozzaadas.Width), CB_AlkalmazottKorhaz.Location.Y + 40);

        }

        //Munkaterület kiválasztás (ez alapján jeleníti meg a további inputokat)
        private void CB_AlkalmazottMunkaterulet_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ellenőrizzük, hogy van-e érvényes kiválasztás
            if (CB_AlkalmazottMunkaterulet.SelectedIndex >= 0 && CB_AlkalmazottMunkaterulet.Text == "Kórház")
            {
                // Megkeressük a "Kórház" nevű szerep azonosítóját a Roles listában
                var hospitalRole = Roles.FirstOrDefault(role => role.name.ToString() == "Kórház");

                if (hospitalRole != null)
                {
                    RoleID = hospitalRole.id; // RoleID értékének beállítása
                    Debug.WriteLine($"Kiválasztott ID: {RoleID}");
                }
                else
                {
                    Debug.WriteLine("A 'Kórház' szerep nem található a Roles listában.");
                }
                Lb_AlkalmazottKorhaz.Text = "Kórházak";
                Lb_AlkalmazottKorhaz.Visible = true;
                CB_AlkalmazottKorhaz.Visible = true;
                Tb_AlkalmazottEmail.Visible = true;
                Lb_KorhazEmail.Visible = true;
                GetHospital();
            }
            else if(CB_AlkalmazottMunkaterulet.SelectedItem.ToString()== "Már regisztrált orvos")
            {
                Lb_AlkalmazottKorhaz.Text = "Orvosok";
                Tb_AlkalmazottEmail.Visible = false;
                Lb_KorhazEmail.Visible  = false;
                Lb_AlkalmazottKorhaz.Visible = true;
                CB_AlkalmazottKorhaz.Visible = true;
                GetNotCompanyDoctors();
            }
            else
            {
                // Ha valamiért nincs érvényes kiválasztás
                Debug.WriteLine("Nincs érvényes kiválasztott szolgáltatás.");
                Tb_AlkalmazottEmail.Visible= true;
                Lb_KorhazEmail.Visible= true;
                Lb_AlkalmazottKorhaz.Visible = false;
                CB_AlkalmazottKorhaz.Visible = false;
            }
        }

        #region DataGridView-ek
        //Orvosok lekérdezése
        private async void GetDoctors()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_Orvosok.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/companies/GetDoctors?company_id={GlobalData.CompanyId}");
            if (data == null || data.company_doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            Doctors = data.company_doctors.ToObject<List<dynamic>>();
            foreach (var doctors in Doctors)
            {
                int rowIndex = dGV_Orvosok.Rows.Add(doctors.doctor_name.ToString(), doctors.doctor_email.ToString());
                dGV_Orvosok.Rows[rowIndex].Cells[dGV_Orvosok.Columns.Count - 1].Value = "Törlés";

            }
            #endregion
        }

        //Moderátorok lekérdezése
        private async void GetModerators()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_Moderatorok.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/companies/GetModerators?company_id={GlobalData.CompanyId}");
            if (data == null || data.moderators == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            Moderators = data.moderators.ToObject<List<dynamic>>();
            foreach (var moderator in Moderators)
            {
                int rowIndex = dGV_Moderatorok.Rows.Add(moderator.user_name.ToString(), moderator.user_email.ToString(), moderator.role_name, moderator.institution_name);
                var deleteButton = dGV_Moderatorok.Rows[rowIndex].Cells[dGV_Moderatorok.Columns.Count - 1];
                if (moderator.role_name.ToString() != "Cég")
                {
                    deleteButton.Value = "Törlés";
                }
                else
                {
                    deleteButton.Value = "";
                    deleteButton.Style.BackColor = Color.Gray;
                    deleteButton.Tag = "Disabled";
                }
            }
            #endregion
            AdjustDataGridViewHeight(dGV_Moderatorok);
        }
        #endregion

        #region ComboBox-ok feltöltése
        //Jogosultságok meghívása
        private async void GetRoles()
        {
            CB_AlkalmazottMunkaterulet.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/company/roles");
            if (data == null || data.roles == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            Roles = data.roles.ToObject<List<dynamic>>();
            foreach (var role in Roles)
            {
                CB_AlkalmazottMunkaterulet.Items.Add(role.name.ToString());
            }
            CB_AlkalmazottMunkaterulet.Items.Add("Már regisztrált orvos");
            #endregion
        }

        //Kórházak lekérdezése
        private async void GetHospital() 
        {
            CB_AlkalmazottKorhaz.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/company/hospitals?company_id={GlobalData.CompanyId}");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            Hospitals = data.hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in Hospitals)
            {
                CB_AlkalmazottKorhaz.Items.Add(hospital.name.ToString());
            }
            #endregion
            
        }

        //A még a cégnél nem lévő orvosok lekérdezése
        private async void GetNotCompanyDoctors()
        {
            CB_AlkalmazottKorhaz.Items.Clear ();
            var data = await FetchData($"/api/v1/api.php/data/company/doctors?company_id={GlobalData.CompanyId}");
            if (data == null || data.doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            NotCompanyDoctors = data.doctors.ToObject<List<dynamic>>();
            foreach (var doctors in NotCompanyDoctors)
            {
                CB_AlkalmazottKorhaz.Items.Add(doctors.name.ToString());
            }
        }

        #endregion

        
        //DataGridView kattintások
        private async void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ellenőrizzük, hogy nem fejlécet vagy érvénytelen cellát kattintottak
            {
                var dgv = sender as DataGridView;
                if (dgv == null) return;

                // Az oszlop neve, ahol a kattintás történt
                string columnName = dgv.Columns[e.ColumnIndex].Name;

                // Ellenőrizzük a gomb letiltott állapotát
                var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Tag != null && cell.Tag.ToString() == "Disabled")
                {
                    // Ha a gomb le van tiltva, akkor nem történik semmi
                 
                    return;
                }
                // Oszlopnév alapján külön logikák
                switch (columnName)
                {
                    case "torles":
                        var doctor = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteDoctor = MessageBox.Show($"Biztosan törlöd a(z) {doctor} nevű orvost?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteDoctor == DialogResult.Yes)
                        {
                            int doctor_id = Doctors[e.RowIndex].doctor_id;
                            int company_id = GlobalData.CompanyId;
                            await DeleteDoctor(doctor_id, company_id);
                            GetDoctors();
                        }
                        break;

                    case "MTorles":
                        var moderator = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {moderator} moderátort?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int moderator_id = Moderators[e.RowIndex].user_id; 
                            int hospital_id = Moderators[e.RowIndex].institution_id;
                            await DeleteHospitalModerator(hospital_id, moderator_id);
                            GetModerators();
                        }
                        break;
                    
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        #region Törlések
        //Orvos törlése 
        private async Task DeleteDoctor(int doctor_id, int company_id)
        {
            var data = new { doctor_id, company_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/companies/RemoveDoctor", data);
        }
        //Moderátor törlése
        private async Task DeleteHospitalModerator(int hospital_id, int moderator_id)
        {
            var data = new { hospital_id, moderator_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/RemoveModerator ", data);
        }
        #endregion

        //Alkalmazott hozzáadaása/regisztrálása click event
        private async void Btn_AlkalmazottHozzaadas_Click(object sender, EventArgs e)
        {
            if(CB_AlkalmazottMunkaterulet.SelectedItem.ToString()!= "Már regisztrált orvos")
            {
                //Tobábbi szűrés/input check
                if (IsGoodEmail(Tb_AlkalmazottEmail.Text.ToString()))
                {
                    string email = Tb_AlkalmazottEmail.Text;
                    int company_id = GlobalData.CompanyId;
                    if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Cég")
                    {

                        await AddCompanyModerator(company_id, email);
                        GetModerators();
                        GetRoles();
                        Tb_AlkalmazottEmail.Visible=false;
                        Lb_KorhazEmail.Visible=false;   
                        Tb_AlkalmazottEmail.Clear();
                    }
                    else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Kórház")
                    {

                        string selectedHospitalName = CB_AlkalmazottKorhaz.SelectedItem.ToString();
                        foreach (var hospital in Hospitals)
                        {
                            if (hospital.name.ToString() == selectedHospitalName)
                            {
                                int hospital_id = Convert.ToInt32(hospital.id);
                                // Api meghívása
                                await AddHospitalModerator(hospital_id, email);
                                GetModerators();
                                Lb_KorhazEmail.Visible=false;
                                Tb_AlkalmazottEmail.Visible = false;
                                Lb_AlkalmazottKorhaz.Visible=false;
                                CB_AlkalmazottKorhaz.Visible = false;
                                GetRoles();
                                Tb_AlkalmazottEmail.Clear();
                            }
                        }
                    }
                    else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Doktor")
                    {
                        //itt még szűrni kell hogy melyik orvos lekérdezést kell csinálni (form-ban kell okoskodni)
                        await RegisterDoctorToCompany(company_id, email);
                        GetDoctors();
                        GetRoles();
                        Lb_KorhazEmail.Visible = false;
                        Tb_AlkalmazottEmail.Visible=false;
                        Tb_AlkalmazottEmail.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Nem megfelelő email");
                } 
            }
            else if(CB_AlkalmazottMunkaterulet.SelectedItem.ToString()== "Már regisztrált orvos")
            {
                //Már létező orvos céghez adása
                int company_id = GlobalData.CompanyId;
                string selectedDoctorName = CB_AlkalmazottKorhaz.SelectedItem.ToString();
                foreach (var doctor in NotCompanyDoctors)
                {
                    if (doctor.name.ToString() == selectedDoctorName)
                    {
                        int doctor_id = Convert.ToInt32(doctor.id);
                        await AddDoctorToCompany(company_id, doctor_id);
                        Lb_AlkalmazottKorhaz.Visible = false;
                        CB_AlkalmazottKorhaz.Visible = false;
                        GetDoctors();
                        GetRoles();
                    }
                }
            }
            else
            {
                MessageBox.Show("Válasszon ki egy szakterületet");
            }
        }

        //Cég moderátor regisztrálása
        private async Task AddCompanyModerator( int company_id, string email)
        {
            var data = new { company_id, email };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/AddModerator", data);
        }

        //Kórház moderátor regisztrálása
        private async Task AddHospitalModerator( int hospital_id, string email)
        {
            var data = new { hospital_id, email};
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/AddModerator", data);
        }

        //Orvos regisztrálása
        private async Task RegisterDoctorToCompany( int company_id, string email)
        {
            var data = new {email, company_id};
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/RegisterDoctor", data);
        }

        //Orvos céghez rendelése
        private async Task AddDoctorToCompany(int company_id, int doctor_id)
        {
            var data = new { doctor_id, company_id};
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/AddDoctor", data);
        }

        //Email ellenőrzés
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
