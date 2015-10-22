using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdminApp
{
    public static class XmlConfig
    {
        private static string _connString;
        private static string configPath = Program.configPath;

        public static string ConnectionString
        {
            get
            {
                _connString = getConnectionString();
                return _connString;
            }
            set
            {
                _connString = value;
            }
        }

        private static string getConnectionString()
        {
            string connString = "";
            if (File.Exists(configPath))
            {
                ReadXml configFile = new ReadXml(configPath);
                connString += "Data Source=" + configFile.getElementName("InstanceName") + ";" +
                              "Initial Catalog=" + configFile.getElementName("DbName") + ";";
                if (configFile.getElementName("IntegratedSecurity").Equals("true"))
                {
                    connString += "Integrated Security=True";
                }
                else
                {
                    connString += "User ID=" + configFile.getElementName("UserName") + ";" +
                              "Password=" + configFile.getElementName("Password") + "";
                }
                Program.connString = connString;
                return connString;
            }
            else
            {
                return null;
            }
        }

    }
}
