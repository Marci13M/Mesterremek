using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using System.Net.Http;
using System.Drawing.Drawing2D;
using CareCompass.Customs;
using System.Windows.Forms.DataVisualization.Charting;
//using System.Drawing2D;


namespace CareCompass
{
    public static class ThemeManager
    {
        
        public static string CurrentTheme { get; private set; }
        

        // A Windows témája alapján beállítja a program témáját
        public static void InitializeTheme()
        {
            if (IsWindowsThemeDark())
            {
                SetTheme("Dark");
            }
            else
            {
                SetTheme("Light");
            }
        }

        // Ellenőrzi, hogy a Windows téma sötét-e
        private static bool IsWindowsThemeDark()
        {
            // A Windows 10/11 sötét témájának ellenőrzése
            try
            {
                var registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (registryKey != null)
                {
                    var themeValue = registryKey.GetValue("AppsUseLightTheme");
                    if (themeValue != null && (int)themeValue == 0)
                    {
                        return true; // Sötét téma
                    }
                }
            }
            catch
            {
                // Hibakezelés, ha a regisztrációs kulcs nem elérhető
            }
            return false; // Alapértelmezett: világos téma
        }

        //Téma beállítása
        public static void SetTheme(string theme)
        {
            CurrentTheme = theme;
            ApplyThemeToAllForms();
            // Gombcsoportok frissítése
            foreach (var form in _buttonGroups.Keys.ToList())
            {
                UpdateButtonGroupsForForm(form);
            }
        }

        //Ikonok meghívása (Menükben)
        private static void SetButtonIcon(Button button, string baseName)
        {
            // Projekt gyökérkönyvtárának elérési útja
            string rootDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            // Kép mappájának elérési útja
            string iconFolderPath = Path.Combine(rootDirectory, "Images", "Icons");
            // Kép fájlneve a témától függően
            string iconFileName = CurrentTheme == "Light" ? "Black" : "White";
            string imageFileName = CurrentTheme == "Light" ? $"{baseName}_Black.png" : $"{baseName}_White.png";
            string imagePath = Path.Combine(iconFolderPath, iconFileName, imageFileName);
            if (File.Exists(imagePath))
            {
                button.Image = Image.FromFile(imagePath);
            }
            else
            {
                Debug.WriteLine($"Icon file not found: {imagePath}");
            }
        }
        
