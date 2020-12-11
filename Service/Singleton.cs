using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class Singleton
    {
        //moramo nekako da pristupamo izvucenim brojevima direktno iz Service, stavio sam singleton(a tu je i Signal da bi svi mogli da pristupaju tom locku)
        private static Singleton instance = null;
        public object Signal { get; set; }
        public List<int> DrawnNumbers { get; set; }
        private static readonly object padlock = new object();
        public IServer proxy = null;


        Singleton()
        {
            DrawnNumbers = new List<int>();
            Signal = new object();

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
