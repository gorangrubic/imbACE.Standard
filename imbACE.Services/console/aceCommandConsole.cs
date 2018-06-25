// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommandConsole.cs" company="imbVeles" >
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

//using System.Windows.Forms;

namespace imbACE.Services.console
{
    using imbACE.Core.application;
    using imbACE.Core.commands;
    using imbACE.Core.commands.menu;
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.commands.tree;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbACE.Services.platform;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files.job;
    using imbSCI.Core.reporting;
    using imbSCI.Core.syntax.param;
    using imbSCI.Data.enums;
    using imbSCI.DataComplex.extensions.text;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Basic command console environment
    /// </summary>
    /// <seealso cref="aceOperationSetExecutorBase" />
    public abstract class aceCommandConsole : aceOperationSetExecutorBase, IAceOperationSetExecutor, IAceCommandConsole
    {
        public Boolean doAskForQuitConfirmation { get; set; } = false;

        /// <summary>
        /// Should user be prompted on <see cref="aceOperation_consoleExit()"/> to confirm?
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do ask for exit configmation]; otherwise, <c>false</c>.
        /// </value>
        public Boolean doAskForExitConfigmation { get; set; } = false;

        #region ------------------- BASIC CONSOLE OPERATIONS --------------------------------------------------------------------------------------------------------------------------

