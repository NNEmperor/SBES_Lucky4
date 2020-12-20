using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Results
    {
        public Results(bool won, List<int> numbers, int credits)
        {
            Won = won;
            Numbers = numbers;
            Credits = credits;
        }
        public Results() { }
        [DataMember]
        public bool Won { get; set; }
        [DataMember]
        public List<int> Numbers { get; set; }
        [DataMember]
        public int Credits { get; set; }
    }
}
