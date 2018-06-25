// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandlineArguments.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.expressions
{
    using System;

    public class commandlineArguments : commandSet<commandlineCommand>
    {
        public commandlineArguments()
        {
        }

        public void callCommandLineArguments(String[] arguments = null)
        {
            if (arguments == null) arguments = Environment.GetCommandLineArgs();

            commandExpression currentExpression = null;
            commandExpressionCollection expressions = new commandExpressionCollection();
            foreach (String arg in arguments)
            {
                if (arg.StartsWith(format.commandPrefixString))
                {
                    currentExpression = new commandExpression(arg);
                    expressions.Add(currentExpression);
                }
                else
                {
                    currentExpression.parameters[currentExpression.parameters.Length] = arg.Trim();
                }
            }
            foreach (commandExpression exp in expressions)
            {
            }
        }

        /// <summary>
        /// Izbacuje u log help instrukcije
        /// </summary>
        /// <param name="_command"></param>
        /// <param name="_args"></param>
        public void onHelpCalled(IAceCommand _command, aceCommandEventArgs _args)
        {
            throw new NotImplementedException();
            //aceCommons.log("HELP :: Command line arguments");
            //foreach (var __comm in this.Values)
            //{
            //    aceCommons.log("   -" + __comm.commandShortName + "  : " + __comm.commandFullname);
            //    aceCommons.log("    " + __comm.description);
            //}
        }

        public void onMenuCalled(IAceCommand _command, aceCommandEventArgs _args)
        {
            IAceLogable logable = _args.logableCaller;
            //if (logable == null) logable = aceCommons.terminal;

            //logable.log("Command menu:");
            //foreach (var __comm in this.Values)
            //{
            //    logable.log("   -" + __comm.commandShortName + "  (" + __comm.commandFullname + ") : " + __comm.description);
            //}

            //IAceLogableWithInput logableInput = logable as IAceLogableWithInput;
            //String selectedCommand = "";
            //if (logableInput == null)
            //{
            //    selectedCommand = Console.ReadLine();
            //} else
            //{
            //    selectedCommand = logableInput.getInputLine();
            //}

            //commandExpression expression = new commandExpression(selectedCommand, format);

            //aceCommandEventArgs args = MakeArguments(expression, this);
            //args.command.callCommand(args);
        }

        public override void defaultCommands()
        {
            Define("h", "help", onHelpCalled, "Showing help output");
            Define("m", "menu", onHelpCalled, "Show all options and wait for input");
        }
    }
}