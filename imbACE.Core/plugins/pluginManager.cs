// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pluginManager.cs" company="imbVeles" >
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
using imbACE.Core.application;
using imbACE.Core.operations;
using imbACE.Core.plugins.core;
using imbACE.Core.plugins.deployer;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.collection.nested;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbACE.Core.plugins
{
    /// <summary>
    /// Loads plugin dlls from the associated directory and creates plugin instances on request
    /// </summary>
    public sealed class pluginManager : pluginManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="pluginManager"/> class.
        /// </summary>
        /// <param name="__folderToLoad">The folder to load.</param>
        public pluginManager(folderNode __folderToLoad)
        {
            folderWithPlugins = __folderToLoad;
        }

        /// <summary>
        /// List of DLL files detected
        /// </summary>
        /// <value>
        /// The DLL file names.
        /// </value>
        public List<String> dllFileNames { get; protected set; } = new List<string>();

        protected override bool supportDirtyNaming { get { return false; } }

        /// <summary>
        /// Gets a new instance of plug-in, specified by type name of sub directory.name path
        /// </summary>
        /// <param name="pluginName">Name of the plugin.</param>
        /// <param name="console">The console.</param>
        /// <param name="deployer">The deployer.</param>
        /// <param name="application">The application.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public IAcePluginBase GetPluginInstance(String pluginName, IAceCommandConsole console, IAcePluginDeployerBase deployer, IAceApplicationBase application, ILogBuilder output = null)
        {
            if (output == null)
            {
                if (console != null) output = console.output;
            }

            Type resolution = resolvePlugin(pluginName, output);

            if (resolution == null) return null;

            IAcePluginBase plugin = Activator.CreateInstance(resolution, new Object[] { }) as IAcePluginBase;

            return plugin;

            // resolution.getInstance();
        }

        /// <summary>
        /// Loads all external plug-ins from the <see cref="folderNode"/> specified
        /// </summary>
        /// <param name="output">The log builder to output info to</param>
        /// <param name="altFolder">Alternative folder with plugins to load from, at the end of the process it will set back to the existing one (if there was no existing folder, it will set this as default)</param>
        public void loadPlugins(ILogBuilder output, folderNode altFolder = null)
        {
            folderNode old = folderWithPlugins;

            if (altFolder != null)
            {
                folderWithPlugins = altFolder;
                if (output != null) output.log("Loading from alternative directory: " + folderWithPlugins.path);
            }

            dllFileNames.AddRange(folderWithPlugins.findFiles("*.dll", System.IO.SearchOption.AllDirectories));

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Count);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);

                try
                {
                    Assembly assembly = Assembly.Load(an);
                    //assemblies.Add(assembly);

                    Type pluginType = typeof(IAcePluginBase);
                    ICollection<Type> pluginTypes = new List<Type>();
                    //foreach (Assembly ass in assemblies)
                    //{
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    registerPlugin(type, dllFile, output);
                                }
                            }
                        }
                    }
                    //}
                }
                catch (IOException ex)
                {
                    if (output != null)
                    {
                        output.log("Assembly load failed - [" + dllFile + "] - consider removing the file from the plugin directory. [" + ex.Message + "] ");
                    }
                }
                catch (BadImageFormatException ex)
                {
                    if (output != null)
                    {
                        output.log("Invalid assembly detected: remove dll file [" + dllFile + "] from the plugin directory. [" + ex.Message + "] ");
                        output.open("fussion-log", "Assembly load failure log:", dllFile);

                        output.Append(ex.FusionLog, imbSCI.Data.enums.appends.appendType.comment, true);

                        output.close();
                    }
                }
                catch (Exception ex)
                {
                    output.log("Plugin assembly import failed [" + dllFile + "] [" + ex.Message + "] ");
                }

                if (output != null) output.log("Plugin assembly loaded: " + an.Name);
            }
            if (old != null) folderWithPlugins = old;
        }
    }
}