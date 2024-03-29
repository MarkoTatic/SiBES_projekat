﻿using Common;
using Manager;
using MonitoringCommon;
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
    public class Program
    {
        private static int Menu()
        {
            int val = -1;
            while (val != 1 && val != 0)
            {
                Console.WriteLine("Choose option by entering number: ");
                Console.WriteLine("\t0 - Close connection.");
                Console.WriteLine("\t1 - Preview connected clients.");
                try
                {
                    val = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("You didn't enter a valid option. Please try again.");
                }
            }
            return val;
        }

        static int Main(string[] args)
        {
            string users = String.Empty;
            string clientId;
            int otherClient;
            string encryptedSecretKey = String.Empty;
            string decryptedSecretKey;
            RSA_Asimm_Algorithm_C rsa = new RSA_Asimm_Algorithm_C();

            string publicKey = rsa.GenerateKeys();

            WCFClient proxy = BindToCentralServer();
            
            Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;
            string senderName = Formatter.ParseName(identity.Name);
            int peerServicePort = 6000;
            try
            {
                clientId = proxy.Connect(identity.Name, winIdentity.User.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection failed. \nDetails: " + e.Message);
                Console.WriteLine("Press any key to close connection. ");
                Console.ReadKey();
                proxy.Abort();
                return 0;
            }

            peerServicePort += Int32.Parse(clientId);
            Console.WriteLine("Current: peer_" + clientId);

            ServiceHost host = OpenPeerService(peerServicePort);
            MonitoringChannel proxyMonitoring = OpenMonitoringChannel();    //open channel for logging all messages
            try
            {
                encryptedSecretKey = proxy.GenerateSecretKey(publicKey); //klijent dobija kriptovan tajni kljuc
            }
            catch (Exception e)
            {
                Console.WriteLine("Encryption of secret key failed. \nDetails: " + e.Message);
                Console.WriteLine("Press any key to close connection. ");
                Console.ReadKey();
                proxy.Abort();
                return 0;
            }
            decryptedSecretKey = rsa.DecryptData(encryptedSecretKey);

            string publicKeyFromMonitoring = String.Empty;
          
            try
            {
                publicKeyFromMonitoring = proxyMonitoring.GenerateRSAKeys();
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting public RSA key from monitoring failed. \nDetails: " + e.Message);
            }

            string encryptedSecretKeyWithMonitoringKey = rsa.EncryptData(publicKeyFromMonitoring, decryptedSecretKey);

            try
            {
                proxyMonitoring.SendSecretKey(encryptedSecretKeyWithMonitoringKey);
            }
            catch (FaultException e)
            {
                Console.WriteLine("Sending secret key to monitoring failed. \nDetails: " + e.Message);
            }

            while (true)
            {
                int m = Menu();
                if (m == 1)
                {
                    try
                    {
                        users = proxy.GetConnectedClients();
                    }
                    catch (FaultException e)
                    {
                        Console.WriteLine("Getting list of connected clients from central server failed. \nDetails: " + e.Message);
                    }
                    DeserializeJson(users);
                    PrintConnectedClients();
                    otherClient = ChooseClient();//otherUser i otherClient se ne podudaraju ali otherUser = dict vrednost sa keyom otherClient
                   
                    if (otherClient == 0)
                        continue;
                    User otherUser = DBClients.connectedClients[otherClient];
                    if (otherUser.Counter == Int32.Parse(clientId))
                    {
                        Console.WriteLine("This is your account. Please choose again.");
                        continue;
                    }
                    Peer proxyPeerClient = OpenPeerClient(otherUser.Counter);
                    while (true)
                    {
                        Console.WriteLine("Enter message to send [type 'x' for disconection]:");
                        string messageToSend = Console.ReadLine();
                        if (messageToSend.Equals("x") || messageToSend.Equals("X"))
                        {
                            break;
                        }
                        try
                        {
                            proxyPeerClient.SendMessage(messageToSend);
                        }catch(Exception e)
                        {
                            Console.WriteLine("Sending message to client failed. \nDetails: " + e.Message);
                            break;
                        }
                        byte[] encryptedMessage = AES_ENCRYPTION.EncryptFile(messageToSend, decryptedSecretKey);
                        byte[] encryptedSenderName = AES_ENCRYPTION.EncryptFile(senderName, decryptedSecretKey);
                        byte[] encryptedRecieverName = AES_ENCRYPTION.EncryptFile(otherUser.Name, decryptedSecretKey);
                        try
                        {
                            proxyMonitoring.LogMessage(encryptedMessage, encryptedSenderName, encryptedRecieverName);
                        }
                        catch (FaultException e)
                        {
                            Console.WriteLine("Logging message on server failed. \nDetails: " + e.Message);
                            break;
                        }
                    }
                    proxyPeerClient.Abort();
                }
                if (m == 0)
                    break;
            }

            Console.WriteLine("Press any key to close..");
            Console.ReadKey();
            try
            {
                proxy.Disconnect(winIdentity.User.ToString());
            } catch(FaultException e)
            {
                Console.WriteLine("Disconnection failed. \nDetails: " + e.Message);
            }
            host.Close();
            proxy.Close();
            return 0;
        }
        #region opening_channels
        private static WCFClient BindToCentralServer()
        {
            //string srvCertCN = "wcfServer1";
            string srvCertCN = "WCFService";
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:5000/WCFCentralServer";

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress addressWithCert = new EndpointAddress(new Uri(address),
                                      new X509CertificateEndpointIdentity(srvCert));

            WCFClient proxy = new WCFClient(binding, addressWithCert);
            return proxy;
        }

        private static ServiceHost OpenPeerService(int peerServicePort)
        {
            NetTcpBinding bindingPeerService = new NetTcpBinding();
            string addressPeerService = "net.tcp://localhost:" + peerServicePort.ToString() + "/P2PTransport";
            ServiceHost host = new ServiceHost(typeof(P2PTransport));

            bindingPeerService.Security.Mode = SecurityMode.Transport;
            bindingPeerService.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingPeerService.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            host.AddServiceEndpoint(typeof(IP2PTransport), bindingPeerService, addressPeerService);
            try
            {
                host.Open();
                Console.WriteLine("You have opened connection to other clients.");
            } catch(Exception e)
            {
                Console.WriteLine("Failed opening host connection. \nDetails: " + e.Message);
            }

            return host;
        }

        private static Peer OpenPeerClient(int otherClient)
        {
            int peerClientPort = 6000 + otherClient;
            NetTcpBinding bindingPeerClient = new NetTcpBinding();
            string addressPeerClient = "net.tcp://localhost:" + peerClientPort.ToString() + "/P2PTransport";

            bindingPeerClient.Security.Mode = SecurityMode.Transport;
            bindingPeerClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingPeerClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            //EndpointAddress endpointAdress = new EndpointAddress(new Uri(addressPeerClient));

            Peer proxyPeerClient = new Peer(bindingPeerClient, addressPeerClient);

            return proxyPeerClient;
        }

        private static MonitoringChannel OpenMonitoringChannel()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:7000/WCFMonitoringServer";

            MonitoringChannel proxy = new MonitoringChannel(binding, new EndpointAddress(address));

            return proxy;
        }
        #endregion

        private static void DeserializeJson(string users)
        {
            DBClients.connectedClients.Clear();
            Dictionary<string,User> dictUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(users);
            int cnt = 1;
            foreach (User item in dictUsers.Values)
            {
                DBClients.connectedClients.Add(cnt, item);
                cnt++;
            }
        }

        private static void PrintConnectedClients()
        {
            int i = 1;
            Console.WriteLine("Connected clients at the moment are: ");
            Console.WriteLine("\t------------------------------------");
            foreach (User item in DBClients.connectedClients.Values)
            {
                Console.WriteLine("\t" + i + ". " + item.Name + ", SID: " + item.SID);
                i++;
            }
            Console.WriteLine("\t------------------------------------");
        }

        private static int ChooseClient()
        {
            Console.WriteLine("Please choose a client you want to connect with by picking ordinal number.");
            Console.WriteLine("\t0 - Back");
            int retVal;
            while (true)
            {
                try
                {
                    retVal = Int32.Parse(Console.ReadLine());
                    if (retVal > DBClients.connectedClients.Count)
                    {
                        Console.WriteLine("Please enter a valid number from the list.");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number from the list.");
                }
            }
            return retVal;
        }
    }
}
