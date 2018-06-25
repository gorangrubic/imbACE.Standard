// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceApplicationBase.cs" company="imbVeles" >
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
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbACE.Core.ataman;
using imbACE.Core.core;
using imbACE.Core.events;
using imbACE.Core.operations;
using imbACE.Core.plugins;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbACE.Core.application
{
    /// <summary>
    /// Interface to the foundation of AceApplication
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public interface IAceApplicationBase
    {
        List<String> commandLineArguments { get; }

        /// <summary>
        /// Main log-output for the application
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        ILogBuilder logger { get; }

        /// <summary>
        /// Name of the application
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String name { get; }

        /// <summary>
        /// Short description of the application
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        String description { get; }

        /// <summary>
        /// Main application folder, where the executable is started
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        folderNode folder { get; }

        /// <summary>
        /// Folder with plugins
        /// </summary>
        folderNode folder_plugins { get; }

        /// <summary>
        /// Folder with plugins
        /// </summary>
        /// <value>
        /// The folder reports.
        /// </value>
        folderNode folder_reports { get; }

        /// <summary>
        /// Folder with report themes and other resources
        /// </summary>
        /// <value>
        /// The folder resources.
        /// </value>
        folderNode folder_resources { get; }

        /// <summary>
        /// Folder with configuration files
        /// </summary>
        /// <value>
        /// The folder configuration.
        /// </value>
        folderNode folder_config { get; }

        /// <summary>
        /// Folder used by the chache system
        /// </summary>
        /// <value>
        /// The folder cache.
        /// </value>
        folderNode folder_cache { get; }

        /// <summary>
        /// Folder with saved application projects.
        /// </summary>
        /// <value>
        /// The folder projects.
        /// </value>
        folderNode folder_projects { get; }

        /// <summary>
        /// Folder with log files and diagnostics
        /// </summary>
        /// <value>
        /// The folder logs.
        /// </value>
        folderNode folder_logs { get; }

        /// <summary>
        /// Ataman - watching logs and other folders for oversize
        /// </summary>
        /// <value>
        /// The ataman.
        /// </value>
        aceSystemAtaman ataman { get; }

        /// <summary>
        /// Plugins from <see cref="folder_plugins"/> folder
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        pluginManager plugins { get; }

        /// <summary>
        /// Base application settings
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        aceApplicationSettings settings { get; }

        //   Boolean doApplicationLoop();

        aceApplicationInfo appAboutInfo { get; }

        /// <summary>Event handler for <see cref="aceEventType.Loaded"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        event EventHandler<aceEventGeneralArgs> onEventApplicationLoaded;

        /// <summary>Event handler for <see cref="aceEventType.Ready"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        event EventHandler<aceEventGeneralArgs> onEventApplicationReady;

        /// <summary>Event handler for <see cref="aceEventType.Closing"/>" at <see cref="aceEventOrigin.Application"/>, to hook reaction method on to. </summary>;
        event EventHandler<aceEventGeneralArgs> onEventApplicationClosing;

        void doQuit();
    }
}