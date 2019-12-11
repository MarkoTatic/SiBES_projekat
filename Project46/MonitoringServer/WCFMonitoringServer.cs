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
        private static string forLogging;
        private string encriptedSecretKey;
        public void LogMessage(byte[] message, byte[] sender, byte[] reciever)
        {
            string decryptedSender = AES_DECRYPTION.DecryptData(sender, encriptedSecretKey);
            string decryptedReciever = AES_DECRYPTION.DecryptData(reciever, encriptedSecretKey);

            forLogging += decryptedSender + " to " + decryptedReciever;
            forLogging += ":[" + DateTime.Now.ToShortTimeString() + "]\t";

            foreach (var item in message)
            {
                forLogging += item;
            }
            
            forLogging += "\n";
            using (StreamWriter sw = new StreamWriter("Logging.txt"))
            {
                sw.Write(forLogging);
            }

        }

        public void SendSecretKey(string key)
        {
            encriptedSecretKey = key;
        }
    }
}
