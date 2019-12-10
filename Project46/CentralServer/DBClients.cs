using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class DBClients
    {
        public static Dictionary<string, User> ConnectedClients = new Dictionary<string, User>();
    }
}
