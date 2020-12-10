using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class Singleton
    {
        //moramo nekako da pristupamo izvucenim brojevima direktno iz Service, stavio sam singleton(a tu je i Signal da bi svi mogli da pristupaju tom locku)
        private static Singleton instance = null;
        public object Signal { get; set; }
        public List<int> DrawnNumbers { get; set; }
        private static readonly object padlock = new object();

        Singleton()
        {
            DrawnNumbers = new List<int>();
            Signal = new object();
        }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }
        }
    }
}
