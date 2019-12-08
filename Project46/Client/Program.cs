﻿using Common;
using Newtonsoft.Json;
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
        private static int i = 1;
        private static int cnt = 1;
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
                DeserializeJson(users);
                int otherClient = PrintConnectedClients();
            }

           

            Console.ReadKey();
        }

        private static void DeserializeJson(string users)
        {
            List<User> listUsers = JsonConvert.DeserializeObject<List<User>>(users);
            //int i = 1;
            foreach (User item in listUsers)
            {
                DBClients.connectedClients.Add(cnt, item);
                cnt++;
            }
        }

        private static int PrintConnectedClients()
        {
            Console.WriteLine("Connected clients at the moment are: ");
            foreach (User item in DBClients.connectedClients.Values)
            {
                Console.WriteLine(i +". "+ item.Name + ", SID: " + item.SID);
                i++;
            }
            Console.WriteLine("Please choose a client you want to connect with by picking ordinal number.");
            int retVal = Int32.Parse(Console.ReadLine());
            return retVal;
        }
    }
}
