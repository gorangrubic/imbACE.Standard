// --------------------------------------------------------------------------------------------------------------------
// <copyright file="emailPlugin.cs" company="imbVeles" >
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
// Project: imbNLP.PartOfSpeech
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.plugins
{
    using imbACE.Core;
    using imbACE.Core.application;
    using imbACE.Core.operations;
    using imbACE.Core.plugins;
    using imbACE.Network.email;
    using imbACE.Services.application;
    using imbACE.Services.console;
    using imbACE.Services.consolePlugins;
    using imbACE.Services.terminal;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.files;
    using imbSCI.Core.files.search;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    /// <summary>
    /// SMTP email sender Plugin for imbACE console - emailPlugin
    /// </summary>
    /// <seealso cref="imbACE.Services.consolePlugins.aceConsolePluginBase" />
    public class emailPlugin : aceConsolePluginBase
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        protected emailServer server { get; set; } = new emailServer();

        /// <summary>
        /// Email message generator
        /// </summary>
        /// <value>
        /// The email factory.
        /// </value>
        protected emailMessageFactory emailFactory { get; set; } = new emailMessageFactory();

        protected IAceAdvancedConsole parentConsole { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="emailPlugin"/> class.
        /// </summary>
        /// <param name="__parent">The parent.</param>
        public emailPlugin(IAceAdvancedConsole __parent) : base(__parent, "emailPlugin", "Creates and sends email messaged")
        {
            parentConsole = __parent;
        }

        [Display(GroupName = "run", Name = "Login", ShortName = "", Description = "Sets the server credentials and tests the connection")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will connect to the email server")]
        /// <summary>
        /// Sets the server credentials and tests the connection
        /// </summary>
        /// <param name="username">Username at SMTP service</param>
        /// <param name="password">Password at SMTP service</param>
        /// <param name="smtpHostUrl">URL to the SMTP service</param>
        /// <param name="smtpPort">The SMTP port.</param>
        /// <param name="useSSL">if set to <c>true</c> it will use SSL.</param>
        /// <remarks>
        /// It will connect to the email server
        /// </remarks>
        /// <seealso cref="aceOperationSetExecutorBase" />
        public void aceOperation_runLogin(
            [Description("Username at SMTP service")] String username = "",
            [Description("Password at SMTP service")] String password = "",
            [Description("URL to the SMTP service")] String smtpHostUrl = "smtp.gmail.com",
            [Description("SMTP server port")] Int32 smtpPort = 587,
            [Description("Turns on SSL")] Boolean useSSL = true
            )
        {
            server = new emailServer();

            server.smtpHostUrl = smtpHostUrl;
            server.smtpServerPort = smtpPort;
            server.useSSL = true;

            if (server.connectSmtp(username, password, smtpHostUrl, useSSL, smtpPort))
            {
                output.log("Connection ok @ " + server.smtpHostUrl);
            }
            else
            {
                if (server.isConnectedToSMTP)
                {
                    output.log("Already connected @ " + server.smtpHostUrl);
                }
                else
                {
                    output.log("Connection failed @ " + server.smtpHostUrl);
                }
            }
        }

        [Display(GroupName = "run", Name = "CreateBlocks", ShortName = "", Description = "It will run message factory")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "If will try to load factory with name specified and execute it")]
        /// <summary>It will run message factory</summary>
        /// <remarks><para>If will try to load factory with name specified and execute it</para></remarks>
        /// <param name="factoryName">Name of factory XML settings</param>
        /// <param name="dataSource">Name of CSV file with data</param>
        /// <param name="debug">if TRUE it will write each message to the output</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runCreateBlocks(
            [Description("Name of factory XML settings")] String factoryName = "emailFactory.xml",
            [Description("Name of CSV file with data")] String dataSource = "",
            [Description("if TRUE it will write each message to the output")] Boolean debug = true)
        {
            String p = parentConsole.workspace.folder.pathFor(factoryName, imbSCI.Data.enums.getWritableFileMode.newOrExisting);
            if (File.Exists(p))
            {
                emailFactory = objectSerialization.loadObjectFromXML<emailMessageFactory>(p, output);
            }
            else
            {
                emailFactory = new emailMessageFactory();

                emailFactory.dataSourcePath = terminal.dialogs.dialogs.openSelectFile(textBlocks.smart.dialogSelectFileMode.selectFileToOpen, "*.xlsx", parentConsole.workspace.folder.path, "Select CSV data source");

                objectSerialization.saveObjectToXML(emailFactory, p);
            }

            emailFactory.deploy(output, parentConsole.workspace.folder);

            emailFactory.createCollections(output, parentConsole.workspace.folder, debug);
        }

        [Display(GroupName = "run", Name = "SendBlock", ShortName = "", Description = "Loads specified block and start sending cycle")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will load the block, ask for confirmation and start sending cycle")]
        /// <summary>Loads specified block and start sending cycle</summary>
        /// <remarks><para>It will load the block, ask for confirmation and start sending cycle</para></remarks>
        /// <param name="blockName">Name of block XML file</param>
        /// <param name="delay">Delay in seconds between two sends</param>
        /// <param name="askConfirm">if TRUE it will ask for confirmation before start sendinging cycle</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runSendBlock(
            [Description("Name of block XML file")] String blockName = "block01.xml",
            [Description("Delay in seconds between two sends")] Int32 delay = 5,
            [Description("if TRUE it will ask for confirmation before start sendinging cycle")] Boolean askConfirm = true)
        {
            String p = parentConsole.workspace.folder.findFile(blockName, SearchOption.TopDirectoryOnly);

            emailMessageCollection block = objectSerialization.loadObjectFromXML<emailMessageCollection>(p, output);

            foreach (emailMessage message in block)
            {
                if (!message.isSent)
                {
                    output.AppendLine(message.subject + "   " + message.address);
                }
            }

            Boolean ok = !askConfirm;

            if (askConfirm) ok = aceTerminalInput.askYesNo("Do you want to start sending cycle?", false);

            if (ok)
            {
                foreach (emailMessage message in block)
                {
                    if (!message.isSent)
                    {
                        message.isSent = server.Send(message, output);

                        if (message.isSent) block.Save(p);

                        Boolean abort = aceTerminalInput.askPressAnyKeyInTime("Press any key to stop the cycle. Leave timeout to send next.", false, delay, true, 1);

                        if (abort)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}