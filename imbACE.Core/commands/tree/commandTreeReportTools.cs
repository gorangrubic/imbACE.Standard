// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandTreeReportTools.cs" company="imbVeles" >
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

//using imbACE.Core.extensions.text;

namespace imbACE.Core.commands.tree
{
    using imbACE.Core.commands.menu;
    using imbACE.Core.operations;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.render;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    [Flags]
    public enum aceCommandConsoleHelpOptions
    {
        none = 0,
        brief = 1,
        parameters = 2,
        commands = 4,
        modules = 8,
        plugins = 16,
        full = brief | parameters | commands | modules | plugins,
    }

    public static class commandTreeReportTools
    {
        public static void ReportCommands(this IEnumerable<commandTreeDescription> items, ITextRender output)
        {
            foreach (commandTreeDescription item in items)
            {
                ReportCommandDesc(item, output);
            }
        }

        public static void ReportCommandDesc(this commandTreeDescription item, ITextRender output, String cTitlePrefix = "")
        {
            String cTitle = cTitlePrefix;

            cTitle = cTitle.add(item.name, ".").Trim('.');
            //node.item.menuMeta[aceMenuItemAttributeRole.Category]
            //  cTitle = node.item.menuMeta.getEntrySafe(aceMenuItemAttributeRole.Category).add(cTitle, ".");

            String k = item.menuMeta.getEntrySafe(aceMenuItemAttributeRole.Key);
            if (!k.isNullOrEmpty())
            {
                if (k.Length < 5)
                {
                    cTitle = cTitle.add(" [" + k + "]");
                }
            }
            //node.item.menuMeta.getp
            output.open("div", cTitle, item.description);
            //output.AppendPair("Caption", node.item.name, true, ": ");
            foreach (var pair in item.menuMeta)
            {
                if (imbSciStringExtensions.isNullOrEmpty(pair.Value))
                {
                }
                else
                {
                    switch (pair.Key)
                    {
                        case aceMenuItemAttributeRole.aliasNames:
                            output.AppendPair("Alias", pair.Value, true, ": ");
                            break;

                        case aceMenuItemAttributeRole.Description:
                        case aceMenuItemAttributeRole.ExpandedHelp:
                            output.AppendComment(pair.Value);
                            break;

                        case aceMenuItemAttributeRole.Key:
                            // output.AppendPair("Shortcut / key", pair.Value, true, ": ");
                            break;

                        case aceMenuItemAttributeRole.Category:
                        case aceMenuItemAttributeRole.DisplayName:
                        case aceMenuItemAttributeRole.CmdParamList:
                            break;

                        default:
                            output.AppendPair(pair.Key.ToString(), pair.Value, true, ": ");
                            break;
                    }
                }
            }

            String example = item.name + " ";
            if (item.menuMeta.cmdParams != null)
            {
                if (item.menuMeta.cmdParams.Any())
                {
                    output.AppendLabel("Command arguments: ");
                    String parLine = "";
                    Int32 pi = 1;

                    List<String> prl = new List<string>();
                    parLine = item.menuMeta.cmdParams.ToString(false, true, true);

                    //foreach (typedParam cmdpar in node.item.menuMeta.cmdParams)
                    //{
                    //    parLine = parLine.add(cmdpar.getString(false), ";");
                    //}

                    output.AppendTable(item.menuMeta.cmdParams.getParameterTable());

                    example = example + parLine;
                }
            }

            output.AppendLabel("Example : ");
            output.AppendCode(example);

            output.close();
        }

