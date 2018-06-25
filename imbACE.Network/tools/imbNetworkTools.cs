namespace imbACE.Network.tools
{
    using imbACE.Core.core;

    #region imbVeles using

    using System;
    using System.Net;
    using System.Net.NetworkInformation;

    #endregion imbVeles using

    /// <summary>
    /// 2013c> Skup zajednickih alata za Network operacije.
    /// </summary>
    public static class imbNetworkTools
    {
        public const String URL_ANCHORSTART = "#";

        /// <summary>
        /// karakter kojim pocinje query u URL-u - ?
        /// </summary>
        public const String URL_QUERYSTART = "?";

        /// <summary>
        /// Karakter koji se razdvajaju parametri u upitu
        /// </summary>
        public const String URL_PARAMSEPARATOR = "&";

        public const String URL_PARAMOPERATOR = "=";

        /// <summary>
        /// Blok kojim se razdvaja SHEMA od ostatka URL-a @"://"
        /// </summary>
        public const String URL_SHEMAEND = @"://";

        /// <summary>
        /// Pokusace da napravi URI - ako ne uspe, rezultat je Null
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri makeUri(String url)
        {
            Uri tmpUrl = null;
            Boolean ok = Uri.TryCreate(url, UriKind.Absolute, out tmpUrl);
            return tmpUrl;
        }

        /// <summary>
        /// Prikazuje trenutne DNS adrese
        /// </summary>
        public static void DisplayDnsAddresses()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    logSystem.log(adapter.Description, logType.Notification);

                    foreach (IPAddress dns in dnsServers)
                    {
                        logSystem.log("  DNS Servers ............................. : {0}" + dns.ToString(),
                                      logType.Notification);
                    }
                }
            }
        }
    }
}