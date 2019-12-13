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
        string GenerateRSAKeys();

        [OperationContract]
        void SendSecretKey(string key);
        
        [OperationContract]
        void LogMessage(byte[] message, byte[] sender, byte[] reciever);
    }
}
