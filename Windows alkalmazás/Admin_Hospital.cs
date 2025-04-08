using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    public partial class Admin_Hospital : Form
    {
        public Admin_Hospital()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Hospitals = new List<dynamic>();
        private List<dynamic> Companies = new List<dynamic>();
        private List<dynamic> Zip = new List<dynamic>();

        //Form betöltése
        private void Admin_Hospital_Load(object sender, EventArgs e)
        {
            MeretValtas();
            GetHospitals();
            GetCompanies();
            GetZip();
            
            dGV_Hospitals.DataBindingComplete += dGV_Hospitals_DataBindingComplete;
            dGV_Hospitals.CellContentClick += DataGridView_CellContentClick;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dGV_Hospitals_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Hospitals);
        }

        //Méret váltás 
        private void MeretValtas()
        {
            Pn_AdminKorhaz.Location = new Point(20, 50);
            Pn_AdminKorhaz.Size = new Size(this.Width - 2 * 20, this.Height - 70);

            Btn_KorhazHozzaadas.Location = new Point(cB_Ceg.Location.X + (cB_Ceg.Width - Btn_KorhazHozzaadas.Width), cB_Ceg.Location.Y + 40);

        }

        //Form méretének változása (event)
        private void Admin_Hospital_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            // Hívjuk meg az újraméretezést a DataGridView-ra is
            if (dGV_Hospitals.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Hospitals);
            }
        }

        //Kórházak lekérdezése
        private async void GetHospitals()
        {
            dGV_Hospitals.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/hospitals/GetHospitals");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            // Phonenumbers DataGridView
            Hospitals = data.hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in Hospitals)
            {
                int rowIndex = dGV_Hospitals.Rows.Add(hospital.hospital_name.ToString(), /*hospital.hospital_address,*/ hospital.hospital_email, hospital.@hospital_active == 1);
                dGV_Hospitals.Rows[rowIndex].Cells[dGV_Hospitals.Columns.Count - 2].Value = "Belépés";
                dGV_Hospitals.Rows[rowIndex].Cells[dGV_Hospitals.Columns.Count - 1].Value = "Törlés";
            }
            
            #endregion
            AdjustDataGridViewHeight(dGV_Hospitals) ;
        }
        //Cégek lekérdezése
        private async void GetCompanies()
        {
            cB_Ceg.Items.Clear();
            var data =await FetchData("/api/v1/api.php/data/admin/companies");
            if (data == null || data.companies == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            // Educations DataGridView
            Companies = data.companies.ToObject<List<dynamic>>();
            foreach (var company in Companies)
            {
                cB_Ceg.Items.Add(company.name.ToString());
            }
            #endregion
            
        }
        //Irányítószám település lekérdezése
        private async void GetZip()
        {
            var data = await FetchData("/api/v1/api.php/data/general/GetZipCodes");
            if (data == null || data.zip_codes == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Zip = data.zip_codes.ToObject<List<dynamic>>();
           
        }

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
                    case "torles":
                        var hospital = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {hospital} kórházat?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int hospital_id = Hospitals[e.RowIndex].hospital_id;
                            await DeleteHospital(hospital_id);
                            GetHospitals();
                        }
                        break;
                    case "felulet":
                        int hospitalid = Hospitals[e.RowIndex].hospital_id;
                        string hospitalName = Hospitals[e.RowIndex].hospital_name;
                        string company_name = Hospitals[e.RowIndex].company_name;

                        foreach (var company in Companies)
                        {
                            if (company.name.ToString() == company_name)
                            {
                                int company_id=company.id;
                                OpenHospital(hospitalid, company_id, hospitalName);
                                GetHospitals();
                            }
                        }
                        break;
                    case "Aktiv":
                        int hospital_Id = Hospitals[e.RowIndex].hospital_id;
                        string company_Name = Hospitals[e.RowIndex].company_name;
                        foreach (var company in Companies)
                        {
                            if (company.name.ToString() == company_Name)
                            {
                                int company_id = company.id; 
                                await ChangeHospitalActiveState(hospital_Id, company_id);
                            }
                        }
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Kórház törlése
        private async Task DeleteHospital(int hospital_id)
        {
            var data = new { hospital_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/Delete", data);
        }
        //Kórház aktív státuszának módosítása
        private async Task ChangeHospitalActiveState(int hospital_id, int company_id)
        {
            var data = new { hospital_id, company_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/ChangeActiveState", data);
        }
        //Kórház felület megnyitása
        private void OpenHospital(dynamic hospitalid,dynamic company_id, dynamic hospitalName)
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

            Debug.WriteLine($"halihó {hospitalid}");
            
            string username = Bejelentkezés.GlobalData.UserName;
            GlobalData.HospitalId = hospitalid;
            GlobalData.CompanyId = company_id;
            GlobalData.UserName = "Korhaz";
            GlobalData.RoleIdentifier = "hospital";
            GlobalData.Institution_name = hospitalName;
            Debug.WriteLine($"hali: {Bejelentkezés.GlobalData.HospitalId}");
            //Main mainForm = new Main();
            //mainForm.Role = role;  // Átadjuk a role értéket
            Main mainForm = new Main();
            mainForm.role_identifier = "hospital";
            mainForm.Text = "Care Compass - " + hospitalName; //Care Compass + a felhasználó neve
            mainForm.IsSubPage = true;
            // Megnyitjuk a Main formot
            mainForm.Show();

            mainForm.Activated += (s, args) =>
            {
                Debug.WriteLine("Main form aktív, RoleIdentifier = hospital");
                Bejelentkezés.GlobalData.RoleIdentifier = "hospital";
                Bejelentkezés.GlobalData.Institution_name = hospitalName;
            };

            // Figyeljük, mikor veszti el a fókuszt a Main form
            mainForm.Deactivate += (s, args) =>
            {
                Debug.WriteLine("Main form elvesztette a fókuszt, RoleIdentifier = admin");
                Bejelentkezés.GlobalData.RoleIdentifier = "admin";
                Bejelentkezés.GlobalData.UserName = username;
            };
            // Amikor a Main form bezárul Form1 bezárul
            mainForm.FormClosed += (s, args) =>
            {
                Bejelentkezés.GlobalData.RoleIdentifier = "admin"; // Visszaállítás
                Bejelentkezés.GlobalData.UserName = username;
                this.Close();
            };

        }

        //Új kórház hozzáadása/Regisztrálása (click event)
        private async void Btn_KorhazHozzaadas_Click(object sender, EventArgs e)
        {
            TextBox[] textboxok = { Tb_Korhaznev, Tb_Iranyitoszam, Tb_Telepules, Tb_Utca, Tb_Hazszam, Tb_KorhazEmail };
            //Ellenőrzés hogy mindegyik mező ki van-e töltve
            if (textboxok.All(tb => !string.IsNullOrWhiteSpace(tb.Text)) && cB_Ceg.SelectedIndex!=-1)
            {
                if (ZipGood()==true)
                {
                    if (IsGoodEmail(Tb_KorhazEmail.Text.ToString()))
                    {
                        string email = Tb_KorhazEmail.Text;
                        string name = Tb_Korhaznev.Text;
                        string address = $"{Tb_Iranyitoszam.Text} {Tb_Telepules.Text}, {Tb_Utca.Text} {Tb_Hazszam.Text}";
                        string selectedCompanyName=cB_Ceg.SelectedItem.ToString();
                        if(cB_Ceg.SelectedIndex>-1)
                        {
                            foreach (var company in Companies)
                            {
                                if (company.name == selectedCompanyName)
                                {
                                    int company_id = Convert.ToInt32(company.id);
                                    // Api meghívása
                                    await AddHospitalToCompany(email, name, address, company_id);
                                    GetHospitals();
                                    GetCompanies();

                                    Tb_KorhazEmail.Clear();
                                    Tb_Korhaznev.Clear();

                                    Tb_Hazszam.Clear();
                                    Tb_Iranyitoszam.Clear();
                                    Tb_Telepules.Clear();
                                    Tb_Utca.Clear();

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nem megfelelő email");
                    }
                }
                else
                {
                    MessageBox.Show("Az irányítószám vagy a település neve nem megfelelő!");
                }
            }
            else
            {
                MessageBox.Show("Valamelyik mező nincs kitöltve!");
            }
        }

        //Kórház regisztrálása 
        private async Task AddHospitalToCompany(string email, string name, string address, int company_id)
        {
            var data = new { email, name, address, company_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/Register", data);
        }

        //Irányítószám és település ellenőrzése
        private bool ZipGood()
        {

            if (Zip == null || Zip.Count == 0)
            {
                return false;
            }

            string zip = Tb_Iranyitoszam.Text.Trim();
            string telepules = Tb_Telepules.Text.Trim();

            var matchingEntries = Zip.Where(entry =>
                entry.zip.ToString() == zip &&
                string.Equals(entry.name.ToString(), telepules, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Ha legalább egyezés van
            return matchingEntries.Count > 0;
        }

        //Irányítószám bemeneti karakterek ellenőrzése (csak szám)
        private void Tb_Iranyitoszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Iranyitoszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokat írjon!");
                Tb_Iranyitoszam.Text = Tb_Iranyitoszam.Text.Remove(Tb_Iranyitoszam.Text.Length - 1);
            }
        }

        //Email formátum ellenőrzése
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
