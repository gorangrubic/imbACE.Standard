using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Services.consolePlugins
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbACE.Core.plugins;
    using imbACE.Core.plugins.core;
    using imbACE.Services.console;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;

    /// <summary>
    /// Manager for console plug-ins that can be dynamically instantiated console plug-ins
    /// </summary>
    /// <seealso cref="internalPluginManager{imbACE.Services.consolePlugins.IAceConsolePlugin}" />
    public class aceConsolePluginManager : internalPluginManager<IAceConsolePlugin>
    {
        public void LoadPlugins(ILogBuilder output)
        {
            loadPlugins(output);
        }

        public IAceConsolePlugin GetInstance(IAceAdvancedConsole console, String plugin_name, ILogBuilder output = null)
        {
            return GetPluginInstance(plugin_name, "", output, new Object[] { console });
        }

        protected override bool supportDirtyNaming
        {
            get
            {
                return true;
            }
        }
    }
}