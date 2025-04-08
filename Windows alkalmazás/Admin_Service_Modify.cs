using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using static CareCompass.ApiClient;
using System.Diagnostics;

namespace CareCompass
{
    public partial class Admin_Service_Modify : Form
    {
        public Admin_Service_Modify()
        {
            InitializeComponent();
        }

        //Form betöltése
        private void Admin_Service_Modify_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("hali");
            if (ThemeManager.CurrentTheme == "Dark")
            {
                ThemeManager.SetTheme("Dark");
            }
            else
            {
                ThemeManager.SetTheme("Light");
            }
        }

        //Lekérdezések tárolására szolgáló listák
        private readonly int serviceID;
        private readonly string serviceName;
        private readonly string serviceDescription;

        // Konstruktor az adatok fogadására
        internal Admin_Service_Modify(dynamic serviceID, dynamic serviceName, dynamic serviceDescription)
        {
            InitializeComponent();

            // Adatok tárolása privát változókban
            this.serviceID = serviceID;
            this.serviceName = serviceName.ToString();
            this.serviceDescription = serviceDescription.ToString();
            // TextBoxok feltöltése
            LoadServiceData();
        }

        // Adatok megjelítése
        private void LoadServiceData()
        {
            Tb_SzolgaltatasNev.Text = serviceName;
            Tb_SzolgaltatasLeiras.Text = serviceDescription;
        }

        //Mégse gomb event
        private void ccButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Szolgáltatás módosítás gomb
        private async void Btn_SzolgaltatasHozzaadas_Click(object sender, EventArgs e)
        {
            int service_id = serviceID;
            string name = Tb_SzolgaltatasNev.Text;
            string description = Tb_SzolgaltatasLeiras.Text;
            await UpdateService(service_id, name, description);
            this.Close();
        }

        //Szolgáltatás módosítása
        private async Task UpdateService(int service_id, string name,  string description)
        {
            var data = new {service_id, name ,description };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/services/UpdateService", data);
        }
    }
}
