// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceAdvancedConsoleStateBase.cs" company="imbVeles" >
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

namespace imbACE.Services.console
{
    using imbACE.Core.application;
    using imbACE.Core.core;
    using imbSCI.Core.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Console state - keeps settings of the console
    /// </summary>
    /// <seealso cref="imbACE.Core.core.aceSettingsStandaloneBase" />
    public abstract class aceAdvancedConsoleStateBase : aceSettingsStandaloneBase
    {
        private String _scopePath = "";

        /// <summary>
        ///
        /// </summary>
        public String scopePath
        {
            get { return _scopePath; }
            set { _scopePath = value; }
        }

        private String _loadedContentFilepath = "";

        /// <summary>
        ///
        /// </summary>
        public String loadedContentFilepath
        {
            get { return _loadedContentFilepath; }
            set { _loadedContentFilepath = value; }
        }

        private List<String> _loadedContent = new List<string>();

        /// <summary>
        ///
        /// </summary>
        public List<String> loadedContent
        {
            get { return _loadedContent; }
            set { _loadedContent = value; }
        }

        /// <summary>
        /// Path of the settings
        /// </summary>
        public override string settings_filepath
        {
            get
            {
                return aceApplicationInfo.FOLDERNAME_CONFIG + Path.DirectorySeparatorChar + this.GetType().Name + "_state.xml";
            }
        }

        /// <summary>
        /// Path where the projects are
        /// </summary>
        /// <value>
        /// The projects path.
        /// </value>
        public virtual String projects_path
        {
            get
            {
                return aceApplicationInfo.FOLDERNAME_PROJECTS + Path.DirectorySeparatorChar;
            }
        }

        private String _currentProjectName = ""; //default(String); // = new String();

                                                 /// <summary>
                                                 /// name of the current console project
                                                 /// </summary>
        [Category("aceCommandConsoleStateBase")]
        [DisplayName("currentProjectName")]
        [Description("name of the current console project")]
        public String currentProjectName
        {
            get
            {
                if (_currentProjectName == "") _currentProjectName = this.GetType().Name.Replace("Console", "01");
                return _currentProjectName;
            }
            set
            {
                _currentProjectName = value;
                OnPropertyChanged(nameof(currentProjectName));
            }
        }

        #region ----------- Boolean [ doWordInDebugMode ] -------  [If true the console will produce all kinds of log outputs, data and serialized objects]

        private Boolean _doWorkInDebugMode = false;

        /// <summary>
        /// If true the console will produce all kinds of log outputs, data and serialized objects
        /// </summary>
        [Category("Switches")]
        [DisplayName("doWorkInDebugMode")]
        [Description("If true the console will produce all kinds of log outputs, data and serialized objects")]
        public Boolean doWorkdInDebugMode
        {
            get { return _doWorkInDebugMode; }
            set { _doWorkInDebugMode = value; OnPropertyChanged("doWordInDebugMode"); }
        }

        #endregion ----------- Boolean [ doWordInDebugMode ] -------  [If true the console will produce all kinds of log outputs, data and serialized objects]

        #region ----------- Boolean [ doRunScriptOnStartup ] -------  [The filename (inside the script folder) of script file to run upon start of the console]

        private String _doRunScriptOnStartup = "autoexec.ace";

        /// <summary>
        /// The filename (inside the script folder) of script file to run upon start of the console
        /// </summary>
        [Category("Switches")]
        [DisplayName("doRunScriptOnStartup")]
        [Description("The filename (inside the script folder) of script file to run upon start of the console")]
        public String doRunScriptOnStartup
        {
            get { return _doRunScriptOnStartup; }
            set { _doRunScriptOnStartup = value; OnPropertyChanged("doRunScriptOnStartup"); }
        }

        #endregion ----------- Boolean [ doRunScriptOnStartup ] -------  [The filename (inside the script folder) of script file to run upon start of the console]
    }
}