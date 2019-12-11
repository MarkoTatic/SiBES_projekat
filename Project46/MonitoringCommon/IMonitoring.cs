using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringCommon
{
    [ServiceContract]
    public interface IMonitoring
    {
        [OperationContract]
        void LogMessage(byte[] message, byte[] sender, byte[] reciever);

        [OperationContract]
        void SendSecretKey(string key);
    }
}
