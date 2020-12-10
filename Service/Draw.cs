using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    class Draw
    {
        private Random rand = new Random();
        public Draw()
        {

        }
        public void Start()
        {
            while (true)
            {
                Thread.Sleep(5000);
                List<int> possible = Enumerable.Range(1, 48).ToList();
                List<int> listNumbers = new List<int>();
                for (int i = 0; i < 35; i++)
                {
                    int index = rand.Next(0, possible.Count);
                    listNumbers.Add(possible[index]);
                    possible.RemoveAt(index);
                }
                lock (Singleton.Instance.Signal)
                {
                    Singleton.Instance.DrawnNumbers = listNumbers;
                    Monitor.PulseAll(Singleton.Instance.Signal);
                }
                Console.WriteLine("Round Finished");
            }
        }
    }
}
