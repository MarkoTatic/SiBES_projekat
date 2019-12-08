using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class User
    {
        public SecurityIdentifier SID { get; set; }
        public string Name { get; set; }
    }
}
