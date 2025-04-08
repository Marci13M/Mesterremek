using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CareCompass
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            // Ellenőrzi, hogy újraindítás miatt indult-e a program
            bool isRestart = Environment.GetCommandLineArgs().Contains("-restart");
            bool ell;
            Mutex m = new Mutex(true, "CareCompassKey", out ell);
            if (!ell && !isRestart)
            {
                MessageBox.Show("Már elindult a program");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Theme beállítása
            ThemeManager.InitializeTheme();
            Application.Run(new Bejelentkezés());
        }
    }
}
