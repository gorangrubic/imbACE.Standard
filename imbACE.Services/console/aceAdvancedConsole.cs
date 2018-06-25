// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceAdvancedConsole.cs" company="imbVeles" >
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
    using imbACE.Core.commands;
    using imbACE.Core.commands.menu;
    using imbACE.Core.commands.tree;
    using imbACE.Core.extensions.io;
    using imbACE.Core.operations;
    using imbACE.Services.terminal;
    using imbACE.Services.terminal.dialogs;
    using imbACE.Services.terminal.dialogs.core;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data.enums;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    //using System.Windows.Forms;
    //using imbACE.Core.reporting;
    //using imbACE.Core.reporting.render.builders;

    /// <summary>
    /// Advanced command console environment with its workspace and other stuff
    /// </summary>
    /// <seealso cref="aceCommandConsole" />
    public abstract class aceAdvancedConsole<TState, TWorkspace> : aceCommandConsole, IAceAdvancedConsole where TState : aceAdvancedConsoleStateBase, new()
        where TWorkspace : aceAdvancedConsoleWorkspace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceAdvancedConsole{TState, TWorkspace}"/> class.
        /// </summary>
        protected aceAdvancedConsole()
        {
        }

        /// <summary>
        /// Get's executed on console startup
        /// </summary>
        public override void onStartUp()
        {
            state.Poke();
            workspace.Poke();

            if (!state.doRunScriptOnStartup.isNullOrEmpty())
            {
                var autoScript = workspace.loadScript(state.doRunScriptOnStartup);
                log("Autoexecution script: " + autoScript.info.Name + " starting");
                executeScript(autoScript);
            }
            else
            {
                log("No auto execution script set");
            }
        }

        /// <summary>
        /// Logs the specified message, using the other color if required
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="otherColor">if set to <c>true</c> [other color].</param>
        public void log(String message, Boolean otherColor = false)
        {
            if (otherColor) output.consoleAltColorToggle();
            output.log(message);
            if (otherColor) output.consoleAltColorToggle();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public Boolean isReady
        {
            get
            {
                if (_workspace == null) return false;
                if (_state == null) return false;

                return true;
            }
        }

        protected TWorkspace _workspace; // = "";

        public abstract TWorkspace workspace { get; }

        protected TState _state; // = "";

                                 /// <summary>
                                 /// Bindable property
                                 /// </summary>
        public virtual TState state
        {
            get
            {
                if (_state == null)
                {
                    _state = new TState();
                    _state.Load();
                }
                return _state;
            }
        }

        private settingsEntriesForObject _stateInfo; // = "";

                                                     /// <summary>
                                                     /// Bindable property
                                                     /// </summary>
        public settingsEntriesForObject stateInfo
        {
            get
            {
                if (_stateInfo == null)
                {
                    _stateInfo = new settingsEntriesForObject(state);
                }

                return _stateInfo;
            }
        }

        private Object _SelectedObject;

        /// <summary>
        ///
        /// </summary>
        public Object SelectedObject
        {
            get
            {
                if (_SelectedObject == null) _SelectedObject = state;
                return _SelectedObject;
            }
            set { _SelectedObject = value; }
        }

        public Object GetScope()
        {
            if (state.scopePath.isNullOrEmpty()) return state;

            if (state.scopePath.Length > 2)
            {
                return state.imbGetPropertySafe(state.scopePath, state, ".");
            }
            return state;
        }

        /// <summary>
        /// </summary>
        public override string linePrefix
        {
            get
            {
                return state.scopePath.add(base.linePrefix, " ");
            }
        }

        aceAdvancedConsoleStateBase IAceAdvancedConsole.state => state;

        aceAdvancedConsoleWorkspace IAceAdvancedConsole.workspace => workspace;

        [Display(GroupName = "script", Name = "Process", ShortName = "", Description = "Processing script into selected form")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will load the specified script and process it into selected format")]
        /// <summary>Processing script into selected form</summary>
        /// <remarks><para>It will load the specified script and process it into selected format</para></remarks>
        /// <param name="script">Filename of script file to process</param>
        /// <param name="format">Format to process script into</param>
        /// <param name="askForFormat">Prompt user to choose the format</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_scriptProcess(
            [Description("Filename of script file to process")] String script = "script.ace",
            [Description("Format to process script into")] commandLineFormat format = commandLineFormat.explicitFormat,
            [Description("Prompt user to choose the format")] Boolean askForFormat = true)
        {
            if (script == "*")
            {
                var list = workspace.getScriptList();
                script = aceTerminalInput.askForOption("Select script file to load", list.First(), list, null).toStringSafe();
            }

            if (askForFormat)
            {
                format = (commandLineFormat)aceTerminalInput.askForOption("Choose command format to be applied on the specified ACE script.", format);
            }

            var ace_script = workspace.loadScript(script);

            String scriptName = aceTerminalInput.askForString("Please enter filename for reformatted script:", ace_script.info.Name);

            var newScript = ace_script.GetScriptInForm(this, format, workspace.folder[aceCCFolders.scripts].pathFor(scriptName));
        }

        [Display(GroupName = "project", Name = "Reset", ShortName = "", Description = "Creating blank work folder (job/project) after saving the current")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will ask you for new job/state name and save the current state before cleaning the memory. If _autorename_ is true it will make new name for new state if the specified one is already taken.")]
        /// <summary>
        /// Creating blank work folder (job/project) after saving the current
        /// </summary>
        /// <param name="name">The name for new project</param>
        /// <remarks>
        /// It will ask you for new job/state name and save the current state before cleaning the memory. If _autorename_ is true it will make new name for new state if the specified one is already taken.
        /// </remarks>
        /// <seealso cref="aceOperationSetExecutorBase" />
        public void aceOperation_projectReset(
            [Description("--")] String name = "newproject")
        {
            name = workspace.getNewProjectName(name);

            state.currentProjectName = name;
            state.Save();

            workspace.deployWorkspace();
            log("New workspace ready [" + workspace.projectRootPath + "].");
        }

        [Display(GroupName = "script", Name = "Execute", ShortName = "exe", Description = "Basic automation facility: reads lines from the script file and executes it as it was typed by the console user. If filename parameter is * it will ask user to select script to load.")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It opens the specified script (.ace) file from the scripts folder and performs commands from the script")]
        /// <summary>
        /// Basic automation facility: reads lines from the script file and executes it as it was typed by the console user. If filename parameter is * it will ask user to select script to load.
        /// </summary>
        /// <param name="filename">Filename for script to execute</param>
        /// <param name="delay">Delay milliseconds between execution of each line/commands</param>
        /// <param name="repeat">Number of times to repeat the script</param>
        /// <param name="askConfirmation">The ask confirmation.</param>
        /// <remarks>
        /// It opens the specified script (.ace) file from the scripts folder and performs commands from the script
        /// </remarks>
        /// <seealso cref="aceOperationSetExecutorBase" />
        public void aceOperation_scriptExecute(
            [Description("Filename for script to execute")] String filename = "script.ace",
            [Description("Delay milliseconds between execution of each line/commands")] Int32 delay = 5,
            [Description("Number of times to repeat the script")] Int32 repeat = 1,
            [Description("Text of the Yes/No confirmation box, if left blank it will not ask user to confirm script execution")] String askConfirmation = "")
        {
            if (filename == "*")
            {
                var list = workspace.getScriptList();
                filename = aceTerminalInput.askForOption("Select script file to load", list.First(), list, null).toStringSafe();
            }

            var script = workspace.loadScript(filename);

            log("Script [" + filename + "] with [" + script.Count() + "] will execute [" + repeat + "] time/s.");

            Boolean ok = true;
            if (!askConfirmation.isNullOrEmpty())
            {
                ok = false;
                var selected = imbACE.Services.terminal.dialogs.dialogs.openDialogWithOptions<String>(new String[] { "Confirm", "Cancel" }, askConfirmation, "Please confirm script [" + filename + "] execution.", dialogStyle.redDialog, dialogSize.mediumBox);

                if (selected == "Confirm")
                {
                    ok = true;
                }
            }
            if (!ok)
            {
                log("Script [" + filename + "] execution canceled by user");
                return;
            }
            while (repeat > 0)
            {
                executeScript(script, delay);
                repeat--;
            }

            log("Script [" + filename + "] executed.");
        }

        [Display(GroupName = "help", Name = "ExportHelp", ShortName = "", Description = "Exports help file into current state project folder")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "Writes a txt file with content equal to the result of help command")]
        /// <summary>
        /// Exports help file into current state project folder
        /// </summary>
        /// <param name="filename">help.txt</param>
        /// <param name="open">true</param>
        /// <param name="onlyThisConsole">if set to <c>true</c> [only this console].</param>
        /// <remarks>
        /// Writes a txt file with content equal to the result of help command
        /// </remarks>
        /// <seealso cref="aceOperationSetExecutorBase" />
        public void aceOperation_helpExportHelp(
            [Description("help.txt")] String filename = "help.txt",
            [Description("true")] Boolean open = true,
            [Description("If true it will generate user manual only for this console")] Boolean onlyThisConsole = false)
        {
            builderForMarkdown mdBuilder = new builderForMarkdown();

            if (onlyThisConsole)
            {
                var cst = commandTreeTools.BuildCommandTree(this, false);
                cst.ReportCommandTree(mdBuilder, false, 0, aceCommandConsoleHelpOptions.full);
                helpContent = mdBuilder.getLastLine();
            }
            else
            {
                commandSetTree.ReportCommandTree(mdBuilder, false, 0, aceCommandConsoleHelpOptions.full);
                helpContent = mdBuilder.GetContent();
            }

            String p = workspace.folder.pathFor(filename);
            if (p.saveToFile(helpContent))
            {
                response.log("Help file saved to: " + p);
            }
            if (open) externalTool.notepadpp.run(p);
        }

        [Display(GroupName = "script", Name = "Template", ShortName = "", Description = "Uses template script file to dynamically create customized execution script")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It loads specified template script file and applies provided parameters to the {n} template placeholders")]
        /// <summary>Uses template script file to dynamically create customized execution script</summary>
        /// <remarks><para>It loads specified template script file and applies provided parameters to the {n} template placeholders</para></remarks>
        /// <param name="templateName">Name of template file</param>
        /// <param name="parameters">Comma separated values for parameters</param>
        /// <param name="saveScript">true</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_scriptTemplate(
            [Description("Name of template file")] String templateName = "word",
            [Description("Comma separated values for parameters")] String parameters = "2,SM-LSD,1,preloadLexicon",
            [Description("true")] Boolean saveScript = true)
        {
            String[] pars = parameters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            aceConsoleScript script = workspace.loadScript(templateName);
            aceConsoleScript scriptInstance = script.DeployTemplate(pars);

            if (saveScript) scriptInstance.Save();

            if (scriptInstance.isReady)
            {
                executeScript(scriptInstance);
            }
            else
            {
                output.log("Script instance [" + scriptInstance.info.Name + "] creation from template script [" + script.info.Name + "] failed to construct. Check number of parameters!");
            }
        }
    }
}