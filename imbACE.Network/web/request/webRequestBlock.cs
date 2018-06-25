namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.core;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using imbSCI.Core.extensions.text;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Blok zahteva prema webLoaderu - izvrsavaju se paralelno
    /// </summary>
    public class webRequestBlock : ObservableCollection<webRequest>
    {
        //public webLoaderBase currentWebLoader = null;

        private webLoaderSettings __settings = null;
        private string _lastLogMessage;
        private logType _lastLogType;

        public webRequestBlock(String __title = null, webRequestBlockEvent __onFinished = null,
                               webRequestBlockEvent __onAborted = null)
        {
            if (String.IsNullOrEmpty(__title)) __title = "Block " + imbStringGenerators.getRandomString(3);

            title = __title;
        }

        #region --- title ------- naziv request bloka

        private String _title = "Untitled block";

        /// <summary>
        /// naziv request bloka
        /// </summary>
        public String title
        {
            get { return _title; }
            set { _title = value; }
        }

        #endregion --- title ------- naziv request bloka

        /// <summary>
        /// Da li jos uvek ima webRequest-a koji se cekaju
        /// </summary>
        public Boolean isWorking
        {
            get { return this.Any(x => x.isActive); }
        }

        #region Event Handlers: BlockFinishedEvent - Svi zahtevi u bloku su izvrseni

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onBlockFinishedEvent_hasHandler
        {
            get { return (onBlockFinishedEvent != null); }
        }

        /// <summary>
        /// Event invoker za BlockFinishedEvent - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callBlockFinishedEvent()
        {
            webRequestBlockEventType status = webRequestBlockEventType.executedAllOk;
            Int32 withErrors = 0;
            foreach (webRequest rq in this)
            {
                if (rq.isErrorStatus)
                {
                    withErrors++;
                }
            }

            if (withErrors == 0)
            {
                status = webRequestBlockEventType.executedAllOk;
            }
            else if (withErrors == Count)
            {
                status = webRequestBlockEventType.executedAllError;
            }
            else
            {
                status = webRequestBlockEventType.executedWithErrors;
            }

            String __message = " Execution success rate: " + imbStringFormats.imbGetPercentage(withErrors, Count, 1);

            webRequestBlockEventArgs args = new webRequestBlockEventArgs(status, __message);

            if (onBlockFinishedEvent != null) onBlockFinishedEvent(this, args);
        }

        /// <summary>
        /// Event handler za BlockFinishedEvent
        /// </summary>
        protected event webRequestBlockEvent onBlockFinishedEvent;

        /// <summary>
        /// Postavlja event handler za BlockFinishedEvent (onBlockFinishedEvent)
        /// </summary>
        public void onBlockFinishedEvent_addHandler(webRequestBlockEvent _onBlockFinishedEvent)
        {
            if (!onBlockFinishedEvent_hasHandler) onBlockFinishedEvent += _onBlockFinishedEvent;
        }

        #endregion Event Handlers: BlockFinishedEvent - Svi zahtevi u bloku su izvrseni

        #region Event Handlers: BlockAbortedEvent - Obustavljeno izvrsavanje bloka

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onBlockAbortedEvent_hasHandler
        {
            get { return (onBlockAbortedEvent != null); }
        }

        /// <summary>
        /// Event invoker za BlockAbortedEvent - ako je ovaj objekat uzrok dogadjaja onda moze i bez argumenata da se pozove
        /// </summary>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje - ako je null smatrace da je ovaj objekat uzrok dogadjaja</param>
        /// <param name="args">Argumenti dogadjaja - ako je null postavlja da je unknown</param>
        public void callBlockAbortedEvent(String __message = "User aborted")
        {
            webRequestBlockEventArgs args = new webRequestBlockEventArgs(webRequestBlockEventType.aborted,
                                                                         "Aborted :: " + title + " [" + output.Count +
                                                                         " / " + Count + "] :: " + __message);

            if (onBlockAbortedEvent != null) onBlockAbortedEvent(this, args);
        }

        /// <summary>
        /// Event handler za BlockAbortedEvent
        /// </summary>
        protected event webRequestBlockEvent onBlockAbortedEvent;

        /// <summary>
        /// Postavlja event handler za BlockAbortedEvent (onBlockAbortedEvent)
        /// </summary>
        public void onBlockAbortedEvent_addHandler(webRequestBlockEvent _onBlockAbortedEvent)
        {
            if (!onBlockAbortedEvent_hasHandler) onBlockAbortedEvent += _onBlockAbortedEvent;
        }

        #endregion Event Handlers: BlockAbortedEvent - Obustavljeno izvrsavanje bloka

        #region Event Handlers: ExecutionProgress - Progress

        public String progressMessage
        {
            get
            {
                return "wRB:[" + title + "]:[" + imbStringFormats.imbGetPercentage(output.Count, Count, 1) +
                       "] :: execute mode [" + executeMode.ToString() + "] :: sync mode [" +
                       executeSyncMode.ToString() + "] ";
            }
        }

        /// <summary>
        /// Proverava da li ima handler vec
        /// </summary>
        public Boolean onExecutionProgress_hasHandler
        {
            get { return (onExecutionProgress != null); }
        }

        /// <summary>
        /// Event invoker za ExecutionProgress - Progress
        /// </summary>
        public void callExecutionProgress()
        {
            if (onExecutionProgress != null)
            {
                webRequestBlockEventArgs args = new webRequestBlockEventArgs(webRequestBlockEventType.progressReport,
                                                                             progressMessage);

                onExecutionProgress(this, args);
            }
        }

        /// <summary>
        /// Event handler za ExecutionProgress
        /// </summary>
        protected event webRequestBlockEvent onExecutionProgress;

        /// <summary>
        /// Postavlja event handler za ExecutionProgress (onExecutionProgress)
        /// </summary>
        public void onExecutionProgress_addHandler(webRequestBlockEvent _onExecutionProgress)
        {
            if (!onExecutionProgress_hasHandler) onExecutionProgress += _onExecutionProgress;
        }

        #endregion Event Handlers: ExecutionProgress - Progress

        #region --- executeMode ------- Nacin na koji se izvrsava Block

        private webRequestBlockExecuteMode _executeMode = webRequestBlockExecuteMode.parallelSingleThread;

        /// <summary>
        /// Nacin na koji se izvrsava Block
        /// </summary>
        public webRequestBlockExecuteMode executeMode
        {
            get { return _executeMode; }
            set { _executeMode = value; }
        }

        #endregion --- executeMode ------- Nacin na koji se izvrsava Block

        #region --- executeSyncMode ------- Mod izvrsavanja

        private executionSyncMode _executeSyncMode = executionSyncMode.asynced;

        /// <summary>
        /// Mod izvrsavanja
        /// </summary>
        public executionSyncMode executeSyncMode
        {
            get { return _executeSyncMode; }
            set { _executeSyncMode = value; }
        }

        #endregion --- executeSyncMode ------- Mod izvrsavanja

        #region --- tickDelay ------- vreme u milisekundama izmedju dve provere statusa

        private Int32 _tickDelay = 500;

        /// <summary>
        /// vreme u milisekundama izmedju dve provere statusa
        /// </summary>
        public Int32 tickDelay
        {
            get { return _tickDelay; }
            set { _tickDelay = value; }
        }

        #endregion --- tickDelay ------- vreme u milisekundama izmedju dve provere statusa

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
        /// izbacuje iz sebe sve koji su gotovi
        /// </summary>
        /// <returns></returns>
        public Int32 purgeExecutedRequests()
        {
            var l = this.Where(x => x.isExecutedOrFailed).ToList();
            foreach (var i in l) Remove(i);
            return l.Count;
        }

        /// <summary>
        /// Prekida izvrsavanje
        /// </summary>
        /// <returns></returns>
        public webResultBlock abortExecution(webLoaderSettings settings)
        {
            executeThread.Abort();

            foreach (webRequest wR in this)
            {
                if (wR.isActive)
                {
                    wR.abortRequest(settings);
                }
            }

            callBlockAbortedEvent();

            return output;
        }

        /// <summary>
        /// Pokreze izvrsavanje bloka
        /// </summary>
        /// <param name="settings">Podesavanja webLoader-a</param>
        /// <param name="__syncMode">mod sinhronizacije</param>
        /// <returns>Referencu prema webResultBlock-u</returns>
        public webResultBlock executeBlock(webLoaderSettings settings, executionSyncMode __syncMode,
                                           webRequestBlockEvent __onFinished = null,
                                           webRequestBlockEvent __onAborted = null,
                                           webRequestBlockEvent __onProgress = null)
        {
            __settings = settings;

            deploySettings(settings, __onFinished, __onAborted, __onProgress);

            if (__syncMode != executionSyncMode.unknown) executeSyncMode = __syncMode;
            output.Clear();

            if (executeSyncMode == executionSyncMode.synced)
            {
                startExecution(settings);
            }
            else
            {
                executeThread = new Thread(startExecution);
                executeThread.Start(settings);
            }

            return output;
        }

        public void deploySettings(webLoaderSettings settings, webRequestBlockEvent __onFinished = null,
                                   webRequestBlockEvent __onAborted = null, webRequestBlockEvent __onProgress = null)
        {
            executeMode = settings.blockExecuteMode;

            onBlockFinishedEvent_addHandler(__onFinished);
            onBlockAbortedEvent_addHandler(__onAborted);
            onExecutionProgress_addHandler(__onProgress);
        }

        protected void startExecution(Object obj)
        {
            webLoaderSettings settings = obj as webLoaderSettings;
            switch (executeMode)
            {
                case webRequestBlockExecuteMode.parallelSingleThread:
                    foreach (webRequest wR in this)
                    {
                        wR.executeRequest(settings, executionSyncMode.asynced, onItemUpdate, onItemUpdate, onItemUpdate);
                    }
                    break;

                case webRequestBlockExecuteMode.parallelThreads:
                    foreach (webRequest wR in this)
                    {
                        Thread newThread = new Thread(startWebRequest);
                        List<Object> input = new List<object>();
                        input.Add(settings);
                        input.Add(wR);
                        newThread.Start(input);
                    }
                    break;

                case webRequestBlockExecuteMode.oneByOne:
                    foreach (webRequest wR in this)
                    {
                        wR.executeRequest(settings, executionSyncMode.synced, onItemUpdate, onItemUpdate, onItemUpdate);
                    }
                    break;
            }

            if (executeSyncMode == executionSyncMode.synced)
            {
                monitoringLoop(settings);
            }
        }

        protected void startWebRequest(Object obj)
        {
            List<Object> input = obj as List<Object>;
            webLoaderSettings settings = input[0] as webLoaderSettings;
            webRequest wR = input[1] as webRequest;
            wR.executeRequest(settings, executionSyncMode.asynced, onItemUpdate, onItemUpdate, onItemUpdate);
        }

        protected void onItemUpdate(webRequest sender, webRequestEventArgs args)
        {
            switch (args.type)
            {
                case webRequestEventType.executedOk:
                    if (!output.Contains(sender.result)) output.Add(sender.result);
                    break;
            }

            if (executeMode != webRequestBlockExecuteMode.oneByOne)
            {
                foreach (webRequest wR in this)
                {
                    wR.checkRequest(__settings);
                }
            }

            callExecutionProgress();

            if (executeSyncMode == executionSyncMode.asynced)
            {
                if (!isWorking)
                {
                    callBlockFinishedEvent();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="__syncMode"></param>
        protected void monitoringLoop(webLoaderSettings settings)
        {
            while (isWorking)
            {
                Thread.Sleep(tickDelay);
            }

            callBlockFinishedEvent();
        }

        #region --- output ------- Blok rezultata - ako je async onda je samo prazan objekat na pocetku izvrsavanja

        private webResultBlock _output = new webResultBlock();

        /// <summary>
        /// Blok rezultata - ako je async onda je samo prazan objekat na pocetku izvrsavanja
        /// </summary>
        [XmlIgnore]
        public webResultBlock output
        {
            get { return _output; }
            set { _output = value; }
        }

        #endregion --- output ------- Blok rezultata - ako je async onda je samo prazan objekat na pocetku izvrsavanja

        #region --- executeThread ------- glavni thread koji je pokrenuo izvrsavanje

        private Thread _executeThread;

        /// <summary>
        /// glavni thread koji je pokrenuo izvrsavanje
        /// </summary>
        public Thread executeThread
        {
            get { return _executeThread; }
            set { _executeThread = value; }
        }

        #endregion --- executeThread ------- glavni thread koji je pokrenuo izvrsavanje
    }
}