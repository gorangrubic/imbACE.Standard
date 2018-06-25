// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceConsolePluginBase.cs" company="imbVeles" >
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

namespace imbACE.Services.consolePlugins
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbACE.Core.plugins;
    using imbACE.Core.plugins.core;
    using imbACE.Services.console;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Console plug-in -- use it as public property of a <see cref="aceCommandConsole"/> class to enable command execution.
    /// </summary>
    /// <remarks>
    /// Methods from a plugin should be called with proper command prefix pointing to property (of <see cref="aceConsolePluginBase"/> type) name.
    /// Like: <c>printPlugin.print</c> where <c>printPlugin</c> is property within a aceCommandConsole object and <c>print</c> is method of the plugin.
    /// </remarks>
    /// <seealso cref="aceOperationSetExecutorBase" />
    /// <seealso cref="IAceOperationSetExecutor" />
    public abstract class aceConsolePluginBase : aceOperationSetExecutorBase, IAceConsolePlugin
    {
        /// <summary>
        /// Reference to the parrent console
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public IAceOperationSetExecutor parent { get; set; }

        /// <summary>
        /// Indicates if this console plugin is running without parent console
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is standalone; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsStandalone
        {
            get
            {
                return (parent == null);
            }
        }

        /// <summary>
        /// Name of this plugin
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        protected String _instanceName = "";

        /// <summary>
        /// Instance name
        /// </summary>
        /// <value>
        /// The name of the instance.
        /// </value>
        public String instanceName
        {
            get
            {
                if (_instanceName == "") return name;
                return _instanceName;
            }
            set
            {
                _instanceName = value;
            }
        }

        /// <summary>
        /// Title of this plugin - if standalone then alias to <see cref="name"/>, if connected to the parent then sufix title, added after parent console title
        /// </summary>
        /// <value>
        /// The console title.
        /// </value>
        public virtual String consoleTitle
        {
            get
            {
                if (IsStandalone) return instanceName;
                return parent.consoleTitle + "." + instanceName;
            }
        }

        private String _consoleHelp = "";

        /// <summary>
        /// First line of help to be shown for this plugin
        /// </summary>
        /// <value>
        /// The console help.
        /// </value>
        public virtual String consoleHelp
        {
            get
            {
                return _consoleHelp;
            }
        }

        protected aceMenu _commands;

        /// <summary>
        /// Interface to it's command list
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        aceMenu IAceOperationSetExecutor.commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new aceMenu();
                    _commands.setItems(this);
                }
                return _commands;
            }
        }

        protected builderForLog _output;
        private ITextRender _response;

        public virtual ILogBuilder output
        {
            get
            {
                if (IsStandalone) return _output;
                return parent.output;
            }
        }

        /// <summary>
        /// Plugin's own response text renderer. By default (when response not set) it is just mirror to the <see cref="output"/> renderer
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public virtual ITextRender response
        {
            get
            {
                if (_response == null) return output;
                return _response;
            }
            set { _response = value; }
        }

        ILogBuilder IAceOperationSetExecutor.output => output;

        /// <summary>
        /// Additional help content to be displayed at beginning of the help file, after <see cref="consoleHelp"/>
        /// </summary>
        /// <value>
        /// The help header.
        /// </value>
        public List<string> helpHeader { get; set; } = new List<string>();

        protected void prepare()
        {
            _output = new builderForLog();

            aceLog.consoleControl.setAsOutput(_output, consoleTitle);
        }

        /// <summary>
        /// Stand-alone intended use
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__help">The help.</param>
        /// <param name="__output">The output.</param>
        protected aceConsolePluginBase(String __name, String __help = "", builderForLog __output = null)
        {
            name = __name;
            _consoleHelp = __help;
            //_output = __output;
            prepare();
        }

        /// <summary>
        /// Use as built-in plugin to a console
        /// </summary>
        /// <param name="__parent">The parent.</param>
        /// <param name="__name">The name.</param>
        protected aceConsolePluginBase(IAceOperationSetExecutor __parent, String __name, String __help = "")
        {
            name = __name;
            _consoleHelp = __help;
            parent = __parent;
            prepare();
        }
    }
}