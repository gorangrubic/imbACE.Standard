namespace imbACE.Network.web.core
{
    #region imbVeles using

    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.request;
    using imbACE.Network.web.result;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Osnovna klasa za objekat koji ucitava resurse preko veba
    /// </summary>
    public abstract class webLoaderBase : imbBindable
    {
        #region -----------  settings  -------  [Podesavanja za webLoader]

        private webLoaderSettings _settings = new webLoaderSettings();

        /// <summary>
        /// Podesavanja za webLoader
        /// </summary>
        [XmlIgnore]
        [Category("webLoaderBase")]
        [DisplayName("settings")]
        [Description("Podesavanja za webLoader")]
        public webLoaderSettings settings
        {
            get { return _settings; }
            set
            {
                // Boolean chg = (_settings != value);
                _settings = value;
                OnPropertyChanged("settings");
                // if (chg) {}
            }
        }

        #endregion -----------  settings  -------  [Podesavanja za webLoader]

        #region Event Handlers: RequestBlockFinished - Kada se izvrsi jedan request block

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onRequestBlockFinished_hasHandler
        {
            get { return (onRequestBlockFinished != null); }
        }

        /// <summary>
        /// Event invoker za RequestBlockFinished - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callRequestBlockFinished(webRequestBlock sender, webRequestBlockEventArgs args)
        {
            if (onRequestBlockAborted != null)
            {
                webLoaderEventArgs loaderArgs = new webLoaderEventArgs(webLoaderEventType.blockFinished, args.message);
                onRequestBlockAborted(this, loaderArgs);
            }
        }

        /// <summary>
        /// Event handler za RequestBlockFinished
        /// </summary>
        protected event webLoaderEvent onRequestBlockFinished;

        /// <summary>
        /// Postavlja event handler za RequestBlockFinished (onRequestBlockFinished)
        /// </summary>
        public void onRequestBlockFinished_addHandler(webLoaderEvent _onRequestBlockFinished)
        {
            if (!onRequestBlockFinished_hasHandler) onRequestBlockFinished += _onRequestBlockFinished;
        }

        #endregion Event Handlers: RequestBlockFinished - Kada se izvrsi jedan request block

        #region Event Handlers: RequestBlockAborted - Kada se obustavi izvrsavanje RequestBlock-a

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onRequestBlockAborted_hasHandler
        {
            get { return (onRequestBlockAborted != null); }
        }

        /// <summary>
        /// Event invoker za RequestBlockAborted - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callRequestBlockAborted(webRequestBlock sender, webRequestBlockEventArgs args)
        {
            if (onRequestBlockAborted != null)
            {
                webLoaderEventArgs loaderArgs = new webLoaderEventArgs(webLoaderEventType.blockAborted, args.message);
                onRequestBlockAborted(this, loaderArgs);
            }
        }

        /// <summary>
        /// Event handler za RequestBlockAborted
        /// </summary>
        protected event webLoaderEvent onRequestBlockAborted;

        /// <summary>
        /// Postavlja event handler za RequestBlockAborted (onRequestBlockAborted)
        /// </summary>
        public void onRequestBlockAborted_addHandler(webLoaderEvent _onRequestBlockAborted)
        {
            if (!onRequestBlockAborted_hasHandler) onRequestBlockAborted += _onRequestBlockAborted;
        }

        #endregion Event Handlers: RequestBlockAborted - Kada se obustavi izvrsavanje RequestBlock-a

        #region Event Handlers: ProxyChangeCall - Poziva se promena Proxyija

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onProxyChangeCall_hasHandler
        {
            get { return (onProxyChangeCall != null); }
        }

        /// <summary>
        /// Event invoker za ProxyChangeCall - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callProxyChangeCall(webLoaderBase sender = null, webLoaderEventArgs args = null)
        {
            if (sender == null) sender = this;
            if (args == null) args = new webLoaderEventArgs();
            if (onProxyChangeCall != null) onProxyChangeCall(sender, args);
        }

        /// <summary>
        /// Event handler za ProxyChangeCall
        /// </summary>
        protected event webLoaderEvent onProxyChangeCall;

        /// <summary>
        /// Postavlja event handler za ProxyChangeCall (onProxyChangeCall)
        /// </summary>
        public void onProxyChangeCall_addHandler(webLoaderEvent _onProxyChangeCall)
        {
            if (!onProxyChangeCall_hasHandler) onProxyChangeCall += _onProxyChangeCall;
        }

        #endregion Event Handlers: ProxyChangeCall - Poziva se promena Proxyija

        #region Event Handlers: ProxyChanged - Nakon sto je doslo do promene proxija

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onProxyChanged_hasHandler
        {
            get { return (onProxyChanged != null); }
        }

        /// <summary>
        /// Event invoker za ProxyChanged - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callProxyChanged(webLoaderBase sender = null, webLoaderEventArgs args = null)
        {
            if (sender == null) sender = this;
            if (args == null) args = new webLoaderEventArgs(webLoaderEventType.proxyChanged, "");
            if (onProxyChanged != null) onProxyChanged(sender, args);
        }

        /// <summary>
        /// Event handler za ProxyChanged
        /// </summary>
        protected event webLoaderEvent onProxyChanged;

        /// <summary>
        /// Postavlja event handler za ProxyChanged (onProxyChanged)
        /// </summary>
        public void onProxyChanged_addHandler(webLoaderEvent _onProxyChanged)
        {
            if (!onProxyChanged_hasHandler) onProxyChanged += _onProxyChanged;
        }

        #endregion Event Handlers: ProxyChanged - Nakon sto je doslo do promene proxija

        #region Event Handlers: ExecutionProgress - Progress

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onExecutionProgress_hasHandler
        {
            get { return (onExecutionProgress != null); }
        }

        public void callExecutionProgress(webRequestBlock sender, webRequestBlockEventArgs args)
        {
            if (onExecutionProgress != null)
            {
                webLoaderEventArgs loaderArgs = new webLoaderEventArgs(webLoaderEventType.progress, args.message);
                onExecutionProgress(this, loaderArgs);
            }
        }

        /// <summary>
        /// Event invoker za ExecutionProgress - Progress
        /// </summary>
        public void callExecutionProgress(String __message)
        {
            webLoaderEventArgs args = new webLoaderEventArgs(webLoaderEventType.progress, __message);
            if (onExecutionProgress != null) onExecutionProgress(this, args);
        }

        /// <summary>
        /// Event handler za ExecutionProgress
        /// </summary>
        protected event webLoaderEvent onExecutionProgress;

        /// <summary>
        /// Postavlja event handler za ExecutionProgress (onExecutionProgress)
        /// </summary>
        public void onExecutionProgress_addHandler(webLoaderEvent _onExecutionProgress)
        {
            if (!onExecutionProgress_hasHandler) onExecutionProgress += _onExecutionProgress;
        }

        #endregion Event Handlers: ExecutionProgress - Progress

        /// <summary>
        /// Pravi novi request block i postavlja ga za current
        /// </summary>
        /// <returns></returns>
        public webRequestBlock createNewRequestBlock(String __blockTitle = null)
        {
            if (String.IsNullOrEmpty(__blockTitle)) __blockTitle = "Block n" + requestBlocks.Count();
            webRequestBlock wRB = new webRequestBlock(__blockTitle, callRequestBlockFinished, callRequestBlockAborted);
            requestBlock = wRB;
            return wRB;
        }

        /// <summary>
        /// Pravi novi request i dodaje ga u trenutni request block
        /// </summary>
        /// <param name="__url">URL koji treba da se izvrsi</param>
        /// <param name="__action">Akcija koja treba da se izvrsi</param>
        /// <param name="__type">Tip webRequesta</param>
        /// <returns>Novo kreirani request</returns>
        public webRequest addNewRequest(String __url, webRequestActionType __action = webRequestActionType.openUrl,
                                        webRequestType __type = webRequestType.unknown)
        {
            webRequest output = webLoadingEngine.createNewRequest(__url, __action, __type);
            requestBlock.Add(output);
            return output;
        }

        /// <summary>
        /// Brise RequestBlock-ove koji su izvrseni
        /// </summary>
        public Int32 purgeExecutedBlocksAndRequests(Boolean purgeRequestsToo = true)
        {
            if (purgeRequestsToo)
            {
                foreach (webRequestBlock bl in requestBlocks)
                {
                    bl.purgeExecutedRequests();
                }
            }
            return requestBlocks.RemoveAll(x => !x.isWorking);
        }

        /// <summary>
        /// Vraca Request block koji sadrzi tmp request listu
        /// </summary>
        /// <returns></returns>
        protected webRequestBlock getCurrentBlock()
        {
            webRequestBlock blck = null;
            String currentName = "Current requests";
            if (!has_requestBlock)
            {
                blck = createNewRequestBlock(currentName);
            }
            else
            {
                blck = requestBlocks.First(x => (x.title == currentName));
                if (blck == null)
                {
                    blck = createNewRequestBlock(currentName);
                }
            }
            return blck;
        }

        /// <summary>
        /// Izvrsava jedan request
        /// </summary>
        /// <param name="request">Zahtev koji treba da se izvrsi</param>
        /// <param name="__syncMode">Rezim izvrsavanja</param>
        /// <param name="purge">Da li da obrise sve ranije izvrsene Blokove i Zahteve ?</param>
        /// <returns>Vraca objekat sa rezultatom</returns>
        public webResult executeRequest(webRequest request, executionSyncMode __syncMode, Boolean purge = true)
        {
            if (purge) purgeExecutedBlocksAndRequests(true);

            webRequestBlock blck = getCurrentBlock();
            blck.Add(request);

            webResultBlock wRB = executeCurrentBlock(__syncMode);

            return request.result;
        }

        /// <summary>
        /// Omogućava ad-hoc izvršavanje webRequest-a
        /// </summary>
        /// <param name="__url"></param>
        /// <param name="__syncMode"></param>
        /// <param name="__action"></param>
        /// <param name="__type"></param>
        /// <param name="purge">Da li da obrise sve ranije izvrsene Blokove i Zahteve ?</param>
        /// <returns>Vraća dobijeni rezultat</returns>
        public webResult executeRequest(String __url, executionSyncMode __syncMode,
                                        webRequestActionType __action = webRequestActionType.openUrl,
                                        webRequestType __type = webRequestType.unknown, Boolean purge = true)
        {
            if (purge) purgeExecutedBlocksAndRequests(true);
            webRequestBlock blck = getCurrentBlock();
            webRequest request = addNewRequest(__url, __action, __type);

            webResultBlock wRB = executeCurrentBlock(__syncMode);

            return request.result;
        }

        public webResultBlock executeCurrentBlock(executionSyncMode __syncMode)
        {
            return requestBlock.executeBlock(settings, __syncMode, callRequestBlockFinished, callRequestBlockAborted,
                                             callExecutionProgress);
        }

        public List<webResultBlock> executeAllBlocks(executionSyncMode __syncMode)
        {
            List<webResultBlock> output = new List<webResultBlock>();
            foreach (webRequestBlock wRB in requestBlocks)
            {
                output.Add(wRB.executeBlock(settings, __syncMode, callRequestBlockFinished, callRequestBlockAborted,
                                            callExecutionProgress));
            }
            return output;
        }

        #region -----------  requestBlock  -------  []

        private List<webRequestBlock> _requestBlocks = new List<webRequestBlock>();

        /// <summary>
        /// First requestBlock - trenutno odabrani request block
        /// </summary>
        [XmlIgnore]
        [Category("requestBlocks")]
        [DisplayName("requestBlock")]
        [Description("Prvi element requestBlock")]
        public webRequestBlock requestBlock
        {
            get
            {
                if (_requestBlocks.Count > 0)
                {
                    return _requestBlocks.First();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_requestBlocks.Count == 0)
                {
                    _requestBlocks.Add(value);
                }
                else
                {
                    if (_requestBlocks.Contains(value))
                    {
                        _requestBlocks.Remove(value);
                    }
                    _requestBlocks.Insert(0, value);
                }
            }
        }

        /// <summary>
        /// Da li ima requestBlock?
        /// </summary>
        public Boolean has_requestBlock
        {
            get { return (_requestBlocks.Count > 0); }
        }

        /// <summary>
        /// Svi elementi: requestBlocks
        /// </summary>
        [Category("requestBlocks")]
        [DisplayName("requestBlock")]
        [Description("Svi elementi: requestBlocks")]
        public List<webRequestBlock> requestBlocks
        {
            get { return _requestBlocks; }
            set
            {
                _requestBlocks = value;
                OnPropertyChanged("requestBlocks");
            }
        }

        #endregion -----------  requestBlock  -------  []
    }
}