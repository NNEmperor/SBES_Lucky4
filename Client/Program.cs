using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Lucky6";
            ChannelFactory<IService> factory = new ChannelFactory<IService>(binding, new EndpointAddress(address));
            IService proxy = factory.CreateChannel();
            while (true)
            {
                List<int> numbers = new List<int>();
                int temp;
                Console.WriteLine("Select 6 numbers:");
                for (int i = 1; i <= 6; i++)
                {
                    while (!Int32.TryParse(Console.ReadLine(), out temp) || !(temp > 0 && temp < 49) || numbers.IndexOf(temp) != -1)
                    {
                        Console.WriteLine("Please insert a valid integer");
                    }
                    numbers.Add(temp);
                }
                Console.WriteLine("Bet:");
                while (!Int32.TryParse(Console.ReadLine(), out temp))
                {
                    Console.WriteLine("Please insert a valid integer");
                }
                Ticket newTicket = new Ticket(numbers, temp);
                Results results = proxy.RegisterForOneRound(newTicket);
                Console.WriteLine(results.Won.ToString());
                foreach(int x in results.Numbers)
                {
                    Console.WriteLine(x);
                }
            }
        }
    }
}
