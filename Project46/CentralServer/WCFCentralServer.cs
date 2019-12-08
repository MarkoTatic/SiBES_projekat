﻿using Common;
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
        public static List<User> ConnectedClients = new List<User>();
        public List<User> GetConnectedClients()
        {
            return ConnectedClients;
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

            var User = new User { Name = Formatter.ParseName(identity.Name), SID = winIdentity.User };
            ConnectedClients.Add(User);

            return "Succesfully connected.";
        }
    }
}
