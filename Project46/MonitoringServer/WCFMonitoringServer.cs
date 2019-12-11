using MonitoringCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServer
{
    public class WCFMonitoringServer : IMonitoring
    { 
        static string forLogging;
        public void LogMessage(string message, string sender, string reciever)
        {
            forLogging += sender + " to " + reciever;
            forLogging += ":["+ DateTime.Now.ToShortTimeString() + "]\t" + message + "\n";
            using (StreamWriter sw = new StreamWriter("Logging.txt"))
            {
                sw.Write(forLogging);
            }

        }
    }
}
