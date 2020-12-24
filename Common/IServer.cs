using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IServer
    {
        [OperationContract]
        void ForwardBet(string bet);//(Ticket t);

        [OperationContract]
        string RequestWinnersForOneRound(string round);

        [OperationContract]
        void MultipleWinners(string winner);
        //void MultipleWinners(int round, int count, string identitet, DateTime time);

        [OperationContract]
        void ReplicateData(List<Ticket> tickets, List<Winner> winners);

        [OperationContract]
        List<Ticket> GetTickets();

        [OperationContract]
        List<Winner> GetWinners();

        [OperationContract]
        List<Winner> CheckNewWinners(DateTime time);

        [OperationContract]
        List<Ticket> CheckNewTickets(DateTime time);

    }
}
