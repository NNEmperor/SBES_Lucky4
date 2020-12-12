using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public sealed class Singleton
    {
        private static Singleton instance = null;
        public object Signal { get; set; }
        public List<int> DrawnNumbers { get; set; }
        private static readonly object padlock = new object();
        public IServer proxy = null;
        public int RoundNumber;
        public int WinnerCount;
        public int ActivePlayers;
        public bool CanBet;

        Singleton()
        {
            DrawnNumbers = new List<int>();
            Signal = new object();
            RoundNumber = 1;
            WinnerCount = 0;
            ActivePlayers = 0;
            CanBet = true;

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/Server";
            ChannelFactory<IServer> factory = new ChannelFactory<IServer>(binding, new EndpointAddress(address));
            proxy = factory.CreateChannel();

        }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }
        }
    }
}
