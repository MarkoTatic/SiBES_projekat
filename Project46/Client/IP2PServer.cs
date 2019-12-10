using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [ServiceContract]
    public interface IP2PServer
    {
        [OperationContract]
        void SendMessage(string myMessage);
    }
}
