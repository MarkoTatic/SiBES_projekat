using Common;
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
        //private static List<User> ConnectedClients = new List<User>();
        public string GetConnectedClients()
        {
            string ret = string.Empty;
            foreach (var user in DBClients.ConnectedClients)
            {
                ret += user.Name;
                ret += "\n";
                ret += user.SID;
                ret += "\n";
            }
            return ret;
        }

        public string TestConnection()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            Console.WriteLine("Name {0}", identity.Name);
            Console.WriteLine("IsAuthenticated {0}", identity.IsAuthenticated);
            Console.WriteLine("AuthenticationType {0}", identity.AuthenticationType);

            WindowsIdentity winIdentity = identity as WindowsIdentity;
            Console.WriteLine("Security Identifier (SID) {0}", winIdentity.User); // ovo ne moze preko IIentity id=nterfejsa jer je Windows-specific

            //foreach (IdentityReference group in winIdentity.Groups)
            //{
            //    SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
            //    var name = sid.Translate(typeof(NTAccount));
            //    Console.WriteLine("{0}", name.ToString());
            //}

            User user = new User(Formatter.ParseName(identity.Name), winIdentity.User);
            DBClients.ConnectedClients.Add(user);

            return "Succesfully connected.";
        }
    }
}
