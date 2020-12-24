using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class Server : IServer
    {
        public List<Ticket> CheckNewTickets(DateTime time)
        {
            List<Ticket> tickets = new List<Ticket>();

            foreach (Ticket t in DataBase.tickets)
            {
                if (t.LastChanged >= time)
                {
                    tickets.Add(t);
                }
            }

            return tickets;
        }

        public List<Winner> CheckNewWinners(DateTime time)
        {
            List<Winner> winners = new List<Winner>();

            foreach (Winner w in DataBase.winners)
            {
                if (w.LastChanged >= time)
                {
                    winners.Add(w);
                }
            }

            return winners;
        }

        public void ForwardBet(string encryptedBet)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            string decryptedBet = RSA_Algorithm.DecryptRsa(encryptedBet, cert);

            string file = "Bets.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(decryptedBet);
            sw.Write(Environment.NewLine);
            DataBase.tickets.Add(Formatter.GetTicket(decryptedBet));
            sw.Close();
            stream.Close();
        }

        public List<Ticket> GetTickets()
        {
            return DataBase.tickets;
        }

        public List<Winner> GetWinners()
        {
            return DataBase.winners;
        }

        public void MultipleWinners(string encryptedWinner)//(int round, int count, string identity, DateTime time)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            string decryptedWinner = RSA_Algorithm.DecryptRsa(encryptedWinner, cert);
            string file = "MultipleWinners.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);

            string text = decryptedWinner;
           
            sw.Write(text);
            sw.Write(Environment.NewLine);
            DataBase.winners.Add(Formatter.GetWinner(text));
            sw.Close();
            stream.Close();
        }

        public void ReplicateData(List<Ticket> tickets, List<Winner> winners)
        {
            string file = "Bets.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Ticket t in tickets)
            {

                sw.Write(t.ToString() + Environment.NewLine);
                DataBase.tickets.Add(t);
            }

            sw.Close();
            stream.Close();

            string file2 = "MultipleWinners.txt";
            FileInfo f2 = new FileInfo(file2);
            string path2 = f2.FullName;
            FileStream stream2 = new FileStream(path2, FileMode.Append, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(stream2);

            foreach (Winner w in winners)
            {
                sw2.Write(w.ToString() + Environment.NewLine);
                DataBase.winners.Add(w);
            }

            sw2.Close();
            stream2.Close();
        }

        public string RequestWinnersForOneRound(string round)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            string decryptedRound = RSA_Algorithm.DecryptRsa(round, cert);
           // Console.WriteLine("RUNDA : " + dekriptovanaRunda + Environment.NewLine);
            
            string clientName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);//smestiti u proxy
            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,
               StoreLocation.LocalMachine, clientName);//cert od klijenta
            //Console.WriteLine("SERVIS: " + clientName + Environment.NewLine);

            string encryptResult = string.Empty;
            string file = "MultipleWinners.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                if (tokens[0] == decryptedRound)
                {
                    sr.Close();
                    stream.Close();

                    //return int.Parse(tokens[1]);
                    encryptResult = RSA_Algorithm.EncryptRsa(tokens[1], clientCert);
                    return encryptResult;
                }
            }

            sr.Close();
            stream.Close();

            //return -1;
            return RSA_Algorithm.EncryptRsa("None", clientCert);

        }
    }
}