        //Témák alkalmazása a megnyitott formok elemeire
        private static void ApplyThemeToAllForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                ApplyTheme(form);
            }
        }

        
        //Gomb hoverek
        #region Gomb hoverek
        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Store the original color for later use
                button.Tag = button.BackColor;
                // Change to blue when hovering
                button.BackColor = CurrentTheme == "Light" ? Color.FromArgb(133, 144, 162) : Color.FromArgb(44, 51, 58);
                button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
            }
        }

        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Restore the original color
                button.BackColor = button.Tag != null ? (Color)button.Tag : (CurrentTheme == "Light" ? Color.Transparent : Color.Transparent);
                button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
            }
        }


        #endregion


        #region Gombcsoport kezelés (statisztikák)
        private static readonly Dictionary<Form, Button> _activeButtons = new Dictionary<Form, Button>();
        private static readonly Dictionary<Form, List<Button>> _buttonGroups = new Dictionary<Form, List<Button>>();

        public static void RegisterButtonGroup(Form form, Button[] buttons, Button initialActiveButton = null)
        {
            if (!_buttonGroups.ContainsKey(form))
            {
                _buttonGroups[form] = new List<Button>();
            }

            foreach (var btn in buttons)
            {
                if (!_buttonGroups[form].Contains(btn))
                {
                    _buttonGroups[form].Add(btn);

                    btn.MouseEnter += (s, e) => HandleButtonHover((Button)s, form);
                    btn.MouseLeave += (s, e) => HandleButtonLeave((Button)s, form);
                    btn.Click += (s, e) => SetActiveButton((Button)s, form);
                }
            }

            if (initialActiveButton != null && buttons.Contains(initialActiveButton))
            {
                SetActiveButton(initialActiveButton, form);
            }
        }
        public static void CleanUpButtonGroups(Form form)
        {
            // Távolítsuk el a gombcsoportokat a memóriából
            if (_buttonGroups.ContainsKey(form))
            {
                foreach (var btn in _buttonGroups[form])
                {
                    // Eseménykezelők leiratkozása
                    btn.MouseEnter -= (s, e) => HandleButtonHover((Button)s, form);
                    btn.MouseLeave -= (s, e) => HandleButtonLeave((Button)s, form);
                    btn.Click -= (s, e) => SetActiveButton((Button)s, form);
                }
                _buttonGroups.Remove(form);
            }

            // Aktív gomb eltávolítása
            _activeButtons.Remove(form);
        }

        public static void SetActiveButton(Button button, Form form)
        {
            if (_buttonGroups.TryGetValue(form, out var buttons))
            {
                // Előző aktív gomb visszaállítása
                if (_activeButtons.TryGetValue(form, out var prevActive) && buttons.Contains(prevActive))
                {
                    ApplyButtonTheme(prevActive, isActive: false);
                }

                // Új aktív gomb beállítása
                ApplyButtonTheme(button, isActive: true);
                _activeButtons[form] = button;
            }
        }

        private static void HandleButtonHover(Button button, Form form)
        {
            if (!_activeButtons.ContainsKey(form) || _activeButtons[form] != button)
            {
                ApplyButtonTheme(button, isHovered: true);
            }
        }

        private static void HandleButtonLeave(Button button, Form form)
        {
            if (!_activeButtons.ContainsKey(form) || _activeButtons[form] != button)
            {
                ApplyButtonTheme(button, isActive: false);
            }
        }

        private static void ApplyButtonTheme(Button button, bool isActive = false, bool isHovered = false)
        {
            if (isActive)
            {
                button.BackColor = CurrentTheme == "Light"
                    ? Color.FromArgb(133, 144, 162)
                    : Color.FromArgb(86, 163, 166);
            }
            else if (isHovered)
            {
                button.BackColor = CurrentTheme == "Light"
                    ? Color.FromArgb(133, 144, 162)
                    : Color.FromArgb(86, 163, 166);
            }
            else
            {
                button.BackColor = CurrentTheme == "Light"
                    ? Color.FromArgb(179, 185, 196)
                    : Color.FromArgb(22, 26, 29);
            }

            button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
        }

        public static void UpdateButtonGroupsForForm(Form form)
        {
            if (_buttonGroups.TryGetValue(form, out var buttons))
            {
                foreach (var btn in buttons)
                {
                    bool isActive = _activeButtons.TryGetValue(form, out var activeBtn) && activeBtn == btn;
                    ApplyButtonTheme(btn, isActive);
                }
            }
        }

        public static Button GetActiveButton(Form form)
        {
            return _activeButtons.TryGetValue(form, out var activeButton) ? activeButton : null;
        }
        #endregion


        public static void ApplyTheme(Control control)
        {
            #region Formok
            if (control is Form form)
            {
                // Ha a form specifikus (pl. a MenüForm)
                if (form.Name == "Doctor_Menu" || form.Name == "Hospital_Menu" || form.Name == "Company_Menu" || form.Name == "Admin_Menu")
                {
                    form.BackColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                    form.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
                // Általános form színek
                else
                {
                    form.BackColor = CurrentTheme == "Light" ? Color.FromArgb(241, 242, 244) : Color.FromArgb(44, 51, 58);
                    form.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
            }
            #endregion

            #region Datagridview-ek
            else if (control is DataGridView dataGridView)
            {
                if (dataGridView.Name == "dGV_Nyitvatartas")
                {
                    dataGridView.BackgroundColor = Color.White;
                    dataGridView.DefaultCellStyle.ForeColor=Color.Black;
                }
                else
                {
                    // Alapértelmezett háttérszínek a témák alapján
                    Color backgroundColor = CurrentTheme == "Light" ? Color.FromArgb(241, 242, 244) : Color.FromArgb(44, 51, 58);
                    Color textColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                    Color firstColumnColor = CurrentTheme == "Light" ? Color.FromArgb(241, 242, 244) : Color.FromArgb(44, 51, 58);
                    Color newRowColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(50, 70, 80);

                    // DataGridView alapértelmezett beállítások
                    dataGridView.BackgroundColor = backgroundColor;
                    dataGridView.DefaultCellStyle.BackColor = backgroundColor;
                    dataGridView.DefaultCellStyle.ForeColor = textColor;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 180, 255); // Kiválasztott cella színe
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

                    // Betűtípus a tartalomhoz (cella adatainak)
                    dataGridView.DefaultCellStyle.Font = new Font("Calibri", 11);

                    // Az első oszlop színének beállítása
                    if (dataGridView.Columns.Count > 0)
                    {
                        DataGridViewColumn firstColumn = dataGridView.Columns[0];
                        firstColumn.DefaultCellStyle.BackColor = firstColumnColor;
                        firstColumn.DefaultCellStyle.ForeColor = textColor;
                        firstColumn.DefaultCellStyle.Font = new Font("Calibri", 11); // Első oszlop betűtípusa
                    }

                    // Új sorok háttérszíne
                    dataGridView.RowsDefaultCellStyle.BackColor = backgroundColor;
                    dataGridView.RowsDefaultCellStyle.ForeColor = textColor;

                    // Új sorok egyedi színe
                    dataGridView.AlternatingRowsDefaultCellStyle.BackColor = newRowColor;
                    dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = textColor;

                    // Oszlopfejlécek formázása
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = CurrentTheme == "Light" ? Color.FromArgb(200, 220, 240) : Color.FromArgb(50, 50, 50);
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
                    dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 11, FontStyle.Bold); // Fejlécek betűtípusa

                    // Gridline szín
                    dataGridView.GridColor = CurrentTheme == "Light" ? Color.FromArgb(200, 200, 200) : Color.FromArgb(70, 70, 70);
                }
            }


            #endregion

            #region PictureBox-ok
            //Picturebox
            else if (control is PictureBox pictureBox)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory; // A futási könyvtár
                string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                if (pictureBox.Name == "Pb_CCLogo")
                {
                    if (CurrentTheme == "Dark")
                    {
                        pictureBox.Image = Image.FromFile(basePath + "logó - fehér.png");
                    }
                    else
                    {
                        pictureBox.Image = Image.FromFile(basePath + "logó - fekete.png");
                    }
                }
            }
            #endregion

            #region Textboxok
            else if(control is TextBox textbox)
            {
                textbox.BackColor = CurrentTheme == "Light" ? Color.White : Color.White;
                textbox.Font = new Font("Calibri", textbox.Font.Size);
            }
            else if(control is CCTextBox textBox)
            {
                textBox.BackColor = Color.White;
                textBox.ForeColor= CurrentTheme == "Light" ?Color.Black : Color.Black;
            }
            #endregion

            #region Combobox
            else if(control is ComboBox combobox)
            {
                combobox.BackColor = CurrentTheme == "Light" ? Color.White : Color.White;
                combobox.Font = new Font("Calibri", combobox.Font.Size);
            }
            #endregion

            #region CheckBox
            else if (control is CheckBox togglebutton)
            {
                if(togglebutton.Name== "ccToggleButton1")
                {
                    togglebutton.Checked = CurrentTheme == "Light" ? false : true;
                }
            }
            #endregion

            #region Panelek
            //Panelek színe
            else if (control is Panel panel)
            {
                //Menükben a logo és a körülötte lévő panelek
                if (panel.Name == "Pn_Theme" || panel.Name == "Pn_LogoFent" || panel.Name == "Pn_LogoLent")
                {
                    panel.BackColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                }

                else if(panel.Name== "Pn_UserModositas")
                {
                    panel.Font = new Font("Calibri", 12);
                }
                else
                {
                    panel.BackColor = CurrentTheme == "Light" ? Color.FromArgb(241, 242, 244) : Color.FromArgb(44, 51, 58);
                }
            }
            #endregion

            #region Gombok (Button)
            // Gomb színek
            else if (control is Button button)
            {
                //Menü gombok
                if (button.Name== "Btn_Profil" || button.Name == "Btn_Naptar" || button.Name == "Btn_Statisztika" || button.Name=="Btn_Kijelentkezes" 
                    || button.Name=="Btn_Alkalmazottak" || button.Name == "Btn_Szolgaltatasok" || button.Name=="Btn_Korhaz" || button.Name=="Btn_Felhasznalok" 
                    || button.Name == "Btn_WebFelhasznalok" || button.Name == "Btn_Ceg" || button.Name == "Btn_Add") 
                {
                    button.BackColor = CurrentTheme == "Light" ? Color.Transparent : Color.Transparent;
                    button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;

                    //Hover kezelés
                    button.MouseEnter -= Button_MouseEnter;
                    button.MouseLeave -= Button_MouseLeave;
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;

                    //Ikonok beállítása
                    if (button.Name == "Btn_Profil")
                    {
                        SetButtonIcon(button, "User");
                    }
                    else if (button.Name == "Btn_Naptar")
                    {
                        SetButtonIcon(button, "Calendar");
                    }
                    else if (button.Name == "Btn_Statisztika")
                    {
                        SetButtonIcon(button, "Statistics");
                    }
                    else if (button.Name == "Btn_Kijelentkezes")
                    {
                        SetButtonIcon(button, "Logout");
                    }
                    else if (button.Name == "Btn_Alkalmazottak")
                    {
                        SetButtonIcon(button, "Doctor");
                    }
                    else if (button.Name == "Btn_Szolgaltatasok")
                    {
                        SetButtonIcon(button, "Services");
                    }
                    else if (button.Name == "Btn_Korhaz")
                    {
                        SetButtonIcon(button, "Hospital");
                    }
                    else if (button.Name == "Btn_Ceg")
                    {
                        SetButtonIcon(button, "Company");
                    }
                    else if (button.Name == "Btn_Add")
                    {
                        SetButtonIcon(button, "Add");
                    }
                    else if (button.Name == "Btn_Felhasznalok")
                    {
                        SetButtonIcon(button, "Admin");
                    }
                    else if (button.Name == "Btn_WebFelhasznalok")
                    {
                        SetButtonIcon(button, "Web");
                    }
                }
                //Statisztika gombok
                else if(button.Name== "Btn_Egyedi" || button.Name == "Btn_Ma" || button.Name == "Btn_Utolso7Nap"
                    || button.Name == "Btn_Utolso30Nap" || button.Name == "Btn_ElozoHonap" || button.Name== "Btn_OrvosValaszt" 
                    || button.Name== "Btn_KorhazValaszt" || button.Name== "Btn_CegValaszt")
                {
                    
                    // Alapértelmezett inaktív szín
                    button.BackColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                    button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;

                }

                //Profil 
                else if(button.Name== "Btn_CegProfil" || button.Name == "Btn_CegModerator")
                {
                    //A jelenleg kattintot gomb (aktív)
                    if (Application.OpenForms.OfType<Profil>().FirstOrDefault() is Profil cegprofil)
                    {
                        if (cegprofil.jelenlegiProfilGomb != null)
                        {
                            var btn = cegprofil.jelenlegiProfilGomb;
                            btn.BackColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(22, 26, 29);
                            btn.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                        }
                        else
                        {
                            button.BackColor = CurrentTheme == "Light" ? Color.Transparent : Color.Transparent;

                        }
                    }
                    button.ForeColor= CurrentTheme =="Light" ? Color.Black : Color.White;
                }
                

                //Statisztika OK gomb (egyedi)
                else if (button.Name == "Btn_OK")
                {
                    button.BackColor = CurrentTheme == "Light" ? Color.FromArgb(133, 144, 162) : Color.FromArgb(86, 163, 166);
                    button.ForeColor=Color.White;
                }
                // Alapértelmezett stílus
                else
                {
                    button.BackColor = CurrentTheme == "Light" ? Color.FromArgb(179, 185, 196) : Color.FromArgb(86, 163, 166);
                    button.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
            }
            #endregion

            #region Labelek
            //Szöveg színek (label)
            else if (control is Label label)
            {
                //User controll számok háttere és szám színe (lényegében user control label)
                if(label.Name== "Lb_UCNapok")
                {
                    label.BackColor = CurrentTheme == "Light" ? Color.LightGray : Color.FromArgb(45, 45, 48);
                    label.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
                //Hónapnév 
                if ( label.Name== "Lb_honapev")
                {
                    label.ForeColor = CurrentTheme == "Light" ? Color.White : Color.Black;
                }
                //Theme felirat (menüben)
                else if(label.Name == "Lb_Theme")
                {
                    label.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
                else
                {
                    label.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                }
            }
            #endregion

            #region Chart vezérlők
            else if (control is System.Windows.Forms.DataVisualization.Charting.Chart chart)
            {
                // Háttérszínek beállítása
                chart.BackColor = Color.Transparent;

                // Chart terület színei
                chart.ChartAreas[0].BackColor = CurrentTheme == "Light" ? Color.FromArgb(241, 242, 244) : Color.FromArgb(44, 51, 58);

                // Szövegszínek beállítása
                chart.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisX.TitleForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisY.TitleForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.Titles[0].ForeColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.Titles[0].Font = new Font("Calibri", 14);

                // Grid vonalak és tengelyek színe
                chart.ChartAreas[0].AxisX.MajorGrid.LineColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisY.MajorGrid.LineColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisX.LineColor = CurrentTheme == "Light" ? Color.Black : Color.White;
                chart.ChartAreas[0].AxisY.LineColor = CurrentTheme == "Light" ? Color.Black : Color.White;

                if (chart.Name == "chart3")
                {
                    // Series színek frissítése (opcionális)
                    foreach (var series in chart.Series)
                    {
                        // Itt állíthatod be a sorok színeit a témának megfelelően
                        series.Color = CurrentTheme == "Light" ? Color.FromArgb(98, 111, 134) : Color.FromArgb(86, 163, 166);

                        // Marker beállítása
                        series.MarkerStyle = MarkerStyle.Circle;
                        series.MarkerSize = 8;
                        series.MarkerColor = CurrentTheme == "Light" ? Color.FromArgb(86, 163, 166) : Color.White;
                    }
                }
                else
                {
                    // Series színek frissítése (opcionális)
                    foreach (var series in chart.Series)
                    {
                        // Itt állíthatod be a sorok színeit a témának megfelelően
                        series.Color = CurrentTheme == "Light" ? Color.FromArgb(98, 111, 134) : Color.FromArgb(86, 163, 166);
                    }
                }
            }
            #endregion

            #region Általános vezélrők
            // Általános vezérlők
            else
            {
                control.BackColor = CurrentTheme == "Light" ? Color.FromArgb(217, 217, 217) : Color.FromArgb(45, 45, 48);
                control.ForeColor = CurrentTheme == "Light" ? Color.Black : Color.Black;
            }
            #endregion

            // Gyermek vezérlők rekurzív kezelése
            foreach (Control childControl in control.Controls)
            {
                ApplyTheme(childControl);
            }
        }
    }
}
