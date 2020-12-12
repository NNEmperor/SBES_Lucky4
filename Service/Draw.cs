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
                Thread.Sleep(10000);
                Singleton.Instance.CanBet = false;
                Thread.Sleep(5000);
                List<int> Drawn = Enumerable.Range(1, 48).ToList();
                for (int i = 0; i < 13; i++)
                {
                    int index = rand.Next(0, Drawn.Count);
                    //Drawn.RemoveAt(index); //za testiranje, da uvek klijent dobije, jer su svi brojevi izvuceni
                }
                Singleton.Instance.DrawnNumbers = Drawn;
                Singleton.Instance.CanBet = true;
                Console.WriteLine("Round " + Singleton.Instance.RoundNumber++ + " Finished");
                lock (Singleton.Instance.Signal)
                {
                    Monitor.PulseAll(Singleton.Instance.Signal);
                }
            }
        }
    }
}
