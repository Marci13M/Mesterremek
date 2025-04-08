using CareCompass.Customs;
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
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Admin_Statistics : Form
    {
        public Admin_Statistics()
        {
            InitializeComponent();
        }

        public Button jelenlegiStatisztika; //Kiválásztot gomb értéke

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Doctor = new List<dynamic>();
        private List<dynamic> Hospital = new List<dynamic>();
        private List<dynamic> Company = new List<dynamic>();
        private List<dynamic> LotDoctor = new List<dynamic>();

        private string currentFilter = "admin";
        private int? selectedId = GlobalData.UserId;

        //Form betöltése
        private void Admin_Statistics_Load(object sender, EventArgs e)
        {
            //GombokBeállítása(Btn_OsszesValaszt);
            var buttons = new[] { Btn_OsszesValaszt, Btn_CegValaszt, Btn_KorhazValaszt, Btn_OrvosValaszt };
            ThemeManager.RegisterButtonGroup(this, buttons, Btn_OsszesValaszt);
            FormMegnyitasPanelben<Doctor_Statistics>();
            CB_Valaszto.Visible = false;
        }

        public Panel pn()
        {
            return Pn_AdminPnFo;
        }

        //Form megnyitása Panelben funkció
        private void FormMegnyitasPanelben<T>() where T : Form, new()
        {
            Panel pnFo = pn();
            T formInstance = new T();

            // Szűréssel megnyitja a Doctor statisztik formot (itt található az összes statisztika)
            if (formInstance is Doctor_Statistics doctorStats)
            {
                doctorStats.SetFilter(currentFilter, selectedId);
            }

            formInstance.TopLevel = false;
            formInstance.FormBorderStyle = FormBorderStyle.None;
            formInstance.Dock = DockStyle.Fill;
            ThemeManager.ApplyTheme(formInstance);
            pnFo.Controls.Clear();
            pnFo.Controls.Add(formInstance);
            formInstance.BringToFront();
            formInstance.Show();
        }

        #region Gombok beállítása
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            ThemeManager.CleanUpButtonGroups(this);
        }
        #endregion

        #region Click eventek
        //Összes gomb kattintás (össz statisztika)
        private void Btn_OsszesValaszt_Click(object sender, EventArgs e)
        {
            CB_Valaszto.Visible = false;
            currentFilter = "admin";
            selectedId = GlobalData.UserId;
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Cég gomb kiválasztása (cég statisztikák)
        private void Btn_CegValaszt_Click(object sender, EventArgs e)
        {
            //GombokBeállítása(sender);
            CB_Valaszto.Visible = true;
            currentFilter = "company";
            GetCompany();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && Company.Count > 0)
            {
                selectedId = (int)Company[0].id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }

        //Kórház kiválasztása
        private void Btn_KorhazValaszt_Click(object sender, EventArgs e)
        {
            //GombokBeállítása(sender);
            CB_Valaszto.Visible = true;
            currentFilter = "hospital";
            GetHospital();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && Hospital.Count > 0)
            {
                selectedId = (int)Hospital[0].id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }

        //Orvos kiválasztása
        private void Btn_OrvosValaszt_Click(object sender, EventArgs e)
        {
            //GombokBeállítása(sender);
            CB_Valaszto.Visible = true;
            currentFilter = "doctor";
            GetDoctors();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && Doctor.Count > 0)
            {
                selectedId = (int)Doctor[0].id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }
        #endregion

        #region ComboBox-ok feltöltése
        //Orvosok lekérdezése
        private async void GetDoctors()
        {
            CB_Valaszto.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/admin/doctors");

            if (data == null || data.doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }

            LotDoctor = data.doctors.ToObject<List<dynamic>>();

            // Egyedi ID-kat tartalmazó lista létrehozása, csak érvényes nevekkel
            Doctor = LotDoctor
                .Where(d => d.name != null && !string.IsNullOrWhiteSpace(d.name.ToString())) // Csak érvényes nevekkel
                .GroupBy(d => d.id.ToString()) // ID alapján csoportosít
                .Select(g => g.First()) // Az első elemet választja minden csoportból
                .ToList();

            // ComboBox feltöltése
            foreach (var doctor in Doctor)
            {
                CB_Valaszto.Items.Add(doctor.name.ToString());
            }
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;
            }
        }
        //Kórházak lekérdezése
        private async void GetHospital()
        {
            CB_Valaszto.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/admin/hospitals");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            // Educations DataGridView
            Hospital = data.hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in Hospital)
            {
                CB_Valaszto.Items.Add(hospital.name.ToString());
            }
            #endregion
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;
            }

        }
        //Cégek lekérdezése
        private async void GetCompany()
        {
            CB_Valaszto.Items.Clear();
            var data = await FetchData("/api/v1/api.php/data/admin/companies");
            if (data == null || data.companies == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            // Educations DataGridView
            Company = data.companies.ToObject<List<dynamic>>();
            foreach (var company in Company)
            {
                CB_Valaszto.Items.Add(company.name.ToString());
            }
            #endregion
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;
            }
        }
        #endregion

        //Kiválasztott combobox elem
        private void CB_Valaszto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Valaszto.SelectedIndex >= 0)
            {
                if (currentFilter == "doctor" && Doctor.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)Doctor[CB_Valaszto.SelectedIndex].id;
                }
                else if (currentFilter == "hospital" && Hospital.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)Hospital[CB_Valaszto.SelectedIndex].id;
                }
                else if (currentFilter == "company" && Company.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)Company[CB_Valaszto.SelectedIndex].id;
                }
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }
    }
}
