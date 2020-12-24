using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    class Service : IService
    {
        public List<string> Connect()
        {
            List<string> retVal = new List<string>();
            List<byte[]> list = new List<byte[]>();

            using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
            {

                IIdentity identity = Thread.CurrentPrincipal.Identity;
                WindowsIdentity windowsIdentity = identity as WindowsIdentity;

                X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, 
                    StoreLocation.LocalMachine, Formatter.ParseName(identity.Name));


                string key;
                string IV;

                list.Add(myAes.Key);
                list.Add(myAes.IV);

                //Singleton.Instance.SecretKey = myAes.Key;
                //Singleton.Instance.IV = myAes.IV;

                if (!Singleton.Instance.SecretKeys.ContainsKey(windowsIdentity.Name))
                {
                    Singleton.Instance.SecretKeys.Add(windowsIdentity.Name, list);
                    key = Convert.ToBase64String(myAes.Key);
                    IV = Convert.ToBase64String(myAes.IV);

                }
                else
                {
                    key = Convert.ToBase64String(Singleton.Instance.SecretKeys[windowsIdentity.Name][0]);
                    IV = Convert.ToBase64String(Singleton.Instance.SecretKeys[windowsIdentity.Name][1]);
                }

                string encryptedKey = RSA_Algorithm.EncryptRsa(key, clientCert);
                string encryptedIV = RSA_Algorithm.EncryptRsa(IV, clientCert);

                retVal.Add(encryptedKey);
                retVal.Add(encryptedIV);
                //retVal.Add(key);
                //retVal.Add(IV);
                return retVal;
            }

        }

        public string RegisterForOneRound(string ticket)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            byte[] secretKey = Singleton.Instance.SecretKeys[windowsIdentity.Name][0];
            byte[] IV = Singleton.Instance.SecretKeys[windowsIdentity.Name][1];

            string decryptedTicket = AES_Algorithm.DecryptMessage_Aes(ticket, secretKey, IV);
            Ticket t = Formatter.GetTicket(decryptedTicket);
            t.Username = windowsIdentity.Name;

            if (!Singleton.Instance.CanBet)
            {
                return AES_Algorithm.EncryptMessage_Aes(Formatter.ResultsToString(new Results(true, new List<int>(), -1)), secretKey, IV);
            }
            
            lock (Singleton.Instance.Signal)
            {
                Singleton.Instance.ActivePlayers++;
                Monitor.Wait(Singleton.Instance.Signal);
                Singleton.Instance.ActivePlayers--;                
                bool won = true;
                foreach (int x in t.Numbers)
                {
                    if (Singleton.Instance.DrawnNumbers.IndexOf(x) == -1)
                    {
                        won = false;
                        break;
                    }
                }
                if (won == true)
                {
                    Singleton.Instance.WinnerCount++;
                }
                if (Singleton.Instance.ActivePlayers == 0)
                {
                    RoundCleanUp();
                    //Console.WriteLine("RoundCleanUp");
                }
                string bet = Formatter.TicketToString(t);
                string encyptedBet = string.Empty;
                //serverski sertifikat,vadimo iz trusted people
                string srvcertCN = "adminPera";
                X509Certificate2 serverCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvcertCN);
                encyptedBet = RSA_Algorithm.EncryptRsa(bet, serverCert);
                Singleton.Instance.proxy.ForwardBet(encyptedBet);
                //Singleton.Instance.proxy.ForwardBet(t);

                return AES_Algorithm.EncryptMessage_Aes(Formatter.ResultsToString(new Results(won, Singleton.Instance.DrawnNumbers, t.Bet)), secretKey, IV);
            }
        }
        private void RoundCleanUp()
        {
            if (Singleton.Instance.WinnerCount >= 1)
            {
                //ispise ime klijenta
              //  Winner winner = new Winner(Singleton.Instance.RoundNumber - 1, Singleton.Instance.WinnerCount, Thread.CurrentPrincipal.Identity.Name, DateTime.Now);
 
                Winner winner = new Winner(Singleton.Instance.RoundNumber - 1, Singleton.Instance.WinnerCount,ServiceSecurityContext.Current.PrimaryIdentity.Name, DateTime.Now);
                string encyptedWinner = string.Empty;
                //serverski sertifikat,vadimo iz trusted people
                string srvcertCN = "adminPera";
                X509Certificate2 serverCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvcertCN);
                encyptedWinner = RSA_Algorithm.EncryptRsa(winner.ToString(), serverCert);
                Singleton.Instance.proxy.MultipleWinners(encyptedWinner);
                //Singleton.Instance.proxy.MultipleWinners(Singleton.Instance.RoundNumber - 1, Singleton.Instance.WinnerCount, Thread.CurrentPrincipal.Identity.Name, DateTime.Now);//Neka neko proveri ovo oko identiteta, pojma nemam da li se ovako poziva lol
            }
            Singleton.Instance.WinnerCount = 0;
        }
    }
}
