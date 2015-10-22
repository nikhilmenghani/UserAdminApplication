using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UserAdminApp
{
    public static class Extension
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    IndentChars = "\t",
                    NewLineOnAttributes = true
                }))
                {

                    xmlserializer.Serialize(writer, value, ns);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static void writeStringToPath(string path, string str)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(str);
            writer.Close();
        }

        public static string decodeAddUserLog(string encodedString)
        {
            string[] encodedForm = encodedString.Split('|');
            if (!encodedForm[1].Contains(';'))
            {
                return encodedForm[0] + Environment.NewLine;
            }
            string encodedStatus = encodedForm[0];
            string[] encodedData = encodedForm[1].Split(';');
            string log = "";
            int length = encodedStatus.Length;
            for (int i = 0; i < length; i++)
            {
                string tempStr = "";
                string[] data = encodedData[i].Split(',');
                switch (encodedStatus[i])
                {
                    case '1':
                        tempStr = "User " + data[0] + " Added, ";
                        tempStr += "Entity " + data[1] + " Associated, ";
                        tempStr += "Role " + data[2] + " Assigned.." + Environment.NewLine;
                        break;
                    case '2':
                        tempStr = "New Entity " + data[0] + " Associated, ";
                        tempStr += "Role " + data[1] + " Assigned.." + Environment.NewLine;
                        break;
                    case '3':
                        tempStr = "New Role " + data[1] + " Assigned to Entity " + data[0] + ".." + Environment.NewLine;
                        break;
                    case '4':
                        if (!log.Contains("User Added Previously"))
                            tempStr = "User Added Previously.." + Environment.NewLine;
                        break;
                    case '5':
                        if (!log.Contains("User Already Exists"))
                            tempStr = "User Already Exists.." + Environment.NewLine;
                        break;
                }
                if (!tempStr.Equals(""))
                {
                    log += tempStr;
                }
            }
            return log;
        }

        public static string decodeDeleteUserLog(string decodedString)
        {

            string[] decodedForm = decodedString.Split('|');
            string decodedStatus = decodedForm[0];
            string[] decodedData = decodedForm[1].Split(';');
            string log = "";
            int length = decodedStatus.Length;
            if (!decodedForm[1].Contains(';') && !decodedData[0].Equals(""))
            {
                return decodedForm[0] + Environment.NewLine;
            }
            for (int i = 0; i < length; i++)
            {
                string tempStr = "";
                string data = decodedData[i];
                switch (decodedStatus[i])
                {
                    case '0':
                        tempStr += "User Does not exists.." + Environment.NewLine;
                        break;
                    case '1':
                        tempStr += "User deleted.." + Environment.NewLine;
                        break;
                    case '2':
                        tempStr += "User already deleted.." + Environment.NewLine;
                        break;
                    case '3':
                        tempStr += "User deactivated.." + Environment.NewLine;
                        break;
                    case '4':
                        tempStr += "User already deactivated.." + Environment.NewLine;
                        break;
                    case '5':
                        tempStr += "Entity " + data + " removed.." + Environment.NewLine;
                        break;
                    case '6':
                        tempStr += "Entity " + data + " does not exists or already removed.." + Environment.NewLine;
                        break;
                    case '7':
                        tempStr += "Role " + data + " removed.." + Environment.NewLine;
                        break;
                    case '8':
                        tempStr += "Role " + data + " already removed.." + Environment.NewLine;
                        break;
                    case '9':
                        tempStr += "No Action Performed, Probably Wrong Instruction Passed.." + Environment.NewLine;
                        break;
                }
                if (!tempStr.Equals(""))
                {
                    log += tempStr;
                }
            }
            return log;
        }
    }
}
