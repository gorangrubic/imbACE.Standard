namespace imbACE.Network.tools
{
    using imbACE.Core.core;
    using imbACE.Core.data.mysql;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Xml;

    #endregion imbVeles using

    /// <summary>
    /// OLD TECH: manager koji učitava podatke o domenu
    /// </summary>
    public class imbDomainManager
    {
        private static String xmlSource = "";
        private static XmlDocument xmlDocument;

        public static List<imbWhoIsServer> servers = new List<imbWhoIsServer>();

        private static List<imbTopLevelDomain> allDomains = new List<imbTopLevelDomain>();
        private static List<imbTopLevelDomain> allDomainsUnique = new List<imbTopLevelDomain>();

        public static List<String> allDomainsStringList = new List<string>();

        private static Dictionary<String, List<imbWhoIsServer>> whoIsServersPerTLD =
            new Dictionary<string, List<imbWhoIsServer>>();

        private static Dictionary<String, imbTopLevelDomain> domainDictionary =
            new Dictionary<string, imbTopLevelDomain>();

        public static List<imbTopLevelDomain> AllDomains
        {
            get
            {
                return allDomains;
            }

            set
            {
                allDomains = value;
            }
        }

        public static List<imbTopLevelDomain> AllDomainsUnique
        {
            get
            {
                return allDomainsUnique;
            }

            set
            {
                allDomainsUnique = value;
            }
        }

        public static Dictionary<string, List<imbWhoIsServer>> WhoIsServersPerTLD
        {
            get
            {
                return whoIsServersPerTLD;
            }

            set
            {
                whoIsServersPerTLD = value;
            }
        }

        public static Dictionary<string, imbTopLevelDomain> DomainDictionary
        {
            get
            {
                return domainDictionary;
            }

            set
            {
                domainDictionary = value;
            }
        }

        public static String getAllTLD()
        {
            String output = "";
            foreach (imbTopLevelDomain tld in AllDomainsUnique)
            {
                output += tld.countryCode + "|" + tld.countryName + ";";
            }

            return output;
        }

        public static void prepare(dataBaseTarget dbSource = null)
        {
            if (dbSource == null) { }

            servers = new List<imbWhoIsServer>();
            WhoIsServersPerTLD = new Dictionary<string, List<imbWhoIsServer>>();

            AllDomains = new List<imbTopLevelDomain>();
            AllDomainsUnique = new List<imbTopLevelDomain>();
            allDomainsStringList = new List<string>();
            DomainDictionary = new Dictionary<string, imbTopLevelDomain>();
        }

        public static void loadXml(String xmlPath)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlPath);

            //imbSettingsManager.current.whoisXmlFileA = xmlPath;
            //imbSettingsManager.saveSettings();

            //foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes) {
            //    if (node.Name=="server") {
            //        imbWhoIsServer tmpServer = new imbWhoIsServer();
            //        tmpServer.loadXml(node);
            //        servers.Add(tmpServer);

            //        allDomains.AddRange(tmpServer.domainsAll);
            //    }
            //}

            ////logSystem.log("WhoIs Server XML list loaded: " + servers.Count(), logType.Done);
        }

        public static void afterLoad()
        {
            foreach (imbTopLevelDomain item in AllDomains)
            {
                if (!(DomainDictionary.ContainsKey(item.domainName)))
                {
                    DomainDictionary.Add(item.domainName, item);
                    AllDomainsUnique.Add(item);
                    allDomainsStringList.Add(item.countryCode);
                }
                else
                {
                    DomainDictionary[item.domainName].relatedServers.Add(item.relatedServer);
                }

                if (!(WhoIsServersPerTLD.ContainsKey(item.countryCode)))
                {
                    WhoIsServersPerTLD.Add(item.countryCode, item.relatedServers);
                }
            }

            logSystem.log("Total TLD: " + AllDomainsUnique.Count + " Total servers: " + WhoIsServersPerTLD.Count,
                          logType.Done);
        }

        public static void transformToKnowledge()
        {
        }
    }
}