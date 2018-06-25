namespace imbACE.Network.web.result
{
    #region imbVeles using

    using HtmlAgilityPack;
    using imbACE.Core.core;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// Osnovni nosilac ucitanog dokumenta/podatka
    /// </summary>
    public class webDocument : imbBindable, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection -- automatically called from webResultBase
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            // this.buildPropertyCollection(false, false, "doc", data);

            data[templateFieldWebRequest.doc_documentType] = documentType;
            data[templateFieldWebRequest.doc_nsPrefix] = nsPrefix;
            data[templateFieldWebRequest.doc_hasDocument] = hasDocument;

            //  data[templateFieldWebRequest.doc_source] = source;
            // data[templateFieldWebRequest.doc_sourceNormalized] = processedDocumentSource();

            // data[target.doc_description] = description;
            // data[target.doc_id] = id;
            // data[target.doc_url] = url;
            return data;
        }

        private XPathNavigator _nav = null;
        private String _nsPrefix;

        [XmlIgnore] public XmlNamespaceManager nsManager;

        #region imbObject Property <String> sourceNSPrefix

        /// <summary>
        /// Podrazumevani namespace prefix dokumenta koji je trenutno ucitan
        /// </summary>
        public String nsPrefix
        {
            get { return _nsPrefix; }
            set
            {
                _nsPrefix = value;
                OnPropertyChanged("sourceNSPrefix");
            }
        }

        #endregion imbObject Property <String> sourceNSPrefix

        /// <summary>
        /// Da li ima document DOM objekat instanciran
        /// </summary>
        [XmlIgnore]
        public Boolean hasDocument
        {
            get { return (_document != null); }
        }

        #region -----------  document  -------  [DOM ucitanog dokumenta]

        private Object _document; // = new Object();

        /// <summary>
        /// DOM ucitanog dokumenta
        /// </summary>
        // [XmlIgnore]
        [Category("webDocument")]
        [DisplayName("document")]
        [Description("DOM ucitanog dokumenta")]
        public Object document
        {
            get { return _document; }
            set
            {
                // Boolean chg = (_document != value);
                _document = value;
                OnPropertyChanged("document");
                // if (chg) {}
            }
        }

        #endregion -----------  document  -------  [DOM ucitanog dokumenta]

        /// <summary>
        /// Preuzima __source String i pravi DOM objekat prema podesavanju
        /// </summary>
        /// <param name="__source"></param>
        /// <param name="action"></param>
        /// <param name="htmlSettings"></param>
        internal void deploySource(String __source, webRequestActionType action, htmlDomSettings htmlSettings = null)
        {
            source = __source;
            // null;

            switch (action)
            {
                case webRequestActionType.openUrl:
                    HtmlDocument html = htmlSettings.getBlankDocument();
                    source = htmlSettings.sourcePreFilter(source);
                    html.LoadHtml(source);
                    document = __source;
                    break;

                case webRequestActionType.ipResolve:
                case webRequestActionType.XML:
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(source);
                    document = xml;
                    nsPrefix = xml.getNamespacePrefix();
                    nsManager = new XmlNamespaceManager(xml.NameTable);
                    break;

                case webRequestActionType.HTMLasXML:
                    HtmlDocument html2 = htmlSettings.getBlankDocument();
                    html2.OptionOutputAsXml = true;

                    source = htmlSettings.sourcePreFilter(source);
                    html2.LoadHtml(source);
                    _nav = html2.CreateNavigator();
                    nsManager = new XmlNamespaceManager(_nav.NameTable);
                    nsPrefix = _nav.NamespaceURI;
                    //html2.DocumentNode as XmlNode;
                    //source.ToXmlDOM();
                    document = html2;
                    processedSource = html2.DocumentNode.OuterHtml;
                    break;

                case webRequestActionType.Text:
                case webRequestActionType.whoIs:
                case webRequestActionType.CheckUrlOnly:
                default:
                    document = __source;
                    break;
            }

            _documentType = detectDocumentType();
        }

        /// <summary>
        /// Utvrdjuje tip dokumenta
        /// </summary>
        /// <returns></returns>
        internal webDocumentType detectDocumentType()
        {
            if (document == null) return webDocumentType.nullObject;

            if (document is imbBindable)
            {
                return webDocumentType.imbBindable;
            }

            String typeName = document.GetType().Name;

            switch (typeName)
            {
                case "String":
                    return webDocumentType.textString;
                    break;

                case "XmlDocument":
                    return webDocumentType.xmlDocument;
                    break;

                case "HtmlDocument":
                    return webDocumentType.htmlAgilityDocument;
                    break;

                case "HTMLDocument":
                    return webDocumentType.htmlMicrosoftDocument;
                    break;

                default:
                    return webDocumentType.unknown;
                    break;
            }
        }

        /// <summary>
        /// Proverava da li dokument ispunjava kriterijum
        /// </summary>
        /// <param name="criteriaValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal Boolean checkCriteria(String criteriaValue, contentCriteriaType type)
        {
            Boolean output = false;
            Int32 length = 0;
            switch (type)
            {
                case contentCriteriaType.regexMatch:
                    Regex regex = new Regex(criteriaValue);
                    output = regex.IsMatch(source);
                    break;

                case contentCriteriaType.xPathExists:
                    if (documentType == webDocumentType.xmlDocument)
                    {
                        logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
                    }
                    else
                    {
                        logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
                    }
                    break;

                case contentCriteriaType.linesCount:
                    Int32 lc = imbStringFormats.getInt32Safe(criteriaValue);
                    String[] lines = source.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    output = (lines.Count() >= lc);
                    break;

                case contentCriteriaType.charactersCount:
                    Int32 cc = imbStringFormats.getInt32Safe(criteriaValue);
                    output = (source.Count() >= cc);
                    break;

                case contentCriteriaType.disabled:
                    output = true;
                    break;

                default:
                    break;
            }

            return output;
        }

        private String _processedSource = "[never set]";

        /// <summary> </summary>
        public String processedSource
        {
            get
            {
                return _processedSource;
            }
            protected set
            {
                _processedSource = value;
                OnPropertyChanged("processedSource");
            }
        }

        /// <summary>
        /// Clears all document source, processed source, HtmlAgility document and XPath navigator from the memory
        /// </summary>
        public void releaseDocumentFromMemory()
        {
            processedSource = "";
            source = "";
            document = null;
            _nav = null;
        }

        /// <summary>
        /// Processing document source
        /// </summary>
        /// <returns></returns>
        internal String processedDocumentSource()
        {
            String output = "";
            switch (documentType)
            {
                case webDocumentType.xmlDocument:
                    var xmlTmp = document as XmlDocument;
                    output = xmlTmp.OuterXml;
                    break;

                case webDocumentType.htmlAgilityDocument:
                    var htmlTmp = document as HtmlDocument;
                    output = htmlTmp.DocumentNode.OuterHtml;
                    break;

                case webDocumentType.htmlMicrosoftDocument:
                    //var xmlTmp = document as
                    //output = xmlTmp.OuterXml;
                    break;

                case webDocumentType.textString:
                    output = document.ToString();
                    break;

                case webDocumentType.unknown:
                    output = document.ToString();
                    break;
            }
            return output;
        }

        public T getDocument<T>() where T : class, IXPathNavigable
        {
            if (hasDocument)
            {
                return document as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Glavna komanda za dobijanje navigatora
        /// </summary>
        /// <returns></returns>
        public XPathNavigator getDocumentNavigator()
        {
            if (_nav != null)
            {
                return _nav;
            }
            XPathNavigator output = null;
            IXPathNavigable __tmp = document as IXPathNavigable;
            if (__tmp != null)
            {
                _nav = __tmp.CreateNavigator();
            }
            return _nav;
        }

        #region -----------  source  -------  [Izvodni kod ucitanog web dokumenta]

        private String _source; // = new String();

        /// <summary>
        /// Izvodni kod ucitanog web dokumenta
        /// </summary>
        // [XmlIgnore]
        [Category("webDocument")]
        [DisplayName("source")]
        [Description("Izvodni kod ucitanog web dokumenta")]
        public String source
        {
            get { return _source; }
            set
            {
                // Boolean chg = (_source != value);
                _source = value;
                OnPropertyChanged("source");
                // if (chg) {}
            }
        }

        #endregion -----------  source  -------  [Izvodni kod ucitanog web dokumenta]

        #region --- documentType ------- tip DOM-a

        private webDocumentType _documentType = webDocumentType.unknown;

        /// <summary>
        /// tip DOM-a
        /// </summary>
        public webDocumentType documentType
        {
            get
            {
                if (_documentType == webDocumentType.unknown) _document = detectDocumentType();
                return _documentType;
            }
            set
            {
                _documentType = value;
                OnPropertyChanged("documentType");
            }
        }

        #endregion --- documentType ------- tip DOM-a
    }
}