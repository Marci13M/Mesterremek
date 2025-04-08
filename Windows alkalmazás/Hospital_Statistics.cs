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
using System.Windows.Markup;
using static CareCompass.Bejelentkezés;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Hospital_Statistics : Form
    {
        public Hospital_Statistics()
        {
            InitializeComponent();
        }

        //Jelenlegi statisztika változó gombnak
        private Button jelenlegiStatisztika;

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> CBDoctor = new List<dynamic>();

        //Filterek a statisztikákhoz
        private string currentFilter = "hospital";
        private int? selectedId = GlobalData.HospitalId;

        //Form betöltése
        private void Korhaz_Statistics_Load(object sender, EventArgs e)
        {
            var buttons = new[] { Btn_OrvosValaszt, Btn_KorhazValaszt};
            ThemeManager.RegisterButtonGroup(this, buttons, Btn_KorhazValaszt);
            CBNemLathato();
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Combobox láthatósága (nem látható)
        private void CBNemLathato()
        {
            CB_Valaszto.Visible = false;
        }

        //panel beállítása
        public Panel pn()
        {
            return Pn_KorhazPnFo;
        }

        //Form megnyitása panelban
        private void FormMegnyitasPanelben<T>() where T : Form, new()
        {
            Panel pnFo = pn();
            T formInstance = new T();

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

        #region Click eventek/Combo box changed event
        //Kórház gomb kiválasztása
        private void Btn_KorhazValaszt_Click(object sender, EventArgs e)
        {
            CBNemLathato();
            currentFilter = "hospital";
            selectedId = GlobalData.HospitalId;
            FormMegnyitasPanelben<Doctor_Statistics>();
        }

        //Orvos gomb kiválasztása
        private void Btn_OrvosValaszt_Click(object sender, EventArgs e)
        {
            CB_Valaszto.Visible = true;
            currentFilter = "doctor";
            GetDoctor();

            // Azonnal állítsuk be az első elem ID-jét és frissítsük a statisztikát
            if (CB_Valaszto.Items.Count > 0 && CBDoctor.Count > 0)
            {
                selectedId = (int)CBDoctor[0].id;
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }

        //ComboBox érétke változott
        private void CB_Valaszto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Valaszto.SelectedIndex >= 0)
            {
                if (currentFilter == "doctor" && CBDoctor.Count > CB_Valaszto.SelectedIndex)
                {
                    selectedId = (int)CBDoctor[CB_Valaszto.SelectedIndex].id;
                }
                
                FormMegnyitasPanelben<Doctor_Statistics>();
            }
        }
        #endregion

        //Orvosok lekérdezése (kórház orvosai)
        private async void GetDoctor()
        {
            CB_Valaszto.Items.Clear();

            var data = await FetchData($"/api/v1/api.php/data/hospital/allDoctors?hospital_id={GlobalData.HospitalId}");
            if (data == null || data.hospitals == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }

            #region Combo Box feltöltése
            // Educations DataGridView
            CBDoctor = data.hospitals.ToObject<List<dynamic>>();
            foreach (var doctor in CBDoctor)
            {
                CB_Valaszto.Items.Add(doctor.name.ToString());
            }
            if (CB_Valaszto.Items.Count > 0)
            {
                CB_Valaszto.SelectedIndex = 0;
               
            }
            #endregion
            
        }
    }
}
