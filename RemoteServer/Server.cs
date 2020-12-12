using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class Server : IServer
    {

        public void ForwardBet(Ticket t)
        {
            string file = "Bets.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(t.ToString());
            sw.Write(Environment.NewLine);
            sw.Close();
            stream.Close();
        }

        public void MultipleWinners(int round, int count, string identity, DateTime time)
        {
            string file = "MultipleWinners.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);

            string text = round.ToString() + ";" +
                count.ToString() + ";" +
                identity + ';' +
                time.ToString();
           
            sw.Write(text);
            sw.Write(Environment.NewLine);
            sw.Close();
            stream.Close();
        }

        public int RequestWinnersForOneRound(int round)
        {
            string file = "MultipleWinners.txt";
            FileInfo f = new FileInfo(file);
            string path = f.FullName;
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                if (tokens[0] == round.ToString())
                {
                    sr.Close();
                    stream.Close();

                    return int.Parse(tokens[1]);
                }
            }

            sr.Close();
            stream.Close();

            return -1;

        }
    }
}
