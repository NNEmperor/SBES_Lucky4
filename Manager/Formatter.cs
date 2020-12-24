using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Formatter
    {
        public static string ParseName(string logName)
        {
            string[] parts = new string[] { };

            if (logName.Contains("@"))
            {
                ///UPN format
                parts = logName.Split('@');
                return parts[0];
            }
            else if (logName.Contains("\\"))
            {
                /// SPN format
                parts = logName.Split('\\');
                return parts[1];
            }
            else if (logName.Contains("CN"))
            {
                // sertifikati, name je formiran kao CN=imeKorisnika;
                int startIndex = logName.IndexOf("=") + 1;
                int endIndex = logName.IndexOf(";");
                string s = logName.Substring(startIndex, endIndex - startIndex);
                return s;
            }
            else
            {
                return logName;
            }
        }

        //public static byte[] ObjectToByteArray(Object obj)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (var ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}

        //public static Object ByteArrayToObject(byte[] arrBytes)
        //{
        //    using (var memStream = new MemoryStream())
        //    {
        //        var binForm = new BinaryFormatter();
        //        memStream.Write(arrBytes, 0, arrBytes.Length);
        //        memStream.Seek(0, SeekOrigin.Begin);
        //        var obj = binForm.Deserialize(memStream);
        //        memStream.Flush();
        //        return obj;
        //    }
        //}

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
                retVal += num.ToString() + ",";
            }
            if (results.Numbers.Count > 0)
                retVal = retVal.Substring(0, retVal.Length - 1);

            retVal += "|" + results.Credits.ToString();

            return retVal;
        }

        public static Results GetResults(string results)
        {
            Results retVal = new Results();
            string[] parts = results.Split('|');
            string[] nums = parts[1].Split(',');

            retVal.Won = bool.Parse(parts[0]);
            retVal.Numbers = new List<int>();
            retVal.Credits = int.Parse(parts[2]);

            foreach (string num in nums)
            {
                if (!num.Equals(String.Empty))
                    retVal.Numbers.Add(int.Parse(num));
            }

            return retVal;
        }

        public static Winner GetWinner(string winner)
        {
            Winner w = new Winner();
            string[] parts = winner.Split(';');
            w.Round = int.Parse(parts[0]);
            w.Count = int.Parse(parts[1]);
            w.Identity = parts[2];
            w.Time = DateTime.Parse(parts[3]);

            return w;

        }

    }
}
