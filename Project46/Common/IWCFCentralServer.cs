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
        string TestConnection(string name, string sid);

        [OperationContract]
        string GetConnectedClients();
    }
}
