using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    class Service : IService
    {
        public Results RegisterForOneRound(Ticket t)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;
            t.Username = windowsIdentity.Name;

            if (!Singleton.Instance.CanBet)
            {
                return new Results(true, new List<int>(), -1);
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
                return new Results(won, Singleton.Instance.DrawnNumbers, t.Bet);
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
