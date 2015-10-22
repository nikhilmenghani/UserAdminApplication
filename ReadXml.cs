using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UserAdminApp
{
    public class ReadXml
    {
        XmlDocument xmlDoc = null;
        XmlNodeList nodeList = null;

        public ReadXml(string path)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Server");
        }

        public string getElementName(string name)
        {
            string value = "";
            foreach (XmlNode node in nodeList)
            {
                switch (name)
                {
                    case "InstanceName":
                        value = node.SelectSingleNode("InstanceName").InnerText;
                        break;
                    case "DbName":
                        value = node.SelectSingleNode("DbName").InnerText;
                        break;
                    case "UserName":
                        value = node.SelectSingleNode("UserName").InnerText;
                        break;
                    case "Password":
                        value = node.SelectSingleNode("Password").InnerText;
                        break;
                    case "IntegratedSecurity":
                        value = node.SelectSingleNode("IntegratedSecurity").InnerText;
                        break;
                }
            }
            return value;
        }

    }
}
