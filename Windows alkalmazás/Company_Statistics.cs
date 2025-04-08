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
    public partial class Company_Statistics : Form
    {
        public Company_Statistics()
        {
            InitializeComponent();
        }
        public Button jelenlegiStatisztika;

        //Fileterek a Doctor_Statisztika form-ra
        private string currentFilter = "company";
        private int? selectedId = GlobalData.CompanyId;

        //Formok betöltése
        private void Ceg_Statistics_Load(object sender, EventArgs e)
        {
            var buttons = new[] { Btn_CegValaszt, Btn_KorhazValaszt, Btn_OrvosValaszt };
            ThemeManager.RegisterButtonGroup(this, buttons, Btn_CegValaszt);
            CBNemLathato();
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Doctors = new List<dynamic>();
        private List<dynamic> Hospitals = new List<dynamic>();

        //A ComboBox láthatósága
        private void CBNemLathato()
        {
            CB_Valaszto.Visible = false;
        }


        public Panel pn()
        {
            return Pn_CegPnFo;
        }
        //Form megnyitása Panelben funkció (Doctor_Statistics)
        private void FormMegnyitasPanelben<T>() where T : Form, new()
        {

            Panel pnFo = pn();
            T formInstance = new T();

            // If the form is Doctor_Statistics, set the filter
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


        #region  Click eventek
        //Orvos kiválasztása
        private void Btn_OrvosValaszt_Click(object sender, EventArgs e)
        {
            
            //GombokBeállítása(sender);
            CB_Valaszto.Visible = true;
            currentFilter = "doctor";
            GetDoctor();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && Doctors.Count > 0)
            {
                selectedId = (int)Doctors[0].doctor_id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }

        //Kórház kiválasztása
        private void Btn_KorhazValaszt_Click(object sender, EventArgs e)
        {
            //GombokBeállítása(sender);
            CB_Valaszto.Visible = true;
            currentFilter = "hospital";
            GetHospitals();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && Hospitals.Count > 0)
            {
                selectedId = (int)Hospitals[0].hopital_id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }

        //Cég kiválasztása
        private void Btn_CegValaszt_Click(object sender, EventArgs e)
        {
            //GombokBeállítása(sender);
            CBNemLathato();
            currentFilter = "company";
            selectedId = GlobalData.CompanyId;
            FormMegnyitasPanelben<Doctor_Statistics>();
            //Beállítás a küldésere Doctor_Statistics-ra
        }

        //ComboBox kiválasztás 
        private void CB_Valaszto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Valaszto.SelectedIndex >= 0)
            {
                if (currentFilter == "doctor" && Doctors.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)Doctors[CB_Valaszto.SelectedIndex].doctor_id;
                }
                else if (currentFilter == "hospital" && Hospitals.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)Hospitals[CB_Valaszto.SelectedIndex].id;
                }
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }
        #endregion

        //Orvosok lekérdezés
        private async void GetDoctor()
        {
            CB_Valaszto.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/companies/GetDoctors?company_id={GlobalData.CompanyId}");
            if (data == null || data.company_doctors == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            
            Doctors = data.company_doctors.ToObject<List<dynamic>>();
            foreach (var doctor in Doctors)
            {
                CB_Valaszto.Items.Add(doctor.doctor_name.ToString());
            }
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;

            }
        }

        //Kórház lekérdezések
        private async void GetHospitals()
        {
            CB_Valaszto.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/company/hospitals?company_id={GlobalData.CompanyId}");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            
            #region Combo Box feltöltése
            // Educations DataGridView
            Hospitals = data.hospitals.ToObject<List<dynamic>>();
            foreach (var hospital in Hospitals)
            {
                CB_Valaszto.Items.Add(hospital.name.ToString());
            }
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;
            }
            #endregion
        }
    }
}
