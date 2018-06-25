//namespace imbACE.Network.web.result
//{
//    using imbSCI.Data.data;
//    #region imbVeles using

//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel;
//    using System.Linq;
//    using System.Net;
//    using System.Xml.Serialization;

//    #endregion

//    /// <summary>
//    /// Objekat sa dns lookup podacima
//    /// </summary>
//    public class dnsLookupDocument : imbBindable
//    {
//        public dnsLookupDocument()
//        {
//        }

//        public dnsLookupDocument(IPHostEntry __hostEntry, String domainName)
//        {
//            deploy(__hostEntry, domainName);
//        }

//        public void deploy(IPHostEntry __hostEntry, String domainName)
//        {
//            foreach (IPAddress ipItem in __hostEntry.AddressList)
//            {
//                ips.Add(ipItem.ToString());
//            }
//            hostEntry = __hostEntry;

//            Request tmpReq = new Request();
//            Question tmpQuestion = new Question(domainName, DnsType.ANAME, DnsClass.IN);

//            tmpReq.AddQuestion(tmpQuestion);

//            Response tmpResponse = Resolver.Lookup(tmpReq, hostEntry.AddressList.First());

//            //List<String> aliasList = new List<string>();
//            foreach (AdditionalRecord alItem in tmpResponse.AdditionalRecords)
//            {
//                aliasDomains.Add(alItem.Record.ToString());
//            }

//            foreach (NameServer alItem in tmpResponse.NameServers)
//            {
//                if (alItem.Record != null)
//                {
//                    nameServers.Add(alItem.Record.ToString());
//                }
//            }
//            //tmpTask.log(nsList, "/NsDomainList");

//            MXRecord[] __mxRecords = Resolver.MXLookup(domainName, hostEntry.AddressList.First());

//            if (__mxRecords != null)
//            {
//                foreach (MXRecord mxItem in __mxRecords)
//                {
//                    mxRecords.Add(mxItem.DomainName + ":" + mxItem.Preference.ToString());
//                }
//            }

//            //tmpTask.log(mxList, "/MxList");
//        }

//        #region --- hostEntry ------- Izvorni hostEntry

//        private IPHostEntry _hostEntry;

//        /// <summary>
//        /// Izvorni hostEntry
//        /// </summary>
//        [XmlIgnore]
//        public IPHostEntry hostEntry
//        {
//            get { return _hostEntry; }
//            set
//            {
//                _hostEntry = value;
//                OnPropertyChanged("hostEntry");
//            }
//        }

//        #endregion

//        #region -----------  mxRecord  -------  []

//        private List<String> _mxRecords = new List<String>();

//        /// <summary>
//        /// First mxRecord -
//        /// </summary>
//        [XmlIgnore]
//        [Category("mxRecords")]
//        [DisplayName("mxRecord")]
//        [Description("Prvi element mxRecord")]
//        public String mxRecord
//        {
//            get
//            {
//                if (_mxRecords.Count > 0)
//                {
//                    return _mxRecords.First();
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            set
//            {
//                Boolean chg = false; //;
//                if (_mxRecords.Count == 0)
//                {
//                    chg = true;
//                    _mxRecords.Add(value);
//                }
//                else
//                {
//                    chg = (_mxRecords[0].GetHashCode() != value.GetHashCode());
//                    _mxRecords[0] = value;
//                }
//                if (chg)
//                {
//                }
//            }
//        }

//        /// <summary>
//        /// Da li ima mxRecord?
//        /// </summary>
//        public Boolean has_mxRecord
//        {
//            get { return (_mxRecords.Count > 0); }
//        }

//        /// <summary>
//        /// Svi elementi: mxRecords
//        /// </summary>
//        [Category("mxRecords")]
//        [DisplayName("mxRecord")]
//        [Description("Svi elementi: mxRecords")]
//        public List<String> mxRecords
//        {
//            get { return _mxRecords; }
//            set
//            {
//                _mxRecords = value;
//                OnPropertyChanged("mxRecords");
//            }
//        }

//        #endregion

//        #region -----------  nameServer  -------  []

//        private List<String> _nameServers = new List<String>();

