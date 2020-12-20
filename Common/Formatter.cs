using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Formatter
    {
        public static string TicketToString(Ticket ticket)
        {
            string retVal = "";

            foreach (int number in ticket.Numbers)
            {
                retVal += number.ToString() + ',';
            }
            retVal = retVal.Substring(0, retVal.Length - 1);
            retVal += "|" + ticket.Username;
            retVal += "|" + ticket.Bet.ToString();

            return retVal;
        }

        public static Ticket GetTicket(string ticket)
        {
            Ticket retVal = new Ticket();
            string[] parts = ticket.Split('|');
            string[] nums = parts[0].Split(',');

            retVal.Username = parts[1];
            retVal.Bet = int.Parse(parts[2]);
            retVal.Numbers = new List<int>();

            foreach (string num in nums)
            {
                retVal.Numbers.Add(int.Parse(num));
            }

            return retVal;
        }

        public static string ResultsToString(Results results)
        {
            string retVal = "";

            retVal += results.Won.ToString() + "|";

            foreach (int num in results.Numbers)
            {
                retVal += num.ToString() + "m";
            }
            if(results.Numbers.Count > 0)
                retVal = retVal.Substring(0, retVal.Length - 1);

            retVal += "|" + results.Credits.ToString();

            return retVal;
        }

        public static Results GetResults(string results)
        {
            Results retVal = new Results();
            string[] parts = results.Split('|');
            string[] nums = parts[1].Split('m');

            retVal.Won = bool.Parse(parts[0]);
            retVal.Numbers = new List<int>();
            retVal.Credits = int.Parse(parts[2]);

            foreach (string num in nums)
            {
                retVal.Numbers.Add(int.Parse(num));
            }

            return retVal;
        }
    }
}
