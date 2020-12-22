using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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

        public byte[] SecretKey { get; set; }
        public byte[] IV { get; set; }


        Singleton()
        {
            DrawnNumbers = new List<int>();
            Signal = new object();
            RoundNumber = 1;
            WinnerCount = 0;
            ActivePlayers = 0;
            CanBet = true;

            string srvCertCN = "adminPera";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9998/Server"),
                                      new X509CertificateEndpointIdentity(srvCert));

            //string address = "net.tcp://localhost:9998/Server";
            ChannelFactory<IServer> factory = new ChannelFactory<IServer>(binding,address);

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            factory.Credentials.ClientCertificate.Certificate =
                CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

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
