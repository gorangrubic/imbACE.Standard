// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommandEntry.cs" company="imbVeles" >
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
    using imbACE.Core.commands;
    using imbACE.Core.commands.menu;
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.commands.tree;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.operations;
    using imbSCI.Core.extensions.data;
    using System;
    using System.Linq;

    //using imbACE.Core.extensions.text;

    public class aceCommandEntry : commandLineEntry
    {
        private aceMenuItem _operation;

        /// <summary> </summary>
        public aceMenuItem operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }

        private IAceOperationSetExecutor _console;

        /// <summary> </summary>
        public IAceOperationSetExecutor console
        {
            get
            {
                return _console;
            }
            protected set
            {
                _console = value;
            }
        }

        public aceCommandEntry(IAceCommandConsole __console, string input) : base(input)
        {
            console = __console as aceCommandConsole;

            validate();
        }

        /// <summary>
        /// Returns the string form of the command with current parameters
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String GetScriptLine(commandLineFormat __format = commandLineFormat.explicitFormat)
        {
            String output = "";

            if (hasCommand)
            {
                switch (__format)
                {
                    case commandLineFormat.emptyLine:
                        output = "";
                        break;

                    case commandLineFormat.explicitFormat:
                        if (carg.paramSet != null)
                        {
                            output = command.add(carg.paramSet.ToString(false, true, true), " ");
                        }
                        else
                        {
                            output = command;
                        }

                        break;

                    case commandLineFormat.implicitFormat:

                        if (carg.paramSet != null)
                        {
                            output = command.add(carg.paramSet.ToString(false, true, false), " ");
                        }
                        else
                        {
                            output = command;
                        }

                        break;

                    case commandLineFormat.onlyCommand:
                        output = command;
                        break;

                    case commandLineFormat.onlyComment:
                        output = commentPrefix + " " + commentLine;
                        break;

                    case commandLineFormat.unknown:
                        output = inputLine;
                        break;
                }
                if (hasComment) output = output.add(commentLine, " " + commentPrefix);
            }
            else
            {
                switch (__format)
                {
                    case commandLineFormat.emptyLine:
                        output = "";
                        break;

                    case commandLineFormat.onlyComment:
                        output = inputLine;
                        break;

                    default:
                    case commandLineFormat.unknown:
                        output = inputLine;
                        break;
                }
            }

            if (hasPrefix)
            {
                output = prefix.toCsvInLine(COMMANDPREFIX_SEPARATOR).add(output, ".");
            }

            return output;
        }

        public aceOperationArgs marg { get; set; }
        public aceOperationArgs carg { get; set; }

        public aceGeneralException axe { get; protected set; }

        /// <summary>
        /// Invokes the command
        /// </summary>
        /// <returns></returns>
        public Boolean invoke()
        {
            if (hasComment)
            {
                console.output.AppendLine("Comment: " + commentLine);
                return true;
            }
            if (!hasCommand)
            {
                errorMessage = "Command line has no command and no comment recognized";
                return false;
            }

            Object[] array = carg.getInvokeArray();

            if (marg.method != null)
            {
                try
                {
                    marg.method.Invoke(marg.executor, array);
                    return true;
                }
                catch (Exception ex)
                {
                    axe = new aceGeneralException("Command " + command + " execution error", ex, console, console.consoleTitle + " execution of " + command + " error");
                    errorMessage = axe.Message;

                    return false;
                }
            }
            else
            {
                errorMessage = "Method not assigned";
                return false;
            }
            return false;
        }

        protected void validate()
        {
            if (hasCommand)
            {
                IAceOperationSetExecutor executor = console;

                if (hasPrefix)
                {
                    String __prefix = prefix.toCsvInLine(COMMANDPREFIX_SEPARATOR);

                    executor = console.imbGetPropertySafe(__prefix, console) as IAceOperationSetExecutor;

                    if (executor == null)
                    {
                        if (console is aceCommandConsole)
                        {
                            aceCommandConsole aceConsole = console as aceCommandConsole;
                            __prefix = __prefix.removeStartsWith(aceConsole.commandSetTree.name + ".");

                            executor = console.imbGetPropertySafe(__prefix, console) as IAceOperationSetExecutor;

                            if (executor == null)
                            {
                                var coms = aceConsole.commandSetTree.GetCommands(__prefix);

                                if (coms.Any())
                                {
                                    var com = coms.FirstOrDefault(x => (x.name == command || x.menuMeta.aliasList.Contains(command)));

                                    if (com != null)
                                    {
                                        commandTreeDescription node = com.parent as commandTreeDescription;
                                        if (node != null)
                                        {
                                            switch (node.nodeLevel)
                                            {
                                                case commandTreeNodeLevel.group:

                                                    break;

                                                case commandTreeNodeLevel.plugin:

                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isSyntaxError = true;
                                        errorMessage = "Command not found at [" + prefix.toCsvInLine(COMMANDPREFIX_SEPARATOR) + "][" + console.consoleTitle + "]. Suggestions:";
                                        foreach (var c in coms)
                                        {
                                            errorMessage = errorMessage.addLine(c.path);
                                        }
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }

                if (isSyntaxError || (executor == null))
                {
                    isSyntaxError = true;
                    errorMessage = "Unknown property prefix [" + prefix.toCsvInLine(COMMANDPREFIX_SEPARATOR) + "] at [" + console.consoleTitle + "].";
                }
                else
                {
                    operation = executor.commands.getItem(command, -1, false, true);
                }

                if (operation == null)
                {
                    isSyntaxError = true;
                    errorMessage = "Unknown command [" + command + "] for console [" + console.consoleTitle + "]. Type 'help' to show list of commands.";
                }
                else
                {
                    if (operation.metaObject is aceOperationArgs)
                    {
                        marg = operation.metaObject as aceOperationArgs;
                        carg = marg.Clone() as aceOperationArgs;
                        String error = "";
                        switch (format)
                        {
                            case commandLineFormat.explicitFormat:
                                error = carg.paramSet.setValues(parameterName, parameterValue);
                                break;

                            case commandLineFormat.implicitFormat:
                                error = carg.paramSet.setValues(parameterValue);
                                break;

                            case commandLineFormat.onlyCommand:
                                break;
                        }
                    }
                    else
                    {
                        isSyntaxError = true;
                        errorMessage = "Command [" + command + "] for console [" + console.consoleTitle + "] not supported. Type 'help' to show list of commands.";
                    }
                }
            }
        }
    }
}