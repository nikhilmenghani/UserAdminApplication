using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UserAdminApp
{
    public class Entity
    {
        [XmlIgnoreAttribute()]
        public int entityId;
        public string userName;
        public string entityName;
        public int homeEntity;
        public int producer;
        public string role;
        [XmlArray("Roles")]
        public List<Role> roles = new List<Role>();
    }
}
