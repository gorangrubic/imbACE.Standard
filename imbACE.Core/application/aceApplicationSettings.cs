// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceApplicationSettings.cs" company="imbVeles" >
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
//using imbACE.Core;
//using imbACE.Core.enums.platform;
//using imbSCI.Core;
//using imbSCI.Core.attributes;
//using imbSCI.Core.enums;
//using imbSCI.Core.extensions.text;
//using imbSCI.Core.extensions.typeworks;
//using imbSCI.Core.interfaces;
//using imbSCI.Data;
//using imbSCI.Data.collection;
//using imbSCI.Data.data;
//using imbSCI.Data.interfaces;
//using imbSCI.DataComplex;
//using imbSCI.Reporting;
//using imbSCI.Reporting.enums;
//using imbSCI.Reporting.interfaces;

namespace imbACE.Core.application
{
    using imbACE.Core.core;
    using imbACE.Core.enums.platform;
    using imbSCI.Core.config;
    using imbSCI.Core.files.folders;
    using imbSCI.Reporting.config;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Base class for application settings
    /// </summary>
    /// <seealso cref="imbACE.Core.core.aceSettingsStandaloneBase" />
    public class aceApplicationSettings : aceSettingsStandaloneBase
    {
        public aceApplicationSettings()
        {
        }

        public void Load(folderNode folder)
        {
            _settings_filepath = folder.pathFor("application.xml");
            Load();
        }

        protected string _settings_filepath;

        [XmlIgnore]
        public override string settings_filepath { get { return _settings_filepath; } }

        /// <summary>
        /// Gets or sets the ace core configuration.
        /// </summary>
        /// <value>
        /// The ace core configuration.
        /// </value>
        public imbACECoreConfig aceCoreConfig { get; set; } = new imbACECoreConfig();

        /// <summary>
        /// Gets or sets the sci core configuration.
        /// </summary>
        /// <value>
        /// The sci core configuration.
        /// </value>
        public imbSCICoreConfig sciCoreConfig { get; set; } = new imbSCICoreConfig();

        /// <summary>
        /// Gets or sets the sci reporting confing.
        /// </summary>
        /// <value>
        /// The sci reporting confing.
        /// </value>
        public imbSCIReportingConfig sciReportingConfing { get; set; } = new imbSCIReportingConfig();

        /// <summary>Main thread loop delay in ms</summary>
        [Category("Thread")]
        [DisplayName("External loop delay")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Main thread loop delay in ms")]
        public Int32 doLoopExternalDelay { get; } = 500;

        /// <summary> Culture code </summary>
        [Category("Label")]
        [DisplayName("Culture")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Culture code")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Culture { get; set; } = "en-US";

        /// <summary> Defines the size of the console application window </summary>
        [Category("Flag")]
        [DisplayName("Console Window size")]
        [Description("Defines the size of the console application window")]
        public windowSize ConsoleWindowSize { get; set; } = windowSize.medium;

        /// <summary> If true it will ask for confirmation on application exit </summary>
        [Category("Flag")]
        [DisplayName("doAskOnApplicationExit")]
        [Description("If true it will ask for confirmation on application exit")]
        public Boolean doAskOnApplicationExit { get; set; } = true;

        /// <summary>  </summary>
        [Category("Cache")]
        [DisplayName("CachePathOverride")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String CachePathOverride { get; set; } = "";

        /// <summary> If true it will run Ataman check on directories with auto-generated data on application start-up </summary>
        [Category("StartUp")]
        [DisplayName("RunAtamanOnStartUp")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("If true it will run Ataman check on directories with auto-generated data on application start-up")] // [imb(imbAttributeName.reporting_escapeoff)]
        public Boolean RunAtamanOnStartUp { get; set; } = true;

        /// <summary> If true it will search and load external plugins during start-up </summary>
        [Category("StartUp")]
        [DisplayName("LoadPluginsOnStartUp")]
        [Description("If true it will search and load external plugins during start-up")]
        public Boolean LoadPluginsOnStartUp { get; set; } = true;

        /// <summary> If <c>true</c> it will regenerate application folder's read-me files </summary>
        [Category("StartUp")]
        [DisplayName("RegenerateReadMeFiles")]
        [Description("If true it will regenerate application folder's read-me files")]
        public Boolean RegenerateReadMeFiles { get; set; } = true;
    }
}