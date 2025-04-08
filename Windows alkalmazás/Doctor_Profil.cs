using Newtonsoft.Json.Linq;
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
using static CareCompass.Bejelentkezés;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Doctor_Profil : Form
    {
        public Doctor_Profil()
        {
            InitializeComponent();
        }

        // Adatok tárolására
        private List<dynamic> phoneNumbers = new List<dynamic>();
        private List<dynamic> educations = new List<dynamic>();
        private List<dynamic> services = new List<dynamic>();
        private List<dynamic> languages = new List<dynamic>();

        private List<dynamic> CBLanguages = new List<dynamic>();
        private List<dynamic> CBServices = new List<dynamic>();
        private List<dynamic> CBEducation = new List<dynamic>();

        //Kép módosításához szükséges változók
        int Image_id;
        string Image_title;
        string Image_alt;

        //Form betöltése
        private void Doctor_Profil_Load(object sender, EventArgs e)
        {
            GetDoctor();
            GetLanguage();
            GetServices();
            GetEducation();
            GetDoctorServices();

            // DataGridView eseménykezelők
            dGV_Nyelv.CellContentClick += DataGridView_CellContentClick;
            dGV_Telefonszam.CellContentClick += DataGridView_CellContentClick;
            dGV_Szekterulet.CellContentClick += DataGridView_CellContentClick;
            dGV_Oktatas.CellContentClick += DataGridView_CellContentClick;
        }

        //Az orvos adatainak betöltése
        private async void GetDoctor()
        {
            dGV_Nyelv.Rows.Clear();
            dGV_Telefonszam.Rows.Clear();
            dGV_Oktatas.Rows.Clear();
            // Alapértelmezett beállítások
            dGV_Telefonszam.ReadOnly = false; // Engedélyezzük a szerkesztést
            dGV_Telefonszam.Columns[0].ReadOnly = true;
            dGV_Telefonszam.Columns[1].ReadOnly = false; // Checkbox szerkeszthető

            var data = await FetchData($"/api/v1/api.php/doctors/GetDoctor?doctor_id={GlobalData.UserId}");
            if (data == null || data.doctor == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }

            #region Textboxok feltöltése
            // Általános információk
            Lb_OrvosNev.Text = data.doctor.doctor_name;
            Lb_Szuldatum.Text = data.doctor.doctor_birthdate;
            dTP_Szuldatum.Value = data.doctor.doctor_birthdate;
            TB_Bemutatkozas.Text = data.doctor.doctor_introduction;
            CB_SzuldatumLathato.Checked = data.doctor.doctor_birthdate_visible;

            #endregion

            #region Kép beállítása

            Image_title = $"Kép {data.doctor.doctor_name}";
            Image_alt = data.doctor.doctor_name;
            if (data.doctor.profile_image != null &&
                data.doctor.profile_image.image != null &&
                !string.IsNullOrEmpty(data.doctor.profile_image.image.ToString()))
                {
                    Image_id = data.doctor.profile_image.id;
                    var Profilkep = data.doctor.profile_image.image.ToString();
                    if (Profilkep.Contains(","))
                    {
                        Profilkep = Profilkep.Split(',')[1]; // Base64 adat kinyerése
                    }
                    try
                    {
                        var imageBytes1 = Convert.FromBase64String(Profilkep);
                        using (MemoryStream ms = new MemoryStream(imageBytes1))
                        {
                            // Közvetlenül a MemoryStream-ből töltjük be, nem konvertáljuk Bitmap-be
                            ms.Position = 0; // Visszaállítjuk a stream pozícióját az elejére
                            MemoryStream persistentStream = new MemoryStream();
                            ms.CopyTo(persistentStream);
                            persistentStream.Position = 0;
                            CPb_OrvosKep.Image = Image.FromStream(persistentStream, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Hiba a kép betöltése során: " + ex.Message);
                    
                        CPb_OrvosKep.Image = null;
                    }
                }
            else
            {
                // Projekt főkönyvtárának meghatározása (bin mappa nélkül)
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                // Kép teljes elérési útja a projekt mappájában
                string replacementImagePath = Path.Combine(projectDirectory, "Images", "Replacement", "doctor_placeholder.png");
                if (File.Exists(replacementImagePath))
                {
                    CPb_OrvosKep.Image = Image.FromFile(replacementImagePath);
                }
                else
                {
                    Debug.WriteLine("A helyettesítő kép nem található: " + replacementImagePath);
                    CPb_OrvosKep.Image = null;
                }
            }
            #endregion

            #region DataGridView-ek feltöltése
            // Educations DataGridView
            educations = data.doctor.educations.ToObject<List<dynamic>>();
            foreach (var education in educations)
            {
                int rowIndex = dGV_Oktatas.Rows.Add(education.name.ToString());
                dGV_Oktatas.Rows[rowIndex].Cells[dGV_Oktatas.Columns.Count - 1].Value = "Törlés";
            }

            // Phonenumbers DataGridView
            phoneNumbers = data.doctor.phonenumbers.ToObject<List<dynamic>>();
            foreach (var phone in phoneNumbers)
            {
                int rowIndex = dGV_Telefonszam.Rows.Add(phone.phone_number.ToString(), phone.@public == 1);
                dGV_Telefonszam.Rows[rowIndex].Cells[dGV_Telefonszam.Columns.Count - 1].Value = "Törlés";
            }

            // Languages DataGridView
            languages = data.doctor.languages.ToObject<List<dynamic>>();
            foreach (var language in languages)
            {
                // Új sor hozzáadása, és az indexének lekérése
                int rowIndex = dGV_Nyelv.Rows.Add(language.name.ToString());

                // Beállítjuk a gomb szövegét az utolsó oszlopban
                dGV_Nyelv.Rows[rowIndex].Cells[dGV_Nyelv.Columns.Count - 1].Value = "Törlés";
            }
            #endregion
            
        }

        //Orvos szoltáltatások
        private async void GetDoctorServices()
        {
            dGV_Szekterulet.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/doctors/GetServices?doctor_id={GlobalData.UserId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            // services DataGridView
            services = data.services.ToObject<List<dynamic>>();
            foreach (var service in services)
            {
                int rowIndex = dGV_Szekterulet.Rows.Add(service.name.ToString());
                dGV_Szekterulet.Rows[rowIndex].Cells[dGV_Szekterulet.Columns.Count - 1].Value = "Törlés";
            }
        }


        #region Combo box-ok feltöltése
        //Nyelvek lekérdezése (még nem kiválasztottak)
        private async void GetLanguage()
        {
            cB_Nyelv.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/doctor/languages?doctor_id={GlobalData.UserId}");
            if (data == null || data.languages == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            CBLanguages = data.languages.ToObject<List<dynamic>>();
            foreach (var language in CBLanguages)
            {
                cB_Nyelv.Items.Add(language.language.ToString());
            }
            #endregion
        }

        //Szolgáltatások lekérdezése (még nem kiválasztottak)
        private async void GetServices()
        {
            Cb_Szakterulet.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/doctor/services?doctor_id={GlobalData.UserId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            CBServices = data.services.ToObject<List<dynamic>>();
            foreach (var service in CBServices)
            {
                Cb_Szakterulet.Items.Add(service.name.ToString());
            }
            #endregion
        }

        //Végzettségek lekérdezése (még nem kiválasztottak)
        private async void GetEducation()
        {
            Cb_Oktatas.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/doctor/educations?doctor_id={GlobalData.UserId}");
            if (data == null || data.educations == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            CBEducation = data.educations.ToObject<List<dynamic>>();
            foreach (var education in CBEducation)
            {
                Cb_Oktatas.Items.Add(education.name.ToString());
            }
            #endregion
           
        }
        #endregion

        //Szüldátum megváltoztatása (a labelen keresztül érjük el a datetimepickert)
        private void Lb_Szuldatum_Click(object sender, EventArgs e)
        {
            dTP_Szuldatum.Select();
            SendKeys.Send("%{DOWN}");
            Debug.WriteLine(educations.ToString());
        }

        //Születésnap megváltoztatása
        private void dTP_Szuldatum_ValueChanged(object sender, EventArgs e)
        {
            Lb_Szuldatum.Text = dTP_Szuldatum.Value.ToString("yyyy.MM.dd");
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
                    case "VegzettsegTörlés":
                        var educationName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteEducation = MessageBox.Show($"Biztosan törlöd a(z) {educationName} oktatást?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteEducation == DialogResult.Yes)
                        {
                            try
                            {
                                int education_id = Convert.ToInt32(educations[e.RowIndex].id);
                                int doctor_id = GlobalData.UserId;

                                await DeleteDoctorEducation(doctor_id, education_id);

                                GetDoctor();
                                GetEducation();
                                MessageBox.Show("Végzettség sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "TelefonTörlés":
                        // Telefonszám törlése logika
                        var phoneNumber = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {phoneNumber} telefonszámot?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            try
                            {
                                int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id);
                                int doctor_id = GlobalData.UserId;

                                await DeleteDoctorPhone(doctor_id, phonenumber_id);

                                GetDoctor();
                                MessageBox.Show("Telefonszám sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "Törlés":
                        // Nyelv törlése logika
                        var languageName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteLanguage = MessageBox.Show($"Biztosan törlöd a(z) {languageName} nyelvet?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteLanguage == DialogResult.Yes)
                        {
                            try
                            {
                                int language_id = Convert.ToInt32(languages[e.RowIndex].id);
                                int doctor_id = GlobalData.UserId;

                                await DeleteDoctorLanguage(doctor_id, language_id);
                                GetDoctor();
                                GetLanguage();
                                MessageBox.Show("Nyelv sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        break;

                    case "SzakTörlés":
                        // Szakma törlése logika
                        var szakName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteSzak = MessageBox.Show($"Biztosan törlöd a(z) {szakName} szakterületet?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteSzak == DialogResult.Yes)
                        {
                            try
                            {
                                int service_id = Convert.ToInt32(services[e.RowIndex].id);
                                int doctor_id = GlobalData.UserId;

                                await DeleteDoctorService(doctor_id, service_id);
                                GetDoctorServices();
                                GetServices();
                                MessageBox.Show("Szakterület sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            //DeleteDoctorLanguage(id); // API hívás a törléshez
                            GetDoctor();
                        }
                        break;

                    case "Látható":
                        // Szakma törlése logika
                        try
                        {
                            int phonenumber_id = Convert.ToInt32(phoneNumbers[e.RowIndex].id); // Get language_id
                            int doctor_id = GlobalData.UserId; // Implement this function to retrieve the doctor's ID

                            // Call the separate function for deleting the language
                            await ChangePhonenumberActiveState(doctor_id, phonenumber_id);

                            //MessageBox.Show("Szakterület sikeresen törölve.", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //DeleteDoctorLanguage(id); // API hívás a törléshez


                        break;


                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        #region Törlések
        //Telefonszám Aktív státuszának változtatása
        private async Task ChangePhonenumberActiveState(int doctor_id, int phonenumber_id)
        {
            var data = new { doctor_id, phonenumber_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/ChangePhonenumberActiveState", data);
        }

        //Orvos nyelvének törlése
        private async Task DeleteDoctorLanguage(int doctor_id, int language_id)
        {
            var data = new { doctor_id, language_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/doctors/DeleteLanguage", data);
        }

        //Tanulmányok/végzettségek törlése
        private async Task DeleteDoctorEducation(int doctor_id, int education_id)
        {
            var data = new { doctor_id, education_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/doctors/DeleteEducation", data);
        }

        //Szakterületek törlése
        private async Task DeleteDoctorService(int doctor_id, int service_id)
        {
            var data = new { doctor_id, service_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/doctors/DeleteService", data);
        }

        //Telefonszám törlése
        private async Task DeleteDoctorPhone(int doctor_id, int phonenumber_id)
        {
            var data = new { doctor_id, phonenumber_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/doctors/DeletePhone", data);
        }
        #endregion
       
        //Hozzáadások click eventjei
        #region Hozzáadások click eventjei
        //Nyelv hozzáadása
        private async void Btn_NyelvHozzaadas_Click(object sender, EventArgs e)
        {
            if (cB_Nyelv.SelectedIndex != -1)
            {
                // 2. Lekéred a kiválasztott nyelvet (szövegesen)
                string selectedLanguageName = cB_Nyelv.SelectedItem.ToString();

                // 3. Megkeresed a CBLanguages listában a kiválasztott nyelvhez tartozó id-t
                foreach (var language in CBLanguages)
                {
                    if (language.language.ToString() == selectedLanguageName)
                    {
                        int language_id = Convert.ToInt32(language.id);
                        int doctor_id = GlobalData.UserId;

                        // Api meghívása
                        await AddDoctorLanguage(doctor_id, language_id);
                        GetDoctor();
                        GetLanguage();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérlek válassz nyelvet a listából!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Szakterület hozzáadása
        private async void Btn_SzakteruletHozzaadas_Click(object sender, EventArgs e)
        {
            if (Cb_Szakterulet.SelectedIndex != -1)
            {
                string selectedServiceName = Cb_Szakterulet.SelectedItem.ToString();

                foreach (var service in CBServices)
                {
                    if (service.name.ToString() == selectedServiceName)
                    {
                        int service_id = Convert.ToInt32(service.id);
                        int doctor_id = GlobalData.UserId;

                        // Api meghívása
                        await AddDoctorService(doctor_id, service_id);
                        GetDoctor();
                        GetServices();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérlek válassz nyelvet a listából!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Oktatás hozzáadása
        private async void Btn_OktatasHozzaadas_Click(object sender, EventArgs e)
        {
            if (Cb_Oktatas.SelectedIndex != -1)
            {
                string selectededucationName = Cb_Oktatas.SelectedItem.ToString();
                foreach (var education in CBEducation)
                {
                    if (education.name.ToString() == selectededucationName)
                    {
                        int education_id = Convert.ToInt32(education.id);
                        int doctor_id = GlobalData.UserId;

                        // Api meghívása
                        await AddDoctorEducation(doctor_id, education_id);
                        GetDoctor();
                        GetEducation();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérlek válassz oktatást a listából!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Telefonszám hozzáadása
        private async void Btn_TelefonszamHozzaadas_Click(object sender, EventArgs e)
        {
            if (Tb_Telefonszam.Text.Length >= 10 && Tb_Telefonszam.Text.Substring(0, 2) == "06")
            {

                string phonenumber = Tb_Telefonszam.Text;
                int doctor_id = GlobalData.UserId;

                // Api meghívása
                await AddDoctorPhonenumber(doctor_id, phonenumber);
                GetDoctor();
                Tb_Telefonszam.Clear();
            }
            else
            {
                MessageBox.Show("Kérlek  írj be egy érvényes telefonszámot! \n Pl (06203869432)", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        //Add/Hozzáadás metódusok
        #region Add/Hozzáadás metódusok
        //Nyelv hozzáadása
        private async Task AddDoctorLanguage(int doctor_id, int language_id)
        {
            var data = new { doctor_id, language_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/AddLanguage", data);
        }

        //Szolgáltatás hozzáadása
        private async Task AddDoctorService(int doctor_id, int service_id)
        {
            var data = new { doctor_id, service_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/AddService", data);
        }

        //Végzettség hozzáadása
        private async Task AddDoctorEducation(int doctor_id, int education_id)
        {
            var data = new { doctor_id, education_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/AddEducation", data);
        }

        //Telefonszám hozzáadása
        private async Task AddDoctorPhonenumber(int doctor_id, string phonenumber)
        {
            var data = new { doctor_id, phonenumber };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/AddPhonenumber", data);
        }
        #endregion

        //Telefonszám input ellenőrzése (csak szám)
        private void Tb_Telefonszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Telefonszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérem csak számokat írjon!");
                Tb_Telefonszam.Text = Tb_Telefonszam.Text.Remove(Tb_Telefonszam.Text.Length - 1);
            }
        }

        //Módosítás gomb click event
        private async void Btn_ModositasMentes_Click(object sender, EventArgs e)
        {
            if(dTP_Szuldatum.Value.Date <= DateTime.Today.AddYears(-18))
            {
                int doctor_id = GlobalData.UserId;
                string introduction = TB_Bemutatkozas.Text;
                DateTime birthdateraw = dTP_Szuldatum.Value;
                string birthdate = birthdateraw.ToString("yyyy-MM-dd");
                bool birthdate_visible = CB_SzuldatumLathato.Checked;
                await UpdateProfile(doctor_id, introduction, birthdate, birthdate_visible);
                GetDoctor();
            }
            else
            {
                MessageBox.Show("A módosítás nem lehetséges! Az életkornak legalább 18-nak kell hogy legyen!");
            }
        }

        //Profil módosítása
        private async Task UpdateProfile(int doctor_id, string introduction, string birthdate, bool birthdate_visible)
        {
            var data=new {doctor_id, introduction, birthdate, birthdate_visible};
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/UpdateProfile", data);
        }

        //Profilkép módosítása (click event)
        private async void Btn_KepModositasa_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Válassz egy képet";
                openFileDialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string image = ConvertImageToBase64(openFileDialog.FileName);
                    int doctor_id = GlobalData.UserId;
                    int old_image_id = Image_id;
                    string image_title = Image_title;
                    string image_alt=Image_alt;
                    await UpdatePhoto(doctor_id, old_image_id, image, image_title, image_alt);
                    GetDoctor();
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

        //A kép file típusának megállapítása
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

        //Kép mósoítása
        private async Task UpdatePhoto(int doctor_id, int old_image_id, string image, string image_title, string image_alt)
        {
            var data = new { doctor_id, old_image_id, image, image_title, image_alt };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/doctors/UpdateProfileImage", data);
        }

    }
}
