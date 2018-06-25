namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.core;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using System;

    #endregion imbVeles using

    /// <summary>
    /// Request koji ze izvrsava preko Browser kontrole
    /// </summary>
    public class webRequestBrowser : webRequest
    {
        //[XmlIgnore]
        //public webResult result
        //{
        //    get
        //    {
        //        if (_result == null) _result = new webResult(this);
        //        return _result;
        //    }
        //    set
        //    {
        //        _result = value;
        //        OnPropertyChanged("result");
        //    }
        //}

        public webRequestBrowser(String __url = "", webRequestActionType __action = webRequestActionType.openUrl)
        {
            url = __url;
            action = __action;
        }

        /// <summary>
        /// Izvrsavanje
        /// </summary>
        /// <param name="settings">Podesavanja loadera</param>
        /// <param name="__syncMode">Tip izvrsavanja</param>
        /// <param name="__onExecutedOk">kada je executedOk</param>
        /// <param name="__onError">za sve vrste gresaka</param>
        /// <param name="__onRetry">kada se desi retry</param>
        public override webResult executeRequest(webLoaderSettings settings, executionSyncMode __syncMode,
                                                 webRequestEvent __onExecutedOk, webRequestEvent __onError,
                                                 webRequestEvent __onRetry)
        {
            return _executeRequest(settings, __syncMode, __onExecutedOk, __onError, __onRetry);
        }

        protected override void executeBegin(webLoaderSettings settings)
        {
            switch (action)
            {
                case webRequestActionType.openUrl:

                case webRequestActionType.GetHeaders:
                case webRequestActionType.Text:
                case webRequestActionType.XML:
                case webRequestActionType.HTMLasXML:

                    break;

                default:
                    logSystem.log("Action :: " + action + " not supported by " + GetType().Name, logType.ExecutionError);
                    callExecutionError(webRequestEventType.error);
                    break;
            }
        }

        protected override void executeEnd(webLoaderSettings settings)
        {
            logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }

        public override void cancelAllActivities()
        {
            logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }

        public override bool hasPreference(webRequestActionType __action)
        {
            switch (__action)
            {
                case webRequestActionType.openUrl:
                    return true;
                    break;

                default:
                    return false;
                    break;
            }
        }

        protected override void getResponse(IAsyncResult result)
        {
            logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }
    }
}