// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbACECoreConfig.cs" company="" >
//
// Copyright (C) 2017 gorangrubic
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
// Project: $projectname$
// Author: gorangrubic
// ------------------------------------------------------------------------------------------------------------------
// Based on Project Item template: imbSCI Global Config
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbACE.Core.commands;
using imbSCI.Core.attributes;
using imbSCI.Core.extensions.io;
using System;

using System;

using System.Collections.Generic;

using System.Collections.Generic;

using System.ComponentModel;
using System.Linq;
using System.Text;

using System.Text;

using System.Xml.Serialization;

namespace imbACE.Core
{
    /// <summary>
    /// General configuration object for domain of <see cref="imbACE.Core"/>
    /// </summary>
    public class imbACECoreConfig
    {
        #region --------------------------------------------------- DO NOT MODIFY --------------------------------------------------------------

        /// <summary>
        /// Constructor without arguments is obligatory for XML serialization
        /// </summary>
        public imbACECoreConfig() { }

        /// <summary>
        /// Gets or sets a value indicating whether, since program start, <see cref="settings"/> were replaced with another instance, i.e. loaded externally
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is default replaced; otherwise, <c>false</c>.
        /// </value>
        public static Boolean isDefaultReplaced { get; set; } = false;

        private static imbACECoreConfig _settings = new imbACECoreConfig();

        /// <summary>
        /// General configuration instance for domain of <see cref="imbACE.Core"/>
        /// </summary>
        /// <value>
        /// Global settings
        /// </value>
        public static imbACECoreConfig settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if ((_settings != value) && (value != null)) isDefaultReplaced = true;

                _settings = value;
            }
        }

        #endregion --------------------------------------------------- DO NOT MODIFY --------------------------------------------------------------

        // Insert below your global configuration properties.
        // Snippets: _imbSCI_DoBool, _imbSCI_String, _imbSCI_Ratio ....

        /// <summary> If true it will allow calling OS GUI dialogs </summary>
        [Category("Switch")]
        [DisplayName("doAllowOSDialogs")]
        [Description("If true it will allow calling OS GUI dialogs")]
        public Boolean doAllowOSDialogs { get; set; } = false;

        /// <summary> If true it send the main log to the screen </summary>
        [Category("Switch")]
        [DisplayName("doDisplayMainLog")]
        [Description("If true it send the main log to the screen")]
        public Boolean doDisplayMainLog { get; set; } = true;

        /// <summary> If true it will by default, show script lines that are just sent to execution </summary>
        [Category("Switch")]
        [DisplayName("doShowScriptLines")]
        [Description("If true it will by default, show script lines that are just sent to execution")]
        public scriptLineShowMode doShowScriptLines { get; set; } = scriptLineShowMode.fullPrefixAndCodeLine;

        /// <summary> If true it will do allow auto save of the globally registered logs </summary>
        [Category("Switch")]
        [DisplayName("doAllowLogAutosave")]
        [Description("If true it will do allow auto save of the globally registered logs")]
        public Boolean doAllowLogAutosave { get; set; } = true;

        /// <summary> If true it will do remove content of the globally registered logs, after reaching Log Page KByte limit </summary>
        [Category("Switch")]
        [DisplayName("doAutoFlushLogPage")]
        [Description("If true it will do remove content of the globally registered logs, after reaching Log Page KByte limit")]
        public Boolean doAutoFlushLogPage { get; set; } = true;

        /// <summary> Size limit of log content that triggers log flush </summary>
        [Category("Count")]
        [DisplayName("autoFlushLogPageKByteLimit")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "Kb")]
        [Description("Size limit of log content that triggers log flush")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 autoFlushLogPageKByteLimit { get; set; } = 1024;

        /// <summary>
        /// If true it will prevent another log save within autosaveTimeSpanInSec seconds
        /// </summary>
        [Category("Switch")]
        [DisplayName("Autosave Time limit On")]
        [Description("If true it will prevent another log save within autosaveTimeSpanInSec seconds")]
        public Boolean autosaveTimeSpanOn { get; set; } = true;

        /// <summary>
        /// Time required to unlock next autosave
        /// </summary>
        [Category("Limit")]
        [DisplayName("Autosave Time limit")]
        [Description("If true it will prevent another log save within autosaveTimeSpanInSec seconds")]
        public Int32 autosaveTimeSpanInSec { get; set; } = 60;

        /// <summary> If true it will use console text markup parsing to color stressed parts of the text </summary>
        [Category("Switch")]
        [DisplayName("doConsoleColorsByMarkupParshing")]
        [Description("If true it will use console text markup parsing to color stressed parts of the text")]
        public Boolean doConsoleColorsByMarkupParshing { get; set; } = false;

        /// <summary> If true it will assign different color to each ILogBuilder registered to output </summary>
        [Category("Switch")]
        [DisplayName("doConsoleColorsCyrcleSelector")]
        [Description("If true it will assign different color to each ILogBuilder registered to output")]
        public Boolean doConsoleColorsCyrcleSelector { get; set; } = true;
    }
}