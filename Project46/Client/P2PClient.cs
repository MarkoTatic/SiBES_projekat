using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class P2PClient : ChannelFactory<IP2PServer>, IP2PServer, IDisposable
    {
        IP2PServer factory;
        public P2PClient(NetTcpBinding binding,
                        EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }


        public void SendMessage(string myMessage)
        {
            try
            {
                factory.SendMessage(myMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
