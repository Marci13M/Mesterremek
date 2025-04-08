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
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Company_Services : Form
    {
        public Company_Services()
        {
            InitializeComponent();
        }

        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Services = new List<dynamic>();
        private List<dynamic> CBServices = new List<dynamic>();

        private int ServiceID;

        //Form betöltése
        private void Ceg_Service_Load(object sender, EventArgs e)
        {
            Pn_SzolgaltatasLeiras.Visible = false;
            Pn_IdoAr.Visible = false;
            nUP_SzolgaltatasIdo.Visible = false;
            GetCompanyServices();
            GetComboBoxService();
            dGV_Szolgaltatasok.CellContentClick += DataGridView_CellContentClick;
            dGV_Szolgaltatasok.DataBindingComplete += dGV_Szolgaltatasok_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dGV_Szolgaltatasok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Szolgaltatasok);
        }

        //Átméretezés
        private void LeirasMeretezese()
        {
            Lb_CegSzolgaltatasok.Location = new Point(20, 10);
            //Panel méretének változtatása
            tableLayoutPanel1.Location = new Point(20, 50);
            tableLayoutPanel1.Size = new Size(this.Width - 2 * 20, this.Height - 70);
            //Tb_SzolgaltatasLeiras.Size = new Size(450, Tb_SzolgaltatasLeiras.Height);
            Btn_SzolgaltatasHozzaadas.Location = new Point(Tb_SzolgaltatasLeiras.Width-Btn_SzolgaltatasHozzaadas.Width+10, Pn_SzolgaltatasLeiras.Height - (Btn_SzolgaltatasHozzaadas.Height + 10));
        }

        //Form átméretezése (event)
        private void Ceg_Service_SizeChanged(object sender, EventArgs e)
        {
            LeirasMeretezese();
            if (dGV_Szolgaltatasok.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Szolgaltatasok);
            }
        }

        //Combobox új szolgáltatás kiválasztva
        private void CB_UjSzolgaltatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pn_SzolgaltatasLeiras.Visible = true;
            Pn_IdoAr.Visible = true;
            nUP_SzolgaltatasAr.Value=0;
            nUP_SzolgaltatasIdo.Value = 5;
            LeirasMeretezese();
            nUP_SzolgaltatasIdo.Visible = true;
            // Ellenőrizzük, hogy van-e érvényes kiválasztás
            if (CB_UjSzolgaltatas.SelectedIndex >= 0 && CB_UjSzolgaltatas.SelectedIndex < CBServices.Count)
            {
                var selectedService = CBServices[CB_UjSzolgaltatas.SelectedIndex];
                ServiceID = selectedService.id;

                Debug.WriteLine($"Kiválasztott ID: {ServiceID}");
                GetChoosenService();
            }
            else
            {
                // Ha valamiért nincs érvényes kiválasztás
                Debug.WriteLine("Nincs érvényes kiválasztott szolgáltatás.");
            }

        }

        //Új szolgáltatás idő beállítása
        private void pn1_ValueChanged(object sender, EventArgs e)
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
        private void pn1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region Get-ek
        // A cég szolgáltatásai
        private async void GetCompanyServices()
        {
            dGV_Szolgaltatasok.Rows.Clear();
            var data = await FetchData($"/api/v1/api.php/services/GetCompanyServices?company_id={GlobalData.CompanyId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            Services = data.services.ToObject<List<dynamic>>();
            foreach (var service in Services)
            {
                int rowIndex = dGV_Szolgaltatasok.Rows.Add(service.service_name.ToString(), service.service_description.ToString(), service.service_duration, service.service_price);
                dGV_Szolgaltatasok.Rows[rowIndex].Cells[dGV_Szolgaltatasok.Columns.Count - 1].Value = "Törlés";
                dGV_Szolgaltatasok.Rows[rowIndex].Cells[dGV_Szolgaltatasok.Columns.Count - 2].Value = "Módosítás";
            }
            AdjustDataGridViewHeight(dGV_Szolgaltatasok);
        }

        //A kiválasztható új szolgáltatások (még nincsenek a céghez rendelve)
        private async void GetComboBoxService()
        {
            CB_UjSzolgaltatas.Items.Clear();
            var data = await FetchData($"/api/v1/api.php/data/company/services?company_id={GlobalData.CompanyId}");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Combo Box feltöltése
            CBServices = data.services.ToObject<List<dynamic>>();
            foreach (var hospital in CBServices)
            {
                CB_UjSzolgaltatas.Items.Add(hospital.name.ToString());
            }
            #endregion
            
        }

        //Kiválasztott szolgáltatás adatai
        private async void GetChoosenService()
        {
            Tb_SzolgaltatasLeiras.Text = "";
            var data = await FetchData($"/api/v1/api.php/services/GetService?service_id={ServiceID}");
            if (data == null || data.service == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Textboxok feltöltése
            // Általános információk
            Tb_SzolgaltatasLeiras.Text = data.service.service_description;
            #endregion
        }
        #endregion

        //DataGridView kattintás eventjei
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
                    case "Torles":
                        var service = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {service} szolgáltatást?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int service_id = Services[e.RowIndex].service_id;
                            int company_id = GlobalData.CompanyId;
                            await DeleteCompanyService(company_id, service_id);
                            GetCompanyServices();
                            GetComboBoxService();
                           
                        }
                        break;
                    case "modositas":
                        var serviceID = Services[e.RowIndex].service_id; 
                        var serviceName = Services[e.RowIndex].service_name;
                        var serviceDescription = Services[e.RowIndex].service_description;
                        var serviceDuration = Services[e.RowIndex].service_duration;
                        var servicePrice = Services[e.RowIndex].service_price;
                        var role_name = GlobalData.RoleIdentifier;
                        OpenServiceModify(serviceID, serviceName, serviceDescription, serviceDuration, servicePrice, role_name);
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Szolgáltatás törlése
        private async Task DeleteCompanyService(int company_id, int service_id)
        {
            var data = new { service_id, company_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/companies/RemoveService", data);
        }

        //Szolgáltatás módosító form megnyitása (Service_Modify)
        private void OpenServiceModify(dynamic serviceID, dynamic serviceName, dynamic serviceDescription, dynamic serviceDuration, dynamic servicePrice, dynamic role_name)
        {
            Main mainForm = this.ParentForm as Main;
            Service_Modify service_modify = new Service_Modify(serviceID, serviceName, serviceDescription, serviceDuration, servicePrice, role_name);
            service_modify.FormClosed += (sender, e) =>
            {
                // Itt hívhatod meg a kívánt függvényt, amikor a dialógus bezáródik
                GetCompanyServices();
            };
            // Form megnyitása modálisan
            service_modify.ShowDialog();
        }
       
        //Szolgáltatás hozzáadása (click event)
        private async void Btn_SzolgaltatasHozzaadas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Tb_SzolgaltatasLeiras.Text))
            {
                string selectedServiceName = CB_UjSzolgaltatas.SelectedItem.ToString();

                foreach (var service in CBServices)
                {
                    if (service.name.ToString() == selectedServiceName)
                    {
                        
                        int service_id = Convert.ToInt32(service.id);
                        int company_id = GlobalData.CompanyId;
                        int price =Convert.ToInt32(nUP_SzolgaltatasAr.Value);
                        int duration=Convert.ToInt32(nUP_SzolgaltatasIdo.Value);
                        string description = Tb_SzolgaltatasLeiras.Text;

                        // Api meghívása
                        await AddService(service_id, company_id, price, duration, description);
                        GetCompanyServices();
                        GetComboBoxService();

                        //az inputok eltüntetése
                        Pn_SzolgaltatasLeiras.Visible = false;
                        Pn_IdoAr.Visible = false;
                        nUP_SzolgaltatasIdo.Visible = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérem töltse ki a leírást!");
            }
        }

        //Szolgáltatás hozzáadása
        private async Task AddService(int service_id, int company_id, int price, int duration, string description)
        {
            var data = new { service_id, company_id, price, duration, description };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/companies/AddService", data);
        }
    }
}
