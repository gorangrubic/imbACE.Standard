// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandTreeTools.cs" company="imbVeles" >
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
    using imbACE.Core.operations;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.collection.nested;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension methods used for Command tree construction
    /// </summary>
    public static class commandTreeTools
    {
        /// <summary>
        /// Builds the command tree.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="includeParentTypes">if set to <c>true</c> it will include only this type.</param>
        /// <returns></returns>
        public static commandTree BuildCommandTree(IAceOperationSetExecutor source, Boolean includeParentTypes = true)
        {
            commandTree rootDesc = new commandTree();
            rootDesc.name = "commands";
            rootDesc.nodeLevel = commandTreeNodeLevel.root;
            rootDesc.description = $"Reference of console commands available at {source.consoleTitle} command line interface.";
            rootDesc.helpLines.Add(source.consoleHelp);
            rootDesc.helpLines.AddRange(source.helpHeader);
            commandTree output = rootDesc;

            List<Type> types = new List<Type>();

            if (includeParentTypes)
            {
                types = source.GetType().GetBaseTypeList(true, true, typeof(aceOperationSetExecutorBase));
            }
            else
            {
                types.Add(source.GetType());
            }

            types.Reverse();

            foreach (Type t in types)
            {
                if (t.IsInterface) continue;
                if (t.IsClass)
                {
                    output.Add(BuildTreeNodeForType(t, source, output), t.Name);
                }
            }

            return output;
        }

        internal static void setByMemberInfo(this commandTreeDescription item, MemberInfo __m)
        {
            settingsMemberInfoEntry mi = new settingsMemberInfoEntry(__m);
            aceMenuItemMeta mm = new aceMenuItemMeta(__m);
            commandTreeDescription desc = item;
            desc.memberMeta = mi;
            desc.menuMeta = mm;

            desc.name = mm.getEntrySafe(aceMenuItemAttributeRole.DisplayName, mi.name);

            desc.category = mm.getEntrySafe(aceMenuItemAttributeRole.Category).or(mi.categoryName, "Main");

            desc.nodeLevel = commandTreeNodeLevel.group;
        }

        /// <summary>
        /// Builds the type of the tree node for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="nameOverride">The name override.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        internal static commandTreeDescription BuildTreeNodeForType(Type type, IAceOperationSetExecutor source, commandTreeDescription parent, String nameOverride = "", commandTreeNodeLevel level = commandTreeNodeLevel.type)
        {
            settingsMemberInfoEntry typeInfo = new settingsMemberInfoEntry(type);

            commandTreeDescription output = parent.Add(nameOverride.or(typeInfo.name, typeInfo.displayName, type.Name));

            commandTree host = parent.root as commandTree;

            output.description = typeInfo.description;
            output.nodeLevel = level;
            output.helpLines.Add(typeInfo.info_helpTitle);
            output.helpLines.Add(typeInfo.info_helpTips);
            output.helpLines.AddRange(typeInfo.additionalInfo);

            var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);

            aceDictionarySet<String, commandTreeDescription> groups = new aceDictionarySet<string, commandTreeDescription>();

            aceDictionarySet<String, commandTreeDescription> group_nodes = new aceDictionarySet<string, commandTreeDescription>();

            foreach (MemberInfo __m in methods)
            {
                if (__m.Name.StartsWith(aceMenuItemMeta.METHOD_PREFIX))
                {
                    commandTreeDescription desc = new commandTreeDescription();

                    desc.setByMemberInfo(__m);
                    desc.nodeLevel = commandTreeNodeLevel.group;
                    groups.Add(desc.category, desc);
                }
            }

            foreach (String group in groups.Keys.OrderBy(x => x))
            {
                var ordered = groups[group].OrderBy(x => x.name);

                commandTreeDescription gdesc = parent.Add(group);

                gdesc.nodeLevel = commandTreeNodeLevel.group;

                foreach (var cdesc in ordered)
                {
                    cdesc.nodeLevel = commandTreeNodeLevel.command;
                    gdesc.Add(cdesc, cdesc.name);

                    host.flatAccess.Add(cdesc.path, cdesc);
                }
            }

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.DeclaringType.Name == "aceCommandConsole") continue;
                if (pi.DeclaringType.Name == "aceAdvancedConsole") continue;

                if (pi.PropertyType.GetInterfaces().Contains(typeof(IAceOperationSetExecutor)))
                {
                    var plugin_instance = source.imbGetPropertySafe(pi) as IAceOperationSetExecutor;
                    var plugin_node = BuildTreeNodeForType(pi.PropertyType, source, parent, pi.Name, commandTreeNodeLevel.plugin);
                    plugin_node.setByMemberInfo(pi);

                    host.plugins.Add(plugin_node.path, plugin_instance);
                }
                else if (pi.PropertyType.IsValueType || pi.PropertyType.isTextOrNumber() || pi.GetCustomAttributes(false).Any())
                {
                    if (pi.CanWrite)
                    {
                        var prop = parent.Add(pi.Name);

                        prop.setByMemberInfo(pi);
                        prop.nodeLevel = commandTreeNodeLevel.parameter;
                        host.properties.Add(prop.path, pi);
                    }
                }
                else if (pi.PropertyType.IsClass || pi.GetCustomAttributes(false).Any())
                {
                    if (!pi.PropertyType.IsGenericType)
                    {
                        var prop = parent.Add(pi.Name);
                        prop.setByMemberInfo(pi);
                        prop.nodeLevel = commandTreeNodeLevel.module;
                        host.modules.Add(prop.path, pi);
                    }
                }
            }

            return output;
        }
    }
}