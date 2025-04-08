using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Company_Hospitals : Form
    {
        public Company_Hospitals()
        {
            InitializeComponent();

        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Hospital = new List<dynamic>();
        private List<dynamic> Zip = new List<dynamic>();

        //Form betöltése
        private void Ceg_Korhaz_Load(object sender, EventArgs e)
        {
            MeretValtas();
            GetCompanyHospitals();
            GetZip();
            dGV_Hospitals.CellContentClick += DataGridView_CellContentClick;
            dGV_Hospitals.DataBindingComplete += dGV_Hospitals_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dGV_Hospitals_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Hospitals);
        }

        //Form átméretezése (event)
        private void Ceg_Korhaz_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_Hospitals.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Hospitals);
            }
        }

        //Méret váltás
        private void MeretValtas()
        {
            //Főcim helyzetének változtatása
            Lb_CegKorhazak.Location = new Point(20, 10);

            //FőPanel méretének változtatása
            Pn_CegKorhazModositasok.Location = new Point(20, 50);
            Pn_CegKorhazModositasok.Size = new Size(this.Width - 2 * 20, this.Height - 70);

            Btn_KorhazHozzaadas.Location = new Point(Tb_KorhazEmail.Location.X + (Tb_KorhazEmail.Width - Btn_KorhazHozzaadas.Width), Tb_KorhazEmail.Location.Y + 40);

        }

        //Kórázak lekérdezése
        private async void GetCompanyHospitals()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_Hospitals.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/companies/GetHospitals?company_id={GlobalData.CompanyId}");
            if (data == null || data.company_hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            Hospital = data.company_hospitals.ToObject<List<dynamic>>();
            foreach (var hospitals in Hospital)
            {
                int rowIndex = dGV_Hospitals.Rows.Add(hospitals.hospital_name.ToString(), hospitals.hospital_address, hospitals.hospital_email.ToString(), hospitals.@hospital_active == 1);
                dGV_Hospitals.Rows[rowIndex].Cells[dGV_Hospitals.Columns.Count - 1].Value = "Törlés";
                dGV_Hospitals.Rows[rowIndex].Cells[dGV_Hospitals.Columns.Count - 2].Value = "Belépes";
            }
            #endregion
            AdjustDataGridViewHeight(dGV_Hospitals);
        }

        //Irányítószám és település lekérdezése
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

        #region Datagridview kattintások (törlés és felületre való belépés)
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
                        var  hospital= dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {hospital} kórházat?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int hospital_id = Hospital[e.RowIndex].hospital_id;
                            await DeleteCompanyHospital(hospital_id);
                            GetCompanyHospitals();
                        }
                        break;
                    case "felulet":
                        var hospitalid = Hospital[e.RowIndex].hospital_id;
                        var hospitalName = Hospital[e.RowIndex].hospital_name;
                        OpenHospital(hospitalid, hospitalName);
                        GetCompanyHospitals();
                        break;
                    case "Aktiv":
                        int hospital_Id = Hospital[e.RowIndex].hospital_id;
                        string company_Name = Hospital[e.RowIndex].company_name;
                        int company_id = GlobalData.CompanyId;
                        await ChangeHospitalActiveState(hospital_Id, company_id); 
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Kórház törlése
        private async Task DeleteCompanyHospital(int hospital_id)
        {
            var data = new {hospital_id};
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/Delete", data);
        }

        //Kórház aktív státuszának változtatása
        private async Task ChangeHospitalActiveState(int hospital_id, int company_id)
        {
            var data = new { hospital_id, company_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/ChangeActiveState", data);
        }

        //Kórház felület megnyitása
        private void OpenHospital(dynamic hospitalid, dynamic hospitalName)
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
            string companyname = GlobalData.Institution_name;
            string username = Bejelentkezés.GlobalData.UserName;
            GlobalData.HospitalId = hospitalid;
            GlobalData.UserName = "Korhaz";
            GlobalData.RoleIdentifier = "hospital";
            GlobalData.Institution_name = hospitalName;
            Debug.WriteLine($"hali: {GlobalData.HospitalId}");
               
            Main mainForm = new Main();
            mainForm.role_identifier = "hospital";
            mainForm.Text = "Care Compass - " + hospitalName; //Care Compass + a felhasználó neve

            // Megnyitjuk a Main formot
            mainForm.Show();
            //mainForm.IsSubPage = true;

            mainForm.Activated += (s, args) =>
            {
                Debug.WriteLine("Main form aktív, RoleIdentifier = hospital");
                GlobalData.RoleIdentifier = "hospital";
                GlobalData.UserName = "Korhaz";
                GlobalData.Institution_name= hospitalName;
            };

            // Figyeljük, mikor veszti el a fókuszt a Main form
            mainForm.Deactivate += (s, args) =>
            {
                Debug.WriteLine("Main form elvesztette a fókuszt, RoleIdentifier = company");
                GlobalData.RoleIdentifier = "company";
                GlobalData.UserName = username;
                GlobalData.Institution_name = companyname;
            };
            // Amikor a Main form bezárul Form1 bezárul
            mainForm.FormClosed += (s, args) =>
            {
                GlobalData.RoleIdentifier = "company"; // Visszaállítás
                GlobalData.UserName = username;
                GlobalData.Institution_name=companyname;
            };
            
        }
        #endregion

        //Kórház regisztrálása céghez
        private async Task AddCompanyHospital(int company_id, string email, string name, string address)
        {
            var data = new { email, name, address, company_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/Register", data);
        }

        //Kórház regisztrálása click event
        private async void Btn_KorhazHozzaadas_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox[] textboxok = { Tb_KorhazNev, Tb_Iranyitoszam, Tb_Telepules, Tb_Utca, Tb_Hazszam, Tb_KorhazEmail };
            //Ellenőrzés hogy mindegyik mező ki van-e töltve
            if (textboxok.All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                if (ZipGood() == true)
                {
                    if (IsGoodEmail(Tb_KorhazEmail.Text.ToString())){
                        string email = Tb_KorhazEmail.Text.ToString();
                        int company_id = GlobalData.CompanyId;
                        string name = Tb_KorhazNev.Text.ToString();
                        string address = $"{Tb_Iranyitoszam.Text} {Tb_Telepules.Text}, {Tb_Utca.Text} {Tb_Hazszam.Text}";
                        // Api meghívása
                        await AddCompanyHospital(company_id, email, name, address);
                        GetCompanyHospitals();

                        Tb_KorhazEmail.Clear();
                        Tb_KorhazNev.Clear();

                        Tb_Hazszam.Clear();
                        Tb_Iranyitoszam.Clear();
                        Tb_Telepules.Clear();
                        Tb_Utca.Clear();

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
                MessageBox.Show("Kérlek !", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        //Irányítószám input ellennőrzése (csak szám)
        private void Tb_Iranyitoszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Iranyitoszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokat írjon!");
                Tb_Iranyitoszam.Text = Tb_Iranyitoszam.Text.Remove(Tb_Iranyitoszam.Text.Length - 1);
            }
        }

        //Email ellenőrzáse (formálya megfelelő-e)
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
