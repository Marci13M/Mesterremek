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
    public partial class Admin_Services : Form
    {
        public Admin_Services()
        {
            InitializeComponent();
        }
        //Lekérdezések tárolására szolgáló listák
        private List<dynamic> Services = new List<dynamic>();

        //Form betöltése
        private void Admin_Services_Load(object sender, EventArgs e)
        {
            GetServices();
            dGV_Szolgaltatasok.CellContentClick += DataGridView_CellContentClick;
            dGV_Szolgaltatasok.DataBindingComplete += dGV_Szolgaltatasok_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dGV_Szolgaltatasok_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dGV_Szolgaltatasok);
        }

        //Form méretének változzatása (event)
        private void Admin_Services_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dGV_Szolgaltatasok.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dGV_Szolgaltatasok);
            }
        }

        //Méretváltás
        private void MeretValtas()
        {
            //FőPanel méretének változtatása
            Pn_AdminSzolgaltatasok.Location = new Point(20, 50);
            Pn_AdminSzolgaltatasok.Size = new Size(this.Width - 2 * 20, this.Height - 70);
        }

        //Szoltáltatások lekérdezés
        private async void GetServices()
        {
            dGV_Szolgaltatasok.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/services/GetServices");
            if (data == null || data.services == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            Services = data.services.ToObject<List<dynamic>>();
            foreach (var service in Services)
            {
                int rowIndex = dGV_Szolgaltatasok.Rows.Add(service.service_name.ToString(), service.service_description.ToString());
                dGV_Szolgaltatasok.Rows[rowIndex].Cells[dGV_Szolgaltatasok.Columns.Count - 1].Value = "Törlés";
                dGV_Szolgaltatasok.Rows[rowIndex].Cells[dGV_Szolgaltatasok.Columns.Count - 2].Value = "Módosítás";

            }
            #endregion
            AdjustDataGridViewHeight(dGV_Szolgaltatasok);
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
                    case "modositas":


                        var serviceID = Services[e.RowIndex].service_id;
                        var serviceName = Services[e.RowIndex].service_name;
                        var serviceDescription = Services[e.RowIndex].service_description;
                        OpenServiceModify(serviceID, serviceName, serviceDescription);
                        GetServices();
                        break;
                    case "Torles":
                        
                        var service = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeletePhone = MessageBox.Show($"Biztosan törlöd a(z) {service} szolgáltatást?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeletePhone == DialogResult.Yes)
                        {
                            int service_id = Services[e.RowIndex].service_id;
                            await DeleteService(service_id);
                            GetServices();
                        }
                        break;
                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Szolgáltatás törlése
        private async Task DeleteService(int service_id)
        {
            var data = new { service_id};
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/services/Delete", data);
        }

        //Szoltáltatás módosító form megynitása
        private void OpenServiceModify(dynamic serviceID, dynamic serviceName, dynamic serviceDescription)
        {
            Main mainForm = this.ParentForm as Main;

            // Új példány létrehozása és adatátadás a konstruktoron keresztül
            Admin_Service_Modify admin_service_modify = new Admin_Service_Modify(serviceID, serviceName, serviceDescription);

            // Form megnyitása modálisan
            admin_service_modify.ShowDialog();
        }

        //Új szolgáltatás hozzáadása (click event)
        private async void Btn_SzolgaltatasHozzaadas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_SzolgaltatasNev.Text.ToString()) && !string.IsNullOrWhiteSpace(Tb_SzolgaltatasLeiras.Text.ToString()))
            {
                string name = Tb_SzolgaltatasNev.Text;
                string description = Tb_SzolgaltatasLeiras.Text;
                await AddService(name, description);
                GetServices();

                Tb_SzolgaltatasLeiras.Clear();
                Tb_SzolgaltatasNev.Clear();
            }
            else
            {
                MessageBox.Show("Valamelyik mező nincs kitöltve!");
            }
            
        }

        //Szolgáltatás hozzáadása
        private async Task AddService (string name, string description)
        {
            var data = new { name, description };
            await SendRequestAsync(HttpMethod.Post, "/api/v1/api.php/services/Add ", data);
        }
    }
}
