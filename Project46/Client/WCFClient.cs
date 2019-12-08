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

        public void printInfo()
        {
            try
            {
                factory.printInfo();
            } catch(Exception e)
            {
                Console.WriteLine("Exception message: " + e.Message);
            }
        }
    }
}
