#region USING

//using imbVelesEngine.imbScript;
//using imbVelesEngine.imbVelesWindows;

#endregion USING

//using imbVelesEngine.imbIO.executors;

//using imbFramework;
//using imbFramework.imbConsole;

namespace imbACE.Network.tools
{
    using imbACE.Core.xml;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Sockets;
    using System.Xml;

    #endregion imbVeles using

    // [imbSql(shemaFromTypeMode.onlyDirect)]
    public class imbWhoIsServer : imbBindable
    {
        #region -----------  url  -------  [Adresa WHO is Servera]

        private String _url;

        /// <summary>
        /// Adresa WHO is Servera
        /// </summary>
        // [XmlIgnore]
        [Category("imbWhoIsServer")]
        [DisplayName("url")]
        [Description("Adresa WHO is Servera")]
        //[imbSql(sqlEntityPropMode.unique)]
        public String url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("url");
            }
        }

        #endregion -----------  url  -------  [Adresa WHO is Servera]

        #region -----------  info  -------  [Opis servera]

        private String _info;

        /// <summary>
        /// Opis servera
        /// </summary>
        // [XmlIgnore]
        [Category("imbWhoIsServer")]
        [DisplayName("info")]
        [Description("Opis servera")]
        public String info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged("info");
            }
        }

        #endregion -----------  info  -------  [Opis servera]

        #region -----------  mainDomainList  -------  [Lista glavnih domena koje pokriva]

        private String _mainDomainList;

        /// <summary>
        /// Lista glavnih domena koje pokriva
        /// </summary>
        // [XmlIgnore]
        [Category("imbWhoIsServer")]
        [DisplayName("mainDomainList")]
        [Description("Lista glavnih domena koje pokriva")]
        public String mainDomainList
        {
            get { return _mainDomainList; }
            set
            {
                _mainDomainList = value;
                OnPropertyChanged("mainDomainList");
            }
        }

        #endregion -----------  mainDomainList  -------  [Lista glavnih domena koje pokriva]

        #region -----------  subDomainList  -------  [lista pod domena]

        private String _subDomainList = "";

        /// <summary>
        /// lista pod domena
        /// </summary>
        // [XmlIgnore]
        [Category("imbWhoIsServer")]
        [DisplayName("subDomainList")]
        [Description("lista pod domena")]
        public String subDomainList
        {
            get { return _subDomainList; }
            set
            {
                _subDomainList = value;
                OnPropertyChanged("subDomainList");
            }
        }

        #endregion -----------  subDomainList  -------  [lista pod domena]

        //public String url = "";

        // public String info = "";

        public int callCount = 0;
        public List<imbTopLevelDomain> domains;

        public List<imbTopLevelDomain> domainsAll;

        public List<String> domainsThatHandle;
        public String notFoundMessage = "";

        public imbWhoIsServer()
        {
            domains = new List<imbTopLevelDomain>();
            domainsAll = new List<imbTopLevelDomain>();
        }

        public void loadXml(XmlNode sourceNode)
        {
            url = imbXmlCommonTools.getAttributeValue(sourceNode, "host");
            domainsThatHandle = new List<String>();

            foreach (XmlNode item in sourceNode.ChildNodes)
            {
                switch (item.Name)
                {
                    case "info":
                        info = item.InnerText;
                        break;

                    case "availstring":
                        notFoundMessage = item.InnerText;
                        break;

                    case "domain":

                        imbTopLevelDomain subDomain = new imbTopLevelDomain();
                        subDomain.relatedServer = this;
                        subDomain.loadXml(item);

                        domains.Add(subDomain);

                        mainDomainList += subDomain.domainName + "; ";

                        domainsAll.Add(subDomain);
                        domainsAll.AddRange(subDomain.subDomains);
                        break;
                }
            }

            foreach (imbTopLevelDomain item in domainsAll)
            {
                domainsThatHandle.Add(item.countryCode);
                subDomainList += item.domainName + "; ";
            }
        }

        /// <summary>
        /// Vraca u formi text fajla WhoIs informaciju
        /// </summary>
        /// <param name="whoisServer">The whois server.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public List<String> GetWhoisInformation(string url, string whoisServer = "")
        {
            List<String> output = new List<string>();
            if (whoisServer == "")
            {
                whoisServer = url;
            }

            TcpClient tcpClinetWhois = new TcpClient(whoisServer, 43);

            NetworkStream networkStreamWhois = tcpClinetWhois.GetStream();

            BufferedStream bufferedStreamWhois = new BufferedStream(networkStreamWhois);
            StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

            streamWriter.WriteLine(url);
            streamWriter.Flush();

            StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois);

            while (!streamReaderReceive.EndOfStream)
                output.Add(streamReaderReceive.ReadLine());

            return output;
        }
    }
}