// --------------------------------------------------------------------------------------------------------------------
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
// Project: 
// Author: 
// ------------------------------------------------------------------------------------------------------------------
// Created with imbVeles / imbACE framework
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace $rootnamespace$
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using imbACE.Core;
    using imbACE.Core.application;
    using imbACE.Core.plugins;
    using imbACE.Core.operations;
    using imbACE.Services.application;
    using imbACE.Services.console;
    using imbACE.Services.consolePlugins;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.files.search;
    using imbSCI.Data.interfaces;

    /// <summary>
    /// Plugin for imbACE console - $safeitemname$
    /// </summary>
    /// <seealso cref="imbACE.Services.consolePlugins.aceConsolePluginBase" />
    public class $safeitemname$ : aceConsolePluginBase
    {        
        /// <summary>
        /// Reference to parent console, containing this plugin instance. This property must stay private.
        /// </summary>
        /// <value>
        /// The parent console.
        /// </value>
        private IAceAdvancedConsole parentConsole { get; set; }

        public $safeitemname$(IAceAdvancedConsole __parent):base(__parent, "$safeitemname$", "This is imbACE advanced console plugin for $safeitemname$")
        {
            parentConsole = __parent;
        }

        // at this class you can add properties and nested classes your plugin implementation requires
        
        // use _imbAceOperationMethod snippet to add ACE Script operations available at this plugin

    }


}

