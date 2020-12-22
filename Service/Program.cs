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
                string encyptedRound = string.Empty;
                //serverski sertifikat,vadimo iz trusted people
                string srvcertCN = "adminPera";
                X509Certificate2 serverCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvcertCN);
                encyptedRound = RSA_Algorithm.EncryptRsa(selectedRound.ToString(), serverCert);
                //Console.WriteLine("SIFROVANO: " + encyptedRound + Environment.NewLine);
                
                string clientCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
                //Console.WriteLine("KLIJENT:" + clientCertCN + Environment.NewLine);
                X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN);
                string response = Singleton.Instance.proxy.RequestWinnersForOneRound(encyptedRound);
                //Console.WriteLine("Odgovor: " + response + Environment.NewLine);
                Console.WriteLine("Multiple winners: " + RSA_Algorithm.DecryptRsa(response, clientCert));//sifrovana runda se salje

                //Console.WriteLine(Singleton.Instance.proxy.RequestWinnersForOneRound(selectedRound));
            }


            Console.ReadLine();


            host.Close();
        }

    }

}
