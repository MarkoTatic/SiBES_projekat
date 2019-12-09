using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        //private static int i = 1;
        private static int cnt = 1;
        private static int Menu()
        {
            int val = -1;
            while (val != 1 && val != 0)
            {
                Console.WriteLine("Choose option by entering number: ");
                Console.WriteLine("0 - Close connection.");
                Console.WriteLine("1 - Preview connected clients.");
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

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5000/WCFCentralServer";
            string users = String.Empty;
            string clientId;
            WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address)));
            //IIdentity identity = Thread.CurrentPrincipal.Identity;
            try
            {
                clientId = proxy.TestConnection();
                Console.WriteLine("Current: peer_"+clientId);
                var peer1 = new WCFP2PTransport("WCF_P2P_" + clientId, "Peer_" + clientId);      //otvaramo p2p konekciju za ostale klijente
                Task.WaitAll(peer1.ChannelOpened);
            } catch (FaultException e)
            {
                throw new FaultException(e.Message);
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
                        throw new FaultException(e.Message);
                    }
                    DeserializeJson(users);
                    PrintConnectedClients();
                    int otherClient = ChooseClient();
                    if (otherClient == 0)
                        continue;
                    var peer2 = new WCFP2PTransport("WCF_P2P_" + otherClient, "Peer_" + clientId);
                    Task.WaitAll(peer2.ChannelOpened);
                    while (true)
                    {
                        Console.WriteLine("Enter message to send, type 'x' for disconection:");
                        string messageToSend = Console.ReadLine();
                        if (messageToSend.Equals("x") || messageToSend.Equals("X"))
                        {
                            break;
                        }
                        peer2.SendToPeer(messageToSend, "Peer_" + clientId);
                    }
                }
                if (m == 0)
                    break;
            }



            Console.WriteLine("Press any key to close..");
            Console.ReadKey();
        }

        private static void DeserializeJson(string users)
        {
            DBClients.connectedClients.Clear();
            List<User> listUsers = JsonConvert.DeserializeObject<List<User>>(users);
            //int i = 1;
            foreach (User item in listUsers)
            {
                DBClients.connectedClients.Add(cnt, item);
                cnt++;
            }
        }

        private static void PrintConnectedClients()
        {
            int i = 1;
            Console.WriteLine("Connected clients at the moment are: ");
            Console.WriteLine("------------------------------------");
            foreach (User item in DBClients.connectedClients.Values)
            {
                Console.WriteLine(i +". "+ item.Name + ", SID: " + item.SID);
                i++;
            }
            Console.WriteLine("------------------------------------");
        }

        private static int ChooseClient()
        {
            Console.WriteLine("Please choose a client you want to connect with by picking ordinal number.");
            Console.WriteLine("0 - Quit option for sending messages.");
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
