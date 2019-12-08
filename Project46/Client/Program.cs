using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static int Menu()
        {
            Console.WriteLine("Choose option by entering number: ");
            Console.WriteLine("1 - Preview connected clients.");
            int val = 0;
            try
            {
                val = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("You didn't enter a valid option. Please try againg.");
            }
            return val;
        }

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5000/WCFCentralServer";
            List<User> connectedClients = new List<User>();
            string users = String.Empty;
            int m = Menu();
            WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address)));
            try
            {
                Console.WriteLine(proxy.TestConnection());
            } catch (FaultException e)
            {
                throw new FaultException(e.Message);
            }
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
            } 
            

            foreach (var client in connectedClients)
            {
                Console.WriteLine(client.SID);
                Console.WriteLine(client.Name);
                Console.WriteLine("-------------");
            }
            Console.WriteLine(users);

            Console.ReadKey();
        }
    }
}
