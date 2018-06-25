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
    /// Base class for various plugin managers
    /// </summary>
    /// <typeparam name="T">Interface for managed plugin type</typeparam>
    public abstract class internalPluginManager<T> : pluginManagerBase where T : class
    {
        protected virtual Boolean AllowNonImbAssemblies { get { return false; } }

        protected internalPluginManager()
        {
            name = typeof(T).Name + " Manager";
        }

        /// <summary>
        /// Initializes loading of plugins
        /// </summary>
        /// <param name="output">The output.</param>
        protected void loadPlugins(ILogBuilder output)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type pluginType = typeof(T);

            foreach (Assembly ass in assemblies)
            {
                try
                {
                    if (ass.FullName.StartsWith("imb") || AllowNonImbAssemblies)
                    {
                        var types = ass.GetTypes();

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
                                    registerPlugin(type, type.Namespace, output);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    output.log("Assembly [" + ass.FullName + "] type harvest failed");
                    output.log("Exception: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Creates the type instance
        /// </summary>
        /// <param name="plugin_name">Name of the plugin.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        protected T GetPluginInstance(String plugin_name, String instanceName = "", ILogBuilder output = null, Object[] constructorSettings = null)
        {
            Type t = resolvePlugin(plugin_name, output);

            if (t == null) return null;

            T instance = null;

            try
            {
                List<Object> settings = new List<Object>();
                if (constructorSettings != null)
                {
                    settings.AddRange(constructorSettings);
                }

                instance = Activator.CreateInstance(t, settings.ToArray()) as T;

                if (instance is IAcePluginBase)
                {
                    IAcePluginBase instance_IAcePluginBase = (IAcePluginBase)instance;
                    instance_IAcePluginBase.instanceName = instanceName;
                }
            }
            catch (Exception ex)
            {
                output.log("Plugin instance creation for [" + t.Name + "] failed as argument-less constructor not found?! Exception: " + ex.Message);
            }
            return instance;
        }
    }
}