namespace imbACE.Network.tools
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.xml;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    #endregion imbVeles using

    // [imbSql(shemaFromTypeMode.onlyDirect)]
    //  [imbSql(imbSqlSettings.setTableName, "topLevelDomains")]
    public class imbTopLevelDomain : imbBindable //imbSqlEntityBase
    {
        public imbTopLevelDomain()
        {
            subDomains = new List<imbTopLevelDomain>();
            relatedServers = new List<imbWhoIsServer>();
        }

        #region -----------  countryCode  -------  [dvoslovna oznaka zemlje]

        private String _countryCode;

        /// <summary>
        /// dvoslovna oznaka zemlje
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("countryCode")]
        [Description("dvoslovna oznaka zemlje")]
        public String countryCode
        {
            get { return _countryCode; }
            set
            {
                _countryCode = value;
                OnPropertyChanged("countryCode");
            }
        }

        #endregion -----------  countryCode  -------  [dvoslovna oznaka zemlje]

        #region -----------  domainName  -------  [Ime domena]

        private String _domainName;

        /// <summary>
        /// Ime domena
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("domainName")]
        [Description("Ime domena")]
        //[imbSql(sqlEntityPropMode.unique)]
        public String domainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                OnPropertyChanged("domainName");
            }
        }

        #endregion -----------  domainName  -------  [Ime domena]

        #region -----------  count  -------  [Broj domena na serveru]

        private Int32 _count;

        /// <summary>
        /// Broj domena na serveru
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("count")]
        [Description("Broj domena na serveru")]
        public Int32 count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("count");
            }
        }

        #endregion -----------  count  -------  [Broj domena na serveru]

        #region -----------  nic  -------  [Adresa NIC-a]

        private String _nic;

        /// <summary>
        /// Adresa NIC-a
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("nic")]
        [Description("Adresa NIC-a")]
        public String nic
        {
            get { return _nic; }
            set
            {
                _nic = value;
                OnPropertyChanged("nic");
            }
        }

        #endregion -----------  nic  -------  [Adresa NIC-a]

        #region -----------  countryName  -------  [Naziv drzave na engleskom]

        private String _countryName;

        /// <summary>
        /// Naziv drzave na engleskom
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("countryName")]
        [Description("Naziv drzave na engleskom")]
        public String countryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;
                OnPropertyChanged("countryName");
            }
        }

        #endregion -----------  countryName  -------  [Naziv drzave na engleskom]

        #region -----------  doPreload  -------  [Da li da unapred ucita ovaj TLD]

        private Boolean _doPreload = false; // = new Boolean();

                                            /// <summary>
                                            /// Da li da unapred ucita ovaj TLD
                                            /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("doPreload")]
        [Description("Da li da unapred ucita ovaj TLD")]
        public Boolean doPreload
        {
            get
            {
                return _doPreload;
            }
            set
            {
                // Boolean chg = (_doPreload != value);
                _doPreload = value;
                OnPropertyChanged("doPreload");
                // if (chg) {}
            }
        }

        #endregion -----------  doPreload  -------  [Da li da unapred ucita ovaj TLD]

        //public string name = ""; // name je dvoslovna oznaka zemlje ustvari
        //public string nic = "";

        //public int count = 0;

        //public string countryCode = "";
        //public string countryName = "";

        //  [imbSql(sqlEntityPropMode.skip)]
        [XmlIgnore]
        public imbWhoIsServer relatedServer;

        //[imbSql(sqlEntityPropMode.skip)]
        [XmlIgnore]
        public List<imbWhoIsServer> relatedServers;

        //  [imbSql(sqlEntityPropMode.skip)]
        public int shuffleIndex = 0;

        [XmlIgnore]
        public List<imbTopLevelDomain> subDomains = new List<imbTopLevelDomain>();

        //[imbSql(sqlEntityPropMode.skip)]
        [XmlIgnore]
        public imbWhoIsServer shuffledServer
        {
            get
            {
                if (shuffleIndex >= relatedServers.Count())
                {
                    shuffleIndex = 0;
                }
                if (relatedServers.Count() > 0)
                {
                    imbWhoIsServer output = relatedServers[shuffleIndex];
                    shuffleIndex++;
                    return output;
                }
                else
                {
                    return null;
                }
            }
        }

        #region -----------  subDomainList  -------  [List obuhvacenih pod domena]

        private String _subDomainList;

        /// <summary>
        /// List obuhvacenih pod domena
        /// </summary>
        // [XmlIgnore]
        [Category("imbTopLevelDomain")]
        [DisplayName("subDomainList")]
        [Description("List obuhvacenih pod domena")]
        public String subDomainList
        {
            get { return _subDomainList; }
            set
            {
                _subDomainList = value;
                OnPropertyChanged("subDomainList");
            }
        }

        #endregion -----------  subDomainList  -------  [List obuhvacenih pod domena]

        private Int32 CompareSubTLD(String x, String y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the
                    // lengths of the two strings.
                    //
                    int retval = x.Length.CompareTo(y.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.CompareTo(y);
                    }
                }
            }
        }

        private List<String> _subTLDs;

        /// <summary>
        /// TLD and all sub TLDS domain
        /// </summary>
        /// <value>
        /// The sub tl ds.
        /// </value>
        [XmlIgnore]
        internal List<String> subTLDs
        {
            get
            {
                if (_subTLDs == null)
                {
                    var tmp = subDomainList.SplitSmart(",", "", true, true);
                    _subTLDs = new List<string>();
                    foreach (var t in tmp)
                    {
                        var ts = t.Replace(Environment.NewLine, "");
                        if (!ts.isNullOrEmpty())
                        {
                            _subTLDs.Add(t);
                        }
                    }
                    _subTLDs.Add(domainName);
                    _subTLDs.Sort(CompareSubTLD);
                    _subTLDs.Reverse();
                }
                return _subTLDs;
            }
        }

        /// <summary>
        /// Removes the Top Level domain from domain name
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns></returns>
        public String RemoveTLD(String name)
        {
            String output = name;
            if (output.Contains("/")) throw new aceGeneralException("Provide clean domainName [" + output + "] please", new ArgumentException(nameof(name)), this, "Only clean domainName allowed at RemoveTLD.");

            foreach (String sTLD in subTLDs)
            {
                if (output.EndsWith(sTLD))
                {
                    output = output.Replace(sTLD, "");
                    return output;
                }
            }

            return output;
        }

        /// <summary>
        /// Obradjuje XML node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public void loadXml(XmlNode node)
        {
            domainName = imbXmlCommonTools.getAttributeValue(node, "name");

            countryCode = domainName.Split('.').Last<String>(); //.ToString();

            String countString = imbXmlCommonTools.getAttributeValue(node, "count");

            count = imbStringFormats.getInt32Safe(countString);
            //count = (Int32)imbDataExecutor.convertValue(countString, imbReportCell_dataType.Int32);

            nic = imbXmlCommonTools.getAttributeValue(node, "nic");

            countryName = imbXmlCommonTools.getAttributeValue(node, "country");
            relatedServers.Add(relatedServer);

            foreach (XmlNode item in node.ChildNodes)
            {
                imbTopLevelDomain subDomain = new imbTopLevelDomain();
                subDomain.relatedServer = relatedServer;

                subDomain.loadXml(item);
                subDomain.countryName = countryName;
                subDomain.countryCode = countryCode;

                subDomains.Add(subDomain);
                subDomainList += subDomain.domainName + "; ";
            }
        }
    }
}