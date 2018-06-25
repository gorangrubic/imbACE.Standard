// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommons.cs" company="imbVeles" >
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
namespace imbACE.Services
{
    using imbACE.Core;
    using imbACE.Core.commands.menu;
    using imbACE.Core.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums;

    using imbACE.Core.enums;

    using imbACE.Core.extensions;
    using imbACE.Core.interfaces;
    using imbACE.Core.operations;
    using imbACE.Services.platform;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Funkcije koje omogucavaju rad aplikacija zasnovanih na ace platformi
    /// </summary>
    public static class aceCommons
    {
        #region --- commandLineOpsMenu ------- static and autoinitiated object

        private static aceCommandLineOptions _commandLineOpsMenu;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static aceCommandLineOptions commandLineOpsMenu
        {
            get
            {
                return _commandLineOpsMenu;
            }
            set
            {
                _commandLineOpsMenu = value;
            }
        }

        #endregion --- commandLineOpsMenu ------- static and autoinitiated object

        private static IPlatform _platform;

        /// <summary>
        /// glavni terminal za logovanje itd
        /// </summary>
        public static IPlatform platform
        {
            get
            {
                if (_platform == null)
                {
                    _platform = new consolePlatform();
                }
                return _platform;
            }
        }

        public static void setTerminal(builderForLog __terminal)
        {
            _terminal = __terminal;
        }

        #region --- terminal ------- glavni terminal za logovanje itd

        private static builderForLog _terminal;

        /// <summary>
        /// glavni terminal za logovanje itd
        /// </summary>
        public static builderForLog terminal
        {
            get
            {
                if (_terminal == null)
                {
                    _terminal = new builderForLog("log.md", true);
                    _terminal.registerLogBuilder(logOutputSpecial.aceServices);

                    // (imbACE.Core.interfaces.ILogBuilder)new aceTerminal(platform);
                }
                return _terminal;
            }
        }

        #endregion --- terminal ------- glavni terminal za logovanje itd

        /// <summary>
        /// Updates log output with new line
        /// </summary>
        /// <param name="message"></param>
        public static void log(this String message)
        {
            terminal.log(message);
        }

        private static String _crashReportPath = "diagnostic";

        /// <summary>
        ///
        /// </summary>
        public static String crashReportPath
        {
            get
            {
                return _crashReportPath;
            }
            set
            {
                _crashReportPath = value;
            }
        }

        private static Int32 _unhandledExceptionCount = 0;

        /// <summary>
        ///
        /// </summary>
        public static Int32 unhandledExceptionCount
        {
            get { return _unhandledExceptionCount; }
            set { _unhandledExceptionCount = value; }
        }

        /// <summary>
        /// Reports exception that none catched
        /// </summary>
        /// <param name="e"></param>
        /// <param name="client"></param>
        public static void reportUnhandledException(Exception e, IAceLogable client)
        {
            unhandledExceptionCount++;

            String msg = e.LogException("Unhandled Exception :: " + appManager.AppInfo.applicationName + ")");

            if (client != null)
            {
                msg += "Instance: " + client.GetType().Name + " " + Environment.NewLine;
                client.log(msg);
                msg += "----------------- log content ----------------" + Environment.NewLine;
                msg += client.logContent;
            }
            else
            {
                msg += "Instance: null" + Environment.NewLine;
            }
            String filename = "_crash_report_" + unhandledExceptionCount.ToString("D3") + DateTime.Now.ToString("MM-dd") + ".txt";

            String path = appManager.Application.folder_logs.pathFor(filename, getWritableFileMode.autoRenameExistingToBack);

            File.WriteAllText(path, msg);

            if (unhandledExceptionCount > 10)
            {
                Environment.Exit(0);
                //AppDomain.CurrentDomain.
            }
            aceLog.saveAllLogs(true);

            throw (e);
        }
    }
}