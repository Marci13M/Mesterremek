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
//using System.Windows.Controls;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using Panel = System.Windows.Forms.Panel;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Hospital_Services : Form
    {
        public Hospital_Services()
        {
            InitializeComponent();
        }


        private List<dynamic> Services = new List<dynamic>();
        private List<dynamic> CBServices = new List<dynamic>();

        private int ServiceID;
        
        //Form betöltése
        private void Korhaz_Services_Load(object sender, EventArgs e)
        {
            Pn_SzolgaltatasLeiras.Visible = false;
            Pn_IdoAr.Visible = false;

            GetHospitalServices();
            GetServices();
            dGV_UjSzolgaltatas.CellContentClick += DataGridView_CellContentClick;
            dGV_UjSzolgaltatas.DataBindingComplete += dGV_UjSzolgaltatas_DataBindingComplete;
        }


        //DataGridView adatbetöltés utáni átméretezése
        private void dGV_UjSzolgaltatas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_UjSzolgaltatas);
        }

        //Form átméretezése event
        private void Korhaz_Services_SizeChanged(object sender, EventArgs e)
        {
            LeirasMeretezese();
            if (dGV_UjSzolgaltatas.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_UjSzolgaltatas);
            }
        }

        //Szolgáltatás méretezése (új szolgáltatás)
        private void LeirasMeretezese()
        {
            Lb_KorhazSzolgaltatasok.Location = new Point(20, 10);
            //Panel méretének változtatása
            tableLayoutPanel1.Location = new Point(20, 50);
            tableLayoutPanel1.Size = new Size(this.Width - 2 * 20, this.Height - 70);
            Btn_SzolgaltatasHozzaadas.Location = new Point(Tb_SzolgaltatasLeiras.Width - Btn_SzolgaltatasHozzaadas.Width + 10, Pn_SzolgaltatasLeiras.Height - (Btn_SzolgaltatasHozzaadas.Height + 10));
        }

        //Új szolgáltatás kiválasztása (combobox-ból)
        private void CB_UjSzolgaltatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pn_SzolgaltatasLeiras.Visible = true;
            Pn_IdoAr.Visible=true;
            
            LeirasMeretezese();
            // Ellenőrizzük, hogy van-e érvényes kiválasztás
            if (CB_UjSzolgaltatas.SelectedIndex >= 0 && CB_UjSzolgaltatas.SelectedIndex < CBServices.Count)
            {
                // Kiválasztott elem ID-jének lekérése a CBServices listából
                var selectedService = CBServices[CB_UjSzolgaltatas.SelectedIndex];
                ServiceID = selectedService.id;

                Debug.WriteLine($"Kiválasztott ID: {ServiceID}");
                GetCompanyServices();
            }
            else
            {
                // Ha valamiért nincs érvényes kiválasztás
                Debug.WriteLine("Nincs érvényes kiválasztott szolgáltatás.");
            }
        }

        //Szolgáltatás idő beállítása
        private void nUP_SzolgaltatasIdo_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            // Ellenőrizd, hogy a szám végződése 5 vagy 0-e
            decimal value = nud.Value;
            decimal roundedValue = Math.Round(value / 5) * 5;

            if (value != roundedValue)
            {
                nud.Value = roundedValue;
            }
        }

        private void nUP_SzolgaltatasIdo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //Szolgáltatások lekérdezése
        private async void GetHospitalServices()
        {
            dGV_UjSzolgaltatas.Rows.Clear();
            dGV_UjSzolgaltatas.ReadOnly = false;
            dGV_UjSzolgaltatas.Columns[0].ReadOnly = true;
            dGV_UjSzolgaltatas.Columns[1].ReadOnly = true;
            dGV_UjSzolgaltatas.Columns[2].ReadOnly = true;
            dGV_UjSzolgaltatas.Columns[3].ReadOnly = true;

            var data = await FetchData($"/api/v1/api.php/services/GetHospitalServices?hospital_id={GlobalData.HospitalId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }

            Services = data.services.ToObject<List<dynamic>>();
            foreach (var service in Services)
            {
                int rowIndex = dGV_UjSzolgaltatas.Rows.Add(service.service_name.ToString(), service.service_description.ToString(), service.service_duration, service.service_price, service.service_active);
                dGV_UjSzolgaltatas.Rows[rowIndex].Cells[dGV_UjSzolgaltatas.Columns.Count - 1].Value = "Módosítás";
            }
            AdjustDataGridViewHeight(dGV_UjSzolgaltatas);
        }

        //Szolgáltatások lekérdezés (azok melyek még nincsenek a kórházhoz rendelve)
        private async void GetServices()
        {
            CB_UjSzolgaltatas.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/hospital/services?hospital_id={GlobalData.HospitalId}&company_id={GlobalData.CompanyId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            CBServices = data.services.ToObject<List<dynamic>>();
            foreach (var service in CBServices)
            {
                CB_UjSzolgaltatas.Items.Add(service.service.ToString());
            }
            #endregion
        }

        //a kiválasztott szolgáltatás adatainak meghívása
        private async void GetCompanyServices()
        {
            Tb_SzolgaltatasLeiras.Text = "";
            var data = await FetchData($"/api/v1/api.php/services/GetCompanyTemporaryServices?company_id={GlobalData.CompanyId}&service_id={ServiceID}");
            if (data == null || data.temporary_service == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Textboxok feltöltése
            // Általános információk
            Tb_SzolgaltatasLeiras.Text = data.temporary_service.service_description;
            nUP_SzolgaltatasAr.Value = data.temporary_service.service_price;
            nUP_SzolgaltatasIdo.Value = data.temporary_service.service_duration;
            #endregion
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
                    case "modositas":
                        var serviceID = Services[e.RowIndex].service_id;
                        var serviceName = Services[e.RowIndex].service_name;
                        var serviceDescription = Services[e.RowIndex].service_description;
                        var serviceDuration = Services[e.RowIndex].service_duration;
                        var servicePrice = Services[e.RowIndex].service_price;
                        var role_name = GlobalData.RoleIdentifier;
                        OpenServiceModify(serviceID, serviceName, serviceDescription, serviceDuration, servicePrice, role_name);
                        GetServices();
                        break;
                    case "aktiv":
                        try
                        {
                            int service_id = Convert.ToInt32(Services[e.RowIndex].service_id);
                            int hospital_id = GlobalData.HospitalId;

                            await ChangeSericeActiveState(service_id, hospital_id);
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

        //A szolgáltatás aktív státuszának beállítása
        private async Task ChangeSericeActiveState(int service_id, int hospital_id)
        {
            var data = new { service_id, hospital_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/services/ChangeHospitalServiceActiveState", data);
        }
        
        //Szolgáltatás módosító form megnyitása (Service_Modify)
        private void OpenServiceModify(dynamic serviceID, dynamic serviceName, dynamic serviceDescription, dynamic serviceDuration, dynamic servicePrice, dynamic role_name)
        {
            Main mainForm = this.ParentForm as Main;
            Service_Modify service_modify = new Service_Modify(serviceID, serviceName, serviceDescription, serviceDuration, servicePrice, role_name);

            service_modify.FormClosed += (sender, e) =>
            {
                // Itt hívhatod meg a kívánt függvényt, amikor a dialógus bezáródik
                GetHospitalServices();
            };
            // Form megnyitása modálisan
            service_modify.ShowDialog();
        }

        //Szolgáltatás hozzáadása (click event)
        private async void Btn_SzolgaltatasHozzaadas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_SzolgaltatasLeiras.Text))
            {
                string selectedServiceName = CB_UjSzolgaltatas.SelectedItem.ToString();

                foreach (var service in CBServices)
                {
                    if (service.service.ToString() == selectedServiceName)
                    {
                        int service_id = Convert.ToInt32(service.id);
                        int hospital_id = GlobalData.HospitalId;
                        string name=service.service.ToString();
                        int duration =Convert.ToInt32(nUP_SzolgaltatasIdo.Value);
                        int price = Convert.ToInt32(nUP_SzolgaltatasAr.Value);
                        string description = Tb_SzolgaltatasLeiras.Text;

                        // Api meghívása
                        await AddService(service_id,name, duration, price, description, hospital_id);
                        Pn_SzolgaltatasLeiras.Visible = false;
                        Pn_IdoAr.Visible = false;
                        GetHospitalServices();
                        GetServices();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérem töltse ki a leírást!");
            }

        }

        //Szolgáltatás hozzáadása
        private async Task AddService(int service_id,string name, int duration, int price, string description, int hospital_id)
        {
            var data = new { service_id,name, duration, price, description, hospital_id };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/hospitals/AddService", data);
        }
    }
}
