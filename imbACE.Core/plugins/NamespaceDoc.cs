// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceDoc.cs" company="imbVeles" >
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
using imbACE.Core.plugins.core;
using imbACE.Core.plugins.deployer;
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

namespace imbACE.Core.plugins
{
    using imbACE.Core.application;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// <para>Namespace covers the external plug-in system from both sides of implementation.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is initial version, still to be improved</para>
    /// <list>
    /// 	<listheader>
    ///			<term>Main parts of the external plug-in system</term>
    ///			<description>Relevant for the use of the API and development of your own plugins for a imbACE application</description>
    ///		</listheader>
    ///		<item>
    ///			<term>Manager: <see cref="pluginManager"/></term>
    ///			<description>
    /// <para>It search specified directory for dlls, loads proper (<see cref="IAcePluginBase"/>) types found there and creates plugin instance when asked to.</para>
    /// <para>One instance per plugin directory (having multiple subdirs and kinds of plugins) - the <see cref="IAceApplicationBase"/> already has one and use it, so in the most scenarios only plugin instance creation is used from this class</para>
    /// </description>
    ///		</item>
    ///		<item>
    ///			<term>Deployer: <see cref="acePluginDeployer{T}"/></term>
    ///			<description>Holds collection of plugin instances (of certain type, {T}) for later assignment and/or application</description>
    ///		</item>
    ///		<item>
    ///			<term>Plugin base: <see cref="acePluginBase"/> and <see cref="IAcePluginBase"/></term>
    ///			<description>Are base resources that you use to create your own plugins, for certain application-specific <see cref="acePluginDeployer{T}"/></description>
    ///		</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="acePluginBase" />
    /// <seealso cref="acePluginApplicationContext" />
    /// <seealso cref="pluginManager" />
    [CompilerGenerated]
    //[xmlFilter(targetOp.group, "summary", "remarks")]
    public class NamespaceDoc
    {
    }
}