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

        public string GenerateRSAKeys()
        {
            string publicRsaKey = String.Empty;
            try
            {
                publicRsaKey = factory.GenerateRSAKeys();
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return publicRsaKey;
        }

        public void LogMessage(byte[] message, byte[] sender, byte[] reciever)
        {
            try
            {
                factory.LogMessage(message, sender, reciever);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendSecretKey(string key)
        {
            try
            {
                factory.SendSecretKey(key);
            }
            catch (FaultException e)
            {
                throw new FaultException(e.Message);
            }
        }
    }
}
