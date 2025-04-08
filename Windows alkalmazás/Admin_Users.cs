using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Admin_Users : Form
    {
        public Admin_Users()
        {
            InitializeComponent();
        }

        //DataGridView-ek listái
        private List<dynamic> Doctor = new List<dynamic>();
        private List<dynamic> Moderators = new List<dynamic>();
        private List<dynamic> Admins = new List<dynamic>();

        //ComboBox-ok listái
        private List<dynamic> Roles = new List<dynamic>();
        private List<dynamic> Company = new List<dynamic>();
        private List<dynamic> Hospital = new List<dynamic>();
        private List<dynamic> LotDoctor = new List<dynamic>();

        private int AdminCompanyID;
        private int AdminHospitalID;

        //Form betöltése
        private void Admin_Felhasznalok_Load(object sender, EventArgs e)
        {
            CB_AlkalmazottCeg.Visible = false;
            CB_AlkalmazottKorhaz.Visible = false;
            Lb_AlkalmazottCeg.Visible=false;
            Lb_AlkalmazottKorhaz.Visible=false;

            GetDoctors();
            GetAdmins();
            GetModerators();
            GetRoles();

            dGV_Admin.CellContentClick += DataGridView_CellContentClick;
            dGV_Moderatorok.CellContentClick += DataGridView_CellContentClick;
            dGV_Orvos.CellContentClick += DataGridView_CellContentClick;
            dGV_Admin.DataBindingComplete += dGV_Admin_DataBindingComplete;
            dGV_Moderatorok.DataBindingComplete += dGV_Moderatorok_DataBindingComplete;
            dGV_Orvos.DataBindingComplete += dGV_Orvos_DataBindingComplete;
        }

        #region DataGridView adatfeltöltés utáni átméretezése
        private void dGV_Moderatorok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Moderatorok);
        }

        private void dGV_Orvos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Orvos);
        }

        private void dGV_Admin_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Admin);
        }
        #endregion

        //Form méretének módosítása (event)
        private void Admin_Felhasznalok_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_Admin.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Admin);
            }
            if (dGV_Moderatorok.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Moderatorok);
            }
            if (dGV_Orvos.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Orvos);
            }
        }

        //Méret módosítása
        private void MeretValtas()
        {
            //FőPanel méretének változtatása
            tLP_Felhasznalok.Location = new Point(20, 20);
            tLP_Felhasznalok.Size = new Size(this.Width - 2 * 20, this.Height - 70);
        }

      
        #region DataGridView-ek feltöltése és comboboxok feltöltése
        //Orvosok lekérdezése
        private async void GetDoctors()
        {
            dGV_Orvos.Rows.Clear();

            var data = await FetchData("/api/v1/api.php/doctors/GetDoctors");
            if (data == null || data.doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            LotDoctor = data.doctors.ToObject<List<dynamic>>();
            // Egyedi ID-kat tartalmazó lista létrehozása
            Doctor = LotDoctor
                .GroupBy(d => d.id.ToString()) // ID alapján csoportosít
                .Select(g => g.First()) // Az első elemet választja minden csoportból
                .ToList();

            foreach (var doctor in Doctor)
            {
                int rowIndex = dGV_Orvos.Rows.Add(doctor.name.ToString(), doctor.email.ToString());

                // Ha a role_name nem "Cég", akkor beállítjuk a "Belépés" gombot
                var loginButton = dGV_Orvos.Rows[rowIndex].Cells[dGV_Orvos.Columns.Count - 2];
                if (doctor.name != null)
                {
                    Debug.WriteLine("jani");
                    loginButton.Value = "Belépés"; // Csak akkor állítjuk be, ha nem "Cég"
                }
                else
                {
                    Debug.WriteLine("peti");
                    loginButton.Value = ""; // Üresen hagyjuk, ha "Cég" a szerepkör
                    loginButton.Style.BackColor = Color.Gray; // Lezárjuk vizuálisan is
                    loginButton.Tag = "Disabled"; // Tag letiltva
                    loginButton.ReadOnly = true;
                }

                // "Törlés" gomb beállítása
                var deleteButton = dGV_Orvos.Rows[rowIndex].Cells[dGV_Orvos.Columns.Count - 1];
                deleteButton.Value = "Törlés";
            }
            #endregion
            AdjustDataGridViewHeight(dGV_Orvos);
        }

        //Moderátorok lekérdezése
        private async void GetModerators()
        {
            dGV_Moderatorok.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/users/GetAllModerators");
            if (data == null || data.moderators == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview feltöltése
            Moderators = data.moderators.ToObject<List<dynamic>>();
            foreach (var moderator in Moderators)
            {
                int rowIndex = dGV_Moderatorok.Rows.Add(moderator.name.ToString(), moderator.email, moderator.role, moderator.institution);
                dGV_Moderatorok.Rows[rowIndex].Cells[dGV_Moderatorok.Columns.Count - 1].Value = "Törlés";
            }
            #endregion
            AdjustDataGridViewHeight(dGV_Moderatorok);
        }

        //Adminok lekérdezése
        private async void GetAdmins()
        {
            dGV_Admin.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/users/GetAdmins");
            if (data == null || data.admins == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview feltöltése
            Admins = data.admins.ToObject<List<dynamic>>();
            foreach (var admin in Admins)
            {
                int rowIndex = dGV_Admin.Rows.Add(admin.user_name.ToString(), admin.user_email);
                dGV_Admin.Rows[rowIndex].Cells[dGV_Admin.Columns.Count - 1].Value = "Törlés";

            }
            #endregion
            AdjustDataGridViewHeight(dGV_Admin);
        }


        //Feladatkörök lekérdezése
        private async void GetRoles ()
        {
            CB_AlkalmazottMunkaterulet.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/admin/roles");
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
            #endregion
            
        }
        //Cégek lekérdezése
        private async void GetCompany()
        {
            CB_AlkalmazottCeg.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/admin/companies");
            if (data == null || data.companies == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            Company = data.companies.ToObject<List<dynamic>>();
            foreach (var company in Company)
            {
                CB_AlkalmazottCeg.Items.Add(company.name.ToString());
            }
            #endregion
            
        }

        //Kórházak lekérdezése
        private async void GetHospital()
        {
            CB_AlkalmazottKorhaz.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/company/hospitals?company_id={AdminCompanyID}");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            Hospital = data.hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in Hospital)
            {
                CB_AlkalmazottKorhaz.Items.Add(hospital.name.ToString());
            }
            #endregion
            
        }
        #endregion

        //DataGridView kattintások kezelése
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
                    case "Torles":
                        // Telefonszám törlése logika
                        var doctor = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteDoctor = MessageBox.Show($"Biztosan törlöd a(z) {doctor} nevű orvost?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteDoctor == DialogResult.Yes)
                        {
                            int doctor_id = Doctor[e.RowIndex].id; // Az ID lekérése a listából
                            await DeleteDoctor(doctor_id); // API hívás a törléshez
                            GetDoctors();
                        }
                        break;

                    case "MTorles":
                        // Telefonszám törlése logika
                        var hospital = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteModerator = MessageBox.Show($"Biztosan törlöd a(z) {hospital} moderátort?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteModerator == DialogResult.Yes)
                        {
                            int moderator_id = Moderators[e.RowIndex].id;
                            if (Moderators[e.RowIndex].role == "Cég")
                            {
                                int company_id = Moderators[e.RowIndex].institution_id;
                                await DeleteCompanyModerator(moderator_id, company_id);
                                GetModerators();
                            }
                            else if (Moderators[e.RowIndex].role == "Kórház")
                            {
                                int hospital_id = Moderators[e.RowIndex].institution_id;
                                await DeleteHospitalModerator(moderator_id, hospital_id);
                                GetModerators();
                            }
                        }
                        break;
                    case "ATorles":
                        // Admin törlése logika
                        var admin = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteAdmin = MessageBox.Show($"Biztosan törlöd a(z) {admin} admint?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteAdmin == DialogResult.Yes)
                        {
                            int user_id = Admins[e.RowIndex].user_id; // Az ID lekérése a listából
                            await DeleteAdmin(user_id); // API hívás a törléshez
                            GetAdmins();
                        }
                        break;
                    case "DFelulet":
                        if (Doctor[e.RowIndex].name == null)
                        {
                            MessageBox.Show("Amig a felhasználó nem töltötte ki a bejelentkezést addig a felület nem elérhető!");
                        }
                        else
                        {
                            var doctorID = Doctor[e.RowIndex].id;
                            var doctorName = Doctor[e.RowIndex].name;
                            OpenDoctor(doctorID, doctorName); // API hívás a törléshez
                            GetModerators();
                        }
                            
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        #region Felhasználók törlése
        //Orvos törlése
        private async Task DeleteDoctor(int doctor_id)
        {
            var data = new { doctor_id};
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/doctors/Delete", data);
        }

        //Cég moderátor törlése
        private async Task DeleteCompanyModerator(int moderator_id, int company_id)
        {
            var data = new { moderator_id, company_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/companies/RemoveModerator", data);
        }

        //Kórház moderátor törlése
        private async Task DeleteHospitalModerator(int moderator_id, int hospital_id)
        {
            var data = new { moderator_id, hospital_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/RemoveModerator", data);
        }

        //Admin törlése
        private async Task DeleteAdmin(int user_id)
        {
            var data = new { user_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/users/Delete", data);
        }
        #endregion

        //Orvos felület megnyitása
        private void OpenDoctor(dynamic doctorID, dynamic doctorName)
        {
            string username = GlobalData.UserName;
            int userid = GlobalData.UserId;
            Debug.WriteLine($"halihó {doctorID}");
            GlobalData.UserId =doctorID;
            GlobalData.UserName = doctorName;
            GlobalData.RoleIdentifier = "doctor";
            Debug.WriteLine($"hali: {GlobalData.HospitalId}");
            Main mainForm = new Main();
            //mainForm.Role = role;  // Átadjuk a role értéket
            mainForm.role_identifier = "doctor";
            mainForm.Text = "Care Compass - " + doctorName; //Care Compass + a felhasználó neve
            mainForm.IsSubPage = true;
            Debug.WriteLine($"ja {mainForm.IsSubPage}");
            // Megnyitjuk a Main formot
            mainForm.Show();

            mainForm.Activated += (s, args) =>
            {
                Debug.WriteLine("Main form aktív, RoleIdentifier = doctor");
                Bejelentkezés.GlobalData.RoleIdentifier = "doctor";
                Bejelentkezés.GlobalData.UserName = doctorName;
                Bejelentkezés.GlobalData.UserId = doctorID;
            };

            // Figyeljük, mikor veszti el a fókuszt a Main form
            mainForm.Deactivate += (s, args) =>
            {
                Debug.WriteLine("Main form elvesztette a fókuszt, RoleIdentifier = admin");
                Bejelentkezés.GlobalData.RoleIdentifier = "admin";
                Bejelentkezés.GlobalData.UserName = username;
                Bejelentkezés.GlobalData.UserId = userid;

            };
            // Amikor a Main form bezárul Form1 bezárul
            mainForm.FormClosed += (s, args) =>
            {
                Bejelentkezés.GlobalData.RoleIdentifier = "admin"; // Visszaállítás
                Bejelentkezés.GlobalData.UserName = username;
                Bejelentkezés.GlobalData.UserId = userid;
                this.Close();
            };
        }

        //Felhasználó regisztrálása/létrehozása
        #region Felhasználó regisztrálása/létrehozása
        private async void Btn_AlkalmazottHozzaadas_Click(object sender, EventArgs e)
        {
            if (IsGoodEmail(Tb_AlkalmazottEmail.Text.ToString()))
            {
                string email = Tb_AlkalmazottEmail.Text;
                //Admin regisztrálása
                if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Admin")
                {
                    foreach (var role in Roles)
                    {
                        if (role.name.ToString() == CB_AlkalmazottMunkaterulet.SelectedItem.ToString())
                        {
                            int role_id = Convert.ToInt32(role.id);

                            // Api meghívása
                            await AddAdmin(email, role_id);
                            GetAdmins();
                            Tb_AlkalmazottEmail.Clear();
                            GetRoles();
                        }
                    }

                }

                else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Cég")
                {
                    if (CB_AlkalmazottCeg.SelectedIndex != -1)
                    {
                        string selectedCompanyName = CB_AlkalmazottCeg.SelectedItem.ToString();

                        foreach (var company in Company)
                        {
                            if (company.name.ToString() == selectedCompanyName)
                            {
                                int company_id = Convert.ToInt32(company.id);

                                // Api meghívása
                                await AddCompanyModerator(company_id, email);
                                GetModerators();
                                Lb_AlkalmazottCeg.Visible = false;
                                CB_AlkalmazottCeg.Visible = false;
                                Tb_AlkalmazottEmail.Clear() ;
                                GetRoles();
                            }
                        }
                    }
                }
                //Kórház moderátor regisztrálása
                else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Kórház")
                {
                    if (CB_AlkalmazottCeg.SelectedIndex != -1)
                    {
                        string selectedHospitalName = CB_AlkalmazottKorhaz.SelectedItem.ToString();

                        foreach (var hospital in Hospital)
                        {
                            if (hospital.name.ToString() == selectedHospitalName)
                            {
                                int hospital_id = Convert.ToInt32(hospital.id);

                                // Api meghívása
                                await AddHospitalModerator(hospital_id, email);
                                GetModerators();
                                CB_AlkalmazottCeg.Visible = false;
                                CB_AlkalmazottKorhaz.Visible= false;
                                Tb_AlkalmazottEmail.Clear();
                                GetRoles();
                            }
                        }
                    }

                }
                //Orvos regisztrálása
                else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Doktor")
                {
                    //Sima orvos regisztráció
                    await RegisterDoctor(email);
                    GetDoctors();
                    Tb_AlkalmazottEmail.Clear();
                    GetRoles();
                }
                else
                {
                    MessageBox.Show("Válasszon  munkaterületet az új felhasználónak");
                } 
            }
            else
            {
                MessageBox.Show("Nem megfelelő email");
            }
        }

        //Orvos regisztrálása
        private async Task RegisterDoctor(string email)
        {
            var data = new { email };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/Add", data);
        }

        //Kórház moderátor regisztrálása
        private async Task AddHospitalModerator(int hospital_id, string email)
        {
            var data = new { hospital_id, email };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/AddModerator", data);
        }

        //Cég moderátor regisztrálása
        private async Task AddCompanyModerator(int company_id, string email)
        {
            var data = new { company_id, email };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/AddModerator", data);
        }
        
        //Admin regisztrálása
        private async Task AddAdmin(string email, int role_id)
        {
            var data = new { email, role_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/users/InviteAdmin", data);
        }

        #endregion

        //Munkaterület kiválasztása
        private void CB_AlkalmazottMunkaterulet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_AlkalmazottMunkaterulet.SelectedIndex > -1)
            {
                //Orvos
                if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Doktor")
                {
                    CB_AlkalmazottCeg.Visible = false;
                    Lb_AlkalmazottCeg.Visible = false;
                    CB_AlkalmazottKorhaz.Visible = false;
                    Lb_AlkalmazottKorhaz.Visible = false;
                }
                else if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Admin")
                {
                    //Cég kiválasztása
                    CB_AlkalmazottCeg.Visible=false;
                    Lb_AlkalmazottCeg.Visible=false;
                    CB_AlkalmazottKorhaz.Visible = false;
                    Lb_AlkalmazottKorhaz.Visible = false;
                }
                //Kórház
                else if(CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Kórház")
                {
                    //Cég kiválaszta
                    CB_AlkalmazottCeg.Visible = true;
                    GetCompany();
                    if(CB_AlkalmazottCeg.SelectedIndex!=0)
                    Lb_AlkalmazottCeg.Visible = true;
                    CB_AlkalmazottKorhaz.Visible = true;
                    Lb_AlkalmazottKorhaz.Visible = true;
                }
                //Cég
                else
                {
                    //Cég kiválasztása
                    CB_AlkalmazottCeg.Visible = true;
                    GetCompany();
                    Lb_AlkalmazottCeg.Visible = true;
                    //Kórház kiválasztása
                    CB_AlkalmazottKorhaz.Visible = false;
                    Lb_AlkalmazottKorhaz.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Válasszon ki munkaterületet");
            }
        }

        //Kiválasztott cég kiválasztása
        private void CB_AlkalmazottCeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_AlkalmazottMunkaterulet.SelectedItem.ToString() == "Kórház")
            {
                if (CB_AlkalmazottCeg.SelectedIndex > -1)
                {
                    string selectedCompanyName = CB_AlkalmazottCeg.SelectedItem.ToString();
                    foreach (var company in Company)
                    {
                        if (company.name.ToString() == selectedCompanyName)
                        {
                            AdminCompanyID = company.id;
                            GetHospital();
                        }
                    }
                    CB_AlkalmazottKorhaz.Visible = true;
                    Lb_AlkalmazottKorhaz.Visible = true;
                }
            }
            else
            {
                CB_AlkalmazottKorhaz.Visible = false;
                Lb_AlkalmazottKorhaz.Visible = false;
            }
        }

        //Email ellenőrzés
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
