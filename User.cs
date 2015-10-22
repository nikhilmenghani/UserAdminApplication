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
    //[XmlRootAttribute("User", Namespace = "", IsNullable = false)]
    public class User
    {
        public string userName = null;
        public string fullName = null;
        public string email = null;
        public string password = "";
        public string xmlData = null;
        public string custom = null;
        [XmlIgnoreAttribute()]
        public int partyId;
        [XmlIgnoreAttribute()]
        public int status = 0;
        public string passwordExpiration = null; //in database datatype is smalldatetime
        public string accountExpiration = null; //in database datatype is smalldatetime
        [XmlIgnoreAttribute()]
        public int failedLogins = 0;
        public string lastFailedAttempt = null; //in database datatype is smalldatetime
        [XmlIgnoreAttribute()]
        public string xml;

        [XmlIgnoreAttribute()]
        public string output;

        [XmlArray("Entities")]
        public List<Entity> entities = new List<Entity>();
    }
}
