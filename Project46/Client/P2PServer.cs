using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class P2PServer : IP2PServer
    {
        public void SendMessage(string myMessage)
        {
            Console.WriteLine(myMessage);
        }
    }
}
