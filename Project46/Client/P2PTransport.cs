using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class P2PTransport : IP2PTransport
    {
        public void SendMessage(string myMessage)
        {
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            Console.WriteLine("IsAuthenticated: "+ identity.IsAuthenticated);
            Console.WriteLine("Type: "+identity.AuthenticationType);
            Console.WriteLine("Message you received: "+ myMessage);
        }
    }
}
