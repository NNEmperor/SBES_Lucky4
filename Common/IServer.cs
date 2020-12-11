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
        void ForwardBet(Ticket t);

        [OperationContract]
        int RequestWinnersForOneRound(int round);

        [OperationContract]
        void MultipleWinners(int round, int count, string identitet, DateTime time);

    }
}
