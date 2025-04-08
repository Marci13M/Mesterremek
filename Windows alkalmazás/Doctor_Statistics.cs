using CareCompass.Customs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using static CareCompass.Bejelentkezés;
using System.Diagnostics;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Doctor_Statistics : Form
    {
        public Doctor_Statistics()
        {
            InitializeComponent();

        }

        //Filterek a statisztikákhoz
        private string filterType = "doctor";
        private int? filterId = GlobalData.UserId;

        //A form bezáródása 
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            ThemeManager.CleanUpButtonGroups(this);
        }

        //Form betöltése
        private void Doctor_Statistics_Load(object sender, EventArgs e)
        {
            var buttons = new[] { Btn_Egyedi, Btn_Ma, Btn_Utolso7Nap, Btn_Utolso30Nap, Btn_ElozoHonap };
            ThemeManager.RegisterButtonGroup(this, buttons, Btn_Utolso7Nap);

            OKNemLathato();
            dTP_Kezdo.Value = DateTime.Now.AddDays(-7);
            dTP_Vege.Value = DateTime.Now;
            Lb_KezdetDatum.Text = dTP_Kezdo.Text;
            Lb_VegDatum.Text = dTP_Vege.Text;
            //LoadData();
            SetFilter(filterType, filterId);
            Debug.WriteLine($"Load{filterType},{filterId}");
            LoadProfileData();
        }

        //Filterek beállítása
        public void SetFilter(string type, int? id)
        {
            filterType = type;
            filterId = id;
        }

        //Felhasználandó chartok betöltése
        private void LoadProfileData()
        {
            if (filterType == "doctor")
            {
                DoctorReservationsChart();
                DoctorIncome();
            }
            else if (filterType == "hospital")
            {
                HospitalReservationsChart();
                HospitalIncome();
            }
            else if (filterType == "company")
            {
                CompanyReservationsChart();
                Company_Income();
            }
            else if (filterType == "admin")
            {
                AdminReservationsChart();
                AdminTotalIncome();
            }
        }

        //OK gomb láthatósága (nem látható)
        private void OKNemLathato()
        {
            Btn_OK.Visible = false;
        }

        #region Gombok click eventek
        //Egyedi gomb 
        private void Btn_Egyedi_Click(object sender, EventArgs e)
        {
            Btn_OK.Visible=true;
        }

        //Mai nap gomb
        private void Btn_Ma_Click(object sender, EventArgs e)
        {
            OKNemLathato();
            dTP_Kezdo.Value=DateTime.Today;
            dTP_Vege.Value=DateTime.Now;
            LoadProfileData();
        }

        //Utolsó 7 nap gomb
        private void Btn_Utolso7Nap_Click(object sender, EventArgs e)
        {
            OKNemLathato();
            dTP_Kezdo.Value = DateTime.Now.AddDays(-7);
            dTP_Vege.Value = DateTime.Now;
            LoadProfileData();
        }

        //Utolsó 30 nap gomb
        private void Btn_Utolso30Nap_Click(object sender, EventArgs e)
        {
            OKNemLathato();
            dTP_Kezdo.Value = DateTime.Now.AddDays(-30);
            dTP_Vege.Value = DateTime.Now;
            Debug.WriteLine(DateTime.Now.AddDays(-30));
            LoadProfileData();
        }

        //Előző hónap gomb
        private void Btn_ElozoHonap_Click(object sender, EventArgs e)
        {
            OKNemLathato();
            // Előző hónap első napja
            DateTime now = DateTime.Now;
            // Előző hónap első napja
            DateTime kezdo = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            // Előző hónap utolsó napja
            DateTime veg = new DateTime(now.Year, now.Month, 1).AddDays(-1);
            // Beállítás
            dTP_Kezdo.Value = kezdo;
            dTP_Vege.Value = veg;
            LoadProfileData();
        }

        //OK gomb kattintása
        private void Btn_OK_Click(object sender, EventArgs e)
        {
            if (dTP_Kezdo.Value.Date == DateTime.Now || dTP_Kezdo.Value.Date>DateTime.Now )
            {
                dTP_Kezdo.Value = DateTime.Now.AddDays(-1);
            }
            if(dTP_Vege.Value.Date == DateTime.Now || dTP_Vege.Value.Date > DateTime.Now)
            {
                dTP_Vege.Value = DateTime.Now;
            }
            LoadProfileData();
        }
        #endregion

        #region Dátumok változása (kezdő és végdátum)
        //Kezdődátum beállítása (click event)
        private void Lb_KezdetDatum_Click(object sender, EventArgs e)
        {
            var activeButton = ThemeManager.GetActiveButton(this);

            if (activeButton == Btn_Egyedi)
            {
                Debug.Write("2");
                dTP_Kezdo.Visible = true;
                dTP_Kezdo.Focus();
                SendKeys.Send("%{DOWN}");
            }
        }

        //Végdátum beállítása (click event)
        private void Lb_VegDatum_Click(object sender, EventArgs e)
        {
            var activeButton = ThemeManager.GetActiveButton(this);

            if (activeButton == Btn_Egyedi)
            {
                dTP_Vege.Visible = true;
                dTP_Vege.Focus();
                SendKeys.Send("%{DOWN}");
            }
        }

        //Kezdő dátum DateTimePicker érték változása
        private void dTP_Kezdo_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_Kezdo.Value.Date == DateTime.Now || dTP_Kezdo.Value.Date > DateTime.Now)
            {
                dTP_Kezdo.Value = DateTime.Now.AddDays(-1);
            }
            
            Lb_KezdetDatum.Text = dTP_Kezdo.Text;
        }

        //Vég dátum DateTimePicker érték változása
        private void dTP_Vege_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_Vege.Value.Date == DateTime.Now || dTP_Vege.Value.Date > DateTime.Now)
            {
                dTP_Vege.Value = DateTime.Now;
            }
            Lb_VegDatum.Text = dTP_Vege.Text;
        }
        #endregion



        //Statisztikák
        #region Doctor Statistics

        //Orvos foglalások statisztikák
        private async void DoctorReservationsChart()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/DoctorReservations?doctor_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.reservations == null)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0)
                        chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var reservations = data.reservations.ToObject<List<dynamic>>();
                int totalReservations = data.total_reservations;

                if (totalReservations == 0 || reservations.Count == 0)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0)
                        chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                if (chart2.Titles.Count == 0)
                {
                    chart2.Titles.Add("Foglalások száma");
                }
                // Cím módosítása a foglalások számával
                chart2.Titles[0].Text = $"Foglalások száma: {totalReservations} db";
                chart2.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart2.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart2.Series[0].ToolTip = "#VALX: #VALY db";
                if (chart2.Series.Count == 0)
                {
                    chart2.Series.Add(new Series("Foglalások"));
                }
                else
                {
                    chart2.Series[0].Points.Clear();
                    chart2.Series[0].Name = "Foglalások";
                }

                foreach (var item in reservations)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var count = (int)item.reservations;
                    chart2.Series[0].Points.AddXY(date.ToShortDateString(), count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        //Orvos bevéel statisztikák
        private async void DoctorIncome()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/DoctorIncome?doctor_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.incomes == null)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var incomes = data.incomes.ToObject<List<dynamic>>();
                decimal totalIncome = data.total_income;
                string formattedIncome = totalIncome.ToString("N0").Replace(",", " ");
                double incomeChange = data.income_change;

                if (totalIncome == 0 || incomes.Count == 0)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                chart3.Titles[0].Text = $"Bevétel: {formattedIncome} Ft";
                chart3.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart3.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart3.Series[0].ToolTip = "#VALX: #VALY Ft";

                if (chart3.Series.Count == 0)
                {
                    chart3.Series.Add(new Series("Bevételek") { ChartType = SeriesChartType.Spline });
                }
                else
                {
                    chart3.Series[0].Points.Clear();
                    chart3.Series[0].Name = "Bevételek";
                }

                foreach (var item in incomes)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var income = decimal.Parse(item.income.ToString());
                    chart3.Series[0].Points.AddXY(date.ToShortDateString(), income);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }
        #endregion

        #region Hospital Statistics

        //Kórház foglalások statisztika
        private async void HospitalReservationsChart()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/HospitalReservations?hospital_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.reservations == null)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var reservations = data.reservations.ToObject<List<dynamic>>();
                int totalReservations = data.total_reservations;

                if (totalReservations == 0 || reservations.Count == 0)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                chart2.Titles[0].Text = $"Foglalások száma: {totalReservations} db";
                chart2.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart2.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart2.Series[0].ToolTip = "#VALX: #VALY db";

                if (chart2.Series.Count == 0)
                {
                    chart2.Series.Add(new Series("Foglalások") { ChartType = SeriesChartType.FastLine });
                }
                else
                {
                    chart2.Series[0].Points.Clear();
                    chart2.Series[0].Name = "Foglalások";
                }
                
                foreach (var item in reservations)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var count = (int)item.reservations;
                    chart2.Series[0].Points.AddXY(date.ToShortDateString(), count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        //Kórház bevétel statisztika
        private async void HospitalIncome()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/HospitalIncome?hospital_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.incomes == null)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var incomes = data.incomes.ToObject<List<dynamic>>();
                decimal totalIncome = data.total_income;
                string formattedIncome = totalIncome.ToString("N0").Replace(",", " ");
                double incomeChange = data.income_change;

                if (totalIncome == 0 || incomes.Count == 0)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                chart3.Titles[0].Text = $"Bevétel: {formattedIncome} Ft";
                chart3.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart3.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart3.Series[0].ToolTip = "#VALX: #VALY Ft";

                if (chart3.Series.Count == 0)
                {
                    chart3.Series.Add(new Series("Bevételek") { ChartType = SeriesChartType.Spline });
                }
                else
                {
                    chart3.Series[0].Points.Clear();
                    chart3.Series[0].Name = "Bevételek";
                }

                foreach (var item in incomes)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var income = decimal.Parse(item.income.ToString());
                    chart3.Series[0].Points.AddXY(date.ToShortDateString(), income);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }
        #endregion

        #region Company Statistics

        //Cég foglalások statisztikák
        private async void CompanyReservationsChart()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/CompanyReservations?company_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.reservations == null)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var reservations = data.reservations.ToObject<List<dynamic>>();
                int totalReservations = data.total_reservations;

                if (totalReservations == 0 || reservations.Count == 0)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                // Cím módosítása a foglalások számával
                chart2.Titles[0].Text = $"Foglalások száma: {totalReservations} db";
                chart2.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart2.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart2.Series[0].ToolTip = "#VALX: #VALY db";

                if (chart2.Series.Count == 0)
                {
                    chart2.Series.Add(new Series("Foglalások") { ChartType = SeriesChartType.FastLine });
                }
                else
                {
                    chart2.Series[0].Points.Clear();
                    chart2.Series[0].Name = "Foglalások";
                }

                foreach (var item in reservations)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var count = (int)item.reservations;
                    chart2.Series[0].Points.AddXY(date.ToShortDateString(), count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        //Cég bevétel statisztikák
        private async void Company_Income()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/CompanyIncome?company_id={filterId}&from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.incomes == null)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var incomes = data.incomes.ToObject<List<dynamic>>();
                decimal totalIncome = data.total_income;
                string formattedIncome = totalIncome.ToString("N0").Replace(",", " ");
                double incomeChange = data.income_change;

                if (totalIncome == 0 || incomes.Count == 0)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                // Cím módosítása a foglalások számával
                chart3.Titles[0].Text = $"Bevétel: {formattedIncome} Ft";
                chart3.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart3.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart3.Series[0].ToolTip = "#VALX: #VALY Ft";

                if (chart3.Series.Count == 0)
                {
                    chart3.Series.Add(new Series("Bevételek") { ChartType = SeriesChartType.Spline });
                }
                else
                {
                    chart3.Series[0].Points.Clear();
                    chart3.Series[0].Name = "Bevételek";
                }

                foreach (var item in incomes)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var income = decimal.Parse(item.income.ToString());
                    chart3.Series[0].Points.AddXY(date.ToShortDateString(), income);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }
        #endregion

        #region Admin Statistics

        //Admin bevétel statisztika
        private async void AdminReservationsChart()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/TotalReservations?from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.reservations == null)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var reservations = data.reservations.ToObject<List<dynamic>>();
                int totalReservations = data.total_reservations;

                if (totalReservations == 0 || reservations.Count == 0)
                {
                    if (chart2.Series.Count > 0) chart2.Series[0].Points.Clear();
                    if (chart2.Titles.Count > 0) chart2.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                // Cím módosítása a foglalások számával
                chart2.Titles[0].Text = $"Foglalások száma: {totalReservations} db";
                chart2.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart2.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart2.Series[0].ToolTip = "#VALX: #VALY db";

                if (chart2.Series.Count == 0)
                {
                    chart2.Series.Add(new Series("Foglalások") { ChartType = SeriesChartType.FastLine });
                }
                else
                {
                    chart2.Series[0].Points.Clear();
                    chart2.Series[0].Name = "Foglalások";
                }

                foreach (var item in reservations)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var count = (int)item.reservations;
                    chart2.Series[0].Points.AddXY(date.ToShortDateString(), count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }

        //Admin bevétel statisztika
        private async void AdminTotalIncome()
        {
            try
            {
                string fromDate = dTP_Kezdo.Value.ToString("yyyy-MM-dd");
                string toDate = dTP_Vege.Value.ToString("yyyy-MM-dd");
                string endpoint = $"/api/v1/api.php/statistics/TotalIncome?from={fromDate}&to={toDate}";

                var data = await FetchData(endpoint);

                if (data == null || data.incomes == null)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                var incomes = data.incomes.ToObject<List<dynamic>>();
                int totalIncome = data.total_income;
                string formattedIncome = totalIncome.ToString("N0").Replace(",", " ");
                double incomeChange = data.income_change;

                if (totalIncome == 0 || incomes.Count == 0)
                {
                    if (chart3.Series.Count > 0) chart3.Series[0].Points.Clear();
                    if (chart3.Titles.Count > 0) chart3.Titles[0].Visible = false; // Cím elrejtése
                    return;
                }

                // Cím módosítása a foglalások számával
                chart3.Titles[0].Text = $"Bevétel: {formattedIncome} Ft";
                chart3.Titles[0].Font = new Font("Calibri", 14, FontStyle.Bold);
                chart3.Titles[0].Visible = true; // Cím megjelenítése

                //Egér az adatra meg
                chart3.Series[0].ToolTip = "#VALX: #VALY Ft";

                if (chart3.Series.Count == 0)
                {
                    chart3.Series.Add(new Series("Bevételek") { ChartType = SeriesChartType.Spline });
                }
                else
                {
                    chart3.Series[0].Points.Clear();
                    chart3.Series[0].Name = "Bevételek";
                }

                foreach (var item in incomes)
                {
                    var date = DateTime.Parse(item.date.ToString());
                    var income = decimal.Parse(item.income.ToString());
                    chart3.Series[0].Points.AddXY(date.ToShortDateString(), income);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}");
            }
        }
        #endregion
    }
}
