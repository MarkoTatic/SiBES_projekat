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
        public string GetConnectedClients()
        {
            string jsonString = JsonConvert.SerializeObject(DBClients.ConnectedClients);
            return jsonString;
        }

        public string TestConnection()
        {
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            Console.WriteLine("Name {0}", identity.Name);
            Console.WriteLine("IsAuthenticated {0}", identity.IsAuthenticated);
            Console.WriteLine("AuthenticationType {0}", identity.AuthenticationType);

            WindowsIdentity winIdentity = identity as WindowsIdentity;
            Console.WriteLine("Security Identifier (SID) {0}", winIdentity.User); // ovo ne moze preko IIentity id=nterfejsa jer je Windows-specific

            WCFCentralServer.ClientCounter += 1;
            User user = new User(Formatter.ParseName(identity.Name), winIdentity.User.ToString());
            DBClients.ConnectedClients.Add(user);

            return WCFCentralServer.ClientCounter.ToString();
        }
    }
}
