// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandTree.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.tree
{
    using imbSCI.Data.collection.nested;
    using operations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Command tree
    /// </summary>
    /// <seealso cref="imbACE.Core.commands.tree.commandTreeDescription" />
    public class commandTree : commandTreeDescription
    {
        public override string path
        {
            get
            {
                return "";
            }
        }

        public List<commandTreeDescription> GetCommands(String needle)
        {
            List<commandTreeDescription> output = new List<commandTreeDescription>();

            foreach (commandTreeDescription cdesc in flatAccess.Values.Where(x => x.name.StartsWith(needle, StringComparison.InvariantCultureIgnoreCase)))
            {
                output.Add(cdesc);
            }

            if (!output.Any())
            {
                foreach (commandTreeDescription cdesc in flatAccess.Values.Where(x => x.name.Contains(needle)))
                {
                    output.Add(cdesc);
                }
            }

            output.OrderBy(x => x.name.Length);

            return output;
        }

        public aceConcurrentDictionary<String> shortCuts { get; set; } = new aceConcurrentDictionary<string>();

        /// <summary>
        /// Flat dictionary of all commands
        /// </summary>
        /// <value>
        /// The flat access.
        /// </value>
        public aceConcurrentDictionary<commandTreeDescription> flatAccess { get; set; } = new aceConcurrentDictionary<commandTreeDescription>();

        /// <summary>
        /// ValueType properties that may be directly assigned
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public aceConcurrentDictionary<IAceOperationSetExecutor> plugins { get; set; } = new aceConcurrentDictionary<IAceOperationSetExecutor>();

        /// <summary>
        /// ValueType properties that may be directly assigned
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public aceConcurrentDictionary<PropertyInfo> properties { get; set; } = new aceConcurrentDictionary<PropertyInfo>();

        /// <summary>
        /// Class properties that are not <see cref="imbACE.Services.operations.IAceOperationSetExecutor"/>
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        public aceConcurrentDictionary<PropertyInfo> modules { get; set; } = new aceConcurrentDictionary<PropertyInfo>();

        public commandTree()
        {
            name = "commands";
        }
    }
}