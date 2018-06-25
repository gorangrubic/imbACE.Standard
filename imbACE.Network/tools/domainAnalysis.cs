namespace imbACE.Network.tools
{
    using imbACE.Core.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Network.extensions;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    /// <summary>
    /// Analisys of domain landing URL
    /// </summary>
    public class domainAnalysis : imbBindable
    {
        /// <summary>
        /// Constructor only for XML Serialization purposes
        /// </summary>
        public domainAnalysis()
        {
        }

        /// <summary>
        /// Creates analysis of domain name from provided <c>url</c> string.
        /// </summary>
        /// <param name="url">The URL.</param>
        public domainAnalysis(String url)
        {
            analyse(url);
        }

        private String _domainName = "";

        /// <summary> Complete domain name like: tim-sistem.rs  when url was http://www.tim-sistem.rs </summary>
        public String domainName
        {
            get
            {
                return _domainName;
            }
            set
            {
                _domainName = value;
                OnPropertyChanged("domainName");
            }
        }

        private List<String> _properUrlWords = new List<String>();

        /// <summary> </summary>
        public List<String> domainUrlTokens
        {
            get
            {
                return _properUrlWords;
            }
            set
            {
                _properUrlWords = value;
                OnPropertyChanged("properUrlWords");
            }
        }

        /// <summary>
        /// Analyses the specified URL.
        /// </summary>
        /// <param name="__url">The URL.</param>
        private void analyse(String __url)
        {
            url = __url;
            if (url.isNullOrEmptyString())
            {
                throw new aceGeneralException("Argument exception, __url is null or empty", null, this, "__url is empty");
            }

            String tmpTld = "";
            domainName = url.getDomainNameFromUrl(false);

            if (domainName.isNullOrEmpty())
            {
                aceLog.log("Domain name extraction failed from [" + __url + "] + Domain name extraction failed");
            }

            List<String> parts = domainName.SplitSmart(".", "", true); //.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).toList();
            parts.Reverse();

            if (parts.Count > 2)
            {
                tmpTld = parts[1] + "." + parts[0];
            }
            else if (parts.Count == 2)
            {
                tmpTld = parts[0];
            }
            else if (parts.Count == 1)
            {
                domainRootName = parts[0];
                isFound = false;
                tld = "";
                tmpTld = "";
            }

            imbTopLevelDomain tmpTldDef = null;
            if (!tmpTld.isNullOrEmpty())
            {
                tmpTldDef = domainAnalysisEngine.getDomain(tmpTld);
                if (tmpTldDef == null)
                {
                    tmpTld = parts[0];
                    tmpTldDef = domainAnalysisEngine.getDomain(tmpTld);
                }

                if (tmpTldDef == null)
                {
                    isFound = false;
                    tld = tmpTld;
                }
                else
                {
                    isFound = true;
                    tld = tmpTldDef.domainName;
                }

                tldDefinition = tmpTldDef;
            }

            domainName = domainName.removeUrlShema();
            domainName = domainName.removeStartsWith("www.");
            if (tmpTldDef != null)
            {
                domainRootName = tmpTldDef.RemoveTLD(domainName);
            }
            else
            {
                if (tld.Length > 0)
                {
                    domainRootName = domainName.Replace(tld, "");
                }
            }
            domainRootName = domainRootName.Trim('.');
            if (domainRootName.Contains("."))
            {
                // aceLog.log("domainAnalysis failed [" + __url + "] - root domain name contains dot -- you have to add support for this TLD to have this working properly");
            }

            if (!domainRootName.isNullOrEmptyString())
            {
                MatchCollection mchs = imbStringSelect._select_wordsFromDomainname.Matches(domainRootName);
                foreach (Match mch in mchs)
                {
                    domainWords.Add(mch.Value);
                }
            }
            else
            {
                throw new Exception("domainAnalysis failed [" + __url + "] as domainRootName is empty");
            }

            urlProper = "http://" + domainName.ensureStartsWith("www.").ensureEndsWith("/"); // url.validateUrlShema(aceCommonTypes.enums.urlShema.http);
            domainUrlTokens = new List<string>();
            domainUrlTokens.Add("http");
            domainUrlTokens.Add("www");
            domainUrlTokens.AddRange(domainWords);
        }

        /// <summary>
        /// Makes the URL version without domain name. Like: from http://www.koplas.co.rs/proizvodi/mtx.php to /proizvodi/mtx.php
        /// </summary>
        /// <param name="fullUrl">The full URL.</param>
        /// <returns></returns>
        public String GetURLWithoutDomainName(String fullUrl)
        {
            if (domainName.isNullOrEmpty()) return fullUrl;

            Int32 toRemove = fullUrl.IndexOf(domainName, StringComparison.InvariantCultureIgnoreCase);
            if (toRemove == -1)
            {
                var mch = REGEX_SELECTDOMAINNAME.Match(fullUrl);
                if (mch.Success)
                {
                    toRemove = mch.Index + mch.Length;
                    String output = fullUrl.Substring(toRemove);
                    // aceLog.log("GetURLWithoutDomainName -> [" + domainName + "] but generic REGEX worked (" + fullUrl + ") => " + output);
                    return output;
                }
                //  aceLog.log("GetURLWithoutDomainName -> domain name [" + domainName + "] not found in: " + fullUrl);
                return fullUrl;
            }

            toRemove = toRemove + domainName.Length;

            return fullUrl.Substring(toRemove);
        }

        private String _urlProper;

        /// <summary>
        /// The proper form of <c>url</c> with shema definition: like: http://www.timsistem.co.rs  or http://emdip.co.rs
        /// </summary>
        public String urlProper
        {
            get { return _urlProper; }
            set { _urlProper = value; }
        }

        private String _url = "";

        /// <summary>The original url form</summary>
        public String url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                OnPropertyChanged("url");
            }
        }

        #region ----------- Boolean [ isFound ] -------  [Da li je pronadjen TLD]

        private Boolean _isFound = false;

        /// <summary>
        /// Da li je pronadjen TLD
        /// </summary>
        [Category("Switches")]
        [DisplayName("isFound")]
        [Description("Da li je pronadjen TLD")]
        public Boolean isFound
        {
            get { return _isFound; }
            set { _isFound = value; OnPropertyChanged("isFound"); }
        }

        public static Regex REGEX_SELECTFOLDER_RELBACKPATH = new Regex("/((?:[\\w\\d\\s]+)/\\.\\.)+");
        public static Regex REGEX_SELECTFOLDER_RELBACKPATH2 = new Regex("/((?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/\\.\\./\\.\\.)+");
        public static Regex REGEX_SELECTFOLDER_RELBACKPATH3 = new Regex("/((?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/\\.\\./\\.\\./\\.\\.)+");
        public static Regex REGEX_SELECTFOLDER_RELBACKPATH4 = new Regex("/((?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/\\.\\./\\.\\./\\.\\./\\.\\.)+");
        public static Regex REGEX_SELECTFOLDER_RELBACKPATH5 = new Regex("/((?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/(?:[\\w\\d\\s]+)/\\.\\./\\.\\./\\.\\./\\.\\./\\.\\.)+");
        public static Regex REGEX_SELECTFOLDER_RELFULLBACK = new Regex("/((?:[\\w\\d\\s/]+)/\\./)+");

        public static Regex REGEX_SELECTDOMAINNAME = new Regex("//([\\w\\.-]+)/");

        public static Regex REGEX_SELECTANCHOR = new Regex("(#[\\w]+)$");

        public string GetResolvedUrl(string url, Boolean trimAnchor)
        {
            String urlResolved = REGEX_SELECTFOLDER_RELBACKPATH.Replace(url, "");
            urlResolved = REGEX_SELECTFOLDER_RELBACKPATH2.Replace(urlResolved, "");
            urlResolved = REGEX_SELECTFOLDER_RELBACKPATH3.Replace(urlResolved, "");
            urlResolved = REGEX_SELECTFOLDER_RELBACKPATH4.Replace(urlResolved, "");
            urlResolved = REGEX_SELECTFOLDER_RELBACKPATH5.Replace(urlResolved, "");
            urlResolved = urlResolved.Replace("../", "");
            urlResolved = REGEX_SELECTFOLDER_RELFULLBACK.Replace(urlResolved, "");

            if (trimAnchor) urlResolved = REGEX_SELECTANCHOR.Replace(urlResolved, "");

            if (urlResolved != url)
            {
            }
            return urlResolved;
        }

        #endregion ----------- Boolean [ isFound ] -------  [Da li je pronadjen TLD]

        #region -----------  domainRootName  -------  [korenska rec imena domena]

        private String _domainRootName = ""; // = new String();

                                             /// <summary>
                                             /// Domain name without the top level domain
                                             /// </summary>
        // [XmlIgnore]
        [Category("domainAnalysis")]
        [DisplayName("domainRootName")]
        [Description("korenska rec imena domena")]
        public String domainRootName
        {
            get
            {
                return _domainRootName;
            }
            set
            {
                // Boolean chg = (_domainRootName != value);
                _domainRootName = value;
                OnPropertyChanged("domainRootName");
                // if (chg) {}
            }
        }

        #endregion -----------  domainRootName  -------  [korenska rec imena domena]

        #region -----------  domainWords  -------  [Reci koje su otkrivene u domainRoot imenu domena]

        private List<String> _domainWords = new List<String>();

        /// <summary>
        /// Reci koje su otkrivene u domainRoot imenu domena
        /// </summary>
        // [XmlIgnore]
        [Category("domainAnalysis")]
        [DisplayName("domainWords")]
        [Description("Reci koje su otkrivene u domainRoot imenu domena")]
        public List<String> domainWords
        {
            get
            {
                return _domainWords;
            }
            set
            {
                // Boolean chg = (_domainWords != value);
                _domainWords = value;
                OnPropertyChanged("domainWords");
                // if (chg) {}
            }
        }

        #endregion -----------  domainWords  -------  [Reci koje su otkrivene u domainRoot imenu domena]

        #region -----------  tldDefinition  -------  [Referenca prema imbTopLevelDomain objektu]

        private imbTopLevelDomain _tldDefinition; // = new imbTopLevelDomain();

                                                  /// <summary>
                                                  /// Referenca prema imbTopLevelDomain objektu
                                                  /// </summary>
        [XmlIgnore]
        [Category("domainAnalysis")]
        [DisplayName("tldDefinition")]
        [Description("Referenca prema imbTopLevelDomain objektu")]
        public imbTopLevelDomain tldDefinition
        {
            get
            {
                return _tldDefinition;
            }
            set
            {
                // Boolean chg = (_tldDefinition != value);
                _tldDefinition = value;
                OnPropertyChanged("tldDefinition");
                // if (chg) {}
            }
        }

        #endregion -----------  tldDefinition  -------  [Referenca prema imbTopLevelDomain objektu]

        #region -----------  tld  -------  [TOP LEVEL DOMAIN deo domena]

        private String _tld = ""; // = new String();

                                  /// <summary>
                                  /// TOP LEVEL DOMAIN deo domena
                                  /// </summary>
        // [XmlIgnore]
        [Category("domainAnalysis")]
        [DisplayName("tld")]
        [Description("TOP LEVEL DOMAIN deo domena")]
        public String tld
        {
            get
            {
                return _tld;
            }
            set
            {
                // Boolean chg = (_tld != value);
                _tld = value;
                OnPropertyChanged("tld");
                // if (chg) {}
            }
        }

        #endregion -----------  tld  -------  [TOP LEVEL DOMAIN deo domena]
    }
}