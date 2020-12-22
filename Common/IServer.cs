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

    }
}
