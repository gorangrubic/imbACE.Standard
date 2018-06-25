namespace imbACE.Network.web.request
{
    #region imbVeles using

    using imbACE.Core.core;
    using imbACE.Network.extensions;
    using imbACE.Network.tools;
    using imbACE.Network.web.core;
    using imbACE.Network.web.enums;
    using imbACE.Network.web.events;
    using imbACE.Network.web.result;
    using imbSCI.Core.files;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Request za izvrsavanje IP Lookup-a, WhoIs, provera URL-a itd
    /// </summary>
    public class webRequestLookup : webRequest
    {
        public webRequestLookup()
        {
        }

        public webRequestLookup(String __url = "", webRequestActionType __action = webRequestActionType.ipResolve,
                                String __customWhoIsServer = "")
        {
            url = __url;
            action = __action;
            customServer = __customWhoIsServer;
        }

        [XmlIgnore]
        public new webResultLookup result
        {
            get
            {
                if (_result == null) _result = new webResultLookup(this);
                return _result as webResultLookup;
            }
            set { _result = value; }
        }

        #region --- tcpClient ------- TCP client

        private TcpClient _tcpClient;

        /// <summary>
        /// TCP client
        /// </summary>
        [XmlIgnore]
        public TcpClient tcpClient
        {
            get { return _tcpClient; }
            set
            {
                _tcpClient = value;
                OnPropertyChanged("tcpClient");
            }
        }

        #endregion --- tcpClient ------- TCP client

        #region --- customServer ------- adresa proizvoljnog WhoIs servera koji treba da koristi

        private String _customServer;

        /// <summary>
        /// adresa proizvoljnog WhoIs servera koji treba da koristi
        /// </summary>
        public String customServer
        {
            get { return _customServer; }
            set
            {
                _customServer = value;
                OnPropertyChanged("customServer");
            }
        }

        #endregion --- customServer ------- adresa proizvoljnog WhoIs servera koji treba da koristi

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
                case webRequestActionType.CheckUrlOnly:
                case webRequestActionType.ipResolve:
                    Dns.BeginGetHostEntry(url, getResponse, this);
                    break;

                case webRequestActionType.whoIs:

                    if (urlObject != null)
                    {
                        String tLD = urlObject.guessTopLevelDomain();
                        imbTopLevelDomain tmpTLD = null;
                        if (imbDomainManager.DomainDictionary.ContainsKey(tLD))
                        {
                            String serverUrl;

                            if (String.IsNullOrEmpty(customServer))
                            {
                                tmpTLD = imbDomainManager.DomainDictionary[tLD];
                                imbWhoIsServer usedServer = tmpTLD.shuffledServer;
                                usedServer.callCount++;
                                serverUrl = usedServer.url;
                            }
                            else
                            {
                                serverUrl = customServer;
                            }

                            tcpClient = new TcpClient();

                            tcpClient.BeginConnect(serverUrl, 43, getResponse, this);
                        }
                        else
                        {
                            callExecutionError(webRequestEventType.error,
                                               "TLD not supported :: TLD [" + tmpTLD.domainName + "]");
                        }
                    }
                    else
                    {
                        callExecutionError(webRequestEventType.error, "URL (uri) object is null :: URL [" + url + "]");
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
            callExecutionDone();
        }

        public override void cancelAllActivities()
        {
        }

        public override bool hasPreference(webRequestActionType __action)
        {
            return (webRequestBase.getPreference(__action) == webRequestType.webRequestLookup);
        }

        protected override void getResponse(IAsyncResult __result)
        {
            IPHostEntry ipEntry = null;

            switch (action)
            {
                case webRequestActionType.CheckUrlOnly:
                    ipEntry = Dns.EndGetHostEntry(__result);
                    if (ipEntry != null) result.document.deploySource("Exists", action);
                    break;

                case webRequestActionType.ipResolve:
                    ipEntry = Dns.EndGetHostEntry(__result);
                    //dnsLookupDocument dLD = new dnsLookupDocument(ipEntry, urlObject.DnsSafeHost);
                    // result.dataObject = dLD;
                    throw new NotImplementedException();
                    String xmlVersion = ""; // objectSerialization.ObjectToXML(dLD);  //imbSerialization.serializeValue(dLD, imbSerializationMode.XML).ToString();
                    result.document.deploySource(xmlVersion, action);
                    break;

                case webRequestActionType.whoIs:
                    tcpClient.EndConnect(__result);
                    StringBuilder sb = new StringBuilder();

                    BufferedStream bufferedStreamWhois = new BufferedStream(tcpClient.GetStream());
                    StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

                    streamWriter.WriteLine(url);
                    streamWriter.Flush();

                    StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois);

                    while (!streamReaderReceive.EndOfStream)
                    {
                        sb.AppendLine(streamReaderReceive.ReadLine());
                    }

                    result.document.deploySource(sb.ToString(), action);

                    break;
            }
        }
    }
}