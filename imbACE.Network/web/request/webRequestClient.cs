namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.core;
    using imbACE.Network.extensions;
    using imbACE.Network.web.cache;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using imbSCI.Data.enums;
    using System;
    using System.IO;
    using System.Net;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Standardni web request - ide preko web client-a
    /// </summary>
    public class webRequestClient : webRequest
    {
        public WebRequest httpRequest;

        #region --- method ------- Method kojim se prostupa web resursu

        private httpRequestMethod _method = httpRequestMethod.unknown;

        /// <summary>
        /// Method kojim se prostupa web resursu
        /// </summary>
        public httpRequestMethod method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged("method");
            }
        }

        #endregion --- method ------- Method kojim se prostupa web resursu

        public webRequestClient(String __url = "", webRequestActionType __action = webRequestActionType.openUrl)
        {
            url = __url;
            action = __action;

            //if (__url.StartsWith("/"))
            //{
            //    throw new aceGeneralException("URL has no shema and domain", null, this, "No shema [" + __url + "]");
            //}
        }

        [XmlIgnore]
        public new webResult result
        {
            get
            {
                if (_result == null) _result = new webResult(this);
                return _result;
            }
            set
            {
                _result = value;
                OnPropertyChanged("result");
            }
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
            url = imbUrlOps.getStandardizedUrl(url, urlShema.http);
            settings.url = url;

            return _executeRequest(settings, __syncMode, __onExecutedOk, __onError, __onRetry);
        }

        protected override void executeBegin(webLoaderSettings settings)
        {
            if (urlObject == null)
            {
                logSystem.log(
                    "Url object is null [" + action.ToString() + "] - aborting webRequest [" +
                    settings.requestType.ToString() + "]", logType.ExecutionError);
                getResponse(null);
                return;
            }

            httpRequest = WebRequest.Create(urlObject);
            httpRequest.Timeout = settings.timeout;

            if (proxyToUse != null)
            {
                httpRequest.Proxy = proxyToUse;
            }

            if (method == httpRequestMethod.unknown)
            {
                httpRequest.Method = "GET";
            }
            else
            {
                httpRequest.Method = method.ToString();
            }

            switch (action)
            {
                case webRequestActionType.openUrl:

                    break;

                case webRequestActionType.GetHeaders:
                    httpRequest.Method = "HEAD";
                    break;

                case webRequestActionType.Text:
                //webClient = new WebClient();
                //webClient.OpenReadAsync(urlObject, this);
                //break;
                case webRequestActionType.XML:
                case webRequestActionType.HTMLasXML:
                    break;

                default:
                    logSystem.log("Action :: " + action + " not supported by " + GetType().Name, logType.ExecutionError);
                    callExecutionError(webRequestEventType.error);
                    return;
                    break;
            }

            switch (action)
            {
                //case webRequestActionType.Text:
                //    webClient = new WebClient();
                //    webClient.OpenReadAsync(urlObject, this);
                //    break;
                default:
                    httpRequest.BeginGetResponse(getResponse, this);
                    break;
            }
        }

        protected override void executeEnd(webLoaderSettings settings)
        {
            //switch (action)
            //{
            //    case webRequestActionType.CheckUrlOnly:

            //        break;
            //    default:
            //        break;
            //}
            //result httpRequest.ContentLength;
            callExecutionDone();
        }

        public override void cancelAllActivities()
        {
            if (httpRequest != null)
            {
                aceLog.loger.AppendLine(" http request aborted [" + url + "]");
                httpRequest?.Abort();
            }
            else
            {
                aceLog.loger.AppendLine(" Non-http request aborted for [" + url + "]");
            }
        }

        public override bool hasPreference(webRequestActionType __action)
        {
            return (webRequestBase.getPreference(__action) == webRequestType.webRequestClient);
        }

        internal String getClientInfo()
        {
            String output = httpRequest.RequestUri.AbsoluteUri.ToString();
            if (proxyToUse != null)
            {
                output += " (" + proxyToUse.ToString() + ") ";
            }
            //output += " "+proxyToUse.GetProxy().
            return output;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="__result">The result.</param>
        protected override void getResponse(IAsyncResult __result)
        {
            if (__result == null)
            {
                status = webRequestEventType.error;
                return;
            }

            if (status == webRequestEventType.error)
            {
                return;
            }
            else
            {
            }

            try
            {
                result.response = new webResponse(httpRequest.EndGetResponse(__result));
            }
            catch (Exception ex)
            {
                // logSystem.log("Web Request error ["+getClientInfo()+"]: " + ex.Message, logType.Warning);
                status = webRequestEventType.error;
                // result.response = new webResponse();
                // result.response.
                //source = ex.Message;

                //throw;
            }

            if (status == webRequestEventType.error)
            {
                return;
            }
            else
            {
            }
            String source = "";
            StreamReader reader = null;
            switch (action)
            {
                //case webRequestActionType.Text:

                //    break;
                default:
                case webRequestActionType.openUrl:
                case webRequestActionType.GetHeaders:
                case webRequestActionType.XML:
                case webRequestActionType.HTMLasXML:
                    try
                    {
                        reader = new StreamReader(result.response._response.GetResponseStream());
                    }
                    catch (Exception ex)
                    {
                        //  this.note(ex);
                        callExecutionError(webRequestEventType.error, "Error in response Stream catch :: " + ex.Message);
                    }

                    try
                    {
                        source = reader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        //this.note(ex);
                        callExecutionError(webRequestEventType.error, "Error in stream reading :: " + ex.Message);
                        if (source == null) source = "";
                        source += "<div> " + ex.Message + "</div>";
                        //throw;
                    }
                    break;
            }

            try
            {
                result.document.deploySource(source, action, result.request.htmlSettings);
                status = webRequestEventType.loaded;
            }
            catch (Exception ex)
            {
                //this.note(ex);
                callExecutionError(webRequestEventType.error, "Error in document sourceDeploying :: " + ex.Message);
                source += "<div> " + ex.Message + "</div>";
                //throw;
            }

            if (settings.doUseCache)
            {
                if (status == webRequestEventType.loaded)
                {
                    webCacheSystem.saveCache(url, source, result.response);
                }
            }
        }

        #region --- webClient ------- Klijent za HTML agility operacije

        private WebClient _webClient;

        /// <summary>
        /// Klijent za HTML agility operacije
        /// </summary>
        public WebClient webClient
        {
            get { return _webClient; }
            set
            {
                _webClient = value;
                OnPropertyChanged("webClient");
            }
        }

        #endregion --- webClient ------- Klijent za HTML agility operacije
    }
}