//        /// <summary>
//        /// First nameServer -
//        /// </summary>
//        [XmlIgnore]
//        [Category("nameServers")]
//        [DisplayName("nameServer")]
//        [Description("Prvi element nameServer")]
//        public String nameServer
//        {
//            get
//            {
//                if (_nameServers.Count > 0)
//                {
//                    return _nameServers.First();
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            set
//            {
//                Boolean chg = false; //;
//                if (_nameServers.Count == 0)
//                {
//                    chg = true;
//                    _nameServers.Add(value);
//                }
//                else
//                {
//                    chg = (_nameServers[0].GetHashCode() != value.GetHashCode());
//                    _nameServers[0] = value;
//                }
//                if (chg)
//                {
//                }
//            }
//        }

//        /// <summary>
//        /// Da li ima nameServer?
//        /// </summary>
//        public Boolean has_nameServer
//        {
//            get { return (_nameServers.Count > 0); }
//        }

//        /// <summary>
//        /// Svi elementi: nameServers
//        /// </summary>
//        [Category("nameServers")]
//        [DisplayName("nameServer")]
//        [Description("Svi elementi: nameServers")]
//        public List<String> nameServers
//        {
//            get { return _nameServers; }
//            set
//            {
//                _nameServers = value;
//                OnPropertyChanged("nameServers");
//            }
//        }

//        #endregion

//        #region -----------  aliasDomain  -------  []

//        private List<String> _aliasDomains = new List<String>();

//        /// <summary>
//        /// First aliasDomain -
//        /// </summary>
//        [XmlIgnore]
//        [Category("aliasDomains")]
//        [DisplayName("aliasDomain")]
//        [Description("Prvi element aliasDomain")]
//        public String aliasDomain
//        {
//            get
//            {
//                if (_aliasDomains.Count > 0)
//                {
//                    return _aliasDomains.First();
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            set
//            {
//                Boolean chg = false; //;
//                if (_aliasDomains.Count == 0)
//                {
//                    chg = true;
//                    _aliasDomains.Add(value);
//                }
//                else
//                {
//                    chg = (_aliasDomains[0].GetHashCode() != value.GetHashCode());
//                    _aliasDomains[0] = value;
//                }
//                if (chg)
//                {
//                }
//            }
//        }

//        /// <summary>
//        /// Da li ima aliasDomain?
//        /// </summary>
//        public Boolean has_aliasDomain
//        {
//            get { return (_aliasDomains.Count > 0); }
//        }

//        /// <summary>
//        /// Svi elementi: aliasDomains
//        /// </summary>
//        [Category("aliasDomains")]
//        [DisplayName("aliasDomain")]
//        [Description("Svi elementi: aliasDomains")]
//        public List<String> aliasDomains
//        {
//            get { return _aliasDomains; }
//            set
//            {
//                _aliasDomains = value;
//                OnPropertyChanged("aliasDomains");
//            }
//        }

//        #endregion

//        #region -----------  ip  -------  []

//        private List<String> _ips = new List<String>();

//        /// <summary>
//        /// First ip -
//        /// </summary>
//        [XmlIgnore]
//        [Category("ips")]
//        [DisplayName("ip")]
//        [Description("Prvi element ip")]
//        public String ip
//        {
//            get
//            {
//                if (_ips.Count > 0)
//                {
//                    return _ips.First();
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            set
//            {
//                Boolean chg = false; //;
//                if (_ips.Count == 0)
//                {
//                    chg = true;
//                    _ips.Add(value);
//                }
//                else
//                {
//                    chg = (_ips[0].GetHashCode() != value.GetHashCode());
//                    _ips[0] = value;
//                }
//                if (chg)
//                {
//                }
//            }
//        }

//        /// <summary>
//        /// Da li ima ip?
//        /// </summary>
//        public Boolean has_ip
//        {
//            get { return (_ips.Count > 0); }
//        }

//        /// <summary>
//        /// Svi elementi: ips
//        /// </summary>
//        [Category("ips")]
//        [DisplayName("ip")]
//        [Description("Svi elementi: ips")]
//        public List<String> ips
//        {
//            get { return _ips; }
//            set
//            {
//                _ips = value;
//                OnPropertyChanged("ips");
//            }
//        }

//        #endregion
//    }
//}