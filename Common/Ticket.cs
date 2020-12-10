using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Ticket
    {
        [DataMember]
        public List<int> Numbers { get; set; }
        [DataMember]
        public int Bet { get; set; }

        public Ticket(List<int> numbers, int bet)
        {
            Numbers = numbers;
            Bet = bet;
        }
    }
}
