using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Service_Modify : Form
    {
        public Service_Modify()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Service_Modify_Load(object sender, EventArgs e)
        {
            if (ThemeManager.CurrentTheme == "Dark")
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
        }

        // Privát változók az adatok tárolására
        private readonly string ServiceID;
        private readonly string ServiceName;
        private readonly string ServiceDescription;
        private readonly string ServicePrice;
        private readonly string ServiceDuration;
        private readonly string role_name;

        // Konstruktor az adatok fogadására
        internal Service_Modify(dynamic serviceID, dynamic serviceName, dynamic serviceDescription, dynamic sercviceDuration, dynamic servicePrice, string role_name)
        {
            InitializeComponent();

            // Adatok tárolása privát változókban
            this.ServiceID = serviceID.ToString();
            this.ServiceName = serviceName.ToString();
            this.ServiceDescription = serviceDescription.ToString();
            this.ServicePrice = servicePrice;
            this.ServiceDuration = sercviceDuration;
            this.role_name = role_name;
            // TextBoxok feltöltése
            LoadServiceData();
            
        }

        // Privát metódus az adatok megjelenítésére
        private void LoadServiceData()
        {
            Tb_SzolgaltatasNev.Text = ServiceName;
            Tb_SzolgaltatasLeiras.Text = ServiceDescription;
            nUP_SzolgaltatasAr.Value = Convert.ToInt32(ServicePrice);
            nUP_SzolgaltatasIdo.Value= Convert.ToInt32(ServiceDuration);
        }

        //Mégse gomb click event
        private void ccButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Szolgáltatás módosítás (click event)
        private async void Btn_SzolgaltatasHozzaadas_Click(object sender, EventArgs e)
        {
            int service_id =Convert.ToInt32(ServiceID);
            string description = Tb_SzolgaltatasLeiras.Text;
            int duration = Convert.ToInt32(nUP_SzolgaltatasIdo.Value);
            int price = Convert.ToInt32(nUP_SzolgaltatasAr.Value);
            if (role_name == "hospital" )
            {
                int hospital_id = GlobalData.HospitalId;
                await UpdateHospitalService(hospital_id, service_id, description, duration, price);
                this.Close();
            }
            else if(role_name == "company")
            {
                int company_id=GlobalData.CompanyId;
                await UpdateCompanyService(service_id, company_id, description, duration, price);
                this.Close();
            }
            else
            {
                Debug.WriteLine("nem futott be!");
            }
        }

        //Kórház szolgáltatás módosítása
        private async Task UpdateHospitalService(int hospital_id, int service_id, string description, int duration, int price)
        {
            var data = new { hospital_id, service_id, description, duration, price };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/services/UpdateHospitalService", data);
        }

        //Cég szolgáltatás módosítása
        private async Task UpdateCompanyService(int service_id, int company_id, string description, int duration, int price)
        {
            var data = new { service_id, company_id, description, duration, price };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/services/UpdateCompanyService", data);
        }
    }
}
