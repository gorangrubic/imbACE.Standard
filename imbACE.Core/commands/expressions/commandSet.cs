// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandSet.cs" company="imbVeles" >
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
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Collection of ACE expression command subsystem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class commandSet<T> : Dictionary<string, T> where T : IAceCommand, new()
    {
        public commandSet()
        {
            defaultCommands();
        }

        /// <summary>
        /// Unosi definiciju komande
        /// </summary>
        /// <param name="__shortname"></param>
        /// <param name="__longname"></param>
        /// <param name="__handler"></param>
        /// <param name="__description"></param>
        /// <returns></returns>
        public T Define(String __shortname, String __longname, aceCommandEvent __handler, String __description = "")
        {
            T output = new T();

            output.commandShortName = __shortname;
            output.commandFullname = __longname;
            output.description = __description;

            output.onCommandCalled += __handler;

            Add(output.commandShortName, output);

            return output;
        }

        #region -----------  format  -------  [Format komande]

        private commandExpressionFormat _format = new commandExpressionFormat(); // = new commandExpressionFormat();

        /// <summary>
        /// Format komande
        /// </summary>
        // [XmlIgnore]
        [Category("commandSet")]
        [DisplayName("format")]
        [Description("Format komande")]
        public commandExpressionFormat format
        {
            get
            {
                return _format;
            }
            set
            {
                // Boolean chg = (_format != value);
                _format = value;
                ///OnPropertyChanged("format");
                // if (chg) {}
            }
        }

        #endregion -----------  format  -------  [Format komande]

        /// <summary>
        /// Dodaje osnovne komande u set
        /// </summary>
        public abstract void defaultCommands();

        public void RunExpression(commandExpression expression)
        {
            aceCommandEventArgs args = MakeArguments(expression, this);
            args.command.callCommand(args);
        }

        public aceCommandEventArgs MakeArguments(commandExpression expression, Object __caller)
        {
            IAceCommand command = null;
            if (ContainsKey(expression.identifier))
            {
                command = this[expression.identifier];
            }
            else
            {
                foreach (IAceCommand __c in this.Values)
                {
                    if (__c.commandFullname.ToLower() == expression.identifier.ToLower())
                    {
                        command = __c;
                    }
                }
            }
            if (command != null)
            {
                aceCommandEventArgs args = new aceCommandEventArgs(aceCommandEventType.unknown, __caller, command,
                                                                   expression.parameters[0]);
                return args;
            }
            else
            {
                return null;
            }
        }
    }
}