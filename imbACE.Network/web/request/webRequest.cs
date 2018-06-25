namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.cache;
    using imbACE.Core.core;
    using imbACE.Network.extensions;
    using imbACE.Network.web.cache;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting;
    using imbSCI.Data.enums;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Net;
    using System.Threading;
    using System.Xml.Serialization;

    //using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

    #endregion imbVeles using

    /// <summary>
    /// Osnovna varijanta zahteva - omogucava TimeOut
    /// </summary>
    public abstract class webRequest : webRequestBase, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection --- automatically called by result base
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            this.buildPropertyCollection(false, false, "target", data);

            // data[target.target_name] = name;
            // data[target.target_description] = description;
            // data[target.target_id] = id;
            // data[target.target_url] = url;
            return data;
        }

        #region --- proxyToUse ------- Definicija proxija koji treba da se koristi

        private IWebProxy _proxyToUse;

        /// <summary>
        /// Definicija proxija koji treba da se koristi
        /// </summary>
        public IWebProxy proxyToUse
        {
            get { return _proxyToUse; }
            set
            {
                _proxyToUse = value;
                OnPropertyChanged("proxyToUse");
            }
        }

        #endregion --- proxyToUse ------- Definicija proxija koji treba da se koristi

        private string _lastLogMessage;
        private logType _lastLogType;

        protected webResult _result;

        [XmlIgnore]
        public webResult result
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

        #region ILogStatusInformation Members

        public logType lastLogType
        {
            get { return _lastLogType; }
            set { _lastLogType = value; }
        }

        public string lastLogMessage
        {
            get { return _lastLogMessage; }
            set { _lastLogMessage = value; }
        }

        #endregion ILogStatusInformation Members

        /// <summary>
        /// Trenutni rezultat requesta
        /// </summary>
        protected void syncWaitingLoop(webLoaderSettings settings, executionSyncMode __syncMode)
        {
            DateTime start = DateTime.Now;

            if (__syncMode == executionSyncMode.synced)
            {
                while (isActive)
                {
                    Thread.Sleep(settings.tickDuration);

                    TimeSpan ts = DateTime.Now.Subtract(start);
                    if (ts.TotalMilliseconds > settings.timeout)
                    {
                        aceLog.saveAllLogs(true);
                        abortRequest(settings);
                    }

                    checkRequest(settings);
                }
            }
        }

        protected abstract void getResponse(IAsyncResult result);

        /// <summary>
        /// Primenjuje podesavanja i postavlja event handlere
        /// </summary>
        /// <param name="settings">Podesavanja koja treba da primenu</param>
        /// <param name="__onExecutedOk">Poziva se nakon finalizacije</param>
        /// <param name="__onError">Poziva se za timeout ili neku drugu gresku</param>
        /// <param name="__onRetry">Poziva se ako dodje do ponavljanja requesta</param>
        public void deploySettings(webLoaderSettings __settings, webRequestEvent __onExecutedOk, webRequestEvent __onError,
                                   webRequestEvent __onRetry)
        {
            status = webRequestEventType.scheduled;
            settings = __settings;
            result = null;

            //   if (settings.action != webRequestActionType.None) action = settings.action;
            //  if (settings.requestType != webRequestType.unknown) requestType = settings.requestType;

            //  imbTypologyExtensions.setObjectBySource(this, settings, new string[]{"url", "action", "requestType"});

            //timeCounter.
            //retryWaitCounter.setup(settings.retryWaitDefault, true);
            //retryCounter.setup(settings.retryLimitDefault, true);
            //loadCooloffCounter.setup(settings.loadCoolOffDefault, true);
            //contentCriteriaCheckDelayCounter.setup(settings.contentCriteriaCheckDelayDefault, true);
            onRetryCalled_addHandler(__onRetry);
            onExecutionTimeout_addHandler(__onError);
            onExecutionError_addHandler(__onError);
            onExecutionDone_addHandler(__onExecutedOk);

            if (String.IsNullOrEmpty(url))
            {
                logSystem.log("Url is null/empty!!!", logType.Notification);
            }
            else
            {
                Uri urlObj = null;
                if (Uri.TryCreate(url, UriKind.Absolute, out urlObj))
                {
                    urlObject = urlObj;
                }
                else
                {
                }
            }
        }

        private webLoaderSettings _settings;

        /// <summary>
        ///
        /// </summary>
        public webLoaderSettings settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Abort the current request
        /// </summary>
        /// <param name="settings"></param>
        public void abortRequest(webLoaderSettings settings)
        {
            cancelAllActivities();
            deploySettings(settings, null, null, null);
        }

        internal void loadCache()
        {
            cacheResponse cr = webCacheSystem.loadCache(url);
            if (cr.cacheFound)
            {
                //result.response = new webResponse();
                result.response = cr.httpContent;
                result.request.status = status;
                if (cr.httpContent == null)
                {
                    status = webRequestEventType.callRetry;
                    // Thread.CurrentThread.Abort();
                }
                else
                {
                    result.request.url = cr.httpContent.responseUrl;

                    result.document.deploySource(cr.content, action, htmlSettings);
                    status = webRequestEventType.executedOk;
                    if (doLogCacheLoaded) aceLog.log("Cache: " + url + " (" + status.toString() + ")");
                }
            }
            else
            {
                status = webRequestEventType.callRetry;
            }
        }

        protected webResult _executeRequest(webLoaderSettings settings, executionSyncMode __syncMode,
                                            webRequestEvent __onExecutedOk, webRequestEvent __onError,
                                            webRequestEvent __onRetry)
        {
            deploySettings(settings, __onExecutedOk, __onError, __onRetry);

            if (settings.doUseCache && webCacheSystem.hasCache(url))
            {
                status = webRequestEventType.executing;
                Thread lc = new Thread(loadCache);
                lc.Start();
                syncWaitingLoop(settings, __syncMode);
                if (status == webRequestEventType.executedOk)
                {
                    return result;
                }
                else
                {
                }
                lc.Abort();
            }

            status = webRequestEventType.executing;
            executeBegin(settings);
            syncWaitingLoop(settings, __syncMode);

            if (doLogNewLoad) aceLog.log("Load: " + url + " (" + status.toString() + ")");
            return result;
        }

        protected void retry(webLoaderSettings settings)
        {
            timeCounter.reset();
            contentCriteriaCheckDelayCounter.reset();
            contentCriteriaTryCounter.reset();
            loadCooloffCounter.reset();
            status = webRequestEventType.executing;

            callRetryCalled();
            result = null;

            executeBegin(settings);
        }

        protected void checkContentCriteria()
        {
            if (hasContentCriteria)
            {
                status = webRequestEventType.waitingContentCriteria;
            }
            else
            {
                status = webRequestEventType.finalization;
            }
        }

        /// <summary>
        /// Calls all counters and check the status of request
        /// </summary>
        /// <param name="settings"></param>
        public void checkRequest(webLoaderSettings settings)
        {
            if (status == webRequestEventType.scheduled)
            {
                return;
            }

            #region isEXECUTED & Retry

            if (isExecutedOrFailed)
            {
                if (retryCounter.isRunning)
                {
                    Boolean tryRetry = (status == webRequestEventType.error);

                    if (settings.doRetryOnContentError) if (status == webRequestEventType.errorContent) tryRetry = true;
                    if (settings.doRetryOnTimeoutError) if (status == webRequestEventType.errorTimeout) tryRetry = true;

                    if (tryRetry)
                    {
                        if (!retryCounter.check(1, true))
                        {
                            // moze jos jedan retry
                            retryWaitCounter.reset();
                            status = webRequestEventType.waitingForRetry;
                            cancelAllActivities();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            // ako ceka retry uraditi retryWaitcheck
            if (status == webRequestEventType.waitingForRetry)
            {
                if (retryWaitCounter.check(1, true))
                {
                    retry(settings);
                }
                return;
            }

            #endregion isEXECUTED & Retry

            if (status == webRequestEventType.loaded)
            {
                if (loadCooloffCounter.isRunning)
                {
                    status = webRequestEventType.waitingCooloff;
                }
                else
                {
                    checkContentCriteria();
                }
            }

            if (status == webRequestEventType.waitingCooloff)
            {
                if (loadCooloffCounter.check(1, true))
                {
                    checkContentCriteria();
                }
            }

            if (status == webRequestEventType.waitingContentCriteria)
            {
                if (contentCriteriaCheckDelayCounter.check(1, true))
                {
                    if (contentCriteriaTryCounter.check(1, false))
                    {
                        status = webRequestEventType.errorContent;
                    }
                    else
                    {
                        if (result.document.checkCriteria(criteriaValue, criteriaType))
                        {
                            status = webRequestEventType.finalization;
                        }
                        else
                        {
                            // nije ispunjen uslov
                        }
                    }
                }
            }

            if (status == webRequestEventType.finalization)
            {
                executeEnd(settings);
                return;
            }

            /// Provera vremena
            if (timeCounter.check(1, false))
            {
                status = webRequestEventType.errorTimeout;
                callExecutionTimeout();
            }
        }

        /// <summary>
        /// Addhock request execution Executes the request
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public virtual webResult executeRequest(webLoaderSettings settings = null)
        {
            if (settings == null) settings = new webLoaderSettings();

            settings.url = imbUrlOps.getStandardizedUrl(url, urlShema.http);
            return _executeRequest(settings, executionSyncMode.synced, null, null, null);
        }

        #region --- htmlSettings ------- Podesavanja za HTML dom

        private htmlDomSettings _htmlSettings = new htmlDomSettings();

        /// <summary>
        /// Podesavanja za HTML dom
        /// </summary>
        [Category("Web Result")]
        [DisplayName("HTML settings")]
        [Description(" Podesavanja za HTML dom")]
        [ExpandableObject]
        public htmlDomSettings htmlSettings
        {
            get { return _htmlSettings; }
            set
            {
                _htmlSettings = value;
                OnPropertyChanged("htmlSettings");
            }
        }

        #endregion --- htmlSettings ------- Podesavanja za HTML dom

        #region Event Handlers: ExecutionTimeout - Execution time limit reached

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onExecutionTimeout_hasHandler
        {
            get { return (onExecutionTimeout != null); }
        }

        /// <summary>
        /// Event invoker za ExecutionTimeout - Execution time limit reached
        /// </summary>
        public void callExecutionTimeout()
        {
            webRequestEventArgs args = new webRequestEventArgs(webRequestEventType.errorTimeout,
                                                               "Execution time limit reached :: " +
                                                               timeCounter.signature());
            //args.saveStatus(this);
            if (onExecutionTimeout != null) onExecutionTimeout(this, args);
        }

        /// <summary>
        /// Event handler za ExecutionTimeout
        /// </summary>
        protected event webRequestEvent onExecutionTimeout;

        /// <summary>
        /// Postavlja event handler za ExecutionTimeout (onExecutionTimeout)
        /// </summary>
        public void onExecutionTimeout_addHandler(webRequestEvent _onExecutionTimeout)
        {
            if (!onExecutionTimeout_hasHandler) onExecutionTimeout += _onExecutionTimeout;
        }

        #endregion Event Handlers: ExecutionTimeout - Execution time limit reached

        #region Event Handlers: ExecutionError - Error occoured

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onExecutionError_hasHandler
        {
            get { return (onExecutionError != null); }
        }

        /// <summary>
        /// Event invoker za ExecutionError - Error occoured
        /// </summary>
        public void callExecutionError(webRequestEventType errorType = webRequestEventType.error,
                                       String errorMessage = "Error occoured")
        {
            if (!status.ToString().Contains("error")) status = errorType;
            webRequestEventArgs args = new webRequestEventArgs(status, errorMessage);
            // args.saveStatus(this);
            if (onExecutionError != null) onExecutionError(this, args);
        }

        /// <summary>
        /// Event handler za ExecutionError
        /// </summary>
        protected event webRequestEvent onExecutionError;

        /// <summary>
        /// Postavlja event handler za ExecutionError (onExecutionError)
        /// </summary>
        public void onExecutionError_addHandler(webRequestEvent _onExecutionError)
        {
            if (!onExecutionError_hasHandler) onExecutionError += _onExecutionError;
        }

        #endregion Event Handlers: ExecutionError - Error occoured

        #region Event Handlers: ExecutionDone - izvrsavanje je uspesno obavljeno

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onExecutionDone_hasHandler
        {
            get { return (onExecutionDone != null); }
        }

        /// <summary>
        /// Event invoker za ExecutionDone - izvrsavanje je uspesno obavljeno
        /// </summary>
        public void callExecutionDone(String message = "Execution completed")
        {
            status = webRequestEventType.executedOk;
            webRequestEventArgs args = new webRequestEventArgs(status, message);
            // args.saveStatus(this);
            if (onExecutionDone != null) onExecutionDone(this, args);
        }

        /// <summary>
        /// Event handler za ExecutionDone
        /// </summary>
        protected event webRequestEvent onExecutionDone;

        /// <summary>
        /// Postavlja event handler za ExecutionDone (onExecutionDone)
        /// </summary>
        public void onExecutionDone_addHandler(webRequestEvent _onExecutionDone)
        {
            if (!onExecutionDone_hasHandler) onExecutionDone += _onExecutionDone;
        }

        #endregion Event Handlers: ExecutionDone - izvrsavanje je uspesno obavljeno

        #region Event Handlers: RetryCalled - request execution retried

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onRetryCalled_hasHandler
        {
            get { return (onRetryCalled != null); }
        }

        /// <summary>
        /// Event invoker za RetryCalled - request execution retried
        /// </summary>
        public void callRetryCalled()
        {
            webRequestEventArgs args = new webRequestEventArgs(status,
                                                               "Request execution retried :: " +
                                                               retryCounter.signature());
            // args.saveStatus(this);
            if (onRetryCalled != null) onRetryCalled(this, args);
        }

        /// <summary>
        /// Event handler za RetryCalled
        /// </summary>
        protected event webRequestEvent onRetryCalled;

        /// <summary>
        /// Postavlja event handler za RetryCalled (onRetryCalled)
        /// </summary>
        public void onRetryCalled_addHandler(webRequestEvent _onRetryCalled)
        {
            if (!onRetryCalled_hasHandler) onRetryCalled += _onRetryCalled;
        }

        #endregion Event Handlers: RetryCalled - request execution retried
    }
}