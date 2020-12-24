using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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

            while (true)
            {

                try
                {

                    ChannelFactory<IServer> fromWhere = new ChannelFactory<IServer>("fromWhere");
                    ChannelFactory<IServer> toWhere = new ChannelFactory<IServer>("toWhere");

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
