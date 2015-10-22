using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdminApp
{
    public class Role
    {
        public string role;
        public string entityName; //this will help in identifying which entity this role is associated with
        public string userName; //this will help in identifying which user this role is assigned to

        public string instruction; //using this variable here as this will distinct all the rows.
    }
}
