using CareCompass.Customs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    public partial class Hospital_Profil : Form
    {
        public Hospital_Profil()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> phoneNumbers = new List<dynamic>();
        private List<dynamic> nyitvatartas = new List<dynamic>();
        private List<dynamic> Zip = new List<dynamic>();

        //Kép módosításhoz szükséges változók
        int Image_id;
        string Image_title;
        string Image_alt;

        int BoritoImage_id;
        string BoritoImage_title;
        string BoritoImage_alt;

        //Form betöltése
        private void Korhaz_Profil_Load(object sender, EventArgs e)
        {
            GetKorhaz();
            GetZip();
            
            dGV_Telefonszam.CellContentClick += DataGridView_CellContentClick;
        }

        //Kórház profil lekérdezése
        private async void GetKorhaz()
        {
            //Nyitvatartás dataGridView módosítások
            dGV_Nyitvatartas.Rows.Clear();
            dGV_Nyitvatartas.Rows.Add("Nyitás");
            dGV_Nyitvatartas.Rows.Add("Zárás");
            dGV_Nyitvatartas.ReadOnly = false;
            dGV_Nyitvatartas.Columns[0].ReadOnly = true;


            dGV_Telefonszam.ReadOnly = false; // Engedélyezzük a szerkesztést
            dGV_Telefonszam.Columns[0].ReadOnly = true; //Nyitás és Zárás nem szerkeszthető
            
            //Telefonszám dataGridView módosítások
            dGV_Telefonszam.Rows.Clear();
            dGV_Telefonszam.ReadOnly = false; // Engedélyezzük a szerkesztést
            dGV_Telefonszam.Columns[0].ReadOnly = true;
            dGV_Telefonszam.Columns[1].ReadOnly = false; //Módsítható

            var data = await FetchData($"/api/v1/api.php/hospitals/GetHospital?hospital_id={GlobalData.HospitalId}");
            if (data == null || data.hospital == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }

            #region Textboxok feltöltése
            // Általános információk
            TB_Bemutatkozas.Text = data.hospital.hospital_description;
            Lb_KorhazNev.Text = data.hospital.hospital_name;


            // Kórház címe feldolgozása
            string companyAddress = data.hospital.hospital_address.ToString();
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

            BoritoImage_title = $"Kép {data.hospital.hospital_name} borító";
            BoritoImage_alt = $"{data.hospital.hospital_name} borito";
            // Borítókép beállítása
            if (data.hospital.images.cover_image != null &&
                data.hospital.images.cover_image.image != null &&
                !string.IsNullOrEmpty(data.hospital.images.cover_image.image.ToString()))
            {
                var BoritoKep = data.hospital.images.cover_image.image.ToString();
                BoritoImage_id = data.hospital.images.cover_image.image_id;

                if (BoritoKep.Contains(","))
                {
                    BoritoKep = BoritoKep.Split(',')[1]; // Base64 adat kinyerése
                }

                try
                {
                    var imageBytes1 = Convert.FromBase64String(BoritoKep);
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
                        PB_KorhazBorito.Image = Image.FromStream(persistentStream, true);
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
                string replacementImagePath = Path.Combine(projectDirectory, "Images", "Replacement", "hospital_cover_placeholder.jpg");

                if (File.Exists(replacementImagePath))
                {
                    PB_KorhazBorito.Image = Image.FromFile(replacementImagePath);
                }
                else
                {
                    Debug.WriteLine("A helyettesítő kép nem található: " + replacementImagePath);
                    PB_KorhazBorito.Image = null;
                }
                
            }

            // Profilkép beállítása

            Image_title = $"Kép {data.hospital.hospital_name} profilkép";
            Image_alt = $"{data.hospital.hospital_name} profilkep";
            if (data.hospital.images.profile_image != null &&
                data.hospital.images.profile_image.image != null &&
                !string.IsNullOrEmpty(data.hospital.images.profile_image.image.ToString()))
            {
                var KorhazKep = data.hospital.images.profile_image.image.ToString();
                Image_id = data.hospital.images.profile_image.image_id;
                Image_title = data.hospital.images.profile_image.image_title;
                Image_alt = data.hospital.images.profile_image.image_alt;
                if (KorhazKep.Contains(","))
                {
                    KorhazKep = KorhazKep.Split(',')[1]; // Base64 adat kinyerése
                }

                try
                {
                    var imageBytes1 = Convert.FromBase64String(KorhazKep);
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

            #region Datagridview-ek feltöltése
            phoneNumbers = data.hospital.phonenumbers.ToObject<List<dynamic>>();
            foreach (var phone in phoneNumbers)
            {
                int rowIndex = dGV_Telefonszam.Rows.Add(phone.phone_number.ToString(), phone.@public == 1);
                dGV_Telefonszam.Rows[rowIndex].Cells[dGV_Telefonszam.Columns.Count - 1].Value = "Törlés";

            }
            // Nyitvatartás adatainak feldolgozása
            
            string hospitalOpenHours = data.hospital.hospital_open_hours;
            if (hospitalOpenHours != null){
                var nyitvatartasDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(hospitalOpenHours);
                // Nyitás és zárás adatok hozzáadása a DataGridView-hez
                for (int i = 0; i < 7; i++)
                {
                    if (nyitvatartasDict.ContainsKey(i.ToString()))
                    {
                        var times = nyitvatartasDict[i.ToString()];
                        dGV_Nyitvatartas.Rows[0].Cells[i + 1].Value = times[0]; // Nyitás
                        dGV_Nyitvatartas.Rows[1].Cells[i + 1].Value = times[1]; // Zárás
                    }
                }
            }
            #endregion
        }


        //Irányítószám és település lekérdezések
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

        //DataGridView click eventjei
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
                        // Telefonszám törlése logika
                        var phoneNumber = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {phoneNumber} telefonszámot?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            try
                            {
                                int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id);
                                int hospital_id = GlobalData.HospitalId;
                                Debug.WriteLine($"{phonenumber_id}, {hospital_id}");
                                await DeleteHospitalPhone(hospital_id, phonenumber_id);
                                
                                MessageBox.Show("Telefonszám sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                GetKorhaz();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    case "Látható":
                        try
                        {
                            int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id);
                            int hospital_id = GlobalData.HospitalId;

                            await ChangePhonenumberActiveState(hospital_id, phonenumber_id);
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
        private async Task ChangePhonenumberActiveState(int hospital_id, int phonenumber_id)
        {
            var data = new { hospital_id, phonenumber_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/ChangePhonenumberActiveState", data);
        }

        //Telefonszám törlése
        private async Task DeleteHospitalPhone(int hospital_id, int phonenumber_id)
        {
            var data = new { hospital_id, phonenumber_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/hospitals/DeletePhonenumber", data);
        }

        //Telefonszám hozzáadása (click event)
        private async void Btn_TelefonszamHozzaadas_Click(object sender, EventArgs e)
        {
            //input check 
            if (Tb_Telefonszam.Text.Length >= 10 && Tb_Telefonszam.Text.Substring(0, 2) == "06")
            {
                string phonenumber = Tb_Telefonszam.Text;
                int hospital_id = GlobalData.HospitalId;
                // Api meghívása
                await AddHospitalPhonenumber(hospital_id, phonenumber);
                GetKorhaz();
                Tb_Telefonszam.Clear();
            }
            else
            {
                MessageBox.Show("Kérlek  írj be egy érvényes telefonszámot! \n Pl (06203869432)", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Telefonszám hozzáadása
        private async Task AddHospitalPhonenumber(int hospital_id, string phonenumber)
        {
            var data = new { phonenumber, hospital_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/AddPhonenumber", data);
        }

        //telefonszám textbox input ellenőrzése (csak szám)
        private void Tb_Telefonszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Telefonszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Telefonszam.Text = Tb_Telefonszam.Text.Remove(Tb_Telefonszam.Text.Length - 1);
            }
        }

        //Módosítás gomb event
        private async void Btn_ModositasMentes_Click(object sender, EventArgs e)
        {
            if (ZipGood() == true)
            {
                int hospital_id = GlobalData.HospitalId;
                string description = TB_Bemutatkozas.Text;
                string address = $"{Tb_Iranyitoszam.Text} {Tb_Telepules.Text}, {Tb_Utca.Text} {Tb_Hazszam.Text}";

                Dictionary<int, List<string>> nyitvatartasDict = new Dictionary<int, List<string>>();
                Regex oraPercRegex = new Regex(@"^(2[0-4]|1\d|0?\d):[0-5]\d$");

                for (int i = 0; i < 7; i++) // Hét napja (0 - 6)
                {
                    string nyitas = dGV_Nyitvatartas.Rows[0].Cells[i + 1].Value?.ToString().Trim() ?? "";
                    string zaras = dGV_Nyitvatartas.Rows[1].Cells[i + 1].Value?.ToString().Trim() ?? "";

                    // Ha mindkét mező üres, akkor az adott nap zárva van -> nem adjuk hozzá
                    if (string.IsNullOrEmpty(nyitas) && string.IsNullOrEmpty(zaras))
                    {
                        continue;
                    }

                    // Ha a mező tartalmaz értéket, ellenőrizzük a formátumot
                    if ((!string.IsNullOrEmpty(nyitas) && !oraPercRegex.IsMatch(nyitas)) ||
                        (!string.IsNullOrEmpty(zaras) && !oraPercRegex.IsMatch(zaras)))
                    {
                        MessageBox.Show($"Hibás időformátum a(z) {GetDayName(i)} napon! (Pl: 08:00, 23:59)", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    nyitvatartasDict[i] = new List<string> { nyitas, zaras };
                }
                string open_hours = JsonConvert.SerializeObject(nyitvatartasDict, Formatting.None);
                await UpdateProfile(hospital_id,description,address, open_hours);
                MessageBox.Show("Sikeres adatmódosítás!");
            }
            else
            {
                MessageBox.Show("Hibás irányítószám");
            }
        }


        // Segédfüggvény a napok kiírásához
        private string GetDayName(int dayIndex)
        {
            string[] napok = { "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat", "Vasárnap" };
            return napok[dayIndex];
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

        #region Képek módosítás
        //Kórházkép módosítás (click event)
        private async void Btn_KorhazKepModositasa_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Válassz egy képet";
                openFileDialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string image = ConvertImageToBase64(openFileDialog.FileName);
                    int hospital_id = GlobalData.HospitalId;
                    int old_image_id = Image_id;
                    string image_title = Image_title;
                    string image_alt = Image_alt;
                    await UpdatePhoto(hospital_id, old_image_id, image, image_title, image_alt);
                    GetKorhaz();
                }
            }
        }

        //Kép base64-re való konvertálása
        private string ConvertImageToBase64(string imagePath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                // Kép átméretezése, hogy kisebb legyen
                var resizedImage = new Bitmap(image, new Size(800, 600)); // Igény szerint módosítható

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

        //Kép fileformátumának megállapítása
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

        //Kórház borítókép módosítás (click event)
        private async void Btn_BoritoModositasa_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Válassz egy képet";
                openFileDialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string image = ConvertImageToBase64(openFileDialog.FileName);
                    int hospital_id = GlobalData.HospitalId;
                    int old_image_id = BoritoImage_id;
                    string image_title = BoritoImage_title;
                    string image_alt = BoritoImage_alt;
                    await UpdateCoverPhoto(hospital_id, old_image_id, image, image_title, image_alt);
                    GetKorhaz();
                }
            }
        }
        #endregion

        #region Updatek
        //Kórház kép módosítása
        private async Task UpdatePhoto(int hospital_id, int old_image_id, string image, string image_title, string image_alt)
        {
            var data = new { hospital_id, old_image_id, image, image_title, image_alt };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/UpdateProfileImage", data);
        }

        //Kórház borítókép módosítása
        private async Task UpdateCoverPhoto(int hospital_id, int old_image_id, string image, string image_title, string image_alt)
        {
            var data = new { hospital_id, old_image_id, image, image_title, image_alt };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/UpdateCoverImage", data);
        }

        //Profil módosítása 
        private async Task UpdateProfile(int hospital_id, string description, string address, string open_hours)
        {
            var data = new { hospital_id, description, address, open_hours };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/UpdateProfile", data);
        }
        #endregion

        //Telefonszám mező input ellenőrzése (csak szám)
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
