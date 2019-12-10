using Common;
using Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{

    public class WCFClient : ChannelFactory<IWCFCentralServer>, IWCFCentralServer, IDisposable
    {
        IWCFCentralServer factory;

        public WCFClient(NetTcpBinding binding,
                         EndpointAddress address) : base(binding, address)
        {
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            string cltCertCN = Formatter.ParseName(identity.Name);
						
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			/// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            factory = this.CreateChannel();
        }

        public void Disconnect(string sid)
        {
            try
            {
                factory.Disconnect(sid);
            } catch(FaultException e)
            {
                throw new FaultException(e.Message);
            }
        }

        public string GetConnectedClients()
        {
            string users = string.Empty;
            try
            {
                users = factory.GetConnectedClients();
            }
            catch (FaultException e)
            {
                throw new FaultException(e.Message);
            }

            return users;
        }

        public string TestConnection(string name, string sid)
        {
            string retVal = string.Empty;
            try
            {
               retVal = factory.TestConnection(name, sid); 
            } catch(Exception e)
            {
                Console.WriteLine("Test connection failed. Exception details: " + e.Message);
                retVal = "-1";
            }
            return retVal;
        }
    }
}
