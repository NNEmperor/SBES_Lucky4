using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/Server";
            ServiceHost host = new ServiceHost(typeof(Server));
            host.AddServiceEndpoint(typeof(IServer), binding, address);
            host.Open();
            Console.WriteLine("Lucky6 Remote servers are Running...");


            Console.Read();

        }
    }
}
