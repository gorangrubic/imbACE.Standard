using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Services.consolePlugins
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbACE.Core.plugins.core;
    using imbACE.Services.console;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;

    public interface IAceConsolePlugin : IAceOperationSetExecutor, IAcePluginBase
    {
    }
}