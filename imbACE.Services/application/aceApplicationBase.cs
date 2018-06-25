// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceApplicationBase.cs" company="imbVeles" >
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
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbACE.Core;
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

namespace imbACE.Services.application
{
    using imbACE.Core.application;
    using imbACE.Core.ataman;
    using imbACE.Core.cache;
    using imbACE.Core.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.events;
    using imbACE.Core.operations;
    using imbACE.Core.plugins;
    using imbACE.Services.terminal;
    using imbSCI.Core.data;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Common Base class for imbACE Applications
    /// </summary>
    /// <seealso cref="imbACE.Core.application.IAceApplicationBase" />
    public abstract class aceApplicationBase : IAceApplicationBase, IAceLogable
    {
        //public const String FOLDERNAME_CACHE = "cache";
        //public const String FOLDERNAME_REPORTS = "reports";
        //public const String FOLDERNAME_RESOURCES = "resources";
        //public const String FOLDERNAME_CONFIG = "config";
        //public const String FOLDERNAME_PLUGINS = "plugin";
        //public const String FOLDERNAME_PROJECTS = "projects";
        //public const String FOLDERNAME_LOGS = "logs";

        /// <summary>
        /// Gets the application name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name
        {
            get
            {
                return appAboutInfo.software;
            }
        }

        protected aceApplicationBase()
        {
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        public abstract void doQuit();

        /// <summary>
        /// Starts the application, keeping thread in the loop. Once it returns <c>false</c>, it means user chosen to quit the application
        /// </summary>
        /// <param name="arguments">The command line arguments.</param>
        /// <returns></returns>
        public Boolean StartApplication(String[] arguments = null)
        {
            startUpProcess(arguments, null);
            StartUp();

            appManager.Application = this;

            externalLoop();

            return false;
        }

        private void externalLoop()
        {
            while (doApplicationLoop())
            {
                Thread.Sleep(settings.doLoopExternalDelay);
            }

            if (settings.doAskOnApplicationExit)
            {
                //aceTerminalInput.askAnyKeyInTime("Please")
                if (!aceTerminalInput.askYesNo("Are you sure you want to close the application [" + name + "]?"))
                {
                    externalLoop();
                }
                else
                {
                    log("Application is closing");
                }
            }
        }

        /// <summary>
        /// Application loop
        /// </summary>
        /// <returns></returns>
        protected abstract Boolean doApplicationLoop();

        /// <summary>
        /// If true it will keep running
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do keep running]; otherwise, <c>false</c>.
        /// </value>
        public Boolean doKeepRunning { get; set; } = true;

