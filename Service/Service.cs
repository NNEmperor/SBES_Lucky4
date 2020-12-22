using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    class Service : IService
    {
        public List<byte[]> Connect()
        {
            List<byte[]> list = new List<byte[]>();

            using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
            {
                list.Add(myAes.Key);
                list.Add(myAes.IV);

                Singleton.Instance.SecretKey = myAes.Key;
                Singleton.Instance.IV = myAes.IV;
            }


            return list;
        }

        public string RegisterForOneRound(string ticket)
        {
            string decryptedTicket = AES_Algorithm.DecryptMessage_Aes(ticket, Singleton.Instance.SecretKey, Singleton.Instance.IV);

            Ticket t = Formatter.GetTicket(decryptedTicket);

            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;
            t.Username = windowsIdentity.Name;

            if (!Singleton.Instance.CanBet)
            {
                return AES_Algorithm.EncryptMessage_Aes(Formatter.ResultsToString(new Results(true, new List<int>(), -1)), Singleton.Instance.SecretKey, Singleton.Instance.IV);
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
                Singleton.Instance.proxy.ForwardBet(t);

                return AES_Algorithm.EncryptMessage_Aes(Formatter.ResultsToString(new Results(won, Singleton.Instance.DrawnNumbers, t.Bet)), Singleton.Instance.SecretKey, Singleton.Instance.IV);
            }
        }
        private void RoundCleanUp()
        {
            if (Singleton.Instance.WinnerCount >= 3)
            {
                Singleton.Instance.proxy.MultipleWinners(Singleton.Instance.RoundNumber - 1, Singleton.Instance.WinnerCount, Thread.CurrentPrincipal.Identity.Name, DateTime.Now);//Neka neko proveri ovo oko identiteta, pojma nemam da li se ovako poziva lol
            }
            Singleton.Instance.WinnerCount = 0;
        }
    }
}
