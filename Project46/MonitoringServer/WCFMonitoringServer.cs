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
        private string decriptedSecretKey;
        private string path = "Logging.txt";
        RSA_Asimm_Algorithm_M rsa = new RSA_Asimm_Algorithm_M();

        public string GenerateRSAKeys()
        {
            string publicRSAKey = rsa.GenerateKeys();
            return publicRSAKey;
        }

        public void SendSecretKey(string key)
        {
            decriptedSecretKey = rsa.DecryptData(key);
        }


        public void LogMessage(byte[] message, byte[] sender, byte[] reciever)
        {
            string decryptedSender = AES_DECRYPTION.DecryptData(sender, decriptedSecretKey);
            string decryptedReciever = AES_DECRYPTION.DecryptData(reciever, decriptedSecretKey);

            forLogging += decryptedSender + " to " + decryptedReciever;
            forLogging += ":[" + DateTime.Now.ToShortTimeString() + "]\t";

            foreach (var item in message)
            {
                forLogging += item;
            }
            
            forLogging += "\n";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.Write(forLogging);
                }

            }
            else if (File.Exists(path))
            {
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.Write(forLogging);
                }
            }

        }
    }
}
