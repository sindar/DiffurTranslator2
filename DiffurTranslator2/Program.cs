using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DiffurTranslator2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm MainWin = new MainForm(Application.StartupPath);
            Application.Run(MainWin);
            
        }
    }
}
