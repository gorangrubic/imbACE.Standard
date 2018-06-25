namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using System;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Abstraktna osnova za webRequest
    /// </summary>
    public abstract class webRequestBase : commonRequestSettings
    {
        /// <summary>
        /// Da li je aktivan Request - if status == webRequestEventType.scheduled
        /// </summary>
        public Boolean isActive
        {
            get
            {
                if (status == webRequestEventType.scheduled) return false;
                return !isExecutedOrFailed;
            }
        }

        /// <summary>
        /// Da li je izvrseno/greska? ako je scheduled ili executing onda vraca false
        /// </summary>
        public Boolean isExecutedOrFailed
        {
            get
            {
                switch (status)
                {
                    case webRequestEventType.executedOk:
                        return true;

                    default:
                        return isErrorStatus;
                }
            }
        }

        /// <summary>
        /// Da li je trenutno u statusu greske?
        /// </summary>
        public Boolean isErrorStatus
        {
            get { return status.ToString().Contains("error"); }
        }

        /// <summary>
        /// da li ima Content Criteria
        /// </summary>
        public Boolean hasContentCriteria
        {
            get
            {
                if (String.IsNullOrEmpty(criteriaValue)) return false;
                if (criteriaType == contentCriteriaType.disabled) return false;
                return true;
            }
        }

        #region --- status ------- Bindable property

        private webRequestEventType _status = webRequestEventType.error;

        /// <summary>
        /// Bindable property
        /// </summary>
        public webRequestEventType status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        #endregion --- status ------- Bindable property

        #region --- urlObject ------- URI objekat za prosledjeni URL

        private Uri _urlObject;

        /// <summary>
        /// URI objekat za prosledjeni URL
        /// </summary>
        [XmlIgnore]
        public Uri urlObject
        {
            get { return _urlObject; }
            set
            {
                _urlObject = value;
                OnPropertyChanged("urlObject");
            }
        }

        #endregion --- urlObject ------- URI objekat za prosledjeni URL

        public abstract webResult executeRequest(webLoaderSettings settings, executionSyncMode __syncMode,
                                                 webRequestEvent __onExecutedOk, webRequestEvent __onError,
                                                 webRequestEvent __onRetry);

        /// <summary>
        /// Pocinje izvrsavanje requesta
        /// </summary>
        /// <param name="settings"></param>
        protected abstract void executeBegin(webLoaderSettings settings);

        /// <summary>
        /// zavrsava izvrsavanje requesta
        /// </summary>
        /// <param name="settings"></param>
        protected abstract void executeEnd(webLoaderSettings settings);

        /// <summary>
        /// Prekida sve aktivnosti web client, tcp client, web browser i drugih objekata
        /// </summary>
        public abstract void cancelAllActivities();

        // da li ima preferencije ka ovoj akciji
        public abstract Boolean hasPreference(webRequestActionType __action);

        public static webRequestType getPreference(webRequestActionType __action)
        {
            switch (__action)
            {
                case webRequestActionType.Download:
                case webRequestActionType.FTPUpload:
                case webRequestActionType.FTPDownload:
                case webRequestActionType.localFile:
                    return webRequestType.webRequestFile;
                    break;

                case webRequestActionType.ipResolve:
                case webRequestActionType.dnsResolve:
                case webRequestActionType.CheckUrlOnly:
                case webRequestActionType.whoIs:
                    return webRequestType.webRequestLookup;
                    break;

                case webRequestActionType.openUrl:
                case webRequestActionType.GetHeaders:
                case webRequestActionType.Text:
                case webRequestActionType.XML:
                case webRequestActionType.HTMLasXML:

                    return webRequestType.webRequestClient;
                    break;

                default:
                    return webRequestType.unknown;
                    break;
            }
        }
    }
}