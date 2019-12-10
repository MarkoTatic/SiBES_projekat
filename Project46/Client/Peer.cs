using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Peer : ChannelFactory<IP2PTransport>, IP2PTransport, IDisposable
    {
        IP2PTransport factory;
        public Peer(NetTcpBinding binding,
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
