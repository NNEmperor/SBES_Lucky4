using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
 //   [Serializable]
    [DataContract]
    public class Ticket
    {
        [DataMember]
        public List<int> Numbers { get; set; }
        [DataMember]
        public int Bet { get; set; }
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public DateTime LastChanged { get; set; }

        public Ticket(List<int> numbers, int bet)
        {
            Numbers = numbers;
            Bet = bet;
            Username = "client";
            LastChanged = DateTime.Now;
        }

        public Ticket() { }

        public override string ToString()
        {
            string numbers = "";
            foreach (int number in Numbers)
            {
                numbers += number.ToString() + ' ';
            }
            return Username + "/" + Bet.ToString() + "/" + numbers;

        }
    }
}
