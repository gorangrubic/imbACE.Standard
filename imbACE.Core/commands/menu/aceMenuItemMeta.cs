// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceMenuItemMeta.cs" company="imbVeles" >
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
    using imbACE.Core.operations;

    using imbACE.Core.operations;

    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.syntax.param;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class aceMenuItemMeta : Dictionary<aceMenuItemAttributeRole, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceMenuItemMeta"/> class.
        /// </summary>
        public aceMenuItemMeta()
        {
        }

        public aceMenuItemMeta(String option)
        {
            this[aceMenuItemAttributeRole.DisplayName] = option;
        }

        public aceMenuItemMeta(Enum enumeration)
        {
            Type t = enumeration.GetType();
            MemberInfo mi = t.findEnumerationMember(enumeration.ToString(), 0);
            deployFromMemberInfo(mi);

            //t.getEm
        }

        public aceMenuItemMeta(MemberInfo memberInfo)
        {
            deployFromMemberInfo(memberInfo);
        }

        /// <summary>
        /// alias names - alternativna imena ovog itema
        /// </summary>
        private List<String> _aliasList = new List<string>();

        /// <summary>
        /// alias names - alternativna imena ovog itema
        /// </summary>
        public List<String> aliasList
        {
            get
            {
                return _aliasList;
            }
        }

        /// <summary>
        /// Expected parameters
        /// </summary>
        private typedParamCollection _cmdParams;

        /// <summary>
        /// Expected parameters
        /// </summary>
        public typedParamCollection cmdParams
        {
            get { return _cmdParams; }
        }

        /// <summary>
        /// If the method has its own parameters and not using <see cref="imbACE.Services.operations.aceOperationArgs"/>
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is method with parameters; otherwise, <c>false</c>.
        /// </value>
        public Boolean isMethodWithParameters { get; set; } = false;

        public const String METHOD_PREFIX = "aceOperation_";

        /// <summary>
        /// Deploys <c>cmdParams</c> and other information from member information.
        /// </summary>
        /// <param name="mi">The mi.</param>
        protected void deployFromMemberInfo(MemberInfo mi)
        {
            this.autoDefaultValues("", true);

            settingsMemberInfoEntry mie = new settingsMemberInfoEntry(mi);
            String methodName = mi.Name.removeStartsWith(METHOD_PREFIX);
            String displayName = methodName;
            this[aceMenuItemAttributeRole.Category] = mie.categoryName;

            if (!mie.categoryName.isNullOrEmpty()) displayName = methodName.removeStartsWith(mie.categoryName);

            this[aceMenuItemAttributeRole.Description] = mie.description;
            this[aceMenuItemAttributeRole.Category] = mie.categoryName;
            this[aceMenuItemAttributeRole.DisplayName] = displayName;
            this[aceMenuItemAttributeRole.Key] = mie.letter;
            if (!mie.prompt.isNullOrEmpty())
            {
                this[aceMenuItemAttributeRole.ConfirmMessage] = mie.prompt;
            }

            var ma = mi.GetCustomAttributes(typeof(aceMenuItemAttribute), false);

            foreach (aceMenuItemAttribute amia in ma)
            {
                this[amia.role] = amia.setting;
            }

            if (!String.IsNullOrEmpty(this[aceMenuItemAttributeRole.aliasNames]))
            {
                var als = this[aceMenuItemAttributeRole.aliasNames].Split(';');
                aliasList.AddRange(als);
            }

            //aliasList.AddUnique(displayName);
            aliasList.AddUnique(methodName);

            if (!String.IsNullOrEmpty(this[aceMenuItemAttributeRole.CmdParamList]))
            {
                var tmp = this[aceMenuItemAttributeRole.CmdParamList];
                _cmdParams = new typedParamCollection(tmp);
            }
            else
            {
                if (mi is MethodInfo)
                {
                    MethodInfo mInfo = mi as MethodInfo;
                    var parms = mInfo.GetParameters();
                    if (parms.count() > 0)
                    {
                        if (parms[0].ParameterType != typeof(aceOperationArgs))
                        {
                            isMethodWithParameters = true;
                            _cmdParams = new typedParamCollection(mi as MethodInfo);
                        }
                    }
                }
                // <-------------- direct method support>>> methods with normal arguments, not aceOperationArgs >>>
            }

            if (_cmdParams == null) _cmdParams = new typedParamCollection();

            if (String.IsNullOrEmpty(this[aceMenuItemAttributeRole.DisplayName]))
            {
                this[aceMenuItemAttributeRole.DisplayName] = mi.Name;
            }
            //t.GetCustomAttributes(false);
        }
    }
}