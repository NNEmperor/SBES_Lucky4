using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
            lock (Singleton.Instance.Signal)
            {
                Monitor.Wait(Singleton.Instance.Signal);
                return new Results(true, Singleton.Instance.DrawnNumbers, 5);//samo za testiranje, logika oko dobitka i to nedostaje
            }
        }
    }
}
