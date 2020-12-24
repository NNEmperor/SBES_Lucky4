using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            ServiceHost hostProcess = null;
            ServiceHost hostServis = null;

            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                string address = "net.tcp://localhost:9901/Server";
                hostServis = new ServiceHost(typeof(Server));
                hostServis.AddServiceEndpoint(typeof(IServer), binding, address);

                NetTcpBinding binding2 = new NetTcpBinding();
                string address2 = "net.tcp://localhost:9911/Process";
                hostProcess = new ServiceHost(typeof(ProcessState));
                hostProcess.AddServiceEndpoint(typeof(IServerState), binding2, address2);

                hostServis.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                hostServis.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                hostServis.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);


                Console.WriteLine("Lucky6 Remote servers are Running...");
            }
            catch (Exception e)
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                string address = "net.tcp://localhost:9902/Server";
                hostServis = new ServiceHost(typeof(Server));
                hostServis.AddServiceEndpoint(typeof(IServer), binding, address);

                NetTcpBinding binding2 = new NetTcpBinding();
                string address2 = "net.tcp://localhost:9912/Process";
                hostProcess = new ServiceHost(typeof(ProcessState));
                hostProcess.AddServiceEndpoint(typeof(IServerState), binding2, address2);

                hostServis.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                hostServis.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                hostServis.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);


                Console.WriteLine("Lucky6 Remote servers are Running...");
            }



            hostProcess.Open();
            hostServis.Open();

            Console.Read();

            hostServis.Close();
            hostProcess.Close();
            }
        }
    }
}
