// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceCommandConsole.cs" company="imbVeles" >
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

namespace imbACE.Core.operations
{
    using imbACE.Core.application;
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.commands.tree;
    using imbACE.Core.core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Low-tier interface to imbACE Command Console.
    /// </summary>
    /// <remarks>
    ///     <para>This is interface to the base class of the imbACE scriptable ACE Command Console.</para>
    ///     <para>Next tier is <c>aceAdvancedConsole</c> - implementing: <c>IAceAdvancedConsole</c> interface with more exposed features.</para>
    /// </remarks>
    /// <seealso cref="imbACE.Core.operations.IAceOperationSetExecutor" />
    public interface IAceCommandConsole : IAceOperationSetExecutor
    {
        /// <summary>It pause ACE script execution, optionally displays custom message and allows user to end the pause</summary>
        /// <remarks><para>If wait set to -1 there will be no time limit. If userCanEnd is true it will allow user to end the pause and continue.</para></remarks>
        /// <param name="wait">How log the pause may last? in seconds. If set to -1 there is no time limit</param>
        /// <param name="msg">Custom message to be displayed to user.</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        void aceOperation_runPause(
            [Description("How log the pause may last? in seconds. If set to -1 there is no time limit")] Int32 wait = 60,
            [Description("Custom message to be displayed to user.")] String msg = "");

        /// <summary>Should be called from executing script.</summary>
        /// <remarks><para>It will abort the running script, and stay in the console shell</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        void aceOperation_runAbort();

        // [aceMenuItem(aceMenuItemAttributeRole.ConfirmMessage, "Are you sure?")]  // [aceMenuItem(aceMenuItemAttributeRole.EnabledRemarks, "")]
        // [aceMenuItem(aceMenuItemAttributeRole.externalHelpFilename, "aceOperation_consoleHelp.md")]
        // [aceMenuItem(aceMenuItemAttributeRole.CmdParamList, "param:type;paramb:type;")]
        /// <summary>
        /// Aces the operation console help.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="onlyThisConsole">if set to <c>true</c> [only this console].</param>
        void aceOperation_consoleHelp(
            [Description("Help option: all, properties, plugins, modules, commands")]
            aceCommandConsoleHelpOptions option=aceCommandConsoleHelpOptions.none,
            Boolean onlyThisConsole = true
        );

        /// <summary> Console title </summary>
        String consoleTitle { get; }

        /// <summary> Short help message about this console </summary>
        String consoleHelp { get; }

        /// <summary> Reference to the application's platform to display/use the console outputs </summary>
        //IPlatform platform { get; }

        /// <summary>
        /// The primary console output. By default pointed to the <see cref="platform"/> via <see cref="aceLog.consoleControl"/>
        /// </summary>
        builderForLog output { get; set; }

        /// <summary>
        /// Secondary output of the console. By default it is pointed to the <see cref="platform"/> output, but it can be directed to a file or other outputs.
        /// </summary>
        builderForLog response { get; set; }

        /// <summary>
        /// osnovni menu
        /// </summary>
        aceMenu commands { get; }

        /// <summary> Aditional points for console help header text. <see cref="imbACE.Services.terminal.console.aceCommandConsole.helpContent"/></summary>
        List<String> helpHeader { get; }

        /// <summary>
        /// command tree, used for help generation
        /// </summary>
        /// <value>
        /// The command set tree.
        /// </value>
        commandTree commandSetTree { get; set; }

        /// <summary>
        /// Help text short header, used when console api is rendered or shown
        /// </summary>
        String helpContent { get; set; }

        /// <summary> Reference to the console script that is running or was running last time</summary>
        IAceConsoleScript scriptRunning { get; }

        /// <summary> It is <c>true</c> if console is turned on, waiting for an input or performing already given command(s) </summary>
        Boolean consoleIsRunning { get; }

        Boolean saveClipboard { get; set; }

        /// <summary>
        ///
        /// </summary>
        String linePrefix { get; set; }

        /// <summary>
        ///
        /// </summary>
        String lastInput { get; set; }

        List<String> history { get; set; }

        /// <summary>
        /// Executes the command console script. <see cref="imbACE.Services.terminal.console.aceConsoleScript"/>
        /// </summary>
        /// <param name="script">The console script to execute</param>
        /// <param name="delay">The delay between two instructions in the script, in miliseconds</param>
        IAceConsoleScript executeScript(IAceConsoleScript script, Int32 delay = 10);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="input">The input.</param>
        void executeCommand(String input);

        /// <summary>
        /// Starts the console
        /// </summary>
        void start(IAceApplicationBase application);

        event PropertyChangedEventHandler PropertyChanged;
    }
}