using MonitoringCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class MonitoringChannel : ChannelFactory<IMonitoring>, IMonitoring, IDisposable
    {
        IMonitoring factory;

        public MonitoringChannel(NetTcpBinding binding,
                        EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
    }

        public void LogMessage(string message, string sender, string reciever)
        {
            try
            {
                factory.LogMessage(message, sender, reciever);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
