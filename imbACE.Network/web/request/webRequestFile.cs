namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.core;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using System;
    using System.IO;
    using System.Net;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Za rad sa fajllovima, downloadom, FTP-om
    /// </summary>
    public class webRequestFile : webRequest
    {
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

        #region --- fileStream ------- tmpFileStream

        private FileStream _fileStream;

        /// <summary>
        /// tmpFileStream
        /// </summary>
        public FileStream fileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;
                OnPropertyChanged("fileStream");
            }
        }

        #endregion --- fileStream ------- tmpFileStream

        #region --- localPath ------- Putanja prema lokalnom fajlu

        private String _localPath;

        /// <summary>
        /// Putanja prema lokalnom fajlu
        /// </summary>
        public String localPath
        {
            get { return _localPath; }
            set
            {
                _localPath = value;
                OnPropertyChanged("localPath");
            }
        }

        #endregion --- localPath ------- Putanja prema lokalnom fajlu

        //public WebRequest httpRequest;

        public webRequestFile(String __url = "", webRequestActionType __action = webRequestActionType.openUrl)
        {
            url = __url;
            action = __action;
        }

        [XmlIgnore]
        public new webResultFile result
        {
            get
            {
                if (_result == null) _result = new webResultLookup(this);
                return _result as webResultFile;
            }
            set { _result = value; }
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
            webClient = new WebClient();

            switch (action)
            {
                case webRequestActionType.Download:
                    webClient.DownloadFileAsync(urlObject, localPath, this);
                    break;

                case webRequestActionType.FTPUpload:
                case webRequestActionType.FTPDownload:
                    logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
                    break;

                case webRequestActionType.localFile:

                    if (File.Exists(url))
                    {
                        fileStream = File.OpenRead(url);
                        fileStream.BeginRead(result.loadedBytes, 0, result.loadedBytes.Length, getResponse, this);
                    }

                    break;

                default:
                    logSystem.log("Action :: " + action + " not supported by " + GetType().Name, logType.ExecutionError);
                    callExecutionError(webRequestEventType.error);
                    break;
            }
        }

        protected override void executeEnd(webLoaderSettings settings)
        {
            status = webRequestEventType.executedOk;
        }

        public override void cancelAllActivities()
        {
            logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }

        public override bool hasPreference(webRequestActionType __action)
        {
            return (webRequestBase.getPreference(__action) == webRequestType.webRequestFile);
        }

        protected override void getResponse(IAsyncResult __result)
        {
            switch (action)
            {
                case webRequestActionType.Download:
                    if (__result.IsCompleted)
                    {
                        status = webRequestEventType.loaded;
                    }
                    break;

                case webRequestActionType.FTPUpload:
                case webRequestActionType.FTPDownload:
                    logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
                    break;

                case webRequestActionType.localFile:
                    Int32 byteCount = fileStream.EndRead(__result);
                    if (byteCount == 0)
                    {
                        callExecutionError(webRequestEventType.error, "Zero bytes loaded");
                    }
                    else
                    {
                        status = webRequestEventType.loaded;
                    }

                    break;

                default:
                    logSystem.log("Action :: " + action + " not supported by " + GetType().Name, logType.ExecutionError);
                    callExecutionError(webRequestEventType.error);
                    break;
            }

            //logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }
    }
}