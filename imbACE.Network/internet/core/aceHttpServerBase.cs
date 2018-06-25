// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceHttpServerBase.cs" company="imbVeles" >
//
// Copyright (C) 2017 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbACE.Network
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Network.internet.core
{
    using imbACE.Core;
    using imbACE.Core.core;
    using imbACE.Network.core;
    using imbACE.Network.internet.config;
    using imbACE.Network.internet.sessions;
    using imbSCI.Core;
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    /// <summary>
    /// Override <see cref="Process"/> to applicate this class
    /// </summary>
    /// <seealso cref="imbACE.Services.core.aceComponentBase{imbACE.Network.internet.config.serverParameters}" />
    /// <seealso cref="imbACE.Network.internet.IAceHttpServer" />
    public abstract class aceHttpServerBase : aceComponentBase<serverParameters>, IAceHttpServer
    {
        /// <summary>
        ///
        /// </summary>
        public enum aceServerDefaultSubDirs
        {
            config,
            resources,
            uploads,
            temp,
            log
        }

        public aceHttpServerBase(string name)
        {
            instanceName = name;
        }

        /// <summary>
        /// Initializes the server
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="port">The port.</param>
        /// <param name="initFlags">The initialize flags.</param>
        /// <param name="__settings">The settings.</param>
        protected void init(int __port = -1, aceServerInitFlags initFlags = aceServerInitFlags.none, serverParameters __settings = null)
        {
            if (initFlags == aceServerInitFlags.none) initFlags = configParameters.main.initFlags;

            flags = initFlags;
            settings = __settings;

            if (__settings == null)
            {
                settings = configParameters.main.settings;
            }
            else
            {
                settings = __settings;
            }

            directory = new DirectoryInfo(settings.filepathToServe);

            if (__port == -1) port = settings.port;

            sessionControl = new serverSessionControl(this);
            accessControl = new aceServerUserALC(this);

            //aceLog.consoleControl.setAsOutput(terminal, "srv:" + instanceName);
            underSuspension = true;

            //folderStructure = new aceFolderStructure(Directory.GetCurrentDirectory(), "aceServer :: " + instanceName);
            //folderStructure.Add(aceServerDefaultSubDirs.config, "Configuration", "Server configuration files i.e.: user list, access control settings ... ");
            //folderStructure.Add(aceServerDefaultSubDirs.log, "Log", "Execution logs");
            //folderStructure.Add(aceServerDefaultSubDirs.resources, "Resources", "Files used by server application.");
            //folderStructure.Add(aceServerDefaultSubDirs.temp, "Temporary", "These files were used temporarly, you are free to delete them on will.");
            //folderStructure.Add(aceServerDefaultSubDirs.uploads, "Uploads", "Application-specific repositorium of files that were created or received by server");
        }

        /// <summary>
        /// Before the instance going to be started
        /// </summary>
        public abstract void beforeStarted();

        private DirectoryInfo _directory;

        /// <summary>Directory path to monitor</summary>
        public DirectoryInfo directory
        {
            get
            {
                return _directory;
            }
            protected set
            {
                _directory = value;
                OnPropertyChanged("directory");
            }
        }

        /// <summary>Session component of the server [by default is not used] -- use it within <see cref="Process(HttpListenerContext)"/> override to check and retrive session data</summary>
        public serverSessionControl sessionControl { get; protected set; }

        /// <summary>ALC component of the server [by default is not used] -- use it within <see cref="Process(HttpListenerContext)"/> override to check access rights</summary>
        public aceServerUserALC accessControl { get; protected set; }

        /// <summary>
        /// Starts this server,
        /// </summary>
        public void Start()
        {
            underSuspension = false;
            doUseTimedSuspension = false;
            terminal.log("Start() invoked... ");

            if (serverThread != null)
            {
                terminal.log("> Existing thread is aborted");
                serverThread.Abort();
            }

            serverThread = new Thread(Listen);
            serverThread.Start();
            terminal.log("Server [" + instanceName + "] started (" + directory.FullName + ":" + port + ")");
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            serverThread.Abort();
            listener.Stop();
            sessionControl.Clear();
            terminal.log("Server [" + instanceName + "] went down (" + directory.FullName + ":" + port + ")");
        }

        /// <summary>
        /// Continues this instance -- if it was suspended
        /// </summary>
        public void Continue()
        {
            underSuspension = false;
            doUseTimedSuspension = false;
        }

        private bool _doUseTimedSuspension;

        /// <summary> </summary>
        protected bool doUseTimedSuspension
        {
            get
            {
                return _doUseTimedSuspension;
            }
            set
            {
                _doUseTimedSuspension = value;
                OnPropertyChanged("doUseTimedSuspension");
            }
        }

        /// <summary>
        /// Processes the context http listener detected
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Process(HttpListenerContext context);

        #region ----------- Boolean [ doListenLoop ] -------  [if true it will keep do listen loop]

        protected bool doListenLoop { get; set; } = false;

        #endregion ----------- Boolean [ doListenLoop ] -------  [if true it will keep do listen loop]

        private bool _underSuspension;

        /// <summary> </summary>
        public bool underSuspension
        {
            get
            {
                return _underSuspension;
            }
            protected set
            {
                _underSuspension = value;
                OnPropertyChanged("underSuspension");
            }
        }

        private DateTime _timeForSuspend;

        /// <summary>Scheduled time for server suspend</summary>
        public DateTime timeForSuspend
        {
            get
            {
                return _timeForSuspend;
            }
            protected set
            {
                _timeForSuspend = value;
                OnPropertyChanged("timeForSuspend");
            }
        }

        private DateTime _timeForRestart;

        /// <summary>Scheduled time for server restart</summary>
        public DateTime timeForRestart
        {
            get
            {
                return _timeForRestart;
            }
            protected set
            {
                _timeForRestart = value;
                OnPropertyChanged("timeForRestart");
            }
        }

        /// <summary>
        /// Suspends the specified in seconds.
        /// </summary>
        /// <param name="inSeconds">The in seconds - from this moment</param>
        /// <param name="restartInSeconds">The restart in seconds - from moment of suspension: in <c>inSeconds</c> + this parametere</param>
        public void Suspend(int inSeconds = -1, int restartInSeconds = -1)
        {
            if (inSeconds > 0)
            {
                timeForSuspend = DateTime.Now.AddSeconds(inSeconds);
                doUseTimedSuspension = true;
            }

            underSuspension = true;

            if (restartInSeconds > 0)
            {
                timeForRestart = DateTime.Now.AddSeconds(inSeconds + restartInSeconds);
                doUseTimedSuspension = true;
            }
        }

        /// <summary>
        /// Listens this instance.
        /// </summary>
        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listener.Start();

            while (doListenLoop)
            {
                if (doUseTimedSuspension)
                {
                    if (timeForSuspend > DateTime.Now)
                    {
                        underSuspension = true;
                    }
                    else
                    {
                        if (timeForRestart < timeForSuspend) doUseTimedSuspension = false;
                        underSuspension = false;
                    }

                    if ((timeForRestart > timeForSuspend) && (timeForRestart > DateTime.Now))
                    {
                        underSuspension = true;
                        doUseTimedSuspension = false;
                    }
                }

                if (!underSuspension)
                {
                    try
                    {
                        HttpListenerContext context = _listener.GetContext();
                        Process(context);
                        Thread.Sleep(10);
                    }
                    catch (Exception ex)
                    {
                        throw new aceServerException("Listening process has been corrupted", ex, this);
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        private string _instanceName;

        /// <summary>Signature of the server</summary>
        public string instanceName
        {
            get
            {
                if (_instanceName.isNullOrEmpty()) _instanceName = GetType().Name;
                return _instanceName;
            }
            protected set
            {
                _instanceName = value;
            }
        }

        /// <summary> </summary>
        protected Thread serverThread { get; set; }

        protected int _port;

        /// <summary>
        /// Port to listen to
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>Initialization flags</summary>
        public aceServerInitFlags flags { get; protected set; } = new aceServerInitFlags();

        /// <summary>Configuration of the server</summary>
        public serverParameters settings { get; protected set; }

        private HttpListener _listener;

        /// <summary>Listener of TCP/IP</summary>
        protected HttpListener listener
        {
            get
            {
                return _listener;
            }
            set
            {
                _listener = value;
                OnPropertyChanged("listener");
            }
        }
    }
}