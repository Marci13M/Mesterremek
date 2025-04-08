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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CareCompass.Bejelentkezés;
using static CareCompass.ApiClient;

namespace CareCompass
{
    public partial class Admin_Web_Users : Form
    {
        public Admin_Web_Users()
        {
            InitializeComponent();
        }

        //DataGridView-ek listái
        private List<dynamic> Users = new List<dynamic>();

        //Méret módosítása
        private void MeretValtas()
        {
            Pn_MainPanel.Location = new Point(20, 50);
            Pn_MainPanel.Size = new Size(this.Width - 2 * 20, this.Height - 70);
        }

        //Form betöltése
        private void Admin_Web_Users_Load(object sender, EventArgs e)
        {
            MeretValtas();
            GetWebUsers();
            dataGridView1.CellContentClick += DataGridView_CellContentClick;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        //DataGridView adatfeltöltés utáni átméretezése
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AdjustDataGridViewHeight(dataGridView1);
        }

        //Form méretének átméretezése (event)
        private void Admin_Web_Users_SizeChanged(object sender, EventArgs e)
        {
            MeretValtas();
            if (dataGridView1.Rows.Count > 0)
            {
                AdjustDataGridViewHeight(dataGridView1);
            }
        }

        //Web felhasználó adatatinak lekérdezése
        private async void GetWebUsers()
        {
            dataGridView1.Rows.Clear();
            var data = await FetchData("/api/v1/api.php/users/GetBasicUsers");
            if (data == null || data.users == null)
            {
                Debug.WriteLine("Nincsenek adatok.");
                return;
            }
            #region Datagridview-ek feltöltése
            Users = data.users.ToObject<List<dynamic>>();
            foreach (var user in Users)
            {
                int rowIndex = dataGridView1.Rows.Add(user.name, user.email, user.birthdate);
                dataGridView1.Rows[rowIndex].Cells[dataGridView1.Columns.Count - 2].Value = "Módosítás";
                dataGridView1.Rows[rowIndex].Cells[dataGridView1.Columns.Count - 1].Value = "Törlés";
            }
            #endregion
            AdjustDataGridViewHeight(dataGridView1);
            
        }

        //DataGridView-ek click eventjei
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
                        var doctor = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        DialogResult confirmDeleteDoctor = MessageBox.Show($"Biztosan törlöd a(z) {doctor} nevű orvost?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirmDeleteDoctor == DialogResult.Yes)
                        {
                            int user_id = Users[e.RowIndex].user_id; // Az ID lekérése a listából
                            await DeleteWebUser(user_id); // API hívás a törléshez
                            GetWebUsers();
                        }
                        break;

                    case "Modositas":
                        var userId = Users[e.RowIndex].user_id;
                        var userName = Users[e.RowIndex].name;
                        var userEmail = Users[e.RowIndex].email;
                        var userGender=Users[e.RowIndex].gender_name;
                        var userGenderId = Users[e.RowIndex].gender_id;
                        var userPhone = Users[e.RowIndex].phone;
                        var userTaj = Users[e.RowIndex].taj;
                        var userAddress = Users[e.RowIndex].address;
                        var userBirthdate = Users[e.RowIndex].birthdate;
                        var userHasTB = Users[e.RowIndex].hasTB;

                        OpenUserModify(userId, userName, userEmail, userGender, userGenderId,userPhone, userTaj, userAddress, userBirthdate, userHasTB);
                        GetWebUsers();

                        break;

                    default:
                        // Ha nem egy ismert oszlopra kattintottak, nem történik semmi
                        break;
                }
            }
        }

        //Web felhasználó törlése
        private async Task DeleteWebUser(int user_id)
        {
            var data = new { user_id };
            await SendRequestAsync(HttpMethod.Delete, "/api/v1/api.php/users/Delete", data);
        }

        //Web felhasználó adatainak módosítása (User_Modify form megnyitása)
        private void OpenUserModify(dynamic userId, dynamic userName, dynamic userEmail, dynamic userGender, dynamic userGenderId, dynamic userPhone, dynamic userTaj, dynamic userAddress, dynamic userBirthdate, dynamic userHasTB)
        {
            Main mainForm = this.ParentForm as Main;

            // Új példány létrehozása és adatátadás a konstruktoron keresztül
            User_Modify user_modify = new User_Modify(userId, userName, userEmail, userGender, userGenderId, userPhone, userTaj, userAddress, userBirthdate, userHasTB);

            // Form megnyitása modálisan
            user_modify.ShowDialog();
        }
    }
}
