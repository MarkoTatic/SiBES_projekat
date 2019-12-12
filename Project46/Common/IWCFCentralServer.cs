using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IWCFCentralServer
    {
        [OperationContract]
        string Connect(string name, string sid);

        [OperationContract]
        string GetConnectedClients();

        [OperationContract]
        void Disconnect(string sid);

        [OperationContract]
        string GenerateSecretKey(string publicKey);
    }
}
