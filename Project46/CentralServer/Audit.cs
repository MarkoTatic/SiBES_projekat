using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Application";
        const string LogName = "Application";
        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void ClientConnectedSuccesffuly(string userName, string sid)
        {
            string logMessage = "Client with name:" + userName + " and SID:" + sid + " connected successfuly on central server.";
            if (customLog != null)
            {
                customLog.WriteEntry(logMessage);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event, to event log.");
            }
        }

        public static void ClientDisconectedSuccesffuly(string sid)
        {
            string logMessage = "Client with SID:" + sid + " disconected successfuly on central server.";
            if (customLog != null)
            {
                customLog.WriteEntry(logMessage);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event, to event log.");
            }
        }


        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
