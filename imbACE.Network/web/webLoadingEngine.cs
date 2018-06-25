namespace imbACE.Network.web
{
    #region imbVeles using

    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.request;
    using imbSCI.Core.extensions.typeworks;
    using System;
    using System.Xml;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// Engine za web ucitavanja
    /// </summary>
    public static class webLoadingEngine
    {
        private static String _defNSprefix;

        #region imbObject Property <String> defNSprefix

        /// <summary>
        /// imbControl property defNSprefix tipa String
        /// </summary>
        public static String defNSprefix
        {
            get { return _defNSprefix; }
            set { _defNSprefix = value; }
        }

        #endregion imbObject Property <String> defNSprefix

        #region --- mainLoader ------- Sistemski web loader

        private static webLoader _mainLoader; // = new webLoader();

        /// <summary>
        ///  Sistemski web loader
        /// </summary>
        public static webLoader mainLoader
        {
            get
            {
                if (_mainLoader == null)
                {
                    _mainLoader = new webLoader();
                }
                return _mainLoader;
            }
            set { _mainLoader = value; }
        }

        #endregion --- mainLoader ------- Sistemski web loader

        public static String getNamespacePrefix(this XmlDocument xml)
        {
            String uri = xml.NamespaceURI;
            return xml.GetPrefixOfNamespace(uri);
        }

        /// <summary>
        /// Pravi equest na osnovu settings a ima mogucnost override-a za URL i ACTION parametre
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="__url"></param>
        /// <param name="__action"></param>
        /// <returns></returns>
        public static webRequest createNewRequest(commonRequestSettings settings, String __url = null,
                                                  webRequestActionType __action = webRequestActionType.None)
        {
            if (String.IsNullOrEmpty(__url)) __url = settings.url;
            if (__action == webRequestActionType.None) __action = settings.action;
            webRequest request = webLoadingEngine.createNewRequest(__url, __action, settings.requestType);
            request.setObjectBySource(settings, new String[] { "url", "action" });
            return request;
        }

        /// <summary>
        /// Kreira novi Request
        /// </summary>
        /// <param name="__url"></param>
        /// <param name="__action"></param>
        /// <param name="__type"></param>
        /// <returns></returns>
        public static webRequest createNewRequest(String __url,
                                                  webRequestActionType __action = webRequestActionType.openUrl,
                                                  webRequestType __type = webRequestType.unknown)
        {
            webRequest output = null;

            if (__type == webRequestType.unknown)
            {
                __type = webRequestBase.getPreference(__action);
            }

            switch (__type)
            {
                case webRequestType.webRequestBrowser:
                    output = new webRequestBrowser(__url, __action);
                    break;

                case webRequestType.webRequestClient:
                    output = new webRequestClient(__url, __action);
                    break;

                case webRequestType.webRequestLookup:
                    output = new webRequestLookup(__url, __action);
                    break;

                case webRequestType.webRequestFile:
                    output = new webRequestFile(__url, __action);
                    break;

                default:
                    break;
            }

            return output;
        }
    }
}