        /// <summary>
        /// Logs the specified message: if logger set with logger, if not directly to console
        /// </summary>
        /// <param name="message">The message.</param>
        public void log(String message)
        {
            if (logger != null)
            {
                logger.log(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Called on the application start up, once settings are loaded and everything is ready to run
        /// </summary>
        protected abstract void StartUp();

        /// <summary>
        /// Boot process, before the <see cref="StartUp"/> method is called
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="_logger">The logger.</param>
        protected void startUpProcess(String[] args, ILogBuilder _logger)
        {
            logger = _logger;
            if (args == null) args = new string[] { };
            commandLineArguments.AddRange(args);

            AppDomain.CurrentDomain.UnhandledException += ReactTo_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            String appDirectory = AppDomain.CurrentDomain.BaseDirectory; //Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase);
            Environment.CurrentDirectory = appDirectory;

            state = new aceApplicationState();

            setAboutInformation();

            folder = new DirectoryInfo(appDirectory);

            // creating folders subsystem
            folder_cache = folder.Add(aceApplicationInfo.FOLDERNAME_CACHE, "Cache", "Repository of [" + name + "] application general cache");
            folder_reports = folder.Add(aceApplicationInfo.FOLDERNAME_REPORTS, "Reporting output", "Repository with reports generated by [" + name + "] application");
            folder_resources = folder.Add(aceApplicationInfo.FOLDERNAME_RESOURCES, "Resources", "Repository with resources used by [" + name + "] application");
            folder_config = folder.Add(aceApplicationInfo.FOLDERNAME_CONFIG, "Configuration", "Application [" + name + "] configuration files");
            folder_plugins = folder.Add(aceApplicationInfo.FOLDERNAME_PLUGINS, "Plug-ins", "Directory with 3rd party plug-ins for application [" + name + "]. Use subdirectories to organize plug-ins into groups.");
            folder_projects = folder.Add(aceApplicationInfo.FOLDERNAME_PROJECTS, "Projects", "Directory with 3rd party plug-ins for application [" + name + "]. Use subdirectories to organize plug-ins into groups.");
            folder_logs = folder.Add(aceApplicationInfo.FOLDERNAME_LOGS, "Logs", "Diagnostic data for [" + name + "] application.");

            if (logger == null) logger = aceLog.loger;

            aceCommons.crashReportPath = folder_logs.path;

            imbSCI.Core.screenOutputControl.logToConsoleControl = aceLog.consoleControl;

            // loading settings -----------------------------------------------------------------------\

            settings = new aceApplicationSettings();
            settings.Load(folder_config);

            deploySettings();

            // ----------------------------------------------------------------------------------------/

            // setting up the culture
            Console.OutputEncoding = Encoding.UTF8;
            state.culture = CultureInfo.CreateSpecificCulture(settings.Culture);
            Thread.CurrentThread.CurrentCulture = state.culture;
            Thread.CurrentThread.CurrentUICulture = state.culture;

            // setup the cache system
            if (settings.CachePathOverride.isNullOrEmpty())
            {
                cacheSystem.defaultCacheDirectoryPath = folder_cache.path;
            }
            else
            {
                cacheSystem.defaultCacheDirectoryPath = settings.CachePathOverride;
            }

            // loading plug-ins
            plugins = new pluginManager(folder_plugins);
            if (settings.LoadPluginsOnStartUp) plugins.loadPlugins(aceLog.loger, folder_plugins);

            // <------------ calling "loaded" event
            callEventApplicationLoaded();
        }

        public List<String> commandLineArguments { get; set; } = new List<string>();

        /// <summary>
        /// Deploys or redeploys the settings - forcing all parts of the system to update their state according to the settings
        /// </summary>
        public void deploySettings()
        {
            imbACECoreConfig.settings = settings.aceCoreConfig;
            imbSCI.Core.config.imbSCICoreConfig.settings = settings.sciCoreConfig;
            imbSCI.Reporting.config.imbSCIReportingConfig.settings = settings.sciReportingConfing;

            if (settings.aceCoreConfig.doDisplayMainLog)
            {
                aceLog.consoleControl.setAsOutput(aceLog.loger.GetMainLogBuilder(), "ace");
            }

            aceCommons.platform.setWindowSize(settings.ConsoleWindowSize);
        }

        /// <summary>
        /// Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            var arg = new aceEventGeneralArgs();

            callEventApplicationClosing(arg);

            settings.Save();

            aceLog.saveAllLogs(true);
        }

        /// <summary>
        /// Handles the UnhandledException event of the ReactTo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void ReactTo_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            aceCommons.reportUnhandledException(e.ExceptionObject as Exception, this);

            aceLog.saveAllLogs(true);
        }

        /// <summary>
        /// State of the application
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public aceApplicationState state { get; protected set; } = null;

        /// <summary>
        /// Sets the author information.
        /// </summary>
        public abstract void setAboutInformation();

        /// <summary>
        /// Information about the application. Used in User Interface and to sign all reports, readme files and exported documents
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public aceApplicationInfo appAboutInfo { get; protected set; } = null;

        /// <summary>
        /// Main log-output for the application
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogBuilder logger { get; protected set; } = null;

        /// <summary>
        /// Short description of the application
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String description { get; protected set; } = "";

        #region FOLDERS ---------------------

        /// <summary>
        /// Main application folder, where the executable is started
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public folderNode folder { get; protected set; } = null;

        /// <summary>
        /// Folder with plugins
        /// </summary>
        public folderNode folder_plugins { get; protected set; } = null;

        /// <summary>
        /// Folder with plugins
        /// </summary>
        /// <value>
        /// The folder reports.
        /// </value>
        public folderNode folder_reports { get; protected set; } = null;

        /// <summary>
        /// Folder with report themes and other resources
        /// </summary>
        /// <value>
        /// The folder resources.
        /// </value>
        public folderNode folder_resources { get; protected set; } = null;

        /// <summary>
        /// Folder with configuration files
        /// </summary>
        /// <value>
        /// The folder configuration.
        /// </value>
        public folderNode folder_config { get; protected set; } = null;

        /// <summary>
        /// Folder used by the chache system
        /// </summary>
        /// <value>
        /// The folder cache.
        /// </value>
        public folderNode folder_cache { get; protected set; } = null;

        /// <summary>
        /// Folder with saved application projects.
        /// </summary>
        /// <value>
        /// The folder projects.
        /// </value>
        public folderNode folder_projects { get; protected set; } = null;

        /// <summary>
        /// Folder with log files and diagnostics
        /// </summary>
        /// <value>
        /// The folder logs.
        /// </value>
        public folderNode folder_logs { get; protected set; } = null;

        #endregion FOLDERS ---------------------

        /// <summary>
        /// Ataman - watching logs and other folders for oversize
        /// </summary>
        /// <value>
        /// The ataman.
        /// </value>
        public aceSystemAtaman ataman { get; protected set; } = null;

        /// <summary>
        /// Plugins from <see cref="folder_plugins"/> folder
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        public pluginManager plugins { get; protected set; } = null;

        /// <summary>
        /// Base application settings
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public aceApplicationSettings settings { get; protected set; } = null;

        /// <summary>
        /// Content of the logger
        /// </summary>
        /// <value>
        /// The content of the log.
        /// </value>
        public string logContent
        {
            get
            {
                if (logger != null) return logger.logContent;
                return "- no logger at [" + name + "]";
            }
        }

        #region EVENTS ---------------------------------------

        /// <summary>Event handler for <see cref="aceEventType.Loaded"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        public event EventHandler<aceEventGeneralArgs> onEventApplicationLoaded;

        /// <summary> The event Loaded at Application caster, with optional pre-created arguments. </summary>
        /// <param name="e">Optional, prefabricated arguments - to provide specific information to particular cause of the event.</param>
        /// <remarks>
        ///   <para>Invokes <see cref="onEventApplicationLoaded"/>, with <see cref="aceEventType.Loaded"/> and <see cref="aceEventOrigin.Application"/></para>
        /// </remarks>
        protected virtual void callEventApplicationLoaded(aceEventGeneralArgs e = null)
        {
            if (e == null) e = new aceEventGeneralArgs();
            if (e.type == aceEventType.unknown) e.type = aceEventType.Loaded;
            if (e.Origin == aceEventOrigin.unknown) e.Origin = aceEventOrigin.Application;
            if (e.RelatedObject == null) e.RelatedObject = null;
            e.Message = "";
            if (onEventApplicationLoaded != null) onEventApplicationLoaded(this, e);
        }

        /// <summary>Event handler for <see cref="aceEventType.Ready"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        public event EventHandler<aceEventGeneralArgs> onEventApplicationReady;

        /// <summary> The event Ready at Application caster, with optional pre-created arguments. </summary>
        /// <param name="e">Optional, prefabricated arguments - to provide specific information to particular cause of the event.</param>
        /// <remarks>
        ///   <para>Invokes <see cref="onEventApplicationReady"/>, with <see cref="aceEventType.Ready"/> and <see cref="aceEventOrigin.Application"/></para>
        /// </remarks>
        protected virtual void callEventApplicationReady(aceEventGeneralArgs e = null)
        {
            if (e == null) e = new aceEventGeneralArgs();
            if (e.type == aceEventType.unknown) e.type = aceEventType.Ready;
            if (e.Origin == aceEventOrigin.unknown) e.Origin = aceEventOrigin.Application;
            if (e.RelatedObject == null) e.RelatedObject = null;
            e.Message = "";
            if (onEventApplicationReady != null) onEventApplicationReady(this, e);
        }

        /// <summary>Event handler for <see cref="aceEventType.Closing"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        public event EventHandler<aceEventGeneralArgs> onEventApplicationClosing;

        /// <summary> The event Closing at Application caster, with optional pre-created arguments. </summary>
        /// <param name="e">Optional, prefabricated arguments - to provide specific information to particular cause of the event.</param>
        /// <remarks>
        ///   <para>Invokes <see cref="onEventApplicationClosing"/>, with <see cref="aceEventType.Closing"/> and <see cref="aceEventOrigin.Application"/></para>
        /// </remarks>
        protected virtual void callEventApplicationClosing(aceEventGeneralArgs e = null)
        {
            if (e == null) e = new aceEventGeneralArgs();
            if (e.type == aceEventType.unknown) e.type = aceEventType.Closing;
            if (e.Origin == aceEventOrigin.unknown) e.Origin = aceEventOrigin.Application;
            if (e.RelatedObject == null) e.RelatedObject = null;
            e.Message = "";
            if (onEventApplicationClosing != null) onEventApplicationClosing(this, e);
        }

        #endregion EVENTS ---------------------------------------
    }
}