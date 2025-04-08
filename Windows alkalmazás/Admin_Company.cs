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
    public partial class Admin_Company : Form
    {
        public Admin_Company()
        {
            InitializeComponent();
        }

        //Átméretezés
        private void MeretValtas()
        {
            //FőPanel méretének változtatása
            Pn_CegMain.Location = new Point(20, 50);
            Pn_CegMain.Size = new Size(this.Width - 2 * 20, this.Height - 70);
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Company = new List<dynamic>();
        private List<dynamic> Zip = new List<dynamic>();

        //Form átméretezése
        private void Admin_CegRegisztracio_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_AdminCegek.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_AdminCegek);
            }
        }

        //Form betöltése
        private void Admin_CegRegisztracio_Load(object sender, EventArgs e)
        {
            GetAllCompany();
            GetZip();
            dGV_AdminCegek.CellContentClick += DataGridView_CellContentClick;
            dGV_AdminCegek.DataBindingComplete += dGV_AdminCegek_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dGV_AdminCegek_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_AdminCegek);
        }

        //Cégek feltöltése
        private async void GetAllCompany()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_AdminCegek.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/companies/GetCompanies");
            if (data == null || data.companies == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            //Cég kórházai DataGridView
            Company = data.companies.ToObject<List<dynamic>>();
            foreach (var company in Company)
            {
                int rowIndex = dGV_AdminCegek.Rows.Add(company.company_name.ToString(), company.company_email.ToString(), company.company_address, company.@company_active==1);
                dGV_AdminCegek.Rows[rowIndex].Cells[dGV_AdminCegek.Columns.Count - 2].Value = "Belépés";
                dGV_AdminCegek.Rows[rowIndex].Cells[dGV_AdminCegek.Columns.Count - 1].Value = "Törlés";
            }
            #endregion
            AdjustDataGridViewHeight(dGV_AdminCegek);
        }

        //Irányítószámok lekérdezése
        private async void GetZip()
        {
            var data =await FetchData("/api/v1/api.php/data/general/GetZipCodes");
            if (data == null || data.zip_codes == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Zip = data.zip_codes.ToObject<List<dynamic>>();
        }

        //Datagridview-(ek) click eventjei
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
                    case "CFelulet":
                        // Cég felületbe belépés
                        var companyID = Company[e.RowIndex].company_id;
                        var companyName = Company[e.RowIndex].company_name;// Az ID lekérése a listából
                        OpenCompany(companyID, companyName); // API hívás a törléshez
                        GetAllCompany();
                        break;
                    case "Aktiv":
                        // Cég felületbe belépés
                        int company_Id = Company[e.RowIndex].company_id;
                        await ChangeCompanyActiveState(company_Id); // API hívás a törléshez
                        GetAllCompany();
                        break;
                    case "Torles":
                        // TCég felületbe belépés
                        var company = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {company} kórházat?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int company_id = Company[e.RowIndex].company_id; // Az ID lekérése a listából
                            await DeleteCompany(company_id); // API hívás a törléshez
                            GetAllCompany();
                        }
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Cég felület megnyitása
        private void OpenCompany(dynamic companyID, dynamic companyName)
        {
            string username = GlobalData.UserName;
            int userid = GlobalData.UserId;
            GlobalData.CompanyId = companyID;
            GlobalData.RoleIdentifier = "company";
            GlobalData.UserName = "company";
            GlobalData.Institution_name = companyName;
            Main mainForm = new Main();
            mainForm.role_identifier = "company";
            mainForm.Text = "Care Compass - " + companyName; //Care Compass + a cég neve

            mainForm.IsSubPage = true;
            // Megnyitjuk a Main formot
            mainForm.Show();

            mainForm.Activated += (s, args) =>
            {
                Debug.WriteLine("Main form aktív, RoleIdentifier = company");
                GlobalData.RoleIdentifier = "company";
                GlobalData.UserName = companyName;
                GlobalData.CompanyId = companyID;
                GlobalData.Institution_name = companyName;
            };

            // Figyeljük, mikor veszti el a fókuszt a Main form
            mainForm.Deactivate += (s, args) =>
            {
                Debug.WriteLine("Main form elvesztette a fókuszt, RoleIdentifier = admin");
                GlobalData.RoleIdentifier = "admin";
                GlobalData.UserName = username;
                GlobalData.UserId = userid;
                GlobalData.Institution_name = username;
            };
            // Amikor a Main form bezárul Form1 bezárul
            mainForm.FormClosed += (s, args) =>
            {
                GlobalData.RoleIdentifier = "admin"; // Visszaállítás
                GlobalData.UserName = username;
                GlobalData.UserId = userid;
                GlobalData.Institution_name = username;
            };
        }

        //Cég aktivitásást állító lekérdezés
        private async Task ChangeCompanyActiveState(int company_id)
        {
            var data = new { company_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/ChangeActiveState", data);
        }
        //Cég törlése
        private async Task DeleteCompany(int company_id)
        {
            var data = new {company_id};
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/companies/Delete", data);
        }

        //Cég hozzáadása gomb
        private async void Btn_CegHozzaadas_Click(object sender, EventArgs e)
        {
            TextBox[] textboxok = { Tb_CegNev, Tb_CegCegjegyzekSzam, Tb_CegAdoszam, Tb_CegEmail,
                        Tb_CegIgazgatoNeve, Tb_CegTelepules, Tb_Iranyitoszam,
                        Tb_CegUtca, Tb_Hazszam };
            //Ellenőrzés hogy mindegyik mező ki van-e töltve
            if (textboxok.All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                if (ZipGood() == true)
                {
                    string cegjegyzekszam = Tb_CegCegjegyzekSzam.Text;
                    string cegpattern = @"^^(0[1-9]|1[0-9]|20)-(0[1-9]|1[0-9]|2[0-3])-\d{6}$";
                    if (Regex.IsMatch(cegjegyzekszam, cegpattern))
                    {
                        //Adószám ellenőrzés
                        string adoszam=Tb_CegAdoszam.Text;
                        string adopattern = @"^\d{8}-[1-5]-((0[2-9]|1[0-9]|20)|(22|2[3-9]|3[0-9]|4[0-4])|51)$";
                        if (Regex.IsMatch(adoszam, adopattern))
                        {
                            if (IsGoodEmail(Tb_CegEmail.Text.ToString()))
                            {
                                if (IsValidTaxNumber(adoszam))
                                {

                                    string name = Tb_CegNev.Text;
                                    string registry_number = Tb_CegCegjegyzekSzam.Text;
                                    string tax_number = Tb_CegAdoszam.Text;
                                    string director_name = Tb_CegIgazgatoNeve.Text;
                                    string email = Tb_CegEmail.Text;
                                    string address = $"{Tb_Iranyitoszam.Text} {Tb_CegTelepules.Text}, {Tb_CegUtca.Text} {Tb_Hazszam.Text}";
                                    await RegisterCompany(name, registry_number, tax_number, director_name, email, address);
                                    GetAllCompany();

                                    //Adatok kitörlése
                                    Tb_CegNev.Clear();
                                    Tb_CegEmail.Clear();
                                    Tb_CegIgazgatoNeve.Clear();
                                    Tb_CegTelepules.Clear();
                                    Tb_CegUtca.Clear();
                                    Tb_Hazszam.Clear();
                                    Tb_Iranyitoszam.Clear();
                                    Tb_CegAdoszam.Clear();
                                    Tb_CegCegjegyzekSzam.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Hibás adószám: az ellenőrző számjegy nem megfelelő!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nem megfelelő email");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hibás adószám formátum!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hibás Cégjegyzékszám!");
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
        //Cég hozzáadása
        private async Task RegisterCompany(string name, string registry_number, string tax_number, string director_name, string email, string address)
        {
            var data = new { name, registry_number, tax_number, director_name, email, address };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/Register", data);
        }

        //Irányítószám települése ellenőrzés
        private bool ZipGood()
        {

            if (Zip == null || Zip.Count == 0)
            {
                return false;
            }
           
            string zip = Tb_Iranyitoszam.Text.Trim();
            string telepules = Tb_CegTelepules.Text.Trim();

            var matchingEntries = Zip.Where(entry =>
                entry.zip.ToString() == zip &&
                string.Equals(entry.name.ToString(), telepules, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Ha legalább egyezés van
            return matchingEntries.Count > 0;
        }

        //Cégjegyzékszám bemeneti formátum kezelő
        private void Tb_CegCegjegyzekSzam_TextChanged(object sender, EventArgs e)
        {
            
            // Mentjük az eredeti kurzorpozíciót
            int cursorPosition = Tb_CegCegjegyzekSzam.SelectionStart;

            // Eltávolítjuk az összes kötőjelet, hogy tiszta szöveggel dolgozzunk
            string text = Tb_CegCegjegyzekSzam.Text.Replace("-", "").ToUpper();

            // Ellenőrizzük, hogy csak számok és betűk legyenek
            if (System.Text.RegularExpressions.Regex.IsMatch(text, "[^0-9, -]"))
            {
                MessageBox.Show("Kérlek, csak számokat és betűket írj!");
                text = System.Text.RegularExpressions.Regex.Replace(text, "[^0-9, -]", ""); // Töröljük a tiltott karaktereket
                cursorPosition--; // Csökkentjük a kurzorpozíciót
            }

            string formattedText = text;

            // Új formázás kötőjelekkel
            if (formattedText.Length > 2)
            {
                formattedText = formattedText.Insert(2, "-");
            }
            if (formattedText.Length > 5)
            {
                formattedText = formattedText.Insert(5, "-");
            }

            // Számoljuk meg, hogy hány kötőjel került a kurzor előtti pozícióba
            int dashCountBeforeCursor = 0;
            for (int i = 0; i < Math.Min(cursorPosition, formattedText.Length); i++)
            {
                if (formattedText[i] == '-')
                {
                    dashCountBeforeCursor++;
                }
            }

            // Ha változott a szöveg, frissítjük a mezőt
            if (Tb_CegCegjegyzekSzam.Text != formattedText)
            {
                Tb_CegCegjegyzekSzam.Text = formattedText;

                // Visszaállítjuk a kurzor pozícióját figyelembe véve a kötőjeleket
                Tb_CegCegjegyzekSzam.SelectionStart = cursorPosition + dashCountBeforeCursor;
            }
        }

        //Adószám ellenőrzése
        #region Adószám ellenőrzése
        //Adőszám bemeneti formátum kezelő
        private void Tb_CegAdoszam_TextChanged(object sender, EventArgs e)
        {
            // Mentjük az eredeti kurzorpozíciót
            int cursorPosition = Tb_CegAdoszam.SelectionStart;

            // Eltávolítjuk az összes kötőjelet, hogy tiszta szöveggel dolgozzunk
            string text = Tb_CegAdoszam.Text.Replace("-", "").ToUpper();

            // Ellenőrizzük, hogy csak számok és betűk legyenek
            if (System.Text.RegularExpressions.Regex.IsMatch(text, "[^0-9, -]"))
            {
                MessageBox.Show("Kérlek, csak számokat és betűket írj!");
                text = System.Text.RegularExpressions.Regex.Replace(text, "[^0-9, -]", ""); // Töröljük a tiltott karaktereket
                cursorPosition--; // Csökkentjük a kurzorpozíciót
            }

            string formattedText = text;

            // Új formázás kötőjelekkel
            if (formattedText.Length > 8)
            {
                formattedText = formattedText.Insert(8, "-");
            }
            if (formattedText.Length > 10)
            {
                formattedText = formattedText.Insert(10, "-");
            }

            // Számoljuk meg, hogy hány kötőjel került a kurzor előtti pozícióba
            int dashCountBeforeCursor = 0;
            for (int i = 0; i < Math.Min(cursorPosition, formattedText.Length); i++)
            {
                if (formattedText[i] == '-')
                {
                    dashCountBeforeCursor++;
                }
            }

            // Ha változott a szöveg, frissítjük a mezőt
            if (Tb_CegAdoszam.Text != formattedText)
            {
                Tb_CegAdoszam.Text = formattedText;

                // Visszaállítjuk a kurzor pozícióját figyelembe véve a kötőjeleket
                Tb_CegAdoszam.SelectionStart = cursorPosition + dashCountBeforeCursor;
            }
        }

        //Adószám ellenőrzése (létezik-e)
        private bool IsValidTaxNumber(string taxNumber)
        {
            // Az első 8 számjegy kinyerése
            string elso8szam = taxNumber.Substring(0, 8);

            // Ellenőrző szám kiszámítása
            int[] multipliers = { 9, 7, 3, 1, 9, 7, 3 }; // Súlyok
            int sum = 0;

            for (int i = 0; i < 7; i++)
            {
                sum += (elso8szam[i] - '0') * multipliers[i];
            }

            int calculatedCheckDigit = (10 - (sum % 10)) % 10; // Ellenőrző szám

            // Valódi 8. szám ellenőrzése
            int actualCheckDigit = elso8szam[7] - '0';

            return calculatedCheckDigit == actualCheckDigit;
        }
        #endregion

        //Email ellenőrző funkció (formátum jó-e)
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        //Irányítószám bemeneti mező ellenőrző (csak szám lehet)
        private void Tb_Iranyitoszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Iranyitoszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Iranyitoszam.Text = Tb_Iranyitoszam.Text.Remove(Tb_Iranyitoszam.Text.Length - 1);
            }
        }
    }
}
