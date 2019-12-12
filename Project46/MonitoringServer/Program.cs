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

            try
            {
                host.Open();
                Console.WriteLine("Ready to logg messages.\nPress <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
            }
        }
    }
}
