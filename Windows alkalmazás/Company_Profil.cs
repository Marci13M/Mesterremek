using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
    public partial class Company_Profil : Form
    {
        public Company_Profil()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Company_Profile_Load(object sender, EventArgs e)
        {
            GetCompany();
            GetHospitals();
            GetZip();
            dGV_Telefonszam.CellContentClick += DataGridView_CellContentClick;
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> phoneNumbers = new List<dynamic>();
        private List<dynamic> hospitalList = new List<dynamic>();
        private List<dynamic> Zip = new List<dynamic>();

        //Kép változtatáshoz szükséges változók
        int Image_id;
        string Image_title;
        string Image_alt;

        //Cég profil adatainak lekérdezése
        private async void GetCompany()
        {
            dGV_Telefonszam.Rows.Clear();
            dGV_Telefonszam.ReadOnly = false; // Engedélyezzük a szerkesztést
            dGV_Telefonszam.Columns[0].ReadOnly = true;
            dGV_Telefonszam.Columns[1].ReadOnly = false;
            dGV_Telefonszam.Columns[2].ReadOnly = false;

            var data = await FetchData($"/api/v1/api.php/companies/GetCompany?company_id={GlobalData.CompanyId}");
            if (data == null || data.company == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Textboxok feltöltése
            // Általános információk
            Lb_CegNev.Text = data.company.company_name;
            Tb_Cegjegyzekszam.Text = data.company.company_register_number;
            Tb_Adoszam.Text = data.company.company_tax_number;
            Tb_Email.Text = data.company.company_email;
            Tb_IgazgatoNev.Text = data.company.company_director_name;
            #endregion

            #region Logó beállítása
            if (data.company.images != null &&
                data.company.images.logo.image != null &&
                !string.IsNullOrEmpty(data.company.images.logo.image.ToString()))
            {
                var Profilkep = data.company.images.logo.image.ToString();
                Image_id = data.company.images.logo.image_id;
                Image_title = data.company.images.logo.image_title;
                Image_alt = data.company.images.logo.image_alt;
                if (Profilkep.Contains(","))
                {
                    Profilkep = Profilkep.Split(',')[1]; // Base64 adat kinyerése
                }

                try
                {
                    var imageBytes1 = Convert.FromBase64String(Profilkep);
                    using (MemoryStream ms = new MemoryStream(imageBytes1))
                    {
                        PB_KorhazProfil.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    PB_KorhazProfil.Image = null; // Ha hiba van, hagyja üresen
                }
            }
            else
            {
                PB_KorhazProfil.Image = null;
            }
            #endregion

            // Cég címe feldolgozása
            #region Cég címe feldolgozása
            string companyAddress = data.company.company_address.ToString();
            if (!string.IsNullOrEmpty(companyAddress))
            {
                // Reguláris kifejezés a cím feldolgozására
                string pattern = @"(?<iranyitoszam>.+) (?<telepules>.+), (?<utca>.+) (?<hazszam>\d+)";
                Match match = Regex.Match(companyAddress, pattern);

                if (match.Success)
                {
                    string iranyitoszam = match.Groups["iranyitoszam"].Value;
                    string telepules = match.Groups["telepules"].Value;
                    string utca = match.Groups["utca"].Value;
                    string hazszam = match.Groups["hazszam"].Value;

                    // Az adatok megjelenítése a megfelelő vezérlőkben
                    Tb_Iranyitoszam.Text = iranyitoszam;
                    Tb_Telepules.Text = telepules;
                    Tb_Utca.Text = utca;
                    Tb_Hazszam.Text = hazszam;
                }
                else
                {
                    MessageBox.Show("Nem sikerült feldolgozni a címet.", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("A cég címe üres.", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            #endregion

            #region Kepek beállítása
            Image_title = $"Kép {data.company.company_name} profilkép";
            Image_alt = $"{data.company.company_name} profilkep";
            if (data.company.images.logo != null &&
                data.company.images.logo.image != null &&
                !string.IsNullOrEmpty(data.company.images.logo.image.ToString()))
            {
                var CegKep = data.company.images.logo.image.ToString();
                Image_id = data.company.images.logo.image_id;
                Image_title = data.company.images.logo.image_title;
                Image_alt = data.company.images.logo.image_alt;
                if (CegKep.Contains(","))
                {
                    CegKep = CegKep.Split(',')[1]; // Base64 adat kinyerése
                }
                try
                {
                    var imageBytes1 = Convert.FromBase64String(CegKep);
                    using (MemoryStream ms = new MemoryStream(imageBytes1))
                    {
                        // Közvetlenül a MemoryStream-ből töltjük be, nem konvertáljuk Bitmap-be
                        ms.Position = 0; // Visszaállítjuk a stream pozícióját az elejére
                                         // Klónozzuk a stream-et, hogy az eredeti bezárása után is használható legyen
                        MemoryStream persistentStream = new MemoryStream();
                        ms.CopyTo(persistentStream);
                        persistentStream.Position = 0;

                        // Az Image.FromStream metódus második paramétere fontos - ezt true-ra állítva 
                        // az eredeti stream adatait használja és nem másolja azokat
                        PB_KorhazProfil.Image = Image.FromStream(persistentStream, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a kép betöltése során: " + ex.Message);
                    PB_KorhazProfil.Image = null;
                }
            }
            else
            {
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

                // Kép teljes elérési útja a projekt mappájában
                string replacementImagePath = Path.Combine(projectDirectory, "Images", "Replacement", "hospital_placeholder.png");
                if (File.Exists(replacementImagePath))
                {
                    PB_KorhazProfil.Image = Image.FromFile(replacementImagePath);
                }
                else
                {
                    Debug.WriteLine("A helyettesítő kép nem található: " + replacementImagePath);
                    PB_KorhazProfil.Image = null;
                }
            }

            #endregion

            // Phonenumbers DataGridView
            phoneNumbers = data.company.phonenumbers.ToObject<List<dynamic>>();
            foreach (var phone in phoneNumbers)
            {
                int rowIndex = dGV_Telefonszam.Rows.Add(phone.phone_number.ToString(), phone.@public == 1);
                dGV_Telefonszam.Rows[rowIndex].Cells[dGV_Telefonszam.Columns.Count - 1].Value = "Törlés";
            }

        }

        //Kórházak lekérdezése
        private async void GetHospitals()
        {
            dGV_CegKorhazak.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/companies/GetHospitals?company_id={GlobalData.CompanyId}");
            if (data == null || data.company_hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            hospitalList = data.company_hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in hospitalList)
            {
                dGV_CegKorhazak.Rows.Add(hospital.hospital_name, hospital.hospital_address);
            }
            
        }

        //Irányítószám és települések lekérdezése
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
                    case "TelefonTörlés":
                        var phoneNumber = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {phoneNumber} telefonszámot?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id);
                            int company_id = GlobalData.CompanyId;

                            await DeleteCompanyPhone(company_id, phonenumber_id);

                            MessageBox.Show("Telefonszám sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetCompany();
                        }
                        break;
                    case "Látható":
                        try
                        {
                            int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id); 
                            int company_id = GlobalData.CompanyId; 
                            await ChangePhonenumberActiveState(company_id, phonenumber_id);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Telefonszám aktív státuszának módosítása
        private async Task ChangePhonenumberActiveState(int company_id, int phonenumber_id)
        {
            var data = new { company_id, phonenumber_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/ChangePhonenumberActiveState", data);
        }

        //Telefonszám törlése
        private async Task DeleteCompanyPhone(int company_id, int phonenumber_id)
        {
            var data = new { phonenumber_id, company_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/companies/DeletePhonenumber", data);
        }

        //Telefonszám hozzáadása click event
        private async void Btn_TelefonszamHozzaadas_Click(object sender, EventArgs e)
        {
            if (Tb_Telefonszam.Text.Length >= 10 && Tb_Telefonszam.Text.Substring(0, 2) == "06")
            {

                string phonenumber = Tb_Telefonszam.Text;
                int comapany_id = GlobalData.CompanyId;

                // Api meghívása
                await AddCompanyPhonenumber(comapany_id, phonenumber);
                GetCompany();
                Tb_Telefonszam.Clear();
            }
            else
            {
                MessageBox.Show("Kérlek  írj be egy érvényes telefonszámot! \n Pl (06203869432)", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Telefonszám hozzáadása
        private async Task AddCompanyPhonenumber(int company_id, string phonenumber)
        {
            var data = new { company_id, phonenumber };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/AddPhonenumber", data);
        }

        //Telefonszám bemeneti adatainak ellenőrzáse (csak szám)
        private void Tb_Telefonszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Telefonszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Telefonszam.Text = Tb_Telefonszam.Text.Remove(Tb_Telefonszam.Text.Length - 1);
            }
        }


        //Módosítás gomb (event)
        #region Adatok módosítása
        private async void Btn_ModositasMentes_Click(object sender, EventArgs e)
        {
            TextBox[] textboxok = { Tb_Cegjegyzekszam, Tb_Adoszam, Tb_Email, Tb_Iranyitoszam,
                        Tb_IgazgatoNev, Tb_Telepules, Tb_Hazszam,
                        Tb_Utca };
            //Ellenőrzés hogy mindegyik mező ki van-e töltve
            if (textboxok.All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                if (ZipGood() == true)
                {
                    string cegjegyzekszam = Tb_Cegjegyzekszam.Text;
                    string cegpattern = @"^^(0[1-9]|1[0-9]|20)-(0[1-9]|1[0-9]|2[0-3])-\d{6}$";
                    if (Regex.IsMatch(cegjegyzekszam, cegpattern))
                    {
                        //Adószám ellenőrzés
                        string adoszam = Tb_Adoszam.Text;
                        string adopattern = @"^\d{8}-[1-5]-((0[2-9]|1[0-9]|20)|(22|2[3-9]|3[0-9]|4[0-4])|51)$";
                        if (Regex.IsMatch(adoszam, adopattern))
                        {
                            if (IsGoodEmail(Tb_Email.Text.ToString()))
                            {
                                if (IsValidTaxNumber(adoszam))
                                {
                                    int company_id = GlobalData.CompanyId;
                                    string address = $"{Tb_Iranyitoszam.Text} {Tb_Telepules.Text}, {Tb_Utca.Text} {Tb_Hazszam.Text}";
                                    string registry_number = Tb_Cegjegyzekszam.Text;
                                    string tax_number = Tb_Adoszam.Text;
                                    string director_name = Tb_IgazgatoNev.Text;
                                    string email = Tb_Email.Text;
                                    await UpdateProfile(company_id, address, registry_number, tax_number, email, director_name);
                                    GetCompany();
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


        //Email ellenőrzés
        private bool IsGoodEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        //Irányítószám ellenőrzés
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

        //Adószám ellenőrzése
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

        //Profil módosítása (a gomb ezt hívja meg)
        private async Task UpdateProfile(int company_id, string address, string company_register_number, string tax_number, string email, string director_name)
        {
            var data = new { company_id, address, company_register_number, tax_number, email, director_name };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/UpdateProfile", data);
        }

        #endregion


        //inputok ellenőrzése
        #region inputok ellenőrzése

        //Cégjegyékszám bemeneti ellenőrzés és formázás
        private void Tb_Cegjegyzekszam_TextChanged(object sender, EventArgs e)
        {
            // Mentjük az eredeti kurzorpozíciót
            int cursorPosition = Tb_Cegjegyzekszam.SelectionStart;

            // Eltávolítjuk az összes kötőjelet, hogy tiszta szöveggel dolgozzunk
            string text = Tb_Cegjegyzekszam.Text.Replace("-", "").ToUpper();

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
            if (Tb_Cegjegyzekszam.Text != formattedText)
            {
                Tb_Cegjegyzekszam.Text = formattedText;

                // Visszaállítjuk a kurzor pozícióját figyelembe véve a kötőjeleket
                Tb_Cegjegyzekszam.SelectionStart = cursorPosition + dashCountBeforeCursor;
            }
        }

        //Adószám bemeneti ellenőrzés és formázás
        private void Tb_Adoszam_TextChanged(object sender, EventArgs e)
        {
            // Mentjük az eredeti kurzorpozíciót
            int cursorPosition = Tb_Adoszam.SelectionStart;

            // Eltávolítjuk az összes kötőjelet, hogy tiszta szöveggel dolgozzunk
            string text = Tb_Adoszam.Text.Replace("-", "").ToUpper();

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
            if (Tb_Adoszam.Text != formattedText)
            {
                Tb_Adoszam.Text = formattedText;

                // Visszaállítjuk a kurzor pozícióját figyelembe véve a kötőjeleket
                Tb_Adoszam.SelectionStart = cursorPosition + dashCountBeforeCursor;
            }
        }
        #endregion

        //Cég profilkép módosítása
        private async void Btn_KorhazKepModositasa_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Válassz egy képet";
                openFileDialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string image = ConvertImageToBase64(openFileDialog.FileName);
                    MessageBox.Show("Base64 kód:\n" + image.Substring(0, 50) + "...", "Sikeres konvertálás");
                    int company_id = GlobalData.CompanyId;
                    int old_image_id = Image_id;
                    string image_title = Image_title;
                    string image_alt = Image_alt;
                    await UpdatePhoto(company_id, old_image_id, image, image_title, image_alt);
                    GetCompany();
                }
            }
        }

        //Kiválasztott kép Base64 kódolásuvá konvertálása
        private string ConvertImageToBase64(string imagePath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                // Kép átméretezése, hogy kisebb legyen
                var resizedImage = new Bitmap(image, new Size(1200, 1200)); // Igény szerint módosítható

                using (var ms = new MemoryStream())
                {
                    resizedImage.Save(ms, ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    string mimeType = GetMimeType(imagePath); // MIME-típus meghatározása

                    return $"data:{mimeType};base64,{base64String}";
                }
            }
        }

        //Kép file formátumának megállapítása
        private string GetMimeType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            Dictionary<string, string> mimeTypes = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".bmp", "image/bmp" },
                { ".gif", "image/gif" }
            };

            return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
        }

        //Logó módosítása
        private async Task UpdatePhoto(int company_id, int old_image_id, string image, string image_title, string image_alt)
        {
            var data = new { company_id, old_image_id, image, image_title, image_alt };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/UpdateLogo", data);
        }

        //Irányítószáma bemeneti ellenőrzés (csak szám)
        private void Tb_Iranyitoszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Iranyitoszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokat írjon!");
                Tb_Iranyitoszam.Text = Tb_Iranyitoszam.Text.Remove(Tb_Iranyitoszam.Text.Length - 1);
            }
        }
    }
}
