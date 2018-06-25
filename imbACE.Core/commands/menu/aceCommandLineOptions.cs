// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommandLineOptions.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.menu
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.operations;
    using imbSCI.Core.files.job;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Osnova za set command line opcija [NOT USED]
    /// </summary>
    public class aceCommandLineOptions : aceMenu
    {
        public void processArgs(string[] args)
        {
            aceOperationArgs opArgs = null;

            List<aceOperationArgs> operations = new List<aceOperationArgs>();

            foreach (String str in args)
            {
                if (str.StartsWith("-"))
                {
                    if (opArgs != null)
                    {
                        operations.Add(opArgs);
                        opArgs = null;
                    }

                    String itemKey = str.removeStartsWith("-");

                    var itm = getItem(itemKey, -1, true);

                    opArgs = itm.metaObject as aceOperationArgs;
                }

                if (opArgs != null)
                {
                    if (opArgs.item == selected)
                    {
                    }
                    //opArgs = selected.metaObject as aceOperationArgs;

                    if (!str.StartsWith("-"))
                    {
                        opArgs.paramSet.addFromString(str);
                    }
                }
            }

            foreach (var op in operations)
            {
                op.method.Invoke(op.executor, new Object[] { op });
            }
        }

        /// <summary>
        /// Menu command line
        /// </summary>
        /// <param name="__component"></param>
        /// <param name="__cmdLineOps"></param>
        public aceCommandLineOptions(IAceComponent __component, params aceOperationSetExecutorBase[] __cmdLineOps)
        {
            // imbACE.Core.extensions.imbEnumExtendBase.getEnumList(typeof (aceCommandLineCommonOptions));.

            new NotImplementedException();

            // setItems(aceCommons.commandLineCommonOps, __component);

            if (__cmdLineOps != null)
            {
                foreach (aceOperationSetExecutorBase __ops in __cmdLineOps)
                {
                    setItems(__ops, __component);
                }
            }

            defaultOption = -1;
        }
    }
}