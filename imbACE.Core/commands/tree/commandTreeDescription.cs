// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandTreeDescription.cs" company="imbVeles" >
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
    using imbACE.Core.commands.menu;
    using imbSCI.Core.data;
    using imbSCI.Data.collection.graph;
    using System;
    using System.Collections.Generic;

    public class commandTreeDescription : graphNode, IObjectWithNameAndDescription
    {
        public commandTreeDescription()
        {
        }

        public void Add(commandTreeDescription item, String __name)
        {
            item.parent = this;
            item.name = __name;
            if (!children.Contains(item.name)) children.Add(item.name, item);
        }

        public commandTreeDescription(String __name, graphNode __parent)
        {
            parent = __parent;
            name = __name;

            commandTreeDescription __p = parent as commandTreeDescription;

            __p.Add(this, __name);
        }

        public commandTreeDescription Add(String __name)
        {
            var output = new commandTreeDescription(__name, this);
            Add(output, __name);
            //children.Add(output.name, output);
            return output;
        }

        public override string pathSeparator
        {
            get
            {
                return ".";
            }
        }

        //private String _name = "";
        ///// <summary>
        ///// Name for this instance
        ///// </summary>
        //public String name
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        private String _description = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public commandTreeNodeLevel nodeLevel { get; set; } = commandTreeNodeLevel.none;

        public List<String> helpLines { get; set; } = new List<string>();

        public aceMenuItemMeta menuMeta { get; set; }

        public settingsMemberInfoEntry memberMeta { get; set; }

        public String category { get; set; }
    }
}