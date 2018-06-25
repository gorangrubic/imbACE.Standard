namespace imbACE.Network.web.core
{
    #region imbVeles using

    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.request;
    using imbSCI.Core.attributes;
    using imbSCI.Core.math;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    // using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

    #endregion imbVeles using

    /// <summary>
    /// Zajednicka podesavanja
    /// </summary>
    public class commonRequestSettings : imbBindable
    {
        public commonRequestSettings()
        {
        }

        #region ----------- Boolean [ doLogCacheLoaded ] -------  [Show log message when cache found]

        private Boolean _doLogCacheLoaded = false;

        /// <summary>
        /// Show log message when cache found
        /// </summary>
        [Category("Switches")]
        [DisplayName("doLogCacheLoaded")]
        [Description("Show log message when cache found")]
        public Boolean doLogCacheLoaded
        {
            get { return _doLogCacheLoaded; }
            set { _doLogCacheLoaded = value; OnPropertyChanged("doLogCacheLoaded"); }
        }

        #endregion ----------- Boolean [ doLogCacheLoaded ] -------  [Show log message when cache found]

        #region ----------- Boolean [ doLogNewLoad ] -------  [Show log message when new page is loaded]

        private Boolean _doLogNewLoad = false;

        /// <summary>
        /// Show log message when new page is loaded
        /// </summary>
        [Category("Switches")]
        [DisplayName("doLogNewLoad")]
        [Description("Show log message when new page is loaded")]
        public Boolean doLogNewLoad
        {
            get { return _doLogNewLoad; }
            set { _doLogNewLoad = value; OnPropertyChanged("doLogNewLoad"); }
        }

        #endregion ----------- Boolean [ doLogNewLoad ] -------  [Show log message when new page is loaded]

        #region ----------- Boolean [ doLogRequestError ] -------  [Do log request error]

        private Boolean _doLogRequestError = false;

        /// <summary>
        /// Do log request error
        /// </summary>
        [Category("Switches")]
        [DisplayName("doLogRequestError")]
        [Description("Do log request error")]
        public Boolean doLogRequestError
        {
            get { return _doLogRequestError; }
            set { _doLogRequestError = value; OnPropertyChanged("doLogRequestError"); }
        }

        #endregion ----------- Boolean [ doLogRequestError ] -------  [Do log request error]

        #region ----------- Boolean [ doTimeoutLimiter ] -------  [Da li da]

        private Boolean _doTimeoutLimiter = true;

        /// <summary>
        /// Da li da
        /// </summary>
        [Category("Switches")]
        [DisplayName("doTimeoutLimiter")]
        [Description("Da li da")]
        public Boolean doTimeoutLimiter
        {
            get { return _doTimeoutLimiter; }
            set
            {
                _doTimeoutLimiter = value;
                OnPropertyChanged("doTimeoutLimiter");
            }
        }

        #endregion ----------- Boolean [ doTimeoutLimiter ] -------  [Da li da]

        #region ----------- Boolean [ doRetryExecution ] -------  [Da li da]

        private Boolean _doRetryExecution = false;

        /// <summary>
        /// Da li da
        /// </summary>
        [Category("Switches")]
        [DisplayName("doRetryExecution")]
        [Description("Da li da")]
        public Boolean doRetryExecution
        {
            get { return _doRetryExecution; }
            set
            {
                _doRetryExecution = value;
                OnPropertyChanged("doRetryExecution");
            }
        }

        #endregion ----------- Boolean [ doRetryExecution ] -------  [Da li da]

        #region ----------- Boolean [ doContentCheck ] -------  [Da li da]

        private Boolean _doContentCheck = false;

        /// <summary>
        /// Da li da
        /// </summary>
        [Category("Switches")]
        [DisplayName("doContentCheck")]
        [Description("Da li da")]
        public Boolean doContentCheck
        {
            get { return _doContentCheck; }
            set
            {
                _doContentCheck = value;
                OnPropertyChanged("doContentCheck");
            }
        }

        #endregion ----------- Boolean [ doContentCheck ] -------  [Da li da]

        #region ----------- Boolean [ doSubdomainVariations ] -------  [Da li da]

        private Boolean _doSubdomainVariations = false;

        /// <summary>
        /// Da li da
        /// </summary>
        [Category("Switches")]
        [DisplayName("doSubdomainVariations")]
        [Description("Da li da")]
        public Boolean doSubdomainVariations
        {
            get { return _doSubdomainVariations; }
            set
            {
                _doSubdomainVariations = value;
                OnPropertyChanged("doSubdomainVariations");
            }
        }

        #endregion ----------- Boolean [ doSubdomainVariations ] -------  [Da li da]

        #region --- subdomainVariations ------- Lista poddomena koji se mogu pojaviti

        private domainElementCollection _subdomainVariations = new domainElementCollection();

        /// <summary>
        /// Lista poddomena koji se mogu pojaviti
        /// </summary>
        public domainElementCollection subdomainVariations
        {
            get
            {
                //if (_subdomainVariations == null) _subdomainVariations = new domainElementCollection();
                return _subdomainVariations;
            }
            set
            {
                _subdomainVariations = value;
                OnPropertyChanged("subdomainVariations");
            }
        }

        #endregion --- subdomainVariations ------- Lista poddomena koji se mogu pojaviti

        #region -----------  requestMethod  -------  [Metod koji se ocekuje od requesta]

        private httpRequestMethod _requestMethod = httpRequestMethod.GET; // = new httpRequestMethod();

        /// <summary>
        /// Metod koji se ocekuje od requesta
        /// </summary>
        // [XmlIgnore]
        [Category("webLoaderSettings")]
        [DisplayName("requestMethod")]
        [Description("Metod koji se ocekuje od requesta")]
        public httpRequestMethod requestMethod
        {
            get { return _requestMethod; }
            set
            {
                // Boolean chg = (_requestMethod != value);
                _requestMethod = value;
                OnPropertyChanged("requestMethod");
                // if (chg) {}
            }
        }

        #endregion -----------  requestMethod  -------  [Metod koji se ocekuje od requesta]

        #region -----------  url  -------  [Path koji treba da otvori]

        private String _url = ""; // = new String();

        /// <summary>
        /// Path koji treba da otvori
        /// </summary>
        // [XmlIgnore]
        [Category("webRequest")]
        [DisplayName("url")]
        [Description("Path koji treba da otvori")]
        public String url
        {
            get { return _url; }
            set
            {
                // Boolean chg = (_url != value);
                _url = value;
                OnPropertyChanged("url");
                // if (chg) {}
            }
        }

        #endregion -----------  url  -------  [Path koji treba da otvori]

        #region --- action ------- Koju akciju izvrsava request

        private webRequestActionType _action;

        /// <summary>
        /// Koju akciju izvrsava request
        /// </summary>
        public webRequestActionType action
        {
            get { return _action; }
            set
            {
                _action = value;
                OnPropertyChanged("action");
            }
        }

        public webRequestType requestType { get; set; }
        public bool doCoolOff { get; set; }

        #endregion --- action ------- Koju akciju izvrsava request

        #region -----------  timeCounter  -------  [ukupno vreme izvrsavanja]

        private counter _timeCounter = new counter("timeCounter", 50, 0, false);

        /// <summary>
        /// Counter - ukupno vreme izvrsavanja
        /// </summary>
        [Category("Counters")]
        [DisplayName("timeCounter")]
        [Description("ukupno vreme izvrsavanja")]
        [ExpandableObject]
        [XmlIgnore]
        public counter timeCounter
        {
            get { return _timeCounter; }
            set
            {
                _timeCounter = value;
                OnPropertyChanged("timeCounter");
            }
        }

        #endregion -----------  timeCounter  -------  [ukupno vreme izvrsavanja]

        #region -----------  retryWaitCounter  -------  [ceka izmedju potrebe za retry i pozivanja retry]

        private counter _retryWaitCounter = new counter("retryWaitCounter", 50, 0, false);

        /// <summary>
        /// Counter - ceka izmedju potrebe za retry i pozivanja retry
        /// </summary>
        [Category("Counters")]
        [DisplayName("retryWaitCounter")]
        [Description("ceka izmedju potrebe za retry i pozivanja retry")]
        [ExpandableObject]
        [XmlIgnore]
        public counter retryWaitCounter
        {
            get { return _retryWaitCounter; }
            set
            {
                _retryWaitCounter = value;
                OnPropertyChanged("retryWaitCounter");
            }
        }

        #endregion -----------  retryWaitCounter  -------  [ceka izmedju potrebe za retry i pozivanja retry]

        #region -----------  retryCounter  -------  [za retry]

        private counter _retryCounter = new counter("retryCounter", 50, 0, false);

        /// <summary>
        /// Counter - za retry
        /// </summary>
        [Category("Counters")]
        [DisplayName("retryCounter")]
        [Description("za retry")]
        [ExpandableObject]
        [XmlIgnore]
        public counter retryCounter
        {
            get { return _retryCounter; }
            set
            {
                _retryCounter = value;
                OnPropertyChanged("retryCounter");
            }
        }

        #endregion -----------  retryCounter  -------  [za retry]

        #region -----------  loadCooloffCounter  -------  [za loadCooloff]

        private counter _loadCooloffCounter = new counter("loadCooloffCounter", 50, 0, false);

        /// <summary>
        /// Counter - za loadCooloff
        /// </summary>
        [Category("Counters")]
        [DisplayName("loadCooloffCounter")]
        [Description("za loadCooloff")]
        [ExpandableObject]
        [XmlIgnore]
        public counter loadCooloffCounter
        {
            get { return _loadCooloffCounter; }
            set
            {
                _loadCooloffCounter = value;
                OnPropertyChanged("loadCooloffCounter");
            }
        }

        #endregion -----------  loadCooloffCounter  -------  [za loadCooloff]

        #region -----------  contentCriteriaTryCounter  -------  [za contentCriteriaTimeout]

        private counter _contentCriteriaTryCounter = new counter("contentCriteriaTryCounter", 50, 0, false);

        /// <summary>
        /// Counter - za contentCriteriaTimeout
        /// </summary>
        [Category("Counters")]
        [DisplayName("contentCriteriaTryCounter")]
        [Description("za contentCriteriaTimeout")]
        [ExpandableObject]
        [XmlIgnore]
        public counter contentCriteriaTryCounter
        {
            get { return _contentCriteriaTryCounter; }
            set
            {
                _contentCriteriaTryCounter = value;
                OnPropertyChanged("contentCriteriaTryCounter");
            }
        }

        #endregion -----------  contentCriteriaTryCounter  -------  [za contentCriteriaTimeout]

        #region -----------  contentCriteriaCheckDelayCounter  -------  [za koliko tickova treba da proveri ContentCriteria]

        private counter _contentCriteriaCheckDelayCounter = new counter("contentCriteriaCheckDelayCounter", 50, 0, false);

        /// <summary>
        /// Counter - za koliko tickova treba da proveri ContentCriteria
        /// </summary>
        [Category("Counters")]
        [DisplayName("contentCriteriaCheckDelayCounter")]
        [Description("za koliko tickova treba da proveri ContentCriteria")]
        [ExpandableObject]
        [XmlIgnore]
        public counter contentCriteriaCheckDelayCounter
        {
            get { return _contentCriteriaCheckDelayCounter; }
            set
            {
                _contentCriteriaCheckDelayCounter = value;
                OnPropertyChanged("contentCriteriaCheckDelayCounter");
            }
        }

        #endregion -----------  contentCriteriaCheckDelayCounter  -------  [za koliko tickova treba da proveri ContentCriteria]

        #region -----------  criteriaValue  -------  [REGEX ili XPATH upit za proveru sadrzaja]

        private String _criteriaValue = ""; // = new String();

        /// <summary>
        /// REGEX ili XPATH upit za proveru sadrzaja
        /// </summary>
        // [XmlIgnore]
        [Category("webRequest")]
        [DisplayName("criteriaValue")]
        [Description("REGEX ili XPATH upit za proveru sadrzaja")]
        public String criteriaValue
        {
            get { return _criteriaValue; }
            set
            {
                // Boolean chg = (_criteriaValue != value);
                _criteriaValue = value;
                OnPropertyChanged("criteriaValue");
                // if (chg) {}
            }
        }

        //public int loadCoolOff
        //{
        //    get { return loadCooloffCounter.limit; }
        //    set { loadCooloffCounter.limit = value; }
        //}

        //public int retryWait
        //{
        //    get { return retryWaitCounter.limit; }
        //    set { retryWaitCounter.limit = value; }
        //}

        //public int retryLimit
        //{
        //    get { return retryCounter.limit; }
        //    set { retryCounter.limit = value; }
        //}

        //public int timeLimit
        //{
        //    get { return timeCounter.limit; }
        //    set { timeCounter.limit = value; }
        //}

        //public int contentCriteriaTimeout
        //{
        //    get { return contentCriteriaTryCounter.limit; }
        //    set { contentCriteriaTryCounter.limit = value; }
        //}

        //public int contentCriteriaCheckDelay
        //{
        //    get { return contentCriteriaCheckDelayCounter.limit; }
        //    set { contentCriteriaCheckDelayCounter.limit = value; }
        //}

        #endregion -----------  criteriaValue  -------  [REGEX ili XPATH upit za proveru sadrzaja]

        #region -----------  contentCriteriaTimeoutAction  -------  [Akcija koju odradjuje kada contentCriteria try ipsadne]

        private webRequestEventType _contentCriteriaTimeoutAction; // = new webRequestEventType();

        /// <summary>
        /// Akcija koju odradjuje kada contentCriteria try ipsadne
        /// </summary>
        // [XmlIgnore]
        [Category("webRequestBase")]
        [DisplayName("contentCriteriaTimeoutAction")]
        [Description("Akcija koju odradjuje kada contentCriteria try ipsadne")]
        public webRequestEventType contentCriteriaTimeoutAction
        {
            get { return _contentCriteriaTimeoutAction; }
            set
            {
                // Boolean chg = (_contentCriteriaTimeoutAction != value);
                _contentCriteriaTimeoutAction = value;
                OnPropertyChanged("contentCriteriaTimeoutAction");
                // if (chg) {}
            }
        }

        #endregion -----------  contentCriteriaTimeoutAction  -------  [Akcija koju odradjuje kada contentCriteria try ipsadne]

        #region -----------  criteriaType  -------  [tip contentCriteria testa]

        private contentCriteriaType _criteriaType = contentCriteriaType.disabled; // = new contentCriteriaType();

        /// <summary>
        /// tip contentCriteria testa
        /// </summary>
        // [XmlIgnore]
        [Category("webRequest")]
        [DisplayName("criteriaType")]
        [Description("tip contentCriteria testa")]
        public contentCriteriaType criteriaType
        {
            get { return _criteriaType; }
            set
            {
                // Boolean chg = (_criteriaType != value);
                _criteriaType = value;
                OnPropertyChanged("criteriaType");
                // if (chg) {}
            }
        }

        #endregion -----------  criteriaType  -------  [tip contentCriteria testa]
    }
}