using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string SID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Counter { get; set; }
        public User()
        {
            SID = null;
            Name = string.Empty;
        }

        public User(string name, string sid)
        {
            this.Name = name;
            this.SID = sid;
        }
    }
}
