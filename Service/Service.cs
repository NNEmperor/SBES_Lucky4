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

            Singleton.Instance.proxy.ForwardBet(t);

            lock (Singleton.Instance.Signal)
            {
                Monitor.Wait(Singleton.Instance.Signal);
                return new Results(true, Singleton.Instance.DrawnNumbers, 5);//samo za testiranje, logika oko dobitka i to nedostaje
            }
        }
    }
}
