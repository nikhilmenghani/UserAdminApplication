using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserAdminApp
{
    static class Program
    {
        public static bool OpenMainUIFlag = false;
        public static string configPath = @"database.config";
        public static string connString = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (XmlConfig.ConnectionString == null)
            {
                Application.Run(new ServerConfigForm());
                if (OpenMainUIFlag)
                    Application.Run(new AppUI());
            }
            else
            {
                Application.Run(new AppUI());
            }
        }
    }
}
