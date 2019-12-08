using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class WCFCentralServer : IWCFCentralServer
    {
        public void printInfo()
        {
            Console.WriteLine("Connected to new client.");
        }
    }
}