        [Display(GroupName = "console", Name = "ClearScreen", ShortName = "", Description = "Clear screen - flushing console output buffer")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "Clears the current screen")]
        [aceMenuItem(aceMenuItemAttributeRole.aliasNames, "Cls")]
        /// <summary>Clear screen - flushing console output buffer</summary>
        /// <remarks><para>Clears the current screen</para></remarks>
        public void aceOperation_consoleCls()
        {
            cls();
        }

        [Display(GroupName = "run", Name = "Pause", ShortName = "", Description = "It pause ACE script execution, optionally displays custom message and allows user to end the pause")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "If wait set to -1 there will be no time limit, the user will have to stop it. It will beep in last 1/5 of wait period.")]
        /// <summary>It pause ACE script execution, optionally displays custom message and allows user to end the pause</summary>
        /// <remarks><para>If wait set to -1 there will be no time limit. If userCanEnd is true it will allow user to end the pause and continue.</para></remarks>
        /// <param name="wait">How log the pause may last? in seconds. If set to -1 there is no time limit</param>
        /// <param name="msg">Custom message to be displayed to user.</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runPause(
            [Description("How log the pause may last? in seconds. If set to -1 there is no time limit")] Int32 wait = 60,
            [Description("Custom message to be displayed to user.")] String msg = "")
        {
            if (msg == "")
            {
                msg = "Pause command called";
            }

            aceTerminalInput.askPressAnyKeyInTime(msg, false, wait, true, wait / 5);
        }

        [Display(GroupName = "run", Name = "Abort", ShortName = "", Description = "Should be called from executing script.")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will abort the running script, and stay in the console shell")]
        /// <summary>Should be called from executing script.</summary>
        /// <remarks><para>It will abort the running script, and stay in the console shell</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runAbort()
        {
            if (scriptRunning != null)
            {
                scriptRunning.AbortExecution();
            }
        }

        [Display(GroupName = "run", Name = "None", ShortName = "none", Description = "Pseudo instruction - does nothing actually")]
        /// <summary>Pseudo instruction - does nothing actually</summary>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runNone()
        {
        }

        [Display(GroupName = "console", Name = "Help", ShortName = "h", Description = "Provides Type-specific help content for the console, lists of all supported commands, plugins, local variables...")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "If help option is not specified, it will ask user for type of help should be displayed")]
        /// <summary>
        /// Provides Type-specific help content for the console, lists of all supported commands, plugins, local variables...
        /// </summary>
        /// <param name="option">Type of help content to show, if not specified it prompts the user</param>
        /// <param name="onlyThisConsole">if set to <c>true</c> [only this console].</param>
        /// <remarks>
        /// If help option is not specified, it will ask user for type of help should be displayed
        /// </remarks>
        /// <seealso cref="aceOperationSetExecutorBase" />
        public void aceOperation_consoleHelp(
            [Description("Type of help content to show, if not specified it prompts the user")] aceCommandConsoleHelpOptions option = aceCommandConsoleHelpOptions.none,
            [Description("If true it will generate user manual only for this console")] Boolean onlyThisConsole = true
            )
        {
            if (option == aceCommandConsoleHelpOptions.none)
            {
                option = aceTerminalInput.askForEnum<aceCommandConsoleHelpOptions>("Select Help option:", aceCommandConsoleHelpOptions.full);
            }

            if (onlyThisConsole)
            {
                var cst = commandTreeTools.BuildCommandTree(this, false);
                cst.ReportCommandTree(output, false, 0, option);
                helpContent = output.getLastLine();
            }
            else
            {
                commandSetTree.ReportCommandTree(output, false, 0, option);
                helpContent = output.getLastLine();
            }
        }

        [Display(GroupName = "console", Name = "Quit", ShortName = "q", Description = "Quiting the aceApplication this console is running in")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will prompt the user to confirm, if specified by console configuration")]
        /// <summary>Quiting the aceApplication this console is running in</summary>
        /// <remarks><para>It will prompt the user to confirm, if specified by console configuration</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_consoleQuit()
        {
            Boolean doExit = true;
            if (doAskForQuitConfirmation)
            {
                doExit = aceTerminalInput.askYesNo("Are you sure to quit the [" + application.name + "] application?");
            }
            if (doExit) Environment.Exit(0);
        }

        [Display(GroupName = "console", Name = "Exit", ShortName = "x", Description = "Exits the current console run-loop")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "If this is the main console of the application - it actually does nothing")]
        /// <summary>Exits the current console run-loop</summary>
        /// <remarks><para>If this is the main console of the application - it actually does nothing</para></remarks>
        public void aceOperation_consoleExit()
        {
            consoleIsRunning = false;
        }

        #endregion ------------------- BASIC CONSOLE OPERATIONS --------------------------------------------------------------------------------------------------------------------------

        private List<String> _helpHeader = new List<String>();

        /// <summary> Additional points for console help header text. <see cref="helpContent"/></summary>
        public List<String> helpHeader
        {
            get
            {
                return _helpHeader;
            }
            protected set
            {
                _helpHeader = value;
                OnPropertyChanged("helpHeader");
            }
        }

        private String _helpContent = "";

        /// <summary>
        /// Help text short header, used when console api is rendered or shown
        /// </summary>
        public String helpContent
        {
            get { return _helpContent; }
            set { _helpContent = value; }
        }

        protected String _consoleTitle = "Command Console";

        /// <summary> Console title </summary>
        public virtual String consoleTitle
        {
            get
            {
                return _consoleTitle;
            }
        }

        protected String _consoleHelp = "Type 'help' and press enter to show list of all commands, 'exit' to close the console.";

        /// <summary> Short help message about this console </summary>
        public virtual String consoleHelp
        {
            get
            {
                return _consoleHelp;
            }
        }

        /// <summary>
        /// Reference to <see cref="aceApplicationBase"/>
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public IAceApplicationBase application { get; protected set; }

        /// <summary> Reference to the application's platform to display/use the console outputs </summary>
        public IPlatform platform { get; protected set; }

        private builderForLog _output;

        /// <summary>
        /// The primary console output. By default pointed to the <see cref="platform"/> via <see cref="aceLog.consoleControl"/>
        /// </summary>
        public virtual builderForLog output { get; set; }

        ILogBuilder IAceOperationSetExecutor.output => output;

        /// <summary>
        /// Secondary output of the console. By default it is pointed to the <see cref="platform"/> output, but it can be directed to a file or other outputs.
        /// </summary>
        public builderForLog response { get; set; }

        /// <summary>
        /// Commands of the console
        /// </summary>
        public aceMenu commands { get; protected set; }

        protected aceCommandConsole(String __title = "", IAceComponent component = null, String __helpLine = "")
        {
            if (!__title.isNullOrEmpty()) _consoleTitle = __title;
            if (!__helpLine.isNullOrEmpty()) _consoleHelp = __helpLine;
            init(component);
        }

        protected virtual void init(IAceComponent component)
        {
            commands = new aceMenu();
            commands.setItems(this, component);

            Type consoleType = this.GetType();
            output = new builderForLog(consoleType.Name + "_output", false);
            response = new builderForLog(consoleType.Name + "_response", false);
            consoleIsRunning = true;
        }

        public virtual aceCommandConsoleIOEncode encode { get { return aceCommandConsoleIOEncode.dos; } }

        /// <summary>
        /// command tree, used for help generation
        /// </summary>
        /// <value>
        /// The command set tree.
        /// </value>
        public commandTree commandSetTree
        {
            get
            {
                if (_commandSetTree == null) _commandSetTree = commandTreeTools.BuildCommandTree(this);
                return _commandSetTree;
            }
            set { _commandSetTree = value; }
        }

        public const Int32 PAGINATE_COMMANDS = 10;

        protected String listCommands(Boolean paginate = false)
        {
            output.getLastLine();
            long l1 = output.Length;

            commandSetTree.ReportCommandTree(output, paginate, 0);
            return output.GetContent(l1, output.Length);

            output.AppendLine(consoleTitle);
            output.AppendLine(consoleHelp);

            foreach (String ln in helpHeader)
            {
                output.AppendLine(ln);
            }

            output.AppendHorizontalLine();

            Int32 pIndex = PAGINATE_COMMANDS;

            foreach (aceMenuItem command in commands)
            {
                output.AppendLine(command.key + " : " + command.itemMetaInfo.aliasList.toCsvInLine());
                output.AppendLine("Info:   " + command.helpLine);

                if (command.itemMetaInfo.ContainsKey(aceMenuItemAttributeRole.ExpandedHelp)) output.AppendLine(command.itemMetaInfo[aceMenuItemAttributeRole.ExpandedHelp]);
                //output.nextTabLevel();

                //  response.open("", command.itemName);
                if (paginate) pIndex--;
                Int32 pi = 1;
                String pars = "";
                String example = command.key.ToLower() + " ";
                String parLine = "";
                output.nextTabLevel();
                output.consoleAltColorToggle();
                if (command.itemMetaInfo.cmdParams != null)
                {
                    if (command.itemMetaInfo.cmdParams.Any())
                    {
                        output.AppendLine("Command parameters: ");
                        foreach (typedParam cmdpar in command.itemMetaInfo.cmdParams)
                        {
                            output.Append("" + pi.ToString("D2") + " " + cmdpar.info.name + " : " + cmdpar.info.type.Name + " ");
                            pi++;

                            parLine = parLine.add(cmdpar.getString(false), ";");
                        }
                        example = example + parLine;
                        output.AppendLine("Example : " + example);
                    }
                }
                else
                {
                    output.AppendLine("Command has no parameters");
                }
                output.consoleAltColorToggle();
                output.prevTabLevel();
                //response.close();
                //output.prevTabLevel();
                output.AppendHorizontalLine();

                if (pIndex == 0)
                {
                    pIndex = PAGINATE_COMMANDS;
                    aceTerminalInput.askAnyKeyInTime("Press key to continue to next page", ConsoleKey.Enter, 5, true, 0);
                }
            }

            return output.getLastLine();
        }

        private aceConsoleScript _scriptRunning;

        /// <summary> Reference to the console script that is running or was running last time</summary>
        public aceConsoleScript scriptRunning
        {
            get
            {
                return _scriptRunning;
            }
            protected set
            {
                _scriptRunning = value;
                OnPropertyChanged("lastScriptRun");
            }
        }

        /// <summary>
        /// Executes the command console script. <see cref="aceConsoleScript"/>
        /// </summary>
        /// <param name="script">The console script to execute</param>
        /// <param name="delay">The delay between two instructions in the script, in miliseconds</param>
        public aceConsoleScript executeScript(IAceConsoleScript script, Int32 delay = 10)
        {
            aceConsoleScript parent = scriptRunning as aceConsoleScript;
            scriptRunning = script as aceConsoleScript;

            scriptRunning.Execute(this, parent, delay);
            return scriptRunning;
        }

        private Boolean _consoleIsRunning = true;

        /// <summary> It is <c>true</c> if console is turned on, waiting for an input or performing already given command(s) </summary>
        public Boolean consoleIsRunning
        {
            get
            {
                return _consoleIsRunning;
            }
            protected set
            {
                _consoleIsRunning = value;
            }
        }

        /// <summary>
        /// Clear Screen
        /// </summary>
        public virtual void cls()
        {
            Console.Clear(); // temporary
            // platform.clear(); <--- final
        }

        /// <summary>
        /// Will be executed upon start-up of the console.
        /// </summary>
        public abstract void onStartUp();

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="input">The input.</param>
        public void executeCommand(String input)
        {
            lastInput = input;

            String input_proper = input.Trim().ToLower();
            response.getLastLine(true);

            switch (encode)
            {
                case aceCommandConsoleIOEncode.none:

                    break;

                case aceCommandConsoleIOEncode.dos:
                    input_proper = input_proper.toDosCharacters(toDosCharactersMode.toCleanChars, true);
                    break;

                case aceCommandConsoleIOEncode.dosx:
                    input_proper = input_proper.toDosCharacters(toDosCharactersMode.toCleanAndXChars, true);
                    break;
            }

            aceCommandEntry entry = new aceCommandEntry(this, input);

            if (entry.isEmptyLine) return;

            if (entry.isSyntaxError)
            {
                output.AppendLine("Syntax error for entry: " + input);
                output.AppendLine(entry.errorMessage);
            }
            else
            {
                Boolean doInvoke = true;

                switch (entry.specialFunction)
                {
                    case commandLineSpecialFunction.askForParameters:
                        foreach (var par in entry.marg.paramSet)
                        {
                            throw new NotImplementedException();
                            // par.setValueDirect(aceTerminalInput.AskFor(par.info.sPE, par.value));
                        }
                        //entry.marg.paramSet.

                        break;

                    case commandLineSpecialFunction.helpOnCommand:
                        doInvoke = false;
                        commandSetTree.GetCommands(entry.command).ReportCommands(response);

                        break;

                    case commandLineSpecialFunction.none:
                        break;
                }

                if (entry.invoke())
                {
                    if (saveClipboard) clipboard.clipboardSetText(response.getLastLine());

                    if (!entry.commentedOut)
                    {
                        history.Add(input);
                    }
                }
                else
                {
                    output.AppendLine("Execution error: " + entry.errorMessage);
                    var axe = entry.axe;
                    if (axe != null)
                    {
                        output.AppendLine("Command execution exception: " + axe.title);
                        output.AppendLine("Message: " + axe.Message);
                        output.AppendLine("Source: " + axe.Source);
                        output.AppendLine("File: " + axe.callInfo.Filepath);
                        output.AppendLine("Method: " + axe.callInfo.methodName);
                        output.AppendDataFields(axe.info);

                        if (aceTerminalInput.askAnyKeyInTime("Press ESCAPE (or wait timeout) to throw the exception --- any other key to continue execution", ConsoleKey.Escape, 15, true, 15) == ConsoleKey.Escape)
                        {
                            throw axe;
                        }
                    }
                }
            }
        }

        public Boolean saveClipboard { get; set; } = false;

        private String _linePrefix = "";//G:\imbWBI_Test\projects\itmPlugin\itm01\industryTermModelProject.xml

        /// <summary>
        /// Prefix to show in the command line
        /// </summary>
        public virtual String linePrefix
        {
            get { return _linePrefix; }
            set { _linePrefix = value; }
        }

        private String _lastInput = "help";
        private commandTree _commandSetTree;

        /// <summary>
        /// Last input from the command line
        /// </summary>
        public String lastInput
        {
            get { return _lastInput; }
            set { _lastInput = value; }
        }

        /// <summary>
        /// Commands executed earlier
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public List<String> history { get; set; } = new List<string>();

        IAceConsoleScript IAceCommandConsole.scriptRunning => throw new NotImplementedException();

        protected abstract void doCustomSpecialCall(aceCommandActiveInput input);

        protected void doSpecialCall(aceCommandActiveInput input)
        {
            switch (input.specialCall)
            {
                case ConsoleKey.F1:
                    aceOperation_consoleHelp(aceCommandConsoleHelpOptions.full);
                    break;

                case ConsoleKey.F2:
                    aceOperation_consoleHelp(aceCommandConsoleHelpOptions.parameters);
                    break;

                case ConsoleKey.F3:
                    aceOperation_consoleHelp(aceCommandConsoleHelpOptions.plugins);
                    break;

                case ConsoleKey.F4:
                    var fi = "buffer.txt".getWritableFile(getWritableFileMode.autoRenameThis);
                    saveBase.saveContentOnFilePath(consolePlatformExtensions.GetBuffer(), fi.FullName);
                    output.log("Buffer content saved to: _" + fi.FullName + "_");
                    break;

                case ConsoleKey.F12:
                    break;

                case ConsoleKey.Escape:

                    break;
            }
            doCustomSpecialCall(input);
        }

        public const Boolean useActiveInput = false;

        /// <summary>
        /// Default command that will be executed on empty imput
        /// </summary>
        /// <value>
        /// The default command.
        /// </value>
        public virtual String DefaultCommand
        {
            get
            {
                if (history.Any())
                {
                    return "";
                }
                return "help";
            }
        }

        /// <summary>
        /// Starts the console
        /// </summary>
        public void start(IAceApplicationBase __application)
        {
            application = __application;

            cls();

            helpHeader.Add("Command with default parameters: [command name]");
            helpHeader.Add("For help on a command: [command name] " + aceCommandEntry.PARAM_HELP);
            helpHeader.Add("To be prompted for parameters: [command name] " + aceCommandEntry.PARAM_WILLCARD);
            // helpHeader.Add("[F1] full help | [F2] properties help | [F3] console plugins list | [F5] buffer to file");
            // helpHeader.Add("[DOWN] history back | [UP] history forward | [TAB] confirm proposal | [F12] clear screen");

            aceLog.consoleControl.setAsOutput(output);
            aceLog.consoleControl.setAsOutput(response);

            aceLog.consoleControl.makeSureHaveDifferentColors(output, response);

            // commandSetTree = commandTreeTools.BuildCommandTree(this);

            output.AppendLine(consoleTitle);
            output.AppendLine(consoleHelp);

            Console.Title = consoleTitle;

            commandSetTree.shortCuts.Add("F1", "help option=\"full\"");
            commandSetTree.shortCuts.Add("F12", "help option=\"full\"");

            onStartUp();

            history.Clear();

            while (consoleIsRunning)
            {
                if (useActiveInput)
                {
                    aceCommandActiveInput input = new aceCommandActiveInput(commandSetTree, history, "", "_>_ ");

                    Thread th = new Thread(input.run);
                    th.Start();
                    while (input.active)
                    {
                        Thread.Sleep(1000);
                    }

                    // aceTerminalInput.askForString("Please type console command and press enter.", "help");

                    if (input.specialCall != ConsoleKey.NoName)
                    {
                        executeCommand(input.current);
                    }
                    else
                    {
                        doSpecialCall(input);
                    }
                }
                else
                {
                    String input = aceTerminalInput.askForStringInline(linePrefix.or(" > "), DefaultCommand);
                    executeCommand(input);
                }

                Thread.Sleep(250);
            }

            aceLog.consoleControl.removeFromOutput(output);
            aceLog.consoleControl.removeFromOutput(response);
        }

        IAceConsoleScript IAceCommandConsole.executeScript(IAceConsoleScript script, int delay) => executeScript(script as aceConsoleScript, delay);
    }
}