using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5000/WCFCentralServer";

            ServiceHost host = new ServiceHost(typeof(WCFCentralServer));
            host.AddServiceEndpoint(typeof(IWCFCentralServer), binding, address);
            host.Open();

            Console.ReadKey();
            host.Close();
        }
    }
}
