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

namespace Replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isFirstTime = true;
            DateTime time = DateTime.Now;
            string srvCertCN = "adminPera";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
                StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address1 = new EndpointAddress(new Uri("net.tcp://localhost:9901/Server"),
                                      new X509CertificateEndpointIdentity(srvCert));

            EndpointAddress address2 = new EndpointAddress(new Uri("net.tcp://localhost:9902/Server"),
                                     new X509CertificateEndpointIdentity(srvCert));

            //string address = "net.tcp://localhost:9998/Server";


            while (true)
            {

                try
                {

                    ChannelFactory<IServer> fromWhere = new ChannelFactory<IServer>(binding,address1);
                    ChannelFactory<IServer> toWhere = new ChannelFactory<IServer>(binding, address2);

                    //ChannelFactory<IServer> factory = new ChannelFactory<IServer>(binding, address);

                    string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                    fromWhere.Credentials.ClientCertificate.Certificate =
                        CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
                    fromWhere.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                    fromWhere.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                    toWhere.Credentials.ClientCertificate.Certificate =
                       CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
                    toWhere.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                    toWhere.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                    IServer proxyFrom = fromWhere.CreateChannel();
                    IServer proxyTo = toWhere.CreateChannel();

                    if (isFirstTime)
                    {
                        proxyTo.ReplicateData(proxyFrom.GetTickets(), proxyFrom.GetWinners());
                        isFirstTime = false;
                        time = DateTime.Now;
                    }
                    else
                    {
                        List<Ticket> tickets = proxyFrom.CheckNewTickets(time);
                        List<Winner> winners = proxyFrom.CheckNewWinners(time);
                        time = DateTime.Now;
                        proxyTo.ReplicateData(tickets, winners);
                    }


                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
