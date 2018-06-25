using imbACE.Core.application;
using imbACE.Core.operations;
using imbACE.Core.plugins.core;
using imbACE.Core.plugins.deployer;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbACE.Core.plugins
{
    /// <summary>
    /// Universal types / plugins manager for general classes, having constructor without arguments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbACE.Core.plugins.internalPluginManager{T}" />
    public class universalInternalParentedPluginManager<T> : internalPluginManager<T> where T : class, IObjectWithParent
    {
        protected override bool supportDirtyNaming { get { return true; } }

        protected Type t { get; set; }

        protected Boolean hasNoArgumentConstructor { get; set; } = false;

        protected Boolean hasConstructorWithParent { get; set; } = false;

        public universalInternalParentedPluginManager()
        {
            t = typeof(T);

            if (t.GetConstructor(new Type[] { }) != null)
            {
                hasNoArgumentConstructor = true;
            }

            if (t.GetConstructor(new Type[] { typeof(IObjectWithParent) }) != null)
            {
                hasConstructorWithParent = true;
            }
        }

        public void LoadPlugins(ILogBuilder output)
        {
            loadPlugins(output);
        }

        public T GetInstance(String plugin_name, T parent, ILogBuilder output = null, Object[] pars = null)
        {
            T instance = null;
            if (hasNoArgumentConstructor)
            {
                instance = GetPluginInstance(plugin_name, "", output);
            }
            if (hasConstructorWithParent)
            {
                instance = GetPluginInstance(plugin_name, "", output, new object[] { parent });
            }
            else
            {
                instance = GetPluginInstance(plugin_name, "", output, pars);
            }

            return instance;
        }
    }
}