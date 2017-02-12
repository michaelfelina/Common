using System.Collections.Generic;
using System.Net;

namespace Common.Methods
{
    public static class IP
    {
        public static string GetIP(int idx)
        {
            var h = Dns.GetHostEntry(Dns.GetHostName());
            var resut = h.AddressList.GetValue(idx).ToString();
            return resut;
        }

        public static List<string> GetIP()
        {
            var h = Dns.GetHostEntry(Dns.GetHostName());
            var resut = new List<string>();
            foreach (var address in h.AddressList)
                resut.Add(address.ToString());
            
            return resut;
        }
    }
}
