using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralServer
{
    public class WCFCentralServer : IWCFCentralServer
    {
        private static int ClientCounter = 0;

        public void Disconnect(string sid)
        {
            DBClients.ConnectedClients.Remove(sid);
        }

        public string GetConnectedClients()
        {
            string jsonString = JsonConvert.SerializeObject(DBClients.ConnectedClients);
            return jsonString;
        }

        public string TestConnection(string name, string sid)
        {
            ClientCounter += 1;
            User user = new User(Formatter.ParseName(name), sid);
            user.Counter = ClientCounter;
            DBClients.ConnectedClients.Add(user.SID, user);

            return ClientCounter.ToString();
        }
    }
}
