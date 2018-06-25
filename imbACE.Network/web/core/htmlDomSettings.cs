namespace imbACE.Network.web.core
{
    using HtmlAgilityPack;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.ComponentModel;
    using System.Net;

    //using imbReportingCore.reporting.process;

    #endregion imbVeles using

    /// <summary>
    /// 2013c: LowLevel resurs
    /// </summary>
    public class htmlDomSettings : imbBindable
    {
        private Boolean _doRemoveHtmlEntities = true;

        private Boolean _doTransliterateToLat = true;

        #region imbObject Property <Boolean> doTransliterateToLat

        /// <summary>
        /// Da izvrsi transliteraciju na latinicu
        /// </summary>
        [Category("HTML Postprocessing")]
        [DisplayName("Transliterate to Latin")]
        [Description("If True phaser will make transliteration of all Cyrilic characherts into Latin")]
        public Boolean doTransliterateToLat
        {
            get { return _doTransliterateToLat; }
            set
            {
                _doTransliterateToLat = value;
                OnPropertyChanged("doTransliterateToLat");
            }
        }

        #endregion imbObject Property <Boolean> doTransliterateToLat

        #region imbObject Property <Boolean> doRemoveHtmlEntities

        /// <summary>
        /// Da skloni HTML entitie
        /// </summary>
        [Category("HTML Postprocessing")]
        [DisplayName("Remove HTML Entities")]
        [Description("If True phaser will remove all HTML entities after loading HTML")]
        public Boolean doRemoveHtmlEntities
        {
            get { return _doRemoveHtmlEntities; }
            set
            {
                _doRemoveHtmlEntities = value;
                OnPropertyChanged("doRemoveHtmlEntities");
            }
        }

        #endregion imbObject Property <Boolean> doRemoveHtmlEntities

        /// <summary>
        /// Izvrsava filtriranje HTML source code-a prema podesavanjima
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String sourcePreFilter(String input)
        {
            if (doRemoveHtmlEntities)
            {
                try
                {
                    input = WebUtility.HtmlDecode(input);
                }
                catch (Exception ex)
                {
                    throw;
                    // this.note(ex);
                }
            }

            if (doTransliterateToLat)
            {
                //input = transliteration.transliterate(input, true);
            }
            return input;
        }

        /// <summary>
        /// Priprema HtmlDocument radi ucitavanja DOM-a
        /// </summary>
        /// <returns></returns>
        public HtmlDocument getBlankDocument()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.OptionCheckSyntax = true;

            htmlDocument.OptionFixNestedTags = true;

            htmlDocument.OptionWriteEmptyNodes = doWriteEmptyNodes;
            htmlDocument.OptionAutoCloseOnEnd = doAutocloseOnEnd;
            htmlDocument.OptionStopperNodeName = "pharserFailed";
            htmlDocument.OptionOutputOptimizeAttributeValues = true;

            htmlDocument.OptionAddDebuggingAttributes = doAddDebugAttributes;
            htmlDocument.OptionOutputUpperCase = doUpperCase;

            return htmlDocument;
        }

        #region ----------- Boolean [ doUpperCase ] -------  [Da li da svi tagovi budu u upper case-u]

        private Boolean _doUpperCase = true;

        /// <summary>
        /// Da li da svi tagovi budu u upper case-u
        /// </summary>
        [Category("Switches")]
        [DisplayName("doUpperCase")]
        [Description("Da li da svi tagovi budu u upper case-u")]
        public Boolean doUpperCase
        {
            get { return _doUpperCase; }
            set
            {
                _doUpperCase = value;
                OnPropertyChanged("doUpperCase");
            }
        }

        #endregion ----------- Boolean [ doUpperCase ] -------  [Da li da svi tagovi budu u upper case-u]

        #region ----------- Boolean [ doAutocloseOnEnd ] -------  [Da li da automatski zatvori tagove koji su ostali otvoreni]

        private Boolean _doAutocloseOnEnd = true;

        /// <summary>
        /// Da li da automatski zatvori tagove koji su ostali otvoreni
        /// </summary>
        [Category("Switches")]
        [DisplayName("doAutocloseOnEnd")]
        [Description("Da li da automatski zatvori tagove koji su ostali otvoreni")]
        public Boolean doAutocloseOnEnd
        {
            get { return _doAutocloseOnEnd; }
            set
            {
                _doAutocloseOnEnd = value;
                OnPropertyChanged("doAutocloseOnEnd");
            }
        }

        #endregion ----------- Boolean [ doAutocloseOnEnd ] -------  [Da li da automatski zatvori tagove koji su ostali otvoreni]

        #region ----------- Boolean [ doWorkInSafeMode ] -------  [Da li da radi u sigurnom modu]

        private Boolean _doWorkInSafeMode = false;

        /// <summary>
        /// Da li da radi u sigurnom modu
        /// </summary>
        [Category("Switches")]
        [DisplayName("doWorkInSafeMode")]
        [Description("Da li da radi u sigurnom modu")]
        public Boolean doWorkInSafeMode
        {
            get { return _doWorkInSafeMode; }
            set
            {
                _doWorkInSafeMode = value;
                OnPropertyChanged("doWorkInSafeMode");
            }
        }

        #endregion ----------- Boolean [ doWorkInSafeMode ] -------  [Da li da radi u sigurnom modu]

        #region ----------- Boolean [ doAddDebugAttributes ] -------  [Da li da]

        private Boolean _doAddDebugAttributes = false;

        /// <summary>
        /// Da li da
        /// </summary>
        [Category("Switches")]
        [DisplayName("doAddDebugAttributes")]
        [Description("Da li da")]
        public Boolean doAddDebugAttributes
        {
            get { return _doAddDebugAttributes; }
            set
            {
                _doAddDebugAttributes = value;
                OnPropertyChanged("doAddDebugAttributes");
            }
        }

        #endregion ----------- Boolean [ doAddDebugAttributes ] -------  [Da li da]

        #region ----------- Boolean [ doWriteEmptyNodes ] -------  [Da li ispisuje i prazne nodove]

        private Boolean _doWriteEmptyNodes = false;

        /// <summary>
        /// Da li ispisuje i prazne nodove
        /// </summary>
        [Category("Switches")]
        [DisplayName("doWriteEmptyNodes")]
        [Description("Da li ispisuje i prazne nodove")]
        public Boolean doWriteEmptyNodes
        {
            get { return _doWriteEmptyNodes; }
            set
            {
                _doWriteEmptyNodes = value;
                OnPropertyChanged("doWriteEmptyNodes");
            }
        }

        #endregion ----------- Boolean [ doWriteEmptyNodes ] -------  [Da li ispisuje i prazne nodove]
    }
}