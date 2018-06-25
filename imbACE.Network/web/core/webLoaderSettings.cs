namespace imbACE.Network.web.core
{
    #region imbVeles using

    using imbACE.Network.web.enums;
    using imbSCI.Core.attributes;
    using System;
    using System.ComponentModel;

    // using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

    #endregion imbVeles using

    /// <summary>
    /// Podesavanja za webLoader
    /// </summary>
    public class webLoaderSettings : commonRequestSettings
    {
        /// <summary>
        /// Time out in milliseconds -- settings set by spiderWebLoader
        /// </summary>
        public Int32 timeout { get; set; } = 2000;

        /// <summary>
        /// Trajanje jednog ticka u milisekundama
        /// </summary>
        // [XmlIgnore]
        [Category("Web Loader Settings")]
        [DisplayName("tickDuration")]
        [Description("Trajanje jednog ticka u milisekundama")]
        public Int32 tickDuration { get; set; } = 250;

        /// <summary>
        /// Posle koliko neuspesno izvrsenih requestova da pozove promenu proxija
        /// </summary>
        // [XmlIgnore]
        [Category("Web Loader Proxy")]
        [DisplayName("proxyErrorChangeTrigger")]
        [Description("Posle koliko neuspesno izvrsenih requestova da pozove promenu proxija")]
        public Int32 proxyErrorChangeTrigger { get; set; } = 1;

        private Int32 _proxyExecutedChangeTrigger = 10; // = new Int32();

        /// <summary>
        /// Posle koliko uspesno izvrsenih requestova da pozove promenu proxija
        /// </summary>
        // [XmlIgnore]
        [Category("Web Loader Proxy")]
        [DisplayName("proxyExecutedChangeTrigger")]
        [Description("Posle koliko uspesno izvrsenih requestova da pozove promenu proxija")]
        public Int32 proxyExecutedChangeTrigger
        {
            get { return _proxyExecutedChangeTrigger; }
            set
            {
                // Boolean chg = (_proxyExecutedChangeTrigger != value);
                _proxyExecutedChangeTrigger = value;
                OnPropertyChanged("proxyExecutedChangeTrigger");
                // if (chg) {}
            }
        }

        private Boolean _doUseProxy = false;

        /// <summary>
        /// Da li da koristi proxy modul - ako postoji
        /// </summary>
        [Category("Web Loader Proxy")]
        [DisplayName("doUseProxy")]
        [Description("Da li da koristi proxy modul - ako postoji")]
        public Boolean doUseProxy
        {
            get { return _doUseProxy; }
            set
            {
                _doUseProxy = value;
                OnPropertyChanged("doUseProxy");
            }
        }

        private Boolean _doUseCache = true;

        /// <summary>
        /// Da li da koristi cache sistem
        /// </summary>
        [Category("Web Loader Settings")]
        [DisplayName("doUseCache")]
        [Description("Da li da koristi cache sistem")]
        public Boolean doUseCache
        {
            get { return _doUseCache; }
            set
            {
                _doUseCache = value;
                OnPropertyChanged("doUseCache");
            }
        }

        /// <summary>
        /// Mode for block execution
        /// </summary>
        // [XmlIgnore]
        [Category("Web Request Block")]
        [DisplayName("blockExecuteMode")]
        [Description("Mode for block execution")]
        public webRequestBlockExecuteMode blockExecuteMode { get; set; } = webRequestBlockExecuteMode.parallelSingleThread;

        /// <summary>
        /// Da li da pokusava ponovo i kada je timeout error
        /// </summary>
        [Category("Web Request Retry")]
        [DisplayName("doRetryOnTimeoutError")]
        [Description("Da li da pokusava ponovo i kada je timeout error")]
        public Boolean doRetryOnTimeoutError { get; set; } = false;

        /// <summary>
        /// Da li da pokusava ponovo i kada je Content error
        /// </summary>
        [Category("Web Request Retry")]
        [DisplayName("doRetryOnContentError")]
        [Description("Da li da pokusava ponovo i kada je Content error")]
        public Boolean doRetryOnContentError { get; set; } = false;

        /// <summary>
        /// Podesavanja za HTML dom
        /// </summary>
        [Category("Web Result")]
        [DisplayName("HTML settings")]
        [Description(" Podesavanja za HTML dom")]
        [ExpandableObject]
        public htmlDomSettings htmlSettings { get; set; } = new htmlDomSettings();
    }
}