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
        void LogMessage(string message, string sender, string reciever);
    }
}
