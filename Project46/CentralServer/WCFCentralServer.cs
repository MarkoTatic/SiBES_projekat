using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralServer
{
    public class WCFCentralServer : IWCFCentralServer
    {
        private static int ClientCounter = 0;

        public void Disconnect(string sid)
        {
            try
            {
                Audit.ClientDisconectedSuccesffuly(sid);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
                //throw new ArgumentException(ae.Message);
            }
            DBClients.ConnectedClients.Remove(sid);
        }

        public string GenerateSecretKey(string publicKey)
        {
            RSA_Asimm_Algorithm_S rsa = new RSA_Asimm_Algorithm_S();

            return rsa.Encryption(publicKey);
        }

        public string GetConnectedClients()
        {
            string jsonString = JsonConvert.SerializeObject(DBClients.ConnectedClients);
            return jsonString;
        }

        public string Connect(string name, string sid)
        {
            try
            {
                Audit.ClientConnectedSuccesffuly(name, sid);
            }catch(ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
                //throw new ArgumentException(ae.Message);
            }

            ClientCounter += 1;
            User user = new User(Formatter.ParseName(name), sid);
            user.Counter = ClientCounter;
            DBClients.ConnectedClients.Add(user.SID, user);

            return ClientCounter.ToString();
        }
    }
}
