using MonitoringCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:7000/WCFMonitoringServer";

            ServiceHost host = new ServiceHost(typeof(WCFMonitoringServer));
            host.AddServiceEndpoint(typeof(IMonitoring), binding, address);

            host.Open();
            Console.WriteLine("Logging chat..");

            Console.ReadKey();
            host.Close();
        }
    }
}
