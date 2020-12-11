﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Draw draw = new Draw();
            Task task = new Task(new Action(draw.Start));
            task.Start();
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Lucky6";
            ServiceHost host = new ServiceHost(typeof(Service));
            host.AddServiceEndpoint(typeof(IService), binding, address);
            host.Open();
            Console.WriteLine("Lucky6 Service is Running...");


            string temp = "";
            int selectedRound = 0;
            while (true)
            {
                Console.WriteLine("For exiting application enter 'exit'");
                Console.WriteLine("Select a round: ");

                temp = Console.ReadLine();
                if (temp == "exit")
                    break;

                int.TryParse(temp, out selectedRound);

                Console.WriteLine(Singleton.Instance.proxy.RequestWinnersForOneRound(selectedRound));
            }


            Console.ReadLine();


            host.Close();
        }

    }

}
