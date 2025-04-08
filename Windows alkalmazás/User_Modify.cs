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
using static System.Net.Mime.MediaTypeNames;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class User_Modify : Form
    {
        public User_Modify()
        {
            InitializeComponent();
        }

        // Privát változók az adatok tárolására
        private readonly int UserID;
        private readonly string UserName;
        private readonly string UserEmail;
        private readonly string UserGender;
        private readonly int UserGenderID;
        private readonly string UserPhone;
        private readonly string UserTaj;
        private readonly string UserAddress;
        private readonly DateTime UserBirthdate;
        private readonly int UserHasTB;

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Zip = new List<dynamic>();
        private List<dynamic> Gender = new List<dynamic>();

        // Konstruktor az adatok fogadására
        internal User_Modify(dynamic userId, dynamic userName, dynamic userEmail, dynamic userGender, dynamic userGenderId, dynamic userPhone, dynamic userTaj, dynamic userAddress, dynamic userBirthdate, dynamic userHasTB)
        {
            InitializeComponent();
            // Adatok tárolása privát változókban
            this.UserID = userId;
            this.UserName = userName.ToString();
            this.UserEmail = userEmail.ToString();
            this.UserGender = userGender;
            this.UserGenderID = userGenderId;
            this.UserPhone =userPhone;
            this.UserTaj =userTaj;
            this.UserAddress = userAddress;
            this.UserBirthdate = userBirthdate;
            this.UserHasTB = userHasTB;
            GetGender();
            GetZip();
        }
        
        //Késleltetett betöltése
        private async void DelayedLoadServiceData()
        {
            await Task.Delay(200);
            LoadServiceData();
        }

        //Adatok betöltése
        private void LoadServiceData()
        {
            Tb_FelhasznaloNev.Text = UserName;
            Tb_Email.Text = UserEmail;
            Tb_Telefonszam.Text = UserPhone.ToString();
            if(UserTaj != null)
            {
                Tb_Tajszam.Text = UserTaj.ToString();
            }
            
            //Cím feldarabolása
            if (!string.IsNullOrWhiteSpace(UserAddress))
            {
                string[] parts = UserAddress.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 4)
                {
                    Tb_Iranyitoszam.Text = parts[0]; // Irányítószám
                    Tb_Telepules.Text = parts[1]; // Település
                    Tb_Utca.Text = string.Join(" ", parts.Skip(2).Take(parts.Length - 3)); // Utcanév
                    TB_Hazszam.Text = parts.Last(); // Házszám
                }
            }
            //Nem (szex) kiválasztása

            if (Gender != null && Gender.Count > 0)
            {
                var selectedGender = Gender.FirstOrDefault(g => g.id == UserGenderID);
                if (selectedGender != null)
                {
                    Cb_Gender.SelectedItem = selectedGender;
                }
                else
                {
                    Debug.WriteLine("Nem található a kiválasztott nem!");
                }
            }

            dTP_Szuldatum.Value = UserBirthdate;
            cB_TB.Checked =Convert.ToBoolean(UserHasTB);
        }

        //Form betöltése
        private void User_Modify_Load(object sender, EventArgs e)
        {
            if (ThemeManager.CurrentTheme == "Dark")
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
            DelayedLoadServiceData();
            Lb_Szuldatum.Text = dTP_Szuldatum.Text;
        }

        //Nemek lekérdezése
        private async void GetGender()
        {
            var data = await FetchData("/api/v1/api.php/data/general/GetGenders");
            if (data == null || data.genders == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Gender = data.genders.ToObject<List<dynamic>>();

            // ComboBox kitisztítása és feltöltése
            Cb_Gender.Items.Clear();
            foreach (var gender in Gender)
            {
                Cb_Gender.Items.Add(gender);
            }
            Cb_Gender.DisplayMember = "name";
            Cb_Gender.ValueMember = "id";
            
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

        //Mégse gomb (click event)
        private void ccButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Születésnap label megnyomása (click event)
        private void Lb_Szuldatum_Click_1(object sender, EventArgs e)
        {
            dTP_Szuldatum.Select();
            SendKeys.Send("%{DOWN}");
        }

        //Születésnap DateTimePicker értékének változtatása
        private void dTP_Szuldatum_ValueChanged(object sender, EventArgs e)
        {
            Lb_Szuldatum.Text = dTP_Szuldatum.Text;
        }

        //Módosítás gomb (click event)
        private async void Btn_FelhasznaloModositas_Click(object sender, EventArgs e)
        {
            TextBox[] textboxok = { Tb_FelhasznaloNev, Tb_Telefonszam, Tb_Iranyitoszam, Tb_Telepules, Tb_Utca, TB_Hazszam };

            if (!textboxok.All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Valamelyik mező nincs kitöltve!");
                return;
            }

            if (!ZipGood())
            {
                MessageBox.Show("Az irányítószám vagy a település neve nem megfelelő!");
                return;
            }

            if (dTP_Szuldatum.Value.Date > DateTime.Today.AddYears(-16))
            {
                MessageBox.Show("Az felhasználónak legalább 16 évesnek kell lennie!");
                return;
            }

            string phone = Tb_Telefonszam.Text;
            if (phone.Length < 10 || !phone.StartsWith("06"))
            {
                MessageBox.Show("Kérlek írj be egy érvényes telefonszámot! \n Pl (06203869432)", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Cb_Gender.SelectedIndex == -1)
            {
                MessageBox.Show("Válasszon ki nemet (Gender)!");
                return;
            }

            dynamic selectedGender = Cb_Gender.SelectedItem;
            int genderId = Gender.FirstOrDefault(g => g.name == selectedGender.name)?.id ?? -1;

            if (genderId == -1)
            {
                MessageBox.Show("Ismeretlen nem típus!");
                return;
            }

            string tajszam = Tb_Tajszam.Text.Trim();
            bool hasTB = cB_TB.Checked;

            if (hasTB)
            {
                if (string.IsNullOrEmpty(tajszam))
                {
                    MessageBox.Show("Nincs tajszám kitöltve");
                    return;
                }
                if (!IsValidTajNumber(tajszam) || tajszam == "000000000")
                {
                    MessageBox.Show("Hibás tajszám!");
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tajszam) && !IsValidTajNumber(tajszam))
                {
                    MessageBox.Show("Kérem vagy töltse ki a tajszám mezőt megfelelően, vagy hagyja üresen.");
                    return;
                }
            }

            string name = Tb_FelhasznaloNev.Text;
            string address = $"{Tb_Iranyitoszam.Text} {Tb_Telepules.Text}, {Tb_Utca.Text} {TB_Hazszam.Text}";
            DateTime birthdate = dTP_Szuldatum.Value;

            await ModifyProfile(UserID, name, phone, tajszam, address, genderId, birthdate, hasTB ? 1 : 0);
            this.Close();
        }

        //Telefonszám input ellenőrzése (csak szám)
        private void Tb_Telefonszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Telefonszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Telefonszam.Text = Tb_Telefonszam.Text.Remove(Tb_Telefonszam.Text.Length - 1);
            }
        }

        //Tajszám input ellenőrzése (csak szám)
        private void Tb_Tajszam_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Tajszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Tajszam.Text = Tb_Tajszam.Text.Remove(Tb_Tajszam.Text.Length - 1);
            }
        }

        //Irányítószám input ellenőrzése (csak szám)
        private void Tb_Iranyitoszam_TextChanged_1(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Tb_Iranyitoszam.Text, "[^0-9]"))
            {
                MessageBox.Show("Kérlek csak számokt írjon!");
                Tb_Iranyitoszam.Text = Tb_Iranyitoszam.Text.Remove(Tb_Iranyitoszam.Text.Length - 1);
            }
        }

        //Irányítószám és település kombináció ellenőrzése
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

        //Tajszám ellenőrzése (létezik-e ilyen tajszám matematikailag)
        private bool IsValidTajNumber(string taj)
        {
            if (taj.Length != 9 || !taj.All(char.IsDigit))
                return false;

            int[] weights = { 3, 7, 3, 7, 3, 7, 3, 7 };
            int sum = 0;

            for (int i = 0; i < 8; i++)
            {
                sum += (taj[i] - '0') * weights[i];
            }

            int checkDigit = sum % 10;
            return checkDigit == (taj[8] - '0');
        }

        //Felhasználó módosítása
        private async Task ModifyProfile(int user_id, string name, string phone, string taj, string address, int gender, DateTime birthdate, int hasTB)
        {
            var data = new { user_id, name, phone, taj, address, gender, birthdate, hasTB };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/users/UpdateUser", data);
        }
    }
}
