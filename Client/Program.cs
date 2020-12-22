using Common;
using Manager;
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
            byte[] secretKey;
            byte[] IV;

            List<byte[]> list = proxy.Connect();
            secretKey = list[0];
            IV = list[1];

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
                //byte[] encryptedTicket = AES_Algorithm.EncryptMessage_Aes(Formatter.TicketToString(newTicket), secretKey, IV);
                //byte[] encryptedResults = proxy.RegisterForOneRound(encryptedTicket);
                //string decryptedResults = AES_Algorithm.DecryptMessage_Aes(encryptedResults, secretKey, IV);
                //Results results = Formatter.GetResults(decryptedResults) as Results;
                string encryptedTicket = AES_Algorithm.EncryptMessage_Aes(Formatter.TicketToString(newTicket), secretKey, IV);
                string encryptedResults = proxy.RegisterForOneRound(encryptedTicket);
                string decryptedResults = AES_Algorithm.DecryptMessage_Aes(encryptedResults, secretKey, IV);
                Results results = Formatter.GetResults(decryptedResults);

                if (results.Won == true && results.Credits == -1)
                {
                    Console.WriteLine("Sign up time for this round has ended. New one is starting shortly!");
                }
                else
                {
                    Console.WriteLine("Drawn Numbers: ");
                    for(int i=0; i < 5; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (results.Numbers[i * 7 + j] < 10)
                            {
                                Console.Write(" " + results.Numbers[i * 7 + j] + " ");
                            }
                            else
                            {
                                Console.Write(results.Numbers[i * 7 + j] + " ");
                            }
                        }
                        Console.WriteLine("");
                    }
                    if (results.Won == true)
                    {
                        Console.WriteLine("CONGRATULATIONS! You won " + results.Credits*3 + " credits!");
                    }
                    else
                    {
                        Console.WriteLine("You lost " + results.Credits + " credits, better luck next time!");
                    }
                }
            }
        }
    }
}
