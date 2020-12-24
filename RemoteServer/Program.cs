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

            //NetTcpBinding binding = new NetTcpBinding();
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            //string address = "net.tcp://localhost:9998/Server";
            //ServiceHost host = new ServiceHost(typeof(Server));
            //host.AddServiceEndpoint(typeof(IServer), binding, address);

            using (ServiceHost hostServis = new ServiceHost(typeof(Server)),
                hostProcess = new ServiceHost(typeof(ProcessState)))
            {


                hostServis.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                hostServis.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                hostServis.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

                hostServis.Open();
                hostProcess.Open();
                Console.WriteLine("Lucky6 Remote servers are Running...");


                Console.Read();

                hostServis.Close();
                hostProcess.Close();
            }
        }
    }
}
