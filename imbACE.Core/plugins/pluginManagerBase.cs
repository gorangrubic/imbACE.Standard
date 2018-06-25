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
    /// Shared infrastructure for external and internal plugin manager types
    /// </summary>
    public abstract class pluginManagerBase
    {
        public static Boolean DOSHOWLOGS { get; set; } = false;

        protected aceConcurrentDictionary<Type> dirtyDictionary { get; set; } = new aceConcurrentDictionary<Type>();

        /// <summary>
        /// If true - it will transform both detected type name and query type name into "dirty name" form that increase chance of name match
        /// </summary>
        /// <value>
        ///   <c>true</c> if [support dirty naming]; otherwise, <c>false</c>.
        /// </value>
        protected abstract Boolean supportDirtyNaming { get; }

        protected String getDirtyForm(String typeName)
        {
            typeName = typeName.Replace("_", "");
            typeName = typeName.Replace("-", "");
            typeName = typeName.Replace(" ", "");
            typeName = typeName.Replace(".", "");
            typeName = typeName.Replace("<", "");
            typeName = typeName.Replace(">", "");
            typeName = typeName.Replace("~", "");
            typeName = typeName.Replace("^", "");
            typeName = typeName.ToUpper();
            return typeName;
        }

        /// <summary>
        /// Dictionary indexing plugins by relative directory path and type short name: e.g. /myPlugins/reporter.dll -> myPlugins.reporter
        /// </summary>
        /// <value>
        ///
        /// </value>
        public aceConcurrentDictionary<Type> pluginTypesByPathName { get; protected set; } = new aceConcurrentDictionary<Type>();

        /// <summary>
        /// Short type name dictionary - for easier resolution/call from the ACE Script
        /// </summary>
        /// <value>
        /// The name of the plugin types by.
        /// </value>
        public aceConcurrentDictionary<Type> pluginTypesByName { get; protected set; } = new aceConcurrentDictionary<Type>();

        /// <summary>
        /// Collection of short type names that are banned because of ambiquity
        /// </summary>
        /// <value>
        /// The banned short names.
        /// </value>
        public aceDictionarySet<String, Type> bannedShortNames { get; protected set; } = new aceDictionarySet<string, Type>();

        /// <summary>
        /// Resolves the plugin by name or directory.name path
        /// </summary>
        /// <param name="plugin_name">Name of the plugin.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        protected Type resolvePlugin(String plugin_name, ILogBuilder output)
        {
            if (!DOSHOWLOGS) output = null;

            if (!bannedShortNames.ContainsKey(plugin_name))
            {
                if (pluginTypesByName.ContainsKey(plugin_name))
                {
                    if (output != null)
                    {
                        output.log("Plugin class [" + plugin_name + "] class resolved. ");
                    }
                    return pluginTypesByName[plugin_name];
                }
            }

            if (pluginTypesByPathName.ContainsKey(plugin_name))
            {
                if (output != null)
                {
                    output.log("Plugin class [" + plugin_name + "] class resolved. ");
                }
                return pluginTypesByPathName[plugin_name];
            }
            else
            {
                if (supportDirtyNaming)
                {
                    String dirtyName = getDirtyForm(plugin_name);
                    if (dirtyDictionary.ContainsKey(dirtyName))
                    {
                        return dirtyDictionary[dirtyName];
                    }
                }

                if (output != null)
                {
                    output.log("Plugin class [" + plugin_name + "] not found.");
                }
            }
            return null;
        }

        /// <summary>
        /// Registers the plugin.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="sourceDllPath">The source DLL path.</param>
        /// <param name="output">The output.</param>
        protected void registerPlugin(Type type, String sourceDllPath, ILogBuilder output)
        {
            if (!DOSHOWLOGS) output = null;
            if (!bannedShortNames.ContainsKey(type.Name))
            {
                if (pluginTypesByName.ContainsKey(type.Name))
                {
                    //if (output != null) output.log("Short-name registration of [" + type.Name + "] failed: name already occupied. You'll have to call both by [directory].[typename] path.");
                    bannedShortNames.Add(type.Name, type);

                    Type pair = null;
                    if (pluginTypesByName.TryRemove(type.Name, out pair))
                    {
                        bannedShortNames.Add(type.Name, pair);
                    }
                }
                else
                {
                    pluginTypesByName.Add(type.Name, type);
                    //if (output != null) output.log("Short-name registration of [" + type.Name + "] done.");
                }
            }
            else
            {
                //if (output != null) output.log("Short-name registration of [" + type.Name + "] failed, the name is banned from short-name registration for [" + bannedShortNames[type.Name] + "] plugins.");
            }

            if (!sourceDllPath.isNullOrEmpty())
            {
                String dirSufix = sourceDllPath;
                if (folderWithPlugins != null)
                {
                    dirSufix = sourceDllPath.removeStartsWith(folderWithPlugins.path).Replace(Path.DirectorySeparatorChar, '.');
                }
                dirSufix = dirSufix.Replace("/", ".");
                dirSufix = dirSufix.Replace("..", ".");

                String dirNamePath = dirSufix.add(type.Name, ".");

                if (pluginTypesByPathName.ContainsKey(dirNamePath))
                {
                    if (output != null) output.log("[directory].[typename] (" + dirNamePath + ") registration of [" + type.Name + "] failed - can't have multiple plugins with the same name, in the same directory. Move it in sub folder or recompile under another class name");
                }
                else
                {
                    if (output != null) output.log("[directory].[typename] (" + dirNamePath + ") registration of [" + type.Name + "] done. ");
                }
            }

            if (supportDirtyNaming)
            {
                String dirtyName = getDirtyForm(type.Name);
                if (!dirtyDictionary.ContainsKey(dirtyName))
                {
                    dirtyDictionary.Add(dirtyName, type);
                }
            }
        }

        /// <summary>
        /// Gets all callable needles for all types registered,
        /// </summary>
        /// <returns></returns>
        public aceDictionarySet<Type, String> GetAllRegistrations()
        {
            aceDictionarySet<Type, String> output = new aceDictionarySet<Type, string>();

            foreach (var p in pluginTypesByName)
            {
                output.Add(p.Value, p.Key);
            }

            foreach (var p in pluginTypesByPathName)
            {
                output.Add(p.Value, p.Key);
            }

            if (supportDirtyNaming)
            {
                foreach (var p in dirtyDictionary)
                {
                    output.Add(p.Value, p.Key);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets or sets the folder with plugins.
        /// </summary>
        /// <value>
        /// The folder with plugins.
        /// </value>
        public folderNode folderWithPlugins { get; set; }

        private String _name = "";

        /// <summary>
        /// Name of the manager
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description = "";

        /// <summary>
        /// Descriptive purpose of this manager
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}