        public static void ReportCommandNode(this commandTreeDescription node, ITextRender output, Boolean paginate, Int32 lastPageLine = 0)
        {
            // output.open("div", node.item.name, node.item.description);
            if (lastPageLine == 0) lastPageLine = Convert.ToInt32(output.Length);

            switch (node.nodeLevel)
            {
                case commandTreeNodeLevel.parameter:
                    output.AppendPair(node.path.Trim('.'), node.memberMeta.relevantTypeName);

                    break;

                case commandTreeNodeLevel.module:
                    output.AppendPair(node.path.Trim('.'), node.memberMeta.relevantTypeName);
                    foreach (commandTreeDescription snode in node)
                    {
                        snode.ReportCommandNode(output, paginate, lastPageLine);
                    }
                    break;

                case commandTreeNodeLevel.group:
                    output.open("div", "Group: " + node.path.Trim('.'), node.description);

                    foreach (commandTreeDescription snode in node)
                    {
                        snode.ReportCommandNode(output, paginate, lastPageLine);
                    }

                    output.close();
                    output.AppendHorizontalLine();
                    break;

                case commandTreeNodeLevel.plugin:
                    output.open("div", "Plugin: " + node.path.Trim('.'), node.description);

                    foreach (commandTreeDescription tnode in node)
                    {
                        node.ReportCommandNode(output, paginate, lastPageLine);
                    }
                    output.close();
                    output.AppendHorizontalLine();
                    break;

                case commandTreeNodeLevel.type:
                    output.open("div", "Console: " + node.path.Trim('.'), node.description);

                    foreach (commandTreeDescription tnode in node)
                    {
                        node.ReportCommandNode(output, paginate, lastPageLine);
                    }
                    output.close();
                    output.AppendHorizontalLine();
                    break;

                case commandTreeNodeLevel.command:

                    String cTitle = "";

                    if (node.parent != null)
                    {
                        cTitle = node.parent.path + " [" + node.name.toStringSafe() + "]";
                    }

                    ReportCommandDesc(node, output, cTitle);

                    if (paginate)
                    {
                        if ((lastPageLine - output.lastLength) >= output.zone.innerBoxedHeight)
                        {
                            lastPageLine = (int)output.lastLength;
                            Paginate();
                        }
                    }
                    break;
            }

            if (node.helpLines.Any())
            {
                output.open("div", "Additional help:");
                foreach (String ln in node.helpLines)
                {
                    output.AppendLine(ln);
                }
                output.close();
            }

            if (paginate)
            {
                if ((lastPageLine - output.lastLength) >= output.zone.innerBoxedHeight)
                {
                    lastPageLine = (int)output.lastLength;
                    Paginate();
                }
            }
        }

        public static void Paginate()
        {
            throw new NotImplementedException();
            //aceTerminalInput.askAnyKeyInTime("Press key to continue to next page", ConsoleKey.Enter, 5, true, 0);
        }

        /// <summary>
        /// Reports the command tree.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="output">The output.</param>
        public static void ReportCommandTree(this commandTree tree, ITextRender output, Boolean paginate, Int32 lastPageLine = 0, aceCommandConsoleHelpOptions option = aceCommandConsoleHelpOptions.full)
        {
            output.AppendHeading(tree.name.ToUpper(), 1);

            output.AppendParagraph(tree.description);

            if (tree.helpLines.Any())
            {
                output.AppendHeading("Description", 2);
                foreach (String ln in tree.helpLines)
                {
                    output.AppendLine(ln);
                }
            }

            if (option.HasFlag(aceCommandConsoleHelpOptions.parameters))
            {
                output.AppendHeading("Properties", 2);

                foreach (var pair in tree.properties)
                {
                    output.AppendPair(pair.Value.Name, pair.Value.PropertyType.Name, true, ":");
                }

                output.AppendHorizontalLine();
            }

            if (option.HasFlag(aceCommandConsoleHelpOptions.plugins))
            {
                output.AppendHeading("Plugins", 2);

                foreach (var pair in tree.plugins)
                {
                    output.AppendPair(pair.Key.Trim('.'), pair.Value.GetType().Name, true, ":");

                    var methods = pair.Value.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
                    if (Enumerable.Any<MethodInfo>(methods))
                    {
                        List<String> lst = new List<string>();
                        foreach (MemberInfo mInfo in Enumerable.Where<MethodInfo>(methods, x => x.Name.StartsWith(aceMenuItemMeta.METHOD_PREFIX)))
                        {
                            lst.Add(pair.Key.add(mInfo.Name.removeStartsWith(aceMenuItemMeta.METHOD_PREFIX), ".").Trim('.'));
                        }

                        output.AppendList(lst);
                    }
                }

                output.AppendHorizontalLine();
            }

            if (option.HasFlag(aceCommandConsoleHelpOptions.modules))
            {
                output.AppendHeading("Modules", 2);

                foreach (var pair in tree.modules)
                {
                    output.AppendPair(pair.Value.Name, pair.Value.PropertyType.Name, true, ":");
                }

                output.AppendHorizontalLine();
            }

            //if (option.HasFlag(aceCommandConsoleHelpOptions.brief))
            //{
            //    output.AppendHeading("Overview", 2);

            //    foreach (var pair in tree.flatAccess)
            //    {
            //        output.AppendPair(pair.Value.path.Trim('.'), pair.Value.menuMeta.cmdParams.ToString(false, true, true), true, " ");
            //    }

            //    output.AppendHorizontalLine();
            //}

            if (option.HasFlag(aceCommandConsoleHelpOptions.commands))
            {
                foreach (commandTreeDescription node in tree)
                {
                    node.ReportCommandNode(output, paginate, lastPageLine);
                }
            }
        }
    }
}