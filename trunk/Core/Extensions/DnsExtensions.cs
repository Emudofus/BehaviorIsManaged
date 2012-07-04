using System;
using System.Linq;
using System.Net;

namespace BiM.Core.Extensions
{
    public class DnsExtensions
    {
        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP = true)
        {
            var addresses = Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException("Unable to retrieve address from specified host name.", "hostName");
            }

            if (throwIfMoreThanOneIP && addresses.Length > 1 && addresses.Distinct().Count() > 1)
            {
                throw new ArgumentException("There is more that one IP address to the specified host.", "hostName");
            }

            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }
    }
}