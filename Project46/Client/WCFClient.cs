using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFCentralServer>, IWCFCentralServer, IDisposable
    {
        IWCFCentralServer factory;

        public WCFClient(NetTcpBinding binding,
                         EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public List<User> GetConnectedClients()
        {
            List<User> connectedClients = new List<User>();
            try
            {
                connectedClients = factory.GetConnectedClients();
            } catch(Exception e)
            {
                Console.WriteLine("Exception details:" + e.Message);
            }
            return connectedClients;
        }

        public string TestConnection()
        {
            string retVal = string.Empty;
            try
            {
               retVal = factory.TestConnection(); 
            } catch(Exception e)
            {
                Console.WriteLine("Exception details: " + e.Message);
            }
            return retVal;
        }
    }
}
