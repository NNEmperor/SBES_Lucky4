using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Winner
    {
        public int Round { get; set; }
        public int Count { get; set; }
        public string Identity { get; set; }
        public DateTime Time { get; set; }
        public DateTime LastChanged { get; set; }

        public Winner()
        {
            LastChanged = DateTime.Now;
        }

        public Winner(int round, int count, DateTime time)
        {
            Round = round;
            Count = count;
            Identity = "";
            Time = time;
        }

        public override string ToString()
        {
            return Round.ToString()+";"+Count.ToString()+";"+Identity+";"+Time.ToString();
        }
    }